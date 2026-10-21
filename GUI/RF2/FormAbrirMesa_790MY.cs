using BE_08YS;
using BLL_08YS;
using GUI_08YS.RF1;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GUI_08YS.Estilo;
using E = GUI_08YS.RF2.EstiloRF2_790MY;

namespace GUI_08YS.RF2
{
    /// <summary>
    /// CUN-004 Abrir Mesa. Recepción da ingreso al salón a un cliente con reserva
    /// Confirmada del turno en curso, o a un Walk-in (sin reserva) eligiendo la mesa
    /// en el plano de FormSeleccionarMesa_790MY, y asigna el camarero responsable.
    /// Autorización, validaciones, DV y bitácora los resuelve ReservaBLL_790MY.
    /// </summary>
    public partial class FormAbrirMesa_790MY : Form, IIdiomaObserver_08YS, IAyudaContextual_790MY
    {
        private readonly ReservaBLL_790MY  _reservaBll;
        private readonly EmpleadoBLL_790MY _empleadoBll;
        private readonly ClienteBLL_790MY  _clienteBll;
        private readonly MesaBLL_790MY     _mesaBll;

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnConfirmar_790MY), Permisos.AbrirMesa },
            };

        private bool _modoWalkIn;
        private TimeSpan? _turno;

        private List<Reserva_790MY> _reservas = new List<Reserva_790MY>();
        private List<Empleado_790MY> _camareros = new List<Empleado_790MY>();
        private Dictionary<int, int> _mesasACargo = new Dictionary<int, int>();

        private Reserva_790MY _reservaSel;
        private Empleado_790MY _camareroSel;
        private Cliente_790MY _clienteWalkIn;
        private Mesa_790MY _mesaWalkIn;

        private readonly Font _fuenteNombre  = new Font("Segoe UI", 11.25F, FontStyle.Bold);
        private readonly Font _fuenteDetalle = new Font("Segoe UI", 9.75F);
        private readonly Font _fuenteAvatar  = new Font("Segoe UI", 10F, FontStyle.Bold);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        public FormAbrirMesa_790MY()
        {
            InitializeComponent();

            _reservaBll  = BLLFactory_790MY.CreateReservaBLL();
            _empleadoBll = BLLFactory_790MY.CreateEmpleadoBLL();
            _clienteBll  = BLLFactory_790MY.CreateClienteBLL();
            _mesaBll     = BLLFactory_790MY.CreateMesaBLL();

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) =>
            {
                TraductorManager_08YS.Instance.Desuscribir(this);
                timerReloj_790MY.Stop();
                _fuenteNombre.Dispose();
                _fuenteDetalle.Dispose();
                _fuenteAvatar.Dispose();
            };
        }

        // Los modales se centran sobre la ventana principal: este formulario vive
        // embebido (TopLevel = false) dentro del panel del MDI.
        private IWin32Window VentanaPropietaria => (this.TopLevelControl as Form) ?? (IWin32Window)this;

        private void FormAbrirMesa_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);

            UpdateIdioma();
            CargarDatos();
            sldModo_790MY.SeleccionarSinAnimacion(OpcionReserva);
            AplicarModo(false);
            timerReloj_790MY.Start();
        }

        #region Carga de datos

        private void CargarDatos()
        {
            _turno = MesaBLL_790MY.ObtenerTurnoVigente(DateTime.Now);

            try
            {
                _reservas = _reservaBll.ObtenerReservasParaApertura();
                _camareros = _empleadoBll.GetCamarerosActivos();
                _mesasACargo = _reservaBll.ContarMesasACargo(_camareros.Select(c => c.DNI));
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }

            // La selección previa deja de valer si la reserva ya no está en la nómina
            // (otro puesto pudo abrirla) o el camarero dejó de estar activo.
            if (_reservaSel != null && !_reservas.Any(r => r.ReservaID == _reservaSel.ReservaID))
                _reservaSel = null;
            if (_camareroSel != null && !_camareros.Contains(_camareroSel))
                _camareroSel = null;

            ActualizarEncabezado();
            ActualizarContadores();
            RenderizarReservas();
            RenderizarCamareros();
            ActualizarResumen();
        }

        private void ActualizarEncabezado()
        {
            var cultura = TraductorManager_08YS.Instance.CulturaActual;
            string dia = DateTime.Now.ToString("ddd dd/MM/yyyy", cultura);
            if (dia.Length > 0) dia = char.ToUpper(dia[0], cultura) + dia.Substring(1);

            string turno = _turno.HasValue
                ? E.T("AM_turno_vigente", ReservaBLL_790MY.FormatearTurno(_turno.Value))
                : E.T("AM_fuera_de_turno");
            if (MesaBLL_790MY.EsTurnoDePrueba(DateTime.Now))
                turno += " " + E.T("AM_modo_prueba");

            lblTurno_790MY.Text = $"{dia} · {turno} · {DateTime.Now:HH:mm}";

            lblListaTitulo_790MY.Text = _turno.HasValue
                ? E.T("AM_lista_titulo", ReservaBLL_790MY.FormatearTurno(_turno.Value))
                : E.T("AM_fuera_de_turno");
        }

        /// <summary>
        /// Contadores del salón para el turno en curso. Usa el mismo cálculo de ocupación
        /// que el plano (MesaBLL_790MY.ObtenerMesasMapa); si el usuario no tiene VerMesas
        /// o no hay turno, los contadores se ocultan sin afectar la apertura.
        /// </summary>
        private void ActualizarContadores()
        {
            int reservadas = _reservas.Count;
            chipReservadas_790MY.Text = E.T("AM_chip_reservadas", reservadas);

            if (!_turno.HasValue || !SessionManager_08YS.Instance.HasPermission(Permisos.VerMesas))
            {
                chipLibres_790MY.Visible = chipOcupadas_790MY.Visible = false;
                return;
            }

            try
            {
                var mapa = _mesaBll.ObtenerMesasMapa(DateTime.Today, _turno.Value, 1);
                int libres = mapa.Count(m => m.EsSeleccionable);
                int ocupadas = Math.Max(0, mapa.Count(m => m.EstadoMapa == EstadoMapa_790MY.Ocupada) - reservadas);

                chipLibres_790MY.Text = E.T("AM_chip_libres", libres);
                chipOcupadas_790MY.Text = E.T("AM_chip_ocupadas", ocupadas);
                chipLibres_790MY.Visible = chipOcupadas_790MY.Visible = true;
            }
            catch (Exception)
            {
                chipLibres_790MY.Visible = chipOcupadas_790MY.Visible = false;
            }
        }

        #endregion

        #region Tarjetas de reservas

        private void RenderizarReservas()
        {
            string filtro = txtBuscar_790MY.Text.Trim();
            var visibles = _reservas
                .Where(r => filtro.Length == 0
                         || r.ReservaID.ToString() == filtro
                         || r.ClienteDNI.ToString().StartsWith(filtro))
                .ToList();

            lblPendientes_790MY.Text = E.T("AM_pendientes", _reservas.Count);

            flpReservas_790MY.SuspendLayout();
            foreach (Control c in flpReservas_790MY.Controls.Cast<Control>().ToList())
                c.Dispose();
            flpReservas_790MY.Controls.Clear();

            if (visibles.Count == 0)
            {
                var vacio = E.CrearLabel(
                    _turno.HasValue ? E.T("AM_sin_reservas") : E.T("msg_rf2_fuera_de_turno"),
                    _fuenteDetalle, E.Gris, autoSize: false);
                vacio.Size = new Size(AnchoTarjeta(), 60);
                vacio.TextAlign = ContentAlignment.MiddleCenter;
                flpReservas_790MY.Controls.Add(vacio);
            }

            foreach (var reserva in visibles)
                flpReservas_790MY.Controls.Add(CrearTarjetaReserva(reserva));

            flpReservas_790MY.ResumeLayout();
        }

        private int AnchoTarjeta()
            => Math.Max(200, flpReservas_790MY.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4);

        private Control CrearTarjetaReserva(Reserva_790MY reserva)
        {
            bool seleccionada = _reservaSel != null && _reservaSel.ReservaID == reserva.ReservaID;

            var card = new PanelRedondeado_790MY
            {
                Size = new Size(AnchoTarjeta(), 72),
                Margin = new Padding(0, 0, 0, 8),
                BackColor = seleccionada ? E.BordoSuave : Color.White,
                ColorBorde = seleccionada ? E.Bordo : E.BordeCard,
                GrosorBorde = seleccionada ? 2f : 1f,
                Tag = reserva
            };

            var lblNombre = E.CrearLabel(reserva.ClienteNombreCompleto ?? reserva.ClienteDNI.ToString(), _fuenteNombre, E.Texto);
            lblNombre.Location = new Point(16, 12);

            var lblDetalle = E.CrearLabel(
                E.T("AM_card_detalle", reserva.ReservaID, reserva.ClienteDNI.ToString("N0", TraductorManager_08YS.Instance.CulturaActual), reserva.CantidadComensales),
                _fuenteDetalle, E.Gris);
            lblDetalle.Location = new Point(16, 40);

            var lblHora = E.CrearLabel(E.Hora(reserva.Hora), _fuenteNombre, E.Texto);
            lblHora.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            var lblMesa = E.CrearLabel(E.T("AM_card_mesa", reserva.MesaNumero), _fuenteDetalle, E.Gris);
            lblMesa.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            card.Controls.Add(lblNombre);
            card.Controls.Add(lblDetalle);
            card.Controls.Add(lblHora);
            card.Controls.Add(lblMesa);

            // Ubicación a la derecha una vez que AutoSize calculó el ancho.
            lblHora.Location = new Point(card.Width - lblHora.PreferredWidth - 16, 12);
            lblMesa.Location = new Point(card.Width - lblMesa.PreferredWidth - 16, 40);

            E.PropagarClick(card, (s, e) => SeleccionarReserva(reserva));
            return card;
        }

        private void SeleccionarReserva(Reserva_790MY reserva)
        {
            _reservaSel = (_reservaSel != null && _reservaSel.ReservaID == reserva.ReservaID) ? null : reserva;
            RenderizarReservas();
            ActualizarResumen();
        }

        private void flpReservas_790MY_Resize(object sender, EventArgs e)
        {
            if (flpReservas_790MY.Controls.Count > 0)
                RenderizarReservas();
        }

        private void txtBuscar_790MY_TextChanged(object sender, EventArgs e)
        {
            RenderizarReservas();
        }

        #endregion

        #region Tarjetas de camareros

        private void RenderizarCamareros()
        {
            flpCamareros_790MY.SuspendLayout();
            foreach (Control c in flpCamareros_790MY.Controls.Cast<Control>().ToList())
                c.Dispose();
            flpCamareros_790MY.Controls.Clear();

            if (_camareros.Count == 0)
            {
                var vacio = E.CrearLabel(E.T("AM_sin_camareros"), _fuenteDetalle, E.Gris);
                flpCamareros_790MY.Controls.Add(vacio);
            }

            int ancho = Math.Max(180, (flpCamareros_790MY.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 16) / 2);
            foreach (var camarero in _camareros)
                flpCamareros_790MY.Controls.Add(CrearTarjetaCamarero(camarero, ancho));

            flpCamareros_790MY.ResumeLayout();
        }

        private Control CrearTarjetaCamarero(Empleado_790MY camarero, int ancho)
        {
            bool seleccionado = camarero.Equals(_camareroSel);

            var card = new PanelRedondeado_790MY
            {
                Size = new Size(ancho, 62),
                Margin = new Padding(0, 0, 8, 8),
                BackColor = seleccionado ? E.BordoSuave : Color.White,
                ColorBorde = seleccionado ? E.Bordo : E.BordeCard,
                GrosorBorde = seleccionado ? 2f : 1f
            };

            var avatar = new EtiquetaChip_790MY
            {
                AutoSize = false,
                Size = new Size(38, 38),
                Location = new Point(12, 12),
                Padding = Padding.Empty,
                Text = E.Iniciales(camarero.Nombre, camarero.Apellido),
                Font = _fuenteAvatar,
                BackColor = seleccionado ? E.Bordo : E.FondoPanel,
                ForeColor = seleccionado ? Color.White : E.Texto
            };

            var lblNombre = E.CrearLabel(camarero.NombreCompleto, _fuenteNombre, E.Texto);
            lblNombre.Location = new Point(60, 10);

            _mesasACargo.TryGetValue(camarero.DNI, out int mesas);
            var lblMesas = E.CrearLabel(E.T("AM_mesas_a_cargo", mesas), _fuenteDetalle, E.Gris);
            lblMesas.Location = new Point(60, 34);

            card.Controls.Add(avatar);
            card.Controls.Add(lblNombre);
            card.Controls.Add(lblMesas);

            E.PropagarClick(card, (s, e) =>
            {
                _camareroSel = seleccionado ? null : camarero;
                RenderizarCamareros();
                ActualizarResumen();
            });
            return card;
        }

        #endregion

        #region Modo (Con reserva / Walk-in)

        // Orden de las opciones de sldModo_790MY (ClavesIdioma en el Designer).
        private const int OpcionReserva = 0;
        private const int OpcionWalkIn = 1;

        private void sldModo_790MY_SeleccionCambiada(object sender, EventArgs e)
            => AplicarModo(sldModo_790MY.IndiceSeleccionado == OpcionWalkIn);

        private void AplicarModo(bool walkIn)
        {
            _modoWalkIn = walkIn;
            sldModo_790MY.IndiceSeleccionado = walkIn ? OpcionWalkIn : OpcionReserva;

            pnlReserva_790MY.Visible = !walkIn;
            pnlWalkIn_790MY.Visible = walkIn;

            ActualizarResumen();
            // Si el cambio vino del selector (clic o flechas), el foco queda en él para poder
            // seguir usando el teclado; al cargar, va al primer campo del modo.
            if (sldModo_790MY.Focused) return;
            if (walkIn) txtDniCliente_790MY.Focus(); else txtBuscar_790MY.Focus();
        }

        #endregion

        #region Walk-in

        private void SoloDigitos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtDniCliente_790MY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            btnBuscarCliente_790MY.PerformClick();
        }

        private void btnBuscarCliente_790MY_Click(object sender, EventArgs e)
        {
            OcultarAvisoPuntual();
            errValidacion_790MY.SetError(txtDniCliente_790MY, string.Empty);

            // DNI: solo números, de 7 u 8 dígitos (misma regla que Clientes y Reservas).
            string texto = txtDniCliente_790MY.Text.Trim();
            if (!_regexDni.IsMatch(texto) || !int.TryParse(texto, out int dni) || dni <= 0)
            {
                string mensaje = E.T("FC_val_dni_formato");
                errValidacion_790MY.SetError(txtDniCliente_790MY, mensaje);
                bnrAvisos_790MY.Mostrar(mensaje);
                txtDniCliente_790MY.Focus();
                return;
            }

            try
            {
                _clienteWalkIn = _clienteBll.GetByDni(dni);
                if (_clienteWalkIn != null) return;

                if (_clienteBll.Exists(dni))
                {
                    bnrAvisos_790MY.Mostrar(E.T("RR_aviso_cliente_inactivo", dni), TipoBanner_790MY.Aviso);
                    return;
                }

                // La reserva Walk-in necesita un cliente registrado (FK): se ofrece el alta rápida.
                bool registrar = ConfirmModal_790MY.Confirmar(VentanaPropietaria,
                    E.T("RR_confirmar_registro_titulo", dni),
                    E.T("RR_confirmar_registro_msg"),
                    E.T("RR_confirmar_registro_accion"));
                if (!registrar) return;

                using (var frmCliente = new FormRegistrarCliente_790MY(dni))
                {
                    if (frmCliente.ShowDialog(VentanaPropietaria) == DialogResult.OK)
                        _clienteWalkIn = frmCliente.ClienteRegistrado;
                }
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }
            finally
            {
                ActualizarResumen();
            }
        }

        private void nudComensales_790MY_ValueChanged(object sender, EventArgs e)
        {
            // La mesa elegida dejó de corresponder a la nueva cantidad de comensales.
            _mesaWalkIn = null;
            OcultarAvisoPuntual();
            ActualizarResumen();
        }

        /// <summary>
        /// Reutiliza el plano de RF1 (FormSeleccionarMesa_790MY) para el turno en curso:
        /// muestra la ocupación por turno + estado físico y la capacidad de cada mesa.
        /// </summary>
        private void btnSeleccionarMesa_790MY_Click(object sender, EventArgs e)
        {
            int comensales = (int)nudComensales_790MY.Value;
            TimeSpan turno;
            OcultarAvisoPuntual();

            try
            {
                turno = _mesaBll.ObtenerTurnoVigenteObligatorio();
                var mapa = _mesaBll.ObtenerMesasMapa(DateTime.Today, turno, comensales);
                if (mapa == null || !mapa.Any(m => m.EsSeleccionable))
                {
                    bnrAvisos_790MY.Mostrar(E.T("RR_aviso_sin_disponibilidad"), TipoBanner_790MY.Aviso);
                    return;
                }
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
                return;
            }

            using (var frmMesa = new FormSeleccionarMesa_790MY(DateTime.Today, turno, comensales))
            {
                if (frmMesa.ShowDialog(VentanaPropietaria) == DialogResult.OK)
                    _mesaWalkIn = frmMesa.MesaSeleccionada;
            }
            ActualizarResumen();
        }

        #endregion

        #region Resumen en vivo

        private void ActualizarResumen()
        {
            string vacio = E.T("RR_res_vacio");

            lblResModoV_790MY.Text = _modoWalkIn ? E.T("AM_modo_walkin") : E.T("AM_modo_reserva");

            if (_modoWalkIn)
            {
                Asignar(lblResClienteV_790MY, _clienteWalkIn != null ? $"{_clienteWalkIn.Nombre} {_clienteWalkIn.Apellido} · {_clienteWalkIn.DNI}" : null, vacio);
                Asignar(lblResReservaV_790MY, E.T("AM_res_reserva_nueva"), vacio);
                Asignar(lblResMesaV_790MY, _mesaWalkIn != null ? E.T("RR_lblMesaSeleccionada", _mesaWalkIn.NroMesa, _mesaWalkIn.Capacidad) : null, vacio);
                Asignar(lblResComensalesV_790MY, ((int)nudComensales_790MY.Value).ToString(), vacio);

                lblClienteInfo_790MY.Text = _clienteWalkIn != null
                    ? $"{_clienteWalkIn.Nombre} {_clienteWalkIn.Apellido}"
                    : E.T("RR_lblClienteSinSeleccionar");
                lblClienteInfo_790MY.ForeColor = _clienteWalkIn != null ? E.Bordo : E.Gris;

                lblMesaWalkIn_790MY.Text = _mesaWalkIn != null
                    ? E.T("RR_lblMesaSeleccionada", _mesaWalkIn.NroMesa, _mesaWalkIn.Capacidad)
                    : E.T("RR_lblMesaSinSeleccionar");
                lblMesaWalkIn_790MY.ForeColor = _mesaWalkIn != null ? E.Texto : E.Gris;
            }
            else
            {
                Asignar(lblResClienteV_790MY, _reservaSel != null ? $"{_reservaSel.ClienteNombreCompleto} · {_reservaSel.ClienteDNI}" : null, vacio);
                Asignar(lblResReservaV_790MY, _reservaSel != null ? E.T("AM_res_reserva_nro", _reservaSel.ReservaID, E.Hora(_reservaSel.Hora)) : null, vacio);
                Asignar(lblResMesaV_790MY, _reservaSel != null ? E.T("AM_card_mesa", _reservaSel.MesaNumero) : null, vacio);
                Asignar(lblResComensalesV_790MY, _reservaSel?.CantidadComensales.ToString(), vacio);
            }

            Asignar(lblResCamareroV_790MY, _camareroSel?.NombreCompleto, vacio);

            var faltantes = new List<string>();
            if (_modoWalkIn)
            {
                if (_clienteWalkIn == null) faltantes.Add(E.T("AM_falta_cliente"));
                if (_mesaWalkIn == null) faltantes.Add(E.T("AM_falta_mesa"));
            }
            else if (_reservaSel == null)
            {
                faltantes.Add(E.T("AM_falta_reserva"));
            }
            if (_camareroSel == null) faltantes.Add(E.T("AM_falta_camarero"));

            bool listo = faltantes.Count == 0;
            lblFaltantes_790MY.Text = listo ? E.T("AM_listo") : E.T("AM_falta", string.Join(", ", faltantes));
            lblResEstado_790MY.Text = lblFaltantes_790MY.Text;
            lblResEstado_790MY.BackColor = listo ? E.ListoFondo : E.PendFondo;
            lblResEstado_790MY.ForeColor = listo ? E.ListoTexto : E.PendTexto;

            // Etapa 2 (E2-D4): Confirmar queda habilitado durante el turno; si falta algo,
            // al tocarlo se marca cada dato con el ErrorProvider y se resume en el banner.
            btnConfirmar_790MY.Enabled = _turno.HasValue;
            btnConfirmar_790MY.BackColor = btnConfirmar_790MY.Enabled ? E.Bordo : E.Deshabilitado;
            btnConfirmar_790MY.ForeColor = btnConfirmar_790MY.Enabled ? Color.White : E.Gris;

            ActualizarValidacion();
        }

        #region Validación y avisos (Etapa 2)

        // DNI de cliente: solo números, de 7 u 8 dígitos (regla del propietario).
        private static readonly Regex _regexDni = new Regex(@"^\d{7,8}$");

        // El resumen de faltantes aparece al intentar confirmar y se va solo al completarse.
        private bool _resumenVisible;

        /// <summary>Datos que faltan para abrir la mesa, en el orden de la pantalla.</summary>
        private Dictionary<Control, string> Faltantes()
        {
            var faltantes = new Dictionary<Control, string>();
            if (_modoWalkIn)
            {
                if (_clienteWalkIn == null) faltantes[txtDniCliente_790MY] = E.T("AM_val_cliente");
                if (_mesaWalkIn == null) faltantes[btnSeleccionarMesa_790MY] = E.T("AM_val_mesa");
            }
            else if (_reservaSel == null)
            {
                faltantes[flpReservas_790MY] = E.T("AM_val_reserva");
            }
            if (_camareroSel == null) faltantes[flpCamareros_790MY] = E.T("AM_val_camarero");
            return faltantes;
        }

        private void ActualizarValidacion()
        {
            if (!_resumenVisible) return;

            Dictionary<Control, string> faltantes = Faltantes();
            foreach (Control control in new Control[] { txtDniCliente_790MY, btnSeleccionarMesa_790MY, flpReservas_790MY, flpCamareros_790MY })
                errValidacion_790MY.SetError(control, faltantes.TryGetValue(control, out string m) ? m : string.Empty);

            if (faltantes.Count == 0)
            {
                _resumenVisible = false;
                bnrAvisos_790MY.Ocultar();
                return;
            }

            var renglones = faltantes.Values.Select(m => "•  " + m);
            bnrAvisos_790MY.Mostrar(E.T("AM_banner_faltan") + Environment.NewLine + string.Join(Environment.NewLine, renglones),
                                    TipoBanner_790MY.Error, cerrable: false);
        }

        /// <summary>Quita un aviso puntual (cliente inactivo, sin disponibilidad, regla de la BLL); el resumen sigue.</summary>
        private void OcultarAvisoPuntual()
        {
            if (!_resumenVisible) bnrAvisos_790MY.Ocultar();
        }

        #endregion

        private static void Asignar(Label etiqueta, string valor, string vacio)
        {
            bool hay = !string.IsNullOrWhiteSpace(valor);
            etiqueta.Text = hay ? valor : vacio;
            etiqueta.ForeColor = hay ? E.Texto : E.Gris;
        }

        #endregion

        #region Confirmar / Limpiar

        private void btnConfirmar_790MY_Click(object sender, EventArgs e)
        {
            if (Faltantes().Count > 0)
            {
                _resumenVisible = true;
                ActualizarValidacion();
                return;
            }

            Reserva_790MY abierta;
            try
            {
                abierta = _modoWalkIn
                    ? _reservaBll.AbrirMesaWalkIn(_clienteWalkIn.DNI, _mesaWalkIn, (int)nudComensales_790MY.Value, _camareroSel.DNI)
                    : _reservaBll.AbrirMesa(_reservaSel.ReservaID, _camareroSel.DNI);
            }
            catch (Exception ex)
            {
                // Una apertura concurrente o una mesa recién ocupada se refleja al recargar;
                // el aviso se muestra después para que la recarga no lo oculte.
                CargarDatos();
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
                return;
            }

            Limpiar();
            E.MostrarExitoToast(this, E.T("msg_rf2_mesa_abierta_ok", abierta.MesaNumero, abierta.ReservaID));
        }

        private void btnLimpiar_790MY_Click(object sender, EventArgs e) => Limpiar();

        private void Limpiar()
        {
            _reservaSel = null;
            _camareroSel = null;
            _clienteWalkIn = null;
            _mesaWalkIn = null;
            _resumenVisible = false;
            bnrAvisos_790MY.Ocultar();
            errValidacion_790MY.Clear();

            txtBuscar_790MY.Clear();
            txtDniCliente_790MY.Clear();
            nudComensales_790MY.Value = 2;

            CargarDatos();
        }

        #endregion

        #region Estética

        // Separador vertical entre el panel de apertura y el resumen.
        private void pnlIzquierda_790MY_Paint(object sender, PaintEventArgs e)
        {
            using (var linea = new Pen(E.BordeCard))
                e.Graphics.DrawLine(linea, pnlIzquierda_790MY.Width - 1, 0, pnlIzquierda_790MY.Width - 1, pnlIzquierda_790MY.Height);
        }

        // Separadores punteados entre las filas del ticket.
        private void tlpTicket_790MY_Paint(object sender, PaintEventArgs e)
        {
            int[] alturas = tlpTicket_790MY.GetRowHeights();
            int y = 0;
            using (var linea = new Pen(Color.FromArgb(200, 180, 160)) { DashStyle = DashStyle.Dot })
            {
                for (int fila = 0; fila < 7 && fila < alturas.Length; fila++)
                {
                    y += alturas[fila];
                    e.Graphics.DrawLine(linea, 0, y - 1, tlpTicket_790MY.Width, y - 1);
                }
            }
        }

        private void timerReloj_790MY_Tick(object sender, EventArgs e)
        {
            // Si cambió el turno (p. ej. 19:59 -> 20:00), la nómina debe recalcularse.
            if (MesaBLL_790MY.ObtenerTurnoVigente(DateTime.Now) != _turno)
                CargarDatos();
            else
                ActualizarEncabezado();
        }

        #endregion

        #region i18n

        public void UpdateIdioma()
        {
            E.TraducirTags(this);
            sldModo_790MY.ActualizarTextos();
            SendMessage(txtBuscar_790MY.Handle, EM_SETCUEBANNER, (IntPtr)1, E.T("AM_buscar_placeholder"));
            SendMessage(txtDniCliente_790MY.Handle, EM_SETCUEBANNER, (IntPtr)1, E.T("AM_dni_placeholder"));

            if (_reservaBll == null) return; // aún en construcción
            ActualizarEncabezado();
            ActualizarContadores();
            RenderizarReservas();
            RenderizarCamareros();
            ActualizarResumen();
        }

        #endregion

        #region Ayuda contextual (Etapa 5)
        /// <summary>Sugerencias del Chef (F1): solo datos; el globo y el resalte los dibuja AyudaSugerencias_790MY.</summary>
        public IReadOnlyList<PistaAyuda_790MY> ObtenerPistas() => new[]
        {
            new PistaAyuda_790MY(nameof(sldModo_790MY), "AY_sug_am1_t", "AY_sug_am1_x"),
            new PistaAyuda_790MY(nameof(txtBuscar_790MY), "AY_sug_am2_t", "AM_lblBuscar"),
            new PistaAyuda_790MY(nameof(flpCamareros_790MY), "AY_sug_am3_t", "AM_lblCamareroAyuda"),
            new PistaAyuda_790MY(nameof(btnConfirmar_790MY), "AY_sug_am4_t", "AM_ayuda"),
        };
        #endregion
    }
}
