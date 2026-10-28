using BE_08YS;
using BLL_08YS;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GUI_08YS.Estilo;

namespace GUI_08YS.RF1
{
    public partial class FormRegistrarReserva_790MY : Form, IIdiomaObserver_08YS, IAyudaContextual_790MY
    {
        private readonly ClienteBLL_790MY _clienteBll;
        private readonly ReservaBLL_790MY _reservaBll;
        private readonly MesaBLL_790MY    _mesaBll;

        private Cliente_790MY _clienteActual;
        private Mesa_790MY _mesaSeleccionada;

        // DNI de cliente: solo números, de 7 u 8 dígitos (regla del propietario).
        private static readonly Regex _regexDni = new Regex(@"^\d{7,8}$");

        // El resumen de faltantes aparece al intentar confirmar y se va solo al completarse.
        private bool _resumenVisible;

        // Fuentes del label de cliente (encontrado / placeholder). Se crean una sola
        // vez porque el resumen se refresca en cada cambio de la pantalla.
        private Font _fuenteClienteOk;
        private Font _fuenteClienteVacio;

        // Paleta El Fogón del Sur
        private static readonly Color ColorBordo       = Color.FromArgb(120, 25, 25);
        private static readonly Color ColorBordoHover  = Color.FromArgb(95, 18, 18);
        private static readonly Color ColorTexto       = Color.FromArgb(45, 42, 40);
        private static readonly Color ColorGris        = Color.FromArgb(110, 100, 95);
        private static readonly Color ColorBordeCard   = Color.FromArgb(226, 216, 202);
        private static readonly Color ColorBordeTicket = Color.FromArgb(200, 180, 160);
        private static readonly Color ColorPendFondo   = Color.FromArgb(250, 236, 214);
        private static readonly Color ColorPendTexto   = Color.FromArgb(140, 90, 20);
        private static readonly Color ColorListoFondo  = Color.FromArgb(225, 242, 230);
        private static readonly Color ColorListoTexto  = Color.FromArgb(30, 110, 60);

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnConfirmar_790MY), Permisos.RegistrarReserva },
            };

        public FormRegistrarReserva_790MY()
        {
            InitializeComponent();

            _clienteBll = BLLFactory_790MY.CreateClienteBLL();
            _reservaBll = BLLFactory_790MY.CreateReservaBLL();
            _mesaBll    = BLLFactory_790MY.CreateMesaBLL();

            _fuenteClienteOk    = new Font(lblClienteInfo_790MY.Font, FontStyle.Bold);
            _fuenteClienteVacio = new Font(lblClienteInfo_790MY.Font, FontStyle.Italic);
            this.FormClosed += (s, e) =>
            {
                _fuenteClienteOk.Dispose();
                _fuenteClienteVacio.Dispose();
            };

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
        }

        // Los modales se centran sobre la ventana principal: este formulario vive
        // embebido (TopLevel = false) dentro del panel del MDI.
        private IWin32Window VentanaPropietaria => (this.TopLevelControl as Form) ?? (IWin32Window)this;

        private void FormRegistrarReserva_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);

            // El logo se asigna en el Designer; si el recurso no cargó, se oculta.
            if (pbLogoResumen_790MY.Image == null)
                pbLogoResumen_790MY.Visible = false;

            dtpFecha_790MY.MinDate = DateTime.Today;
            dtpFecha_790MY.Value = DateTime.Today;

            // Los textos de los turnos usan el mismo formato que el comprobante
            // (DisplayMember se configura en el Designer).
            foreach (var inicio in new[] { new TimeSpan(18, 0, 0), new TimeSpan(20, 0, 0), new TimeSpan(22, 0, 0) })
                cmbHora_790MY.Items.Add(new OpcionCombo_790MY<TimeSpan> { Texto = ReservaBLL_790MY.FormatearTurno(inicio), Valor = inicio });

            UpdateIdioma();
            LimpiarFormulario();
        }

        #region Estética (cards y pre-ticket)

        // Card blanca con borde suave y una franja bordó a la izquierda.
        private void Card_Paint(object sender, PaintEventArgs e)
        {
            var panel = (Control)sender;
            using (var borde = new Pen(ColorBordeCard))
                e.Graphics.DrawRectangle(borde, 0, 0, panel.Width - 1, panel.Height - 1);
            using (var franja = new SolidBrush(ColorBordo))
                e.Graphics.FillRectangle(franja, 0, 0, 4, panel.Height);
        }

        // Hover del botón principal (el Designer no expone color de hover).
        private void btnConfirmar_790MY_MouseEnter(object sender, EventArgs e)
        {
            btnConfirmar_790MY.BackColor = ColorBordoHover;
        }

        private void btnConfirmar_790MY_MouseLeave(object sender, EventArgs e)
        {
            btnConfirmar_790MY.BackColor = ColorBordo;
        }

        // Borde punteado: el resumen se lee como un ticket todavía sin emitir.
        private void pnlResumen_790MY_Paint(object sender, PaintEventArgs e)
        {
            using (var borde = new Pen(ColorBordeTicket) { DashStyle = DashStyle.Dash })
                e.Graphics.DrawRectangle(borde, 0, 0, pnlResumen_790MY.Width - 1, pnlResumen_790MY.Height - 1);
        }

        // Separador punteado entre las filas de datos del resumen (cliente, fecha,
        // turno, comensales y mesa), de lado a lado de la tarjeta.
        private void tlpResumen_790MY_Paint(object sender, PaintEventArgs e)
        {
            int[] alturas = tlpResumen_790MY.GetRowHeights();
            int x1 = tlpResumen_790MY.Padding.Left;
            int x2 = tlpResumen_790MY.Width - tlpResumen_790MY.Padding.Right;
            int y  = tlpResumen_790MY.Padding.Top;

            using (var linea = new Pen(ColorBordeTicket) { DashStyle = DashStyle.Dot })
            {
                for (int fila = 0; fila < 4 && fila < alturas.Length; fila++)
                {
                    y += alturas[fila];
                    e.Graphics.DrawLine(linea, x1, y - 1, x2, y - 1);
                }
            }
        }

        #endregion

        #region Resumen en vivo

        private void ActualizarResumen()
        {
            var t = TraductorManager_08YS.Instance;
            string vacio = t.GetTexto("RR_res_vacio");

            // Cliente
            if (_clienteActual != null)
            {
                lblClienteInfo_790MY.Text      = $"{_clienteActual.Nombre} {_clienteActual.Apellido}";
                lblClienteInfo_790MY.ForeColor = ColorBordo;
                lblClienteInfo_790MY.Font      = _fuenteClienteOk;
            }
            else
            {
                lblClienteInfo_790MY.Text      = t.GetTexto("RR_lblClienteSinSeleccionar");
                lblClienteInfo_790MY.ForeColor = ColorGris;
                lblClienteInfo_790MY.Font      = _fuenteClienteVacio;
            }
            AsignarValor(lblResClienteV_790MY,
                _clienteActual != null ? $"{_clienteActual.Nombre} {_clienteActual.Apellido} · {_clienteActual.DNI}" : null, vacio);

            // Fecha (con el nombre del día en el idioma activo)
            var cultura = t.CulturaActual;
            string dia  = dtpFecha_790MY.Value.ToString("dddd", cultura);
            if (dia.Length > 0) dia = char.ToUpper(dia[0], cultura) + dia.Substring(1);
            AsignarValor(lblResFechaV_790MY, $"{dia} {dtpFecha_790MY.Value:dd/MM/yyyy}", vacio);

            // Turno
            var turno = cmbHora_790MY.SelectedItem as OpcionCombo_790MY<TimeSpan>;
            AsignarValor(lblResTurnoV_790MY, turno?.Texto, vacio);

            // Comensales
            int comensales = (int)nudComensales_790MY.Value;
            AsignarValor(lblResComensalesV_790MY,
                comensales == 1 ? t.GetTexto("RR_res_persona") : string.Format(t.GetTexto("RR_res_personas"), comensales),
                vacio);

            // Mesa
            string textoMesa = _mesaSeleccionada != null
                ? string.Format(t.GetTexto("RR_lblMesaSeleccionada"), _mesaSeleccionada.NroMesa, _mesaSeleccionada.Capacidad)
                : null;
            lblMesaSeleccionada_790MY.Text      = textoMesa ?? t.GetTexto("RR_lblMesaSinSeleccionar");
            lblMesaSeleccionada_790MY.ForeColor = textoMesa != null ? ColorTexto : ColorGris;
            AsignarValor(lblResMesaV_790MY, textoMesa, vacio);

            // Estado: qué falta para poder confirmar
            var faltantes = new List<string>();
            if (_clienteActual == null)     faltantes.Add(t.GetTexto("RR_res_falta_cliente"));
            if (turno == null)              faltantes.Add(t.GetTexto("RR_res_falta_turno"));
            if (_mesaSeleccionada == null)  faltantes.Add(t.GetTexto("RR_res_falta_mesa"));

            if (faltantes.Count == 0)
            {
                lblResEstado_790MY.Text      = t.GetTexto("RR_res_listo");
                lblResEstado_790MY.BackColor = ColorListoFondo;
                lblResEstado_790MY.ForeColor = ColorListoTexto;
            }
            else
            {
                lblResEstado_790MY.Text      = string.Format(t.GetTexto("RR_res_falta"), string.Join(", ", faltantes));
                lblResEstado_790MY.BackColor = ColorPendFondo;
                lblResEstado_790MY.ForeColor = ColorPendTexto;
            }

            ActualizarValidacion();
        }

        private static void AsignarValor(Label etiqueta, string valor, string vacio)
        {
            bool hay = !string.IsNullOrWhiteSpace(valor);
            etiqueta.Text      = hay ? valor : vacio;
            etiqueta.ForeColor = hay ? ColorTexto : ColorGris;
        }

        #endregion

        // Si cambia la fecha, el turno o la cantidad de comensales después de haber
        // elegido una mesa, esa mesa deja de ser válida para el nuevo contexto:
        // se limpia la selección para forzar a elegir de nuevo. En todos los casos
        // se refresca el resumen en vivo.
        private void InvalidarMesaSeleccionada(object sender, EventArgs e)
        {
            _mesaSeleccionada = null;
            // «Sin disponibilidad» u otro aviso sobre el contexto anterior ya no aplica.
            OcultarAvisoPuntual();
            errValidacion_790MY.SetError(cmbHora_790MY, string.Empty);
            ActualizarResumen();
        }

        private void txtDniCliente_790MY_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo dígitos (y teclas de control como Backspace).
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
            var t = TraductorManager_08YS.Instance;
            OcultarAvisoPuntual();
            errValidacion_790MY.SetError(txtDniCliente_790MY, string.Empty);

            // DNI: solo números, de 7 u 8 dígitos (misma regla que el Maestro de Clientes).
            string texto = txtDniCliente_790MY.Text.Trim();
            if (!_regexDni.IsMatch(texto) || !int.TryParse(texto, out int dni) || dni <= 0)
            {
                string mensaje = t.GetTexto("FC_val_dni_formato");
                errValidacion_790MY.SetError(txtDniCliente_790MY, mensaje);
                bnrAvisos_790MY.Mostrar(mensaje);
                txtDniCliente_790MY.Focus();
                return;
            }

            try
            {
                // Escenario 1: cliente activo — GetByDni filtra por Activo = 1.
                _clienteActual = _clienteBll.GetByDni(dni);

                if (_clienteActual != null)
                    return;

                // Escenario 2: DNI existe en BD pero inactivo (baja lógica).
                // Exists no filtra por Activo, por lo que devuelve true en ese caso.
                if (_clienteBll.Exists(dni))
                {
                    bnrAvisos_790MY.Mostrar(string.Format(t.GetTexto("RR_aviso_cliente_inactivo"), dni),
                                            TipoBanner_790MY.Aviso);
                    return;
                }

                // Escenario 3: DNI no existe en absoluto — ofrecer registro rápido.
                bool registrar = ConfirmModal_790MY.Confirmar(VentanaPropietaria,
                    string.Format(t.GetTexto("RR_confirmar_registro_titulo"), dni),
                    t.GetTexto("RR_confirmar_registro_msg"),
                    t.GetTexto("RR_confirmar_registro_accion"));

                if (!registrar) return;

                using (var frmCliente = new FormRegistrarCliente_790MY(dni))
                {
                    if (frmCliente.ShowDialog(VentanaPropietaria) == DialogResult.OK)
                        _clienteActual = frmCliente.ClienteRegistrado;
                }
            }
            catch (Exception ex)
            {
                MostrarErrorFatal(string.Format(t.GetTexto("msg_rr_error_buscar_cliente"), ex.Message));
            }
            finally
            {
                ActualizarResumen();
            }
        }

        private void btnSeleccionarMesa_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;
            OcultarAvisoPuntual();

            if (cmbHora_790MY.SelectedItem == null)
            {
                string mensaje = t.GetTexto("msg_rr_falta_turno");
                errValidacion_790MY.SetError(cmbHora_790MY, mensaje);
                bnrAvisos_790MY.Mostrar(mensaje);
                cmbHora_790MY.Focus();
                return;
            }

            TimeSpan hora       = ((OpcionCombo_790MY<TimeSpan>)cmbHora_790MY.SelectedItem).Valor;
            int      comensales = (int)nudComensales_790MY.Value;
            DateTime fecha      = dtpFecha_790MY.Value.Date;

            // ── Verificar disponibilidad antes de abrir el plano ──────────────
            // Se consulta el mapa para el mismo contexto que usará FormSeleccionarMesa.
            // Si ninguna mesa resulta seleccionable, se informa al usuario y se
            // cancela la apertura del modal preservando todos los campos del formulario.
            try
            {
                var mesas = _mesaBll.ObtenerMesasMapa(fecha, hora, comensales);
                if (mesas == null || !mesas.Any(m => m.EsSeleccionable))
                {
                    bnrAvisos_790MY.Mostrar(t.GetTexto("RR_aviso_sin_disponibilidad"), TipoBanner_790MY.Aviso);
                    return;
                }
            }
            catch (ArgumentException ex)
            {
                bnrAvisos_790MY.Mostrar(ex.Message);
                return;
            }
            // ─────────────────────────────────────────────────────────────────

            using (var frmMesa = new FormSeleccionarMesa_790MY(fecha, hora, comensales))
            {
                if (frmMesa.ShowDialog(VentanaPropietaria) == DialogResult.OK)
                {
                    _mesaSeleccionada = frmMesa.MesaSeleccionada;
                    ActualizarResumen();
                }
            }
        }

        private void btnConfirmar_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;

            // Etapa 2 (E2-D4): todo lo que falta se marca a la vez y se resume en el banner.
            if (Faltantes().Count > 0)
            {
                _resumenVisible = true;
                ActualizarValidacion();
                return;
            }

            TimeSpan hora = ((OpcionCombo_790MY<TimeSpan>)cmbHora_790MY.SelectedItem).Valor;
            int comensales = (int)nudComensales_790MY.Value;

            Reserva_790MY reserva;
            try
            {
                reserva = _reservaBll.RegistrarReserva(_clienteActual.DNI, _mesaSeleccionada,
                                                       dtpFecha_790MY.Value.Date, hora, comensales);
            }
            catch (ArgumentException ex)
            {
                bnrAvisos_790MY.Mostrar(string.Format(t.GetTexto("RR_err_bll_registrar"), ex.Message));
                return;
            }
            catch (Exception ex)
            {
                MostrarErrorFatal(string.Format(t.GetTexto("msg_rr_error_registrar"), ex.Message));
                return;
            }

            // La reserva ya quedó persistida: el comprobante (vista previa, PDF y
            // email) no puede deshacerla, solo informarla.
            Cliente_790MY cliente = _clienteActual;
            LimpiarFormulario();

            using (var frmComprobante = new FormComprobanteReserva_790MY(reserva, cliente, _reservaBll))
                frmComprobante.ShowDialog(VentanaPropietaria);

            ToastHost_790MY.Mostrar(this, string.Format(t.GetTexto("RR_toast_registrada"),
                                                        reserva.ReservaID, cliente.Nombre, cliente.Apellido));
        }

        #region Validación y avisos (Etapa 2)

        /// <summary>Datos que faltan para confirmar, en el orden de la pantalla.</summary>
        private Dictionary<Control, string> Faltantes()
        {
            var t = TraductorManager_08YS.Instance;
            var faltantes = new Dictionary<Control, string>();
            if (_clienteActual == null)
                faltantes[txtDniCliente_790MY] = t.GetTexto("msg_rr_falta_cliente");
            if (cmbHora_790MY.SelectedItem == null)
                faltantes[cmbHora_790MY] = t.GetTexto("msg_rr_falta_turno2");
            if (_mesaSeleccionada == null)
                faltantes[btnSeleccionarMesa_790MY] = t.GetTexto("msg_rr_falta_mesa");
            return faltantes;
        }

        // Tras intentar confirmar, las marcas y el resumen se actualizan con cada cambio
        // de la pantalla y desaparecen solos cuando ya no falta nada.
        private void ActualizarValidacion()
        {
            if (!_resumenVisible) return;

            Dictionary<Control, string> faltantes = Faltantes();
            foreach (Control control in new Control[] { txtDniCliente_790MY, cmbHora_790MY, btnSeleccionarMesa_790MY })
                errValidacion_790MY.SetError(control, faltantes.TryGetValue(control, out string m) ? m : string.Empty);

            if (faltantes.Count == 0)
            {
                _resumenVisible = false;
                bnrAvisos_790MY.Ocultar();
                return;
            }

            var renglones = faltantes.Values.Select(m => "•  " + m);
            bnrAvisos_790MY.Mostrar(TraductorManager_08YS.Instance.GetTexto("RR_banner_faltan")
                + Environment.NewLine + string.Join(Environment.NewLine, renglones),
                TipoBanner_790MY.Error, cerrable: false);
        }

        /// <summary>Quita un aviso puntual (cliente inactivo, sin disponibilidad, regla de la BLL); el resumen sigue.</summary>
        private void OcultarAvisoPuntual()
        {
            if (!_resumenVisible) bnrAvisos_790MY.Ocultar();
        }

        /// <summary>Error inesperado (base de datos, excepción no prevista): se conserva el MessageBox nativo (E2-D1).</summary>
        private void MostrarErrorFatal(string mensaje)
        {
            MessageBox.Show(VentanaPropietaria, mensaje, TraductorManager_08YS.Instance.GetTexto("error"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        private void LimpiarFormulario()
        {
            _clienteActual = null;
            _mesaSeleccionada = null;
            _resumenVisible = false;
            bnrAvisos_790MY.Ocultar();
            errValidacion_790MY.Clear();

            txtDniCliente_790MY.Clear();

            dtpFecha_790MY.Value = DateTime.Today;
            if (cmbHora_790MY.Items.Count > 0) cmbHora_790MY.SelectedIndex = -1;
            nudComensales_790MY.Value = 2;

            // Los cambios de arriba disparan InvalidarMesaSeleccionada, pero se
            // refresca explícitamente por si ningún valor cambió realmente.
            ActualizarResumen();
            txtDniCliente_790MY.Focus();
        }

        #region i18n
        public void UpdateIdioma()
        {
            AplicarTraducciones(this);

            // Textos dinámicos no cubiertos por Tags (cliente, mesa, resumen y estado).
            ActualizarResumen();
        }

        private void AplicarTraducciones(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                if (c.Tag is string clave && !string.IsNullOrWhiteSpace(clave))
                    c.Text = TraductorManager_08YS.Instance.GetTexto(clave);
                if (c.HasChildren)
                    AplicarTraducciones(c);
            }
        }
        #endregion

        #region Ayuda contextual (Etapa 5)
        /// <summary>Sugerencias del Chef (F1): solo datos; el globo y el resalte los dibuja AyudaSugerencias_790MY.</summary>
        public IReadOnlyList<PistaAyuda_790MY> ObtenerPistas() => new[]
        {
            new PistaAyuda_790MY(nameof(txtDniCliente_790MY), "AY_sug_rr1_t", "FC_val_dni_formato", "AY_sug_rr1_p"),
            new PistaAyuda_790MY(nameof(cmbHora_790MY), "AY_sug_rr2_t", "AY_sug_rr2_x", "AY_sug_rr2_p"),
            new PistaAyuda_790MY(nameof(btnSeleccionarMesa_790MY), "AY_sug_rr3_t", "AY_sug_rr3_x"),
            new PistaAyuda_790MY(nameof(btnConfirmar_790MY), "AY_sug_rr4_t", "RR_subtitulo"),
        };
        #endregion
    }
}
