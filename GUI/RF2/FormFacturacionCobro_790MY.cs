using BE_08YS;
using BLL_08YS;
using BLL_08YS.Exceptions;
using GUI_08YS.Estilo;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using E = GUI_08YS.RF2.EstiloRF2_790MY;

namespace GUI_08YS.RF2
{
    /// <summary>
    /// Caja y cobro unificado: CUN-006 Generar Factura (cuenta / pre-ticket) y su
    /// extensión CUN-007 Registrar Pago.
    ///  - Izquierda: mesas ocupadas (reservas Asistidas).
    ///  - Centro: consumos entregados, descuento (% o monto fijo), subtotal, IVA y total.
    ///  - Derecha: medio de pago, propina, validación en tiempo real (Luhn / vuelto) y
    ///    "Registrar Cobro y Liberar Mesa", que emite la Factura B en estado PAGADA.
    /// Los cálculos y validaciones definitivos los repite la BLL; la pantalla solo
    /// anticipa el resultado. Bitácora y DV los registra la BLL después del COMMIT.
    /// Etapa 4: errores y rechazos en el banner, éxito en toast, confirmación con
    /// ConfirmModal y el cobro (GenerarFactura + RegistrarCobro) en un Task.Run.
    /// </summary>
    public partial class FormFacturacionCobro_790MY : Form, IIdiomaObserver_08YS
    {
        private static readonly CultureInfo CulturaMoneda = CultureInfo.GetCultureInfo("es-AR");

        private readonly FacturaBLL_790MY _facturaBll;
        private readonly PagoBLL_790MY _pagoBll;

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnPreTicket_790MY),      Permisos.GenerarFactura },
                { nameof(btnRegistrarCobro_790MY), Permisos.RegistrarCobro },
            };

        private List<MesaCobroDTO_790MY> _mesas = new List<MesaCobroDTO_790MY>();
        private DetalleCuentaDTO_790MY _detalle;
        private ImportesCuenta_790MY _importes;       // null => descuento inválido o sin mesa
        private MetodoPago_790MY _metodo = MetodoPago_790MY.Tarjeta;
        private int _cobradasEnTurno;
        private bool _cargando;
        private bool _listo;
        private bool _procesando;      // cobro en curso en segundo plano (E4-D6)
        private bool _rechazoVisible;  // el banner muestra un rechazo de tarjeta (E4-D5)

        private readonly Font _fMesaTitulo = new Font("Segoe UI", 14F, FontStyle.Bold);
        private readonly Font _fMesaDetalle = new Font("Segoe UI", 9.75F);
        private readonly Font _fMesaMonto = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        private readonly Font _fChip = new Font("Segoe UI", 8.75F, FontStyle.Bold);

        public FormFacturacionCobro_790MY()
        {
            InitializeComponent();

            _facturaBll = BLLFactory_790MY.CreateFacturaBLL();
            _pagoBll = BLLFactory_790MY.CreatePagoBLL();

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) =>
            {
                TraductorManager_08YS.Instance.Desuscribir(this);
                foreach (var f in new[] { _fMesaTitulo, _fMesaDetalle, _fMesaMonto, _fChip })
                    f.Dispose();
            };
        }

        private IWin32Window VentanaPropietaria => (this.TopLevelControl as Form) ?? (IWin32Window)this;

        private void FormFacturacionCobro_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);

            Maestros.EstiloMaestros_790MY.EstilizarGrilla(dgvConsumos_790MY);
            dgvConsumos_790MY.ColumnHeadersDefaultCellStyle.BackColor = E.Bordo;
            dgvConsumos_790MY.ColumnHeadersDefaultCellStyle.SelectionBackColor = E.Bordo;
            colPrecio_790MY.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            colSubtotal_790MY.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            foreach (var b in new[] { btnPropinaSin_790MY, btnPropina10_790MY, btnPropina15_790MY })
                Maestros.EstiloMaestros_790MY.EstilizarBoton(b, primario: false);

            _cargando = true;
            cmbTipoDescuento_790MY.SelectedIndex = 0;
            _cargando = false;

            UpdateIdioma();
            _listo = true;
            sldMetodo_790MY.SeleccionarSinAnimacion(OpcionTarjeta);
            SeleccionarMetodo(MetodoPago_790MY.Tarjeta);
            CargarMesas(mantenerSeleccion: false);
        }

        #region Mesas (panel izquierdo)

        private void CargarMesas(bool mantenerSeleccion)
        {
            int? seleccion = mantenerSeleccion ? _detalle?.ReservaID : null;
            try
            {
                _mesas = _facturaBll.ObtenerMesasParaCobrar();
            }
            catch (Exception ex)
            {
                _mesas = new List<MesaCobroDTO_790MY>();
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }

            RenderizarMesas();

            if (seleccion.HasValue && _mesas.Any(m => m.ReservaID == seleccion.Value))
                SeleccionarMesa(seleccion.Value);
            else
                LimpiarCuenta(_mesas.Count == 0 ? E.T("msg_fc_sin_mesas") : E.T("CJ_seleccione_mesa"));
        }

        private void RenderizarMesas()
        {
            flpMesas_790MY.SuspendLayout();
            foreach (Control c in flpMesas_790MY.Controls.Cast<Control>().ToList())
                c.Dispose();
            flpMesas_790MY.Controls.Clear();

            int ancho = AnchoTarjetaMesa();

            if (_mesas.Count == 0)
            {
                // Escenario alternativo 1.1: no hay mesas ocupadas para facturar.
                var vacio = E.CrearLabel(E.T("msg_fc_sin_mesas"), _fMesaDetalle, E.Gris, autoSize: false);
                vacio.Size = new Size(ancho, 90);
                vacio.TextAlign = ContentAlignment.MiddleCenter;
                flpMesas_790MY.Controls.Add(vacio);
            }

            foreach (var mesa in _mesas)
                flpMesas_790MY.Controls.Add(CrearTarjetaMesa(mesa, ancho));

            flpMesas_790MY.ResumeLayout();
            lblCobradasCnt_790MY.Text = _cobradasEnTurno.ToString();
        }

        private int AnchoTarjetaMesa()
            => Math.Max(220, flpMesas_790MY.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2);

        private Control CrearTarjetaMesa(MesaCobroDTO_790MY mesa, int ancho)
        {
            bool seleccionada = _detalle != null && _detalle.ReservaID == mesa.ReservaID;

            var card = new PanelRedondeado_790MY
            {
                Width = ancho,
                Height = 118,
                Margin = new Padding(0, 0, 0, 12),
                BackColor = seleccionada ? E.BordoSuave : Color.White,
                ColorBorde = seleccionada ? E.Bordo : E.BordeCard,
                GrosorBorde = seleccionada ? 2.5f : 1f,
                Radio = 10
            };

            var lblMesa = E.CrearLabel(E.T("CJ_mesa_nro", mesa.MesaNumero), _fMesaTitulo, E.Texto);
            lblMesa.Location = new Point(16, 12);
            card.Controls.Add(lblMesa);

            var chip = E.CrearChip(mesa.TieneFacturaAbierta ? E.T("CJ_chip_cuenta_emitida") : E.T("CJ_chip_asistida"),
                                   mesa.TieneFacturaAbierta ? E.PendFondo : E.ListoFondo,
                                   mesa.TieneFacturaAbierta ? E.PendTexto : E.ListoTexto, _fChip);
            card.Controls.Add(chip);
            chip.Location = new Point(ancho - chip.PreferredWidth - 14, 16);

            var lblCamarero = E.CrearLabel(E.T("CJ_mesa_detalle", mesa.CamareroNombre, mesa.CantidadComensales),
                                           _fMesaDetalle, E.Gris, autoSize: false);
            lblCamarero.Location = new Point(16, 50);
            lblCamarero.Size = new Size(ancho - 32, 22);
            card.Controls.Add(lblCamarero);

            var lblDesde = E.CrearLabel(E.T("CJ_mesa_desde", E.Hora(mesa.Desde)), _fMesaDetalle, E.Gris);
            lblDesde.Location = new Point(16, 80);
            card.Controls.Add(lblDesde);

            var lblMonto = E.CrearLabel(Moneda(mesa.SubtotalConsumido), _fMesaMonto, E.Texto);
            card.Controls.Add(lblMonto);
            lblMonto.Location = new Point(ancho - lblMonto.PreferredWidth - 16, 80);

            E.PropagarClick(card, (s, e) =>
            {
                // Elegir otra mesa descarta el aviso anterior (error o rechazo de otra cuenta).
                OcultarRechazo();
                bnrAvisos_790MY.Ocultar();
                SeleccionarMesa(mesa.ReservaID);
            });
            return card;
        }

        private void flpMesas_790MY_Resize(object sender, EventArgs e)
        {
            if (flpMesas_790MY.Controls.Count > 0) RenderizarMesas();
        }

        #endregion

        #region Cuenta (panel central) — CUN-006

        private void SeleccionarMesa(int reservaId)
        {
            try
            {
                _detalle = _facturaBll.ObtenerDetalleConsumoMesa(reservaId);
            }
            catch (ArgumentException ex)
            {
                // Escenario 2.1: la reserva no tiene consumos entregados para facturar.
                _detalle = null;
                MesaCobroDTO_790MY mesa = _mesas.FirstOrDefault(m => m.ReservaID == reservaId);
                LimpiarCuenta(ex.Message, mesa);
                RenderizarMesas();
                return;
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
                return;
            }

            _cargando = true;
            if (_detalle.FacturaAbierta != null)
                PrecargarDescuento(_detalle.FacturaAbierta);
            else
            {
                cmbTipoDescuento_790MY.SelectedIndex = 0;
                txtDescuento_790MY.Text = "0";
            }
            txtPropina_790MY.Text = "0";
            txtMontoRecibido_790MY.Text = string.Empty;
            txtNroTarjeta_790MY.Text = string.Empty;
            _cargando = false;

            MostrarEncabezadoCuenta();
            LlenarGrilla();
            RenderizarMesas();
            Recalcular();
        }

        /// <summary>Recupera el descuento de la factura Abierta para no emitir otra igual.</summary>
        private void PrecargarDescuento(Factura_790MY abierta)
        {
            decimal porPorcentaje = Math.Round(abierta.Subtotal * abierta.PorcentajeDescuento / 100m, 2, MidpointRounding.AwayFromZero);
            if (porPorcentaje == abierta.MontoDescuento)
            {
                cmbTipoDescuento_790MY.SelectedIndex = 0;
                txtDescuento_790MY.Text = abierta.PorcentajeDescuento.ToString("0.##", CulturaMoneda);
            }
            else
            {
                cmbTipoDescuento_790MY.SelectedIndex = 1;
                txtDescuento_790MY.Text = abierta.MontoDescuento.ToString("0.##", CulturaMoneda);
            }
        }

        private void MostrarEncabezadoCuenta()
        {
            if (_detalle == null) return;
            lblCuentaTitulo_790MY.Text = E.T("CJ_cuenta_titulo", _detalle.MesaNumero);
            lblCuentaSub_790MY.Text = E.T("CJ_cuenta_sub", _detalle.ReservaID, _detalle.CamareroNombre, _detalle.CantidadComensales);

            var avisos = new List<string>();
            if (_detalle.FacturaAbierta != null)
                avisos.Add(E.T("CJ_aviso_factura_abierta", _detalle.FacturaAbierta.NumeroComprobante));
            if (_detalle.PedidosSinDespachar > 0)
                avisos.Add(E.T("CJ_aviso_pedidos_pendientes", _detalle.PedidosSinDespachar));
            lblAvisoCuenta_790MY.Text = string.Join(Environment.NewLine, avisos);
            lblAvisoCuenta_790MY.Visible = avisos.Count > 0;
        }

        private void LlenarGrilla()
        {
            dgvConsumos_790MY.Rows.Clear();
            if (_detalle == null) return;
            foreach (var item in _detalle.Items)
                dgvConsumos_790MY.Rows.Add(item.Cantidad, item.Descripcion, Moneda(item.PrecioUnitario), Moneda(item.Subtotal));
            dgvConsumos_790MY.ClearSelection();
        }

        private void LimpiarCuenta(string mensaje, MesaCobroDTO_790MY mesa = null)
        {
            _detalle = null;
            _importes = null;
            dgvConsumos_790MY.Rows.Clear();
            lblCuentaTitulo_790MY.Text = mesa != null ? E.T("CJ_cuenta_titulo", mesa.MesaNumero) : E.T("CJ_cuenta");
            lblCuentaSub_790MY.Text = mesa != null
                ? E.T("CJ_cuenta_sub", mesa.ReservaID, mesa.CamareroNombre, mesa.CantidadComensales)
                : "-";
            lblAvisoCuenta_790MY.Text = mensaje;
            lblAvisoCuenta_790MY.Visible = !string.IsNullOrEmpty(mensaje);
            lblErrorDescuento_790MY.Text = string.Empty;
            MostrarImportes(null);
            ActualizarCobro();
        }

        private void txtDescuento_790MY_TextChanged(object sender, EventArgs e)
        {
            if (!_cargando) Recalcular();
        }

        /// <summary>CUN-006 paso 4: valida el descuento y recalcula subtotal, IVA y total.</summary>
        private void Recalcular()
        {
            _importes = null;
            lblErrorDescuento_790MY.Text = string.Empty;

            if (_detalle != null)
            {
                if (!TryParseImporte(txtDescuento_790MY.Text, out decimal valor, permitirVacio: true))
                {
                    lblErrorDescuento_790MY.Text = E.T("msg_fc_descuento_formato");
                }
                else
                {
                    try
                    {
                        _importes = FacturaBLL_790MY.CalcularImportes(_detalle.Items.Sum(i => i.Subtotal), valor, TipoDescuentoSeleccionado);
                    }
                    catch (ArgumentException ex)
                    {
                        // Escenario 4.1: descuento inválido, se informa el motivo y no se avanza.
                        lblErrorDescuento_790MY.Text = ex.Message;
                    }
                }
            }

            errValidacion_790MY.SetError(txtDescuento_790MY, lblErrorDescuento_790MY.Text);
            MostrarImportes(_importes);
            ActualizarCobro();
        }

        private TipoDescuento_790MY TipoDescuentoSeleccionado
            => cmbTipoDescuento_790MY.SelectedIndex == 1 ? TipoDescuento_790MY.MontoFijo : TipoDescuento_790MY.Porcentaje;

        private void MostrarImportes(ImportesCuenta_790MY imp)
        {
            decimal subtotal = _detalle?.Items.Sum(i => i.Subtotal) ?? 0m;
            lblSubtotal_790MY.Text = Moneda(subtotal);
            lblDescuentoTxt_790MY.Text = E.T("CJ_descuento_pct", (imp?.PorcentajeDescuento ?? 0m).ToString("0.##", CulturaMoneda));
            lblDescuentoMonto_790MY.Text = imp == null || imp.MontoDescuento == 0 ? Moneda(0) : "- " + Moneda(imp.MontoDescuento);
            lblIva_790MY.Text = imp == null ? "-" : Moneda(imp.IVA);
            lblTotal_790MY.Text = imp == null ? "-" : Moneda(imp.MontoTotal);
            btnPreTicket_790MY.Enabled = imp != null;
        }

        private void btnPreTicket_790MY_Click(object sender, EventArgs e)
        {
            if (_procesando || _detalle == null || _importes == null) return;
            try
            {
                bnrAvisos_790MY.Ocultar();
                SessionManager_08YS.Instance.ValidatePermission(Permisos.GenerarFactura);
                TryParseImporte(txtDescuento_790MY.Text, out decimal valor, permitirVacio: true);

                // CUN-006 pasos 5-7: factura Abierta con importes congelados.
                Factura_790MY factura = _facturaBll.GenerarFactura(_detalle.ReservaID, valor, TipoDescuentoSeleccionado);

                using (var comprobante = new FormComprobanteFactura_790MY(factura, null))
                    comprobante.ShowDialog(VentanaPropietaria);

                CargarMesas(mantenerSeleccion: true);
                E.MostrarExitoToast(this, E.T("msg_fc_factura_generada", factura.NumeroComprobante, Moneda(factura.MontoTotal)));
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }
        }

        #endregion

        #region Cobro (panel derecho) — CUN-007

        // Orden de las opciones de sldMetodo_790MY (ClavesIdioma en el Designer).
        private const int OpcionEfectivo = 0;
        private const int OpcionTarjeta = 1;

        private void sldMetodo_790MY_SeleccionCambiada(object sender, EventArgs e)
            => SeleccionarMetodo(sldMetodo_790MY.IndiceSeleccionado == OpcionEfectivo
                ? MetodoPago_790MY.Efectivo : MetodoPago_790MY.Tarjeta);

        private void SeleccionarMetodo(MetodoPago_790MY metodo)
        {
            if (metodo != _metodo) OcultarRechazo();
            _metodo = metodo;
            sldMetodo_790MY.IndiceSeleccionado = metodo == MetodoPago_790MY.Efectivo ? OpcionEfectivo : OpcionTarjeta;
            tlpEfectivo_790MY.Visible = metodo == MetodoPago_790MY.Efectivo;
            tlpTarjeta_790MY.Visible = metodo == MetodoPago_790MY.Tarjeta;
            ActualizarCobro();
        }

        private void txtPropina_790MY_TextChanged(object sender, EventArgs e)
        {
            if (!_cargando) ActualizarCobro();
        }

        private void btnPropinaSin_790MY_Click(object sender, EventArgs e) => txtPropina_790MY.Text = "0";

        private void btnPropina10_790MY_Click(object sender, EventArgs e) => AplicarPropinaPorcentaje(10m);

        private void btnPropina15_790MY_Click(object sender, EventArgs e) => AplicarPropinaPorcentaje(15m);

        private void AplicarPropinaPorcentaje(decimal porcentaje)
        {
            if (_importes == null) return;
            decimal propina = Math.Round(_importes.MontoTotal * porcentaje / 100m, 0, MidpointRounding.AwayFromZero);
            txtPropina_790MY.Text = propina.ToString("0", CulturaMoneda);
        }

        private void txtMontoRecibido_790MY_TextChanged(object sender, EventArgs e)
        {
            if (!_cargando) ActualizarCobro();
        }

        private void txtNroTarjeta_790MY_TextChanged(object sender, EventArgs e)
        {
            if (_cargando) return;
            OcultarRechazo();
            ActualizarCobro();
        }

        private void cmbCuotas_790MY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_cargando) ActualizarCobro();
        }

        private int CuotasSeleccionadas
            => cmbCuotas_790MY.SelectedIndex >= 0 && cmbCuotas_790MY.SelectedIndex < Pago_790MY.CuotasPermitidas.Length
                ? Pago_790MY.CuotasPermitidas[cmbCuotas_790MY.SelectedIndex]
                : 1;

        /// <summary>
        /// Validación en tiempo real del panel de cobro: total con propina, vuelto (efectivo)
        /// o Luhn y cuotas (tarjeta). Habilita "Registrar Cobro" solo con datos válidos y deja
        /// en lblValidacion el motivo cuando no se puede confirmar.
        /// </summary>
        private void ActualizarCobro()
        {
            string motivo = null;
            decimal propina;
            bool propinaValida = TryParseImporte(txtPropina_790MY.Text, out propina, permitirVacio: true) && propina >= 0;

            if (_detalle == null)
                motivo = E.T("CJ_val_seleccione_mesa");
            else if (_importes == null)
                motivo = E.T("CJ_val_descuento");
            else if (!propinaValida)
                motivo = E.T("msg_fc_propina_invalida");
            else if (_detalle.PedidosSinDespachar > 0)
                motivo = E.T("CJ_aviso_pedidos_pendientes", _detalle.PedidosSinDespachar);

            decimal total = _importes == null ? 0m : _importes.MontoTotal + Math.Max(0m, propina);
            lblTotalCobrar_790MY.Text = _importes == null ? "-" : Moneda(total);

            // ─── Efectivo: vuelto ───
            if (TryParseImporte(txtMontoRecibido_790MY.Text, out decimal recibido, permitirVacio: false) && _importes != null)
            {
                decimal cambio = PagoBLL_790MY.CalcularCambio(total, recibido);
                lblCambio_790MY.Text = Moneda(Math.Max(0m, cambio));
                lblEfectivoEstado_790MY.Text = cambio >= 0 ? E.T("CJ_efectivo_ok") : E.T("msg_fc_efectivo_insuficiente");
                lblEfectivoEstado_790MY.ForeColor = cambio >= 0 ? E.ListoTexto : E.KdsRojoTexto;
                if (_metodo == MetodoPago_790MY.Efectivo && motivo == null && cambio < 0)
                    motivo = E.T("msg_fc_efectivo_insuficiente");
            }
            else
            {
                lblCambio_790MY.Text = Moneda(0);
                lblEfectivoEstado_790MY.Text = E.T("CJ_efectivo_ingrese");
                lblEfectivoEstado_790MY.ForeColor = E.Gris;
                if (_metodo == MetodoPago_790MY.Efectivo && motivo == null)
                    motivo = E.T("CJ_val_monto_recibido");
            }

            // ─── Tarjeta: Luhn y cuotas ───
            string digitos = PagoBLL_790MY.SoloDigitos(txtNroTarjeta_790MY.Text);
            if (string.IsNullOrEmpty(txtNroTarjeta_790MY.Text))
            {
                lblTarjetaEstado_790MY.Text = E.T("CJ_tarjeta_ayuda");
                lblTarjetaEstado_790MY.ForeColor = E.Gris;
                if (_metodo == MetodoPago_790MY.Tarjeta && motivo == null) motivo = E.T("CJ_val_tarjeta");
            }
            else if (digitos == null || digitos.Length < 13 || digitos.Length > 19)
            {
                lblTarjetaEstado_790MY.Text = E.T("CJ_tarjeta_ayuda");
                lblTarjetaEstado_790MY.ForeColor = E.PendTexto;
                if (_metodo == MetodoPago_790MY.Tarjeta && motivo == null) motivo = E.T("CJ_val_tarjeta");
            }
            else if (!PagoBLL_790MY.ValidarTarjetaLuhn(digitos))
            {
                lblTarjetaEstado_790MY.Text = E.T("CJ_tarjeta_luhn_invalida");
                lblTarjetaEstado_790MY.ForeColor = E.KdsRojoTexto;
                if (_metodo == MetodoPago_790MY.Tarjeta && motivo == null) motivo = E.T("msg_fc_tarjeta_invalida");
            }
            else
            {
                lblTarjetaEstado_790MY.Text = E.T("CJ_tarjeta_valida", PagoBLL_790MY.DetectarMarca(digitos), digitos.Substring(digitos.Length - 4));
                lblTarjetaEstado_790MY.ForeColor = E.ListoTexto;
            }

            int cuotas = CuotasSeleccionadas;
            lblCuotaResumen_790MY.Text = _importes == null
                ? "-"
                : E.T("CJ_cuota_resumen", cuotas, Moneda(Math.Round(total / cuotas, 2, MidpointRounding.AwayFromZero)));

            // ─── Validación en línea (ErrorProvider) con las mismas reglas ───
            bool efectivoInsuficiente = _metodo == MetodoPago_790MY.Efectivo && _importes != null
                && TryParseImporte(txtMontoRecibido_790MY.Text, out decimal recibidoValidar, permitirVacio: false)
                && PagoBLL_790MY.CalcularCambio(total, recibidoValidar) < 0;
            bool luhnInvalido = _metodo == MetodoPago_790MY.Tarjeta && digitos != null
                && digitos.Length >= 13 && digitos.Length <= 19 && !PagoBLL_790MY.ValidarTarjetaLuhn(digitos);
            errValidacion_790MY.SetError(txtPropina_790MY,
                _importes != null && !propinaValida ? E.T("msg_fc_propina_invalida") : string.Empty);
            errValidacion_790MY.SetError(txtMontoRecibido_790MY,
                efectivoInsuficiente ? E.T("msg_fc_efectivo_insuficiente") : string.Empty);
            errValidacion_790MY.SetError(txtNroTarjeta_790MY,
                luhnInvalido ? E.T("msg_fc_tarjeta_invalida")
                : _rechazoVisible ? E.T("CJ_tarjeta_rechazada") : string.Empty);

            // Durante el cobro el botón queda en «Procesando»; al terminar, FinalizarProcesamiento
            // vuelve a llamar a este método y lo deja Disponible o Deshabilitado.
            if (_procesando) return;
            bool puedeCobrar = motivo == null && SessionManager_08YS.Instance.HasPermission(Permisos.RegistrarCobro);
            btnRegistrarCobro_790MY.Estado = puedeCobrar ? EstadoBotonCobro_790MY.Disponible : EstadoBotonCobro_790MY.Deshabilitado;
            lblValidacion_790MY.Text = motivo ?? string.Empty;
        }

        /// <summary>
        /// CUN-007 paso 9 en adelante. Todos los valores de pantalla se leen ANTES del
        /// Task.Run: el hilo secundario solo ejecuta GenerarFactura + RegistrarCobro con datos
        /// ya capturados (leer un control desde otro hilo lanza InvalidOperationException).
        /// Los textos, el banner y el comprobante se resuelven después del await, ya en el hilo
        /// de la interfaz.
        /// </summary>
        private async void btnRegistrarCobro_790MY_Click(object sender, EventArgs e)
        {
            if (_procesando || _detalle == null || _importes == null) return;

            // ─── Captura en el hilo de la interfaz ───
            TryParseImporte(txtPropina_790MY.Text, out decimal propina, permitirVacio: true);
            TryParseImporte(txtDescuento_790MY.Text, out decimal valorDescuento, permitirVacio: true);
            decimal total = _importes.MontoTotal + propina;
            TipoDescuento_790MY tipoDescuento = TipoDescuentoSeleccionado;
            MetodoPago_790MY metodo = _metodo;
            decimal? montoRecibido = metodo == MetodoPago_790MY.Efectivo && TryParseImporte(txtMontoRecibido_790MY.Text, out decimal recibido, false)
                ? recibido : (decimal?)null;
            string nroTarjeta = metodo == MetodoPago_790MY.Tarjeta ? txtNroTarjeta_790MY.Text : null;
            int cuotas = CuotasSeleccionadas;
            int reservaId = _detalle.ReservaID;
            int mesaNumero = _detalle.MesaNumero;

            // Sin la patente GenerarFactura (solo cobra) se usa la cuenta ya emitida por el camarero.
            bool generarFactura = SessionManager_08YS.Instance.HasPermission(Permisos.GenerarFactura);
            Factura_790MY facturaEmitida = _detalle.FacturaAbierta;
            if (!generarFactura && facturaEmitida == null)
            {
                bnrAvisos_790MY.Mostrar(E.T("msg_fc_factura_requerida"), TipoBanner_790MY.Error, cerrable: true);
                return;
            }

            // Paso 9: el camarero verifica los datos y confirma (E4-D2).
            string medio = metodo == MetodoPago_790MY.Efectivo
                ? E.T("CJ_efectivo").Trim()
                : E.T("CJ_medio_tarjeta", PagoBLL_790MY.DetectarMarca(nroTarjeta), cuotas);
            bool confirmado = ConfirmModal_790MY.Confirmar(VentanaPropietaria,
                E.T("CJ_confirmar_titulo_mesa", mesaNumero),
                E.T("msg_fc_confirmar_cobro_modal", Moneda(total), medio),
                E.T("CJ_btn_confirmar_cobro"));
            if (!confirmado || _procesando) return;

            // ─── Proceso en segundo plano ───
            IniciarProcesamiento();
            ResultadoCobroDTO_790MY resultado = null;
            Exception error = null;
            try
            {
                resultado = await Task.Run(() =>
                {
                    // CUN-006 (punto de extensión 8.1): factura vigente con los importes mostrados.
                    var reloj = Stopwatch.StartNew();
                    Factura_790MY factura = generarFactura
                        ? _facturaBll.GenerarFactura(reservaId, valorDescuento, tipoDescuento)
                        : facturaEmitida;
                    Trace.WriteLine($"GenerarFactura: {reloj.ElapsedMilliseconds} ms", "Cobro");

                    // CUN-007: pago, factura Pagada, reserva Finalizada y mesa Libre.
                    var solicitud = new SolicitudCobro_790MY
                    {
                        NroFactura = factura.NroFactura,
                        MetodoPago = metodo,
                        MontoPropina = propina,
                        MontoRecibido = montoRecibido,
                        NroTarjeta = nroTarjeta,
                        CantidadCuotas = cuotas
                    };
                    reloj.Restart();
                    ResultadoCobroDTO_790MY cobro = _pagoBll.RegistrarCobro(solicitud);
                    Trace.WriteLine($"RegistrarCobro (total): {reloj.ElapsedMilliseconds} ms", "Cobro");
                    return cobro;
                });
            }
            catch (Exception ex)
            {
                // Con await, la excepción del hilo secundario llega acá, en el hilo de la interfaz.
                error = ex;
            }
            finally
            {
                FinalizarProcesamiento();
            }

            if (IsDisposed) return;

            if (error is PagoRechazadoException_790MY rechazo)
            {
                // Escenario 8.1: sin cambios de estado; se solicita otro medio de pago (E4-D5).
                MostrarRechazo(rechazo);
                return;
            }
            if (error != null)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, error);
                CargarMesas(mantenerSeleccion: true);
                return;
            }

            _cobradasEnTurno++;
            using (var comprobante = new FormComprobanteFactura_790MY(resultado.Factura, resultado.Pago))
                comprobante.ShowDialog(VentanaPropietaria);

            _detalle = null;
            CargarMesas(mantenerSeleccion: false);
            E.MostrarExitoToast(this, E.T("msg_fc_cobro_ok", resultado.Factura.NumeroComprobante, mesaNumero));
        }

        /// <summary>Controles que no se pueden usar mientras el cobro está en curso.</summary>
        private IEnumerable<Control> ControlesDeEntrada
            => new Control[] { tlpMesas_790MY, tlpCuenta_790MY, sldMetodo_790MY, tlpPropina_790MY, pnlMetodoHost_790MY };

        private FormMDI_08YS Contenedor => TopLevelControl as FormMDI_08YS;

        /// <summary>
        /// E4-D6: la bandera se activa antes del primer await (bloquea doble clic y
        /// reentradas), el botón pasa a «Procesando», se deshabilitan las entradas y el
        /// contenedor principal bloquea navegación, idioma y cierre.
        /// </summary>
        private void IniciarProcesamiento()
        {
            _procesando = true;
            bnrAvisos_790MY.Ocultar();
            OcultarRechazo();
            btnRegistrarCobro_790MY.Estado = EstadoBotonCobro_790MY.Procesando;
            foreach (Control c in ControlesDeEntrada) c.Enabled = false;
            lblValidacion_790MY.Text = E.T("CJ_val_procesando");

            FormMDI_08YS contenedor = Contenedor;
            if (contenedor != null)
            {
                contenedor.CierreBloqueado += AvisarCobroEnCurso;
                contenedor.BloquearNavegacion(true);
            }
        }

        /// <summary>Restaura el estado original (también tras un rechazo o un error).</summary>
        private void FinalizarProcesamiento()
        {
            FormMDI_08YS contenedor = Contenedor;
            if (contenedor != null)
            {
                contenedor.BloquearNavegacion(false);
                contenedor.CierreBloqueado -= AvisarCobroEnCurso;
            }

            _procesando = false;
            if (IsDisposed) return;
            foreach (Control c in ControlesDeEntrada) c.Enabled = true;
            if (bnrAvisos_790MY.Visible && bnrAvisos_790MY.Mensaje == E.T("msg_fc_cobro_en_curso"))
                bnrAvisos_790MY.Ocultar();
            ActualizarCobro();   // devuelve el botón a Disponible o Deshabilitado según los datos
        }

        private void AvisarCobroEnCurso()
            => bnrAvisos_790MY.Mostrar(E.T("msg_fc_cobro_en_curso"), TipoBanner_790MY.Aviso, cerrable: false);

        private void FormFacturacionCobro_790MY_FormClosing(object sender, FormClosingEventArgs e)
        {
            // No se cierra la ventana a mitad del cobro (salvo apagado de Windows).
            if (!_procesando || e.CloseReason == CloseReason.WindowsShutDown) return;
            e.Cancel = true;
            AvisarCobroEnCurso();
        }

        private void MostrarRechazo(PagoRechazadoException_790MY ex)
        {
            bnrAvisos_790MY.Mostrar(ex.Message, TipoBanner_790MY.Error, cerrable: true);
            _rechazoVisible = true;
            errValidacion_790MY.SetError(txtNroTarjeta_790MY, E.T("CJ_tarjeta_rechazada"));
            if (_metodo != MetodoPago_790MY.Tarjeta) return;
            txtNroTarjeta_790MY.Focus();
            txtNroTarjeta_790MY.SelectAll();
        }

        /// <summary>El aviso de rechazo se quita al corregir la tarjeta o cambiar de medio de pago.</summary>
        private void OcultarRechazo()
        {
            if (!_rechazoVisible) return;
            _rechazoVisible = false;
            if (bnrAvisos_790MY.Visible) bnrAvisos_790MY.Ocultar();
            errValidacion_790MY.SetError(txtNroTarjeta_790MY, string.Empty);
        }

        #endregion

        #region Helpers

        /// <summary>"$ 102.584" o "$ 102.583,80" (sin decimales si son cero).</summary>
        internal static string Moneda(decimal valor)
            => "$ " + valor.ToString(valor == decimal.Truncate(valor) ? "N0" : "N2", CulturaMoneda);

        /// <summary>Acepta "1.500", "1500,50" o "1500.50"; vacío = 0 si permitirVacio.</summary>
        private static bool TryParseImporte(string texto, out decimal valor, bool permitirVacio)
        {
            valor = 0m;
            if (string.IsNullOrWhiteSpace(texto)) return permitirVacio;

            string t = texto.Trim().Replace("$", "").Replace(" ", "");
            if (decimal.TryParse(t, NumberStyles.Number, CulturaMoneda, out valor)) return true;
            return decimal.TryParse(t, NumberStyles.Number, CultureInfo.InvariantCulture, out valor);
        }

        #endregion

        #region i18n

        public void UpdateIdioma()
        {
            E.TraducirTags(this);
            sldMetodo_790MY.ActualizarTextos();
            btnRegistrarCobro_790MY.TextoProcesando = E.T("CJ_btn_procesando");
            colCantidad_790MY.HeaderText = E.T("CJ_col_cantidad");
            colProducto_790MY.HeaderText = E.T("CJ_col_producto");
            colPrecio_790MY.HeaderText = E.T("CJ_col_precio");
            colSubtotal_790MY.HeaderText = E.T("CJ_col_subtotal");

            _cargando = true;
            int cuotasIdx = Math.Max(0, cmbCuotas_790MY.SelectedIndex);
            cmbCuotas_790MY.Items.Clear();
            foreach (int c in Pago_790MY.CuotasPermitidas)
                cmbCuotas_790MY.Items.Add(E.T(c == 1 ? "CJ_cuota_una" : "CJ_cuota_varias", c));
            cmbCuotas_790MY.SelectedIndex = cuotasIdx;
            _cargando = false;

            if (!_listo) return; // aún no se ejecutó Load
            if (_detalle != null) MostrarEncabezadoCuenta();
            else lblCuentaTitulo_790MY.Text = E.T("CJ_cuenta");
            RenderizarMesas();
            MostrarImportes(_importes);
            ActualizarCobro();
        }

        #endregion
    }
}
