using BE_08YS;
using BLL_08YS;
using BLL_08YS.Exceptions;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GUI_08YS.Estilo;
using Service_08YS;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Maestros
{
    public partial class FormClientes_790MY : Form, IIdiomaObserver_08YS, IAyudaContextual_790MY
    {
        private enum ModoEdicion { Reposo, Alta, Modificacion }

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnAnadir_790MY), Permisos.CrearCliente },
                { nameof(btnModificar_790MY), Permisos.CrearCliente },
                { nameof(btnEliminar_790MY), Permisos.CrearCliente }
            };

        // Mismas expresiones que ya usaba el formulario (y que aplica la BLL para el email).
        // DNI: solo números, sin puntos ni guiones, de 7 u 8 dígitos (regla del propietario).
        private static readonly Regex _regexEmail = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        private static readonly Regex _regexCelular = new Regex(@"^\d+$");
        private static readonly Regex _regexDni = new Regex(@"^\d{7,8}$");

        private readonly ClienteBLL_790MY _clienteBll;

        private ModoEdicion _modo = ModoEdicion.Reposo;
        private List<Cliente_790MY> _todos = new List<Cliente_790MY>();
        private List<string> _emailsAdicionalesEdicion = new List<string>();
        private List<string> _telefonosAdicionalesEdicion = new List<string>();

        // Validación en línea (P16, E2-D4): un campo muestra su error (ErrorProvider) recién
        // cuando el usuario sale de él, para no llenar de rojo una ficha nueva vacía.
        // El resumen en el banner aparece al intentar guardar y se va solo al corregirse.
        private readonly HashSet<Control> _camposTocados = new HashSet<Control>();
        private bool _cargandoCampos;
        private bool _resumenVisible;

        // «Guardar o descartar» (P18): huella de la ficha al empezar a editar, DNI de la
        // fila a la que el usuario quiso ir con cambios sin guardar, y bandera para que
        // los cambios de selección hechos por código no se tomen como del usuario.
        private string _huellaInicial = string.Empty;
        private int? _dniPendiente;
        private bool _hayDestinoPendiente;
        private bool _ignorarSeleccion;

        public FormClientes_790MY()
        {
            InitializeComponent();
            _clienteBll = BLLFactory_790MY.CreateClienteBLL();
            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
        }

        private void FormClientes_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);
            CargarGrilla(null);
            EstablecerModo(ModoEdicion.Reposo);
            UpdateIdioma();
        }

        #region Lista y búsqueda (P15)
        private void CargarGrilla(int? dniSeleccionar)
        {
            _todos = _clienteBll.GetAll() ?? new List<Cliente_790MY>();
            AplicarFiltro(dniSeleccionar);
        }

        private void AplicarFiltro(int? dniSeleccionar)
        {
            string texto = txtBuscar_790MY.Text.Trim();
            List<Cliente_790MY> visibles = string.IsNullOrEmpty(texto)
                ? _todos
                : _todos.Where(c => Coincide(c, texto)).ToList();

            _ignorarSeleccion = true;
            try
            {
                dgvClientes_790MY.DataSource = null;
                dgvClientes_790MY.DataSource = visibles;
                dgvClientes_790MY.ClearSelection();
                if (dniSeleccionar.HasValue)
                    SeleccionarFila(dniSeleccionar.Value);
            }
            finally
            {
                _ignorarSeleccion = false;
            }

            ActualizarResumen(visibles.Count);
            if (_modo == ModoEdicion.Reposo)
            {
                MostrarFicha();
                ActualizarHabilitacionBotonesAccion();
            }
        }

        private static bool Coincide(Cliente_790MY c, string texto)
        {
            bool Contiene(string valor) =>
                !string.IsNullOrEmpty(valor) && valor.IndexOf(texto, StringComparison.CurrentCultureIgnoreCase) >= 0;

            return c.DNI.ToString().Contains(texto)
                || Contiene(c.Apellido) || Contiene(c.Nombre)
                || Contiene(c.Email) || Contiene(c.Telefono);
        }

        private void ActualizarResumen(int visibles)
        {
            var t = TraductorManager_08YS.Instance;
            lblResumen_790MY.Text = visibles == _todos.Count
                ? string.Format(t.GetTexto("FC_resumen"), _todos.Count)
                : string.Format(t.GetTexto("FC_resumen_filtrado"), visibles, _todos.Count);

            lblSinResultados_790MY.Text = _todos.Count == 0 ? t.GetTexto("FC_sin_clientes") : t.GetTexto("FC_sin_resultados");
            lblSinResultados_790MY.Visible = visibles == 0;
            lblSinResultados_790MY.BringToFront();
        }

        private void SeleccionarFila(int dni)
        {
            foreach (DataGridViewRow fila in dgvClientes_790MY.Rows)
            {
                if (fila.DataBoundItem is Cliente_790MY c && c.DNI == dni)
                {
                    dgvClientes_790MY.CurrentCell = fila.Cells[colApellidos_790MY.Index];
                    fila.Selected = true;
                    return;
                }
            }
        }

        private void txtBuscar_790MY_TextChanged(object sender, EventArgs e)
        {
            // Retardo de 250 ms: se filtra cuando el usuario deja de escribir.
            tmrBusqueda_790MY.Stop();
            tmrBusqueda_790MY.Start();
        }

        private void tmrBusqueda_790MY_Tick(object sender, EventArgs e)
        {
            tmrBusqueda_790MY.Stop();
            AplicarFiltro(DniEnFoco());
        }

        /// <summary>DNI que debe quedar elegido al refiltrar: el que se edita o el de la fila actual.</summary>
        private int? DniEnFoco()
        {
            if (_modo == ModoEdicion.Modificacion && int.TryParse(txtDni_790MY.Text.Trim(), out int dni))
                return dni;
            return ClienteSeleccionado()?.DNI;
        }

        private Cliente_790MY ClienteSeleccionado()
        {
            if (dgvClientes_790MY.SelectedRows.Count != 1) return null;
            return dgvClientes_790MY.SelectedRows[0].DataBoundItem as Cliente_790MY;
        }

        private void dgvClientes_790MY_SelectionChanged(object sender, EventArgs e)
        {
            if (_ignorarSeleccion) return;

            if (_modo == ModoEdicion.Reposo)
            {
                // Un aviso sobre el cliente anterior (por ejemplo, una baja rechazada) ya no aplica.
                bnrAvisos_790MY.Ocultar();
                MostrarFicha();
                ActualizarHabilitacionBotonesAccion();
                return;
            }

            // Editando: si el usuario va a otra fila, se resuelve qué hacer con la ficha.
            ResolverCambioDeFila();
        }

        // El usuario eligió otra fila mientras editaba: si no hay cambios se sale de la
        // edición; si los hay, se vuelve a la fila editada y se ofrece guardar o descartar.
        private void ResolverCambioDeFila()
        {
            Cliente_790MY destino = ClienteSeleccionado();
            int? dniEditado = _modo == ModoEdicion.Modificacion && int.TryParse(txtDni_790MY.Text.Trim(), out int d) ? d : (int?)null;
            if (destino != null && dniEditado.HasValue && destino.DNI == dniEditado.Value)
                return;
            if (destino == null && dgvClientes_790MY.SelectedRows.Count == 0)
                return;

            if (!HayCambiosSinGuardar())
            {
                SalirDeEdicion();
                MostrarFicha();
                ActualizarHabilitacionBotonesAccion();
                return;
            }

            // Cambios sin guardar: se vuelve a la fila editada y se pregunta en línea.
            _dniPendiente = destino?.DNI;
            _hayDestinoPendiente = true;
            _ignorarSeleccion = true;
            try
            {
                dgvClientes_790MY.ClearSelection();
                if (dniEditado.HasValue) SeleccionarFila(dniEditado.Value);
            }
            finally
            {
                _ignorarSeleccion = false;
            }
            btnGuardarCambios_790MY.Enabled = btnAplicar_790MY.Enabled;
            pnlAvisoCambios_790MY.Visible = true;
        }
        #endregion

        #region Ficha
        private void MostrarFicha()
        {
            var t = TraductorManager_08YS.Instance;
            int seleccionadas = dgvClientes_790MY.SelectedRows.Count;
            Cliente_790MY cliente = ClienteSeleccionado();

            if (cliente != null)
            {
                lblDetalleTitulo_790MY.Text = $"{cliente.Apellido}, {cliente.Nombre}";
                lblDetalleAyuda_790MY.Text = string.Format(t.GetTexto("FC_ficha_dni"), cliente.DNI);
                CargarCamposDesde(cliente);
            }
            else
            {
                lblDetalleTitulo_790MY.Text = t.GetTexto("FC_ficha_titulo");
                lblDetalleAyuda_790MY.Text = seleccionadas > 1
                    ? string.Format(t.GetTexto("FC_ficha_varios"), seleccionadas)
                    : t.GetTexto("FC_ficha_vacia");
                LimpiarCampos();
            }
        }

        private void ActualizarTituloFicha()
        {
            var t = TraductorManager_08YS.Instance;
            switch (_modo)
            {
                case ModoEdicion.Alta:
                    lblDetalleTitulo_790MY.Text = t.GetTexto("FC_ficha_nuevo");
                    lblDetalleAyuda_790MY.Text = t.GetTexto("FC_ficha_requeridos");
                    break;
                case ModoEdicion.Modificacion:
                    lblDetalleTitulo_790MY.Text = t.GetTexto("FC_ficha_editar");
                    lblDetalleAyuda_790MY.Text = t.GetTexto("FC_ficha_requeridos");
                    break;
                default:
                    MostrarFicha();
                    break;
            }
        }

        private void pnlDetalle_790MY_Paint(object sender, PaintEventArgs e)
        {
            using (var lapiz = new Pen(T.Linea))
                e.Graphics.DrawLine(lapiz, 0, 0, 0, pnlDetalle_790MY.Height);
        }
        #endregion

        private void ActualizarHabilitacionBotonesAccion()
        {
            // Con multiselección, Modificar/Eliminar actúan sobre un único cliente:
            // se habilitan solo si hay exactamente una fila seleccionada.
            bool haySeleccion = ClienteSeleccionado() != null;
            btnModificar_790MY.Enabled = haySeleccion;
            btnEliminar_790MY.Enabled = haySeleccion;
        }

        private void EstablecerModo(ModoEdicion modo)
        {
            _modo = modo;
            bool editando = modo != ModoEdicion.Reposo;

            // ReadOnly (y no Enabled = false) para que la ficha se lea bien en reposo.
            txtDni_790MY.ReadOnly = modo != ModoEdicion.Alta;
            txtApellidos_790MY.ReadOnly = !editando;
            txtNombres_790MY.ReadOnly = !editando;
            txtEmail_790MY.ReadOnly = !editando;
            txtCelular_790MY.ReadOnly = !editando;
            txtDireccion_790MY.ReadOnly = !editando;
            foreach (TextBox caja in CamposFicha())
            {
                caja.TabStop = !caja.ReadOnly;
                caja.BackColor = caja.ReadOnly ? T.Panel : T.Tarjeta;
            }

            txtEmailAdicional_790MY.Visible = editando;
            btnEmailsAdicionales_790MY.Visible = editando;
            txtCelularAdicional_790MY.Visible = editando;
            btnTelefonosAdicionales_790MY.Visible = editando;
            txtEmailAdicional_790MY.Clear();
            txtCelularAdicional_790MY.Clear();
            DibujarEtiquetas();

            btnAplicar_790MY.Enabled = editando;
            btnCancelar_790MY.Enabled = editando;
            btnAplicar_790MY.Visible = editando;
            btnCancelar_790MY.Visible = editando;

            // La lista sigue disponible mientras se edita: al cambiar de fila con datos
            // sin guardar se ofrece «Guardar o descartar» (P18).
            btnAnadir_790MY.Enabled = !editando;

            // La sección de Serialización opera sobre la lista ya cargada en la grilla,
            // asi que se deshabilita mientras se está dando de alta o modificando
            // (evita serializar datos a medio confirmar).
            btnSerializar_790MY.Enabled = !editando;
            btnDeserializar_790MY.Enabled = !editando;
            btnExaminarSerializar_790MY.Enabled = !editando;
            btnExaminarDeserializar_790MY.Enabled = !editando;

            if (!editando)
            {
                pnlAvisoCambios_790MY.Visible = false;
                _hayDestinoPendiente = false;
                ActualizarHabilitacionBotonesAccion();
            }
            else
            {
                btnModificar_790MY.Enabled = btnEliminar_790MY.Enabled = false;
            }

            _camposTocados.Clear();
            _resumenVisible = false;
            bnrAvisos_790MY.Ocultar();
            errValidacion_790MY.Clear();
            ActualizarTituloFicha();
            ActualizarValidacion();
        }

        private IEnumerable<TextBox> CamposFicha()
        {
            yield return txtDni_790MY;
            yield return txtApellidos_790MY;
            yield return txtNombres_790MY;
            yield return txtEmail_790MY;
            yield return txtCelular_790MY;
            yield return txtDireccion_790MY;
        }

        private void LimpiarCampos()
        {
            _cargandoCampos = true;
            txtDni_790MY.Clear();
            txtApellidos_790MY.Clear();
            txtNombres_790MY.Clear();
            txtEmail_790MY.Clear();
            txtCelular_790MY.Clear();
            txtDireccion_790MY.Clear();
            _emailsAdicionalesEdicion = new List<string>();
            _telefonosAdicionalesEdicion = new List<string>();
            _cargandoCampos = false;
            DibujarEtiquetas();
        }

        private void CargarCamposDesde(Cliente_790MY cliente)
        {
            _cargandoCampos = true;
            txtDni_790MY.Text = cliente.DNI.ToString();
            txtApellidos_790MY.Text = cliente.Apellido;
            txtNombres_790MY.Text = cliente.Nombre;
            txtEmail_790MY.Text = cliente.Email;
            txtCelular_790MY.Text = cliente.Telefono;
            txtDireccion_790MY.Text = cliente.Direccion;
            _emailsAdicionalesEdicion = new List<string>(cliente.EmailsAdicionales ?? new List<string>());
            _telefonosAdicionalesEdicion = new List<string>(cliente.TelefonosAdicionales ?? new List<string>());
            _cargandoCampos = false;
            DibujarEtiquetas();
        }

        private string HuellaFicha()
            => string.Join("\u001F", CamposFicha().Select(c => c.Text.Trim()))
             + "\u001E" + string.Join("\u001F", _emailsAdicionalesEdicion)
             + "\u001E" + string.Join("\u001F", _telefonosAdicionalesEdicion);

        private bool HayCambiosSinGuardar()
            => _modo != ModoEdicion.Reposo && HuellaFicha() != _huellaInicial;

        private void btnAnadir_790MY_Click(object sender, EventArgs e)
        {
            _ignorarSeleccion = true;
            dgvClientes_790MY.ClearSelection();
            _ignorarSeleccion = false;

            LimpiarCampos();
            EstablecerModo(ModoEdicion.Alta);
            _huellaInicial = HuellaFicha();
            txtDni_790MY.Focus();
        }

        private void btnModificar_790MY_Click(object sender, EventArgs e)
        {
            if (!(ClienteSeleccionado() is Cliente_790MY seleccionado))
                return;

            CargarCamposDesde(seleccionado);
            EstablecerModo(ModoEdicion.Modificacion);
            _huellaInicial = HuellaFicha();
            txtApellidos_790MY.Focus();
        }

        private void btnEliminar_790MY_Click(object sender, EventArgs e)
        {
            if (!(ClienteSeleccionado() is Cliente_790MY seleccionado))
                return;

            var t = TraductorManager_08YS.Instance;
            bool confirmado = ConfirmModal_790MY.Confirmar(this,
                t.GetTexto("FC_confirmar_baja_titulo"),
                string.Format(t.GetTexto("msg_fc_confirmar_baja"),
                              seleccionado.Nombre, seleccionado.Apellido, seleccionado.DNI),
                t.GetTexto("FC_confirmar_baja_accion"));

            if (!confirmado) return;

            try
            {
                _clienteBll.EliminarCliente(seleccionado.DNI);
                CargarGrilla(null);
                ToastHost_790MY.Mostrar(dgvClientes_790MY,
                    string.Format(t.GetTexto("msg_fc_baja_ok"), seleccionado.Nombre, seleccionado.Apellido));
            }
            catch (ArgumentException ex)
            {
                MostrarErrorEnBanner(string.Format(t.GetTexto("FC_err_bll_baja"), ex.Message));
            }
            catch (Exception)
            {
                MostrarErrorFatal(t.GetTexto("msg_fc_error_eliminar"));
            }
        }

        #region Avisos (Etapa 2: banner, toast y MessageBox solo para errores fatales)
        /// <summary>Error recuperable (regla de la BLL, archivo, etc.): queda en el banner hasta cerrarlo o corregirlo.</summary>
        private void MostrarErrorEnBanner(string mensaje, TipoBanner_790MY tipo = TipoBanner_790MY.Error)
        {
            _resumenVisible = false;
            bnrAvisos_790MY.Mostrar(mensaje, tipo, cerrable: true);
        }

        /// <summary>Error inesperado (base de datos, excepción no prevista): se conserva el MessageBox nativo (E2-D1).</summary>
        private void MostrarErrorFatal(string mensaje)
        {
            MessageBox.Show(this, mensaje, TraductorManager_08YS.Instance.GetTexto("error"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region Emails y celulares adicionales como etiquetas (P17)
        private void btnEmailsAdicionales_790MY_Click(object sender, EventArgs e)
            => AgregarAdicional(txtEmailAdicional_790MY, _emailsAdicionalesEdicion);

        private void btnTelefonosAdicionales_790MY_Click(object sender, EventArgs e)
            => AgregarAdicional(txtCelularAdicional_790MY, _telefonosAdicionalesEdicion);

        private void txtAdicional_790MY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            if (sender == txtEmailAdicional_790MY)
                AgregarAdicional(txtEmailAdicional_790MY, _emailsAdicionalesEdicion);
            else
                AgregarAdicional(txtCelularAdicional_790MY, _telefonosAdicionalesEdicion);
        }

        // Mismas reglas que FormListaSimple_790MY: se ignora el valor vacío y no se
        // admiten duplicados. El duplicado se marca en el campo con el ErrorProvider.
        private void AgregarAdicional(TextBox origen, List<string> destino)
        {
            string valor = origen.Text.Trim();
            if (string.IsNullOrWhiteSpace(valor))
                return;

            // Mismo formato que el email y el celular principales, en alta y en modificación.
            string errorFormato = ErrorFormatoAdicional(origen == txtEmailAdicional_790MY, valor);
            string error = errorFormato
                ?? (destino.Contains(valor) ? TraductorManager_08YS.Instance.GetTexto("FC_val_adicional_duplicado") : null);
            if (error != null)
            {
                origen.BackColor = T.ErrorFondo;
                errValidacion_790MY.SetError(origen, error);
                return;
            }

            destino.Add(valor);
            origen.Clear();
            origen.BackColor = T.Tarjeta;
            errValidacion_790MY.SetError(origen, string.Empty);
            origen.Focus();
            DibujarEtiquetas();
            ActualizarValidacion();
        }

        /// <summary>Motivo si el email/celular adicional no tiene formato válido; null si es válido.</summary>
        private static string ErrorFormatoAdicional(bool esEmail, string valor)
        {
            var t = TraductorManager_08YS.Instance;
            if (esEmail)
                return _regexEmail.IsMatch(valor) ? null : t.GetTexto("FC_val_email_formato");
            return _regexCelular.IsMatch(valor) ? null : t.GetTexto("FC_val_celular_numeros");
        }

        /// <summary>
        /// Adicionales ya cargados con formato inválido (p. ej. datos viejos guardados antes de
        /// validarlos). Al guardar se informan para que se quiten con su ✕.
        /// </summary>
        private List<string> AdicionalesInvalidos()
            => _emailsAdicionalesEdicion.Where(v => ErrorFormatoAdicional(true, v) != null)
                .Concat(_telefonosAdicionalesEdicion.Where(v => ErrorFormatoAdicional(false, v) != null))
                .ToList();

        private void DibujarEtiquetas()
        {
            LlenarEtiquetas(flpEmailsAdicionales_790MY, _emailsAdicionalesEdicion);
            LlenarEtiquetas(flpTelefonosAdicionales_790MY, _telefonosAdicionalesEdicion);
        }

        private void LlenarEtiquetas(FlowLayoutPanel panel, List<string> valores)
        {
            bool editando = _modo != ModoEdicion.Reposo;
            var t = TraductorManager_08YS.Instance;

            panel.SuspendLayout();
            foreach (Control c in panel.Controls.Cast<Control>().ToList())
            {
                panel.Controls.Remove(c);
                c.Dispose();
            }

            foreach (string valor in valores)
            {
                var etiqueta = new Button
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font(T.Familia, 8.25F),
                    BackColor = T.Tarjeta,
                    ForeColor = T.Tinta,
                    Margin = new Padding(0, 0, T.Espacio1, T.Espacio1),
                    Padding = new Padding(2, 0, 2, 0),
                    Height = 24,
                    Text = editando ? valor + "   ✕" : valor,
                    Enabled = editando,
                    TabStop = editando,
                    Cursor = editando ? Cursors.Hand : Cursors.Default,
                    AccessibleName = editando ? string.Format(t.GetTexto("FC_quitar_valor"), valor) : valor,
                    UseVisualStyleBackColor = false
                };
                etiqueta.FlatAppearance.BorderColor = T.Linea;
                etiqueta.FlatAppearance.MouseOverBackColor = T.AccionSuave;
                string quitar = valor;
                etiqueta.Click += (s, e) =>
                {
                    valores.Remove(quitar);
                    DibujarEtiquetas();
                    ActualizarValidacion();
                };
                panel.Controls.Add(etiqueta);
            }

            if (valores.Count == 0 && !editando)
            {
                panel.Controls.Add(new Label
                {
                    AutoSize = true,
                    Font = new Font(T.Familia, 8.25F),
                    ForeColor = T.TintaSuave,
                    Margin = new Padding(0, 2, 0, 0),
                    Text = panel == flpEmailsAdicionales_790MY
                        ? t.GetTexto("FC_sin_emails_adicionales")
                        : t.GetTexto("FC_sin_celulares_adicionales")
                });
            }
            panel.ResumeLayout(true);
        }
        #endregion

        #region Validación en línea (P16)
        // Mientras se escribe solo se re-evalúan los campos ya visitados: un error se
        // borra apenas se corrige, pero uno nuevo recién aparece al salir del campo.
        private void CampoFicha_TextChanged(object sender, EventArgs e)
        {
            if (_cargandoCampos || _modo == ModoEdicion.Reposo) return;

            // Un error devuelto por la BLL (DNI duplicado, etc.) se da por atendido al editar.
            if (!_resumenVisible) bnrAvisos_790MY.Ocultar();
            ActualizarValidacion();
        }

        private void CampoFicha_Leave(object sender, EventArgs e)
        {
            if (_modo == ModoEdicion.Reposo) return;
            _camposTocados.Add((Control)sender);
            ActualizarValidacion();
        }

        /// <summary>
        /// Reglas simples repetidas en la GUI (E2-H3); ClienteBLL_790MY sigue siendo la
        /// autoridad al guardar. Obligatorios: DNI, apellido, nombre, email y celular.
        /// La dirección es opcional (la BLL no la exige).
        /// </summary>
        private Dictionary<TextBox, string> ValidarFicha()
        {
            var t = TraductorManager_08YS.Instance;
            var errores = new Dictionary<TextBox, string>();

            string dni = txtDni_790MY.Text.Trim();
            if (!_regexDni.IsMatch(dni))
                errores[txtDni_790MY] = t.GetTexto("FC_val_dni_formato");
            else if (int.Parse(dni) <= 0)
                errores[txtDni_790MY] = t.GetTexto("msg_fc_dni_positivo");

            if (string.IsNullOrWhiteSpace(txtApellidos_790MY.Text))
                errores[txtApellidos_790MY] = t.GetTexto("FC_val_apellido");

            if (string.IsNullOrWhiteSpace(txtNombres_790MY.Text))
                errores[txtNombres_790MY] = t.GetTexto("FC_val_nombre");

            string email = txtEmail_790MY.Text.Trim();
            if (string.IsNullOrWhiteSpace(email))
                errores[txtEmail_790MY] = t.GetTexto("msg_fc_email_requerido");
            else if (!_regexEmail.IsMatch(email))
                errores[txtEmail_790MY] = t.GetTexto("FC_val_email_formato");

            string celular = txtCelular_790MY.Text.Trim();
            if (string.IsNullOrWhiteSpace(celular))
                errores[txtCelular_790MY] = t.GetTexto("msg_fc_celular_requerido");
            else if (!_regexCelular.IsMatch(celular))
                errores[txtCelular_790MY] = t.GetTexto("FC_val_celular_numeros");

            return errores;
        }

        private void ActualizarValidacion()
        {
            if (_modo == ModoEdicion.Reposo)
            {
                foreach (TextBox caja in CamposFicha()) errValidacion_790MY.SetError(caja, string.Empty);
                if (_resumenVisible) OcultarResumen();
                return;
            }

            Dictionary<TextBox, string> errores = ValidarFicha();
            foreach (TextBox caja in CamposFicha())
            {
                bool mostrar = errores.ContainsKey(caja) && _camposTocados.Contains(caja);
                errValidacion_790MY.SetError(caja, mostrar ? errores[caja] : string.Empty);
                if (!caja.ReadOnly)
                    caja.BackColor = mostrar ? T.ErrorFondo : T.Tarjeta;
            }

            if (!_resumenVisible) return;
            if (errores.Count == 0)
                OcultarResumen();
            else
                bnrAvisos_790MY.Mostrar(TextoResumen(errores), TipoBanner_790MY.Error, cerrable: false);
        }

        // Resumen del banner al intentar guardar: un renglón por campo, con su motivo.
        private string TextoResumen(Dictionary<TextBox, string> errores)
        {
            var renglones = errores.Select(par => "•  " + NombreCampo(par.Key) + ": " + par.Value);
            return TraductorManager_08YS.Instance.GetTexto("FC_banner_revisar")
                 + Environment.NewLine + string.Join(Environment.NewLine, renglones);
        }

        private void OcultarResumen()
        {
            _resumenVisible = false;
            bnrAvisos_790MY.Ocultar();
        }

        private string NombreCampo(TextBox caja)
        {
            Label etiqueta =
                caja == txtDni_790MY ? lblDni_790MY :
                caja == txtApellidos_790MY ? lblApellidos_790MY :
                caja == txtNombres_790MY ? lblNombres_790MY :
                caja == txtEmail_790MY ? lblEmail_790MY :
                caja == txtCelular_790MY ? lblCelular_790MY : lblDireccion_790MY;
            return etiqueta.Text.TrimEnd(':', ' ');
        }
        #endregion

        #region Guardar, cancelar y «Guardar o descartar» (P18)
        private void btnAplicar_790MY_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        /// <summary>Guarda la ficha. Devuelve true si la BLL aceptó los datos.</summary>
        private bool Guardar()
        {
            var t = TraductorManager_08YS.Instance;

            Dictionary<TextBox, string> errores = ValidarFicha();
            if (errores.Count > 0)
            {
                foreach (TextBox caja in CamposFicha()) _camposTocados.Add(caja);
                _resumenVisible = true;
                ActualizarValidacion();
                errores.Keys.First().Focus();
                return false;
            }

            List<string> adicionalesInvalidos = AdicionalesInvalidos();
            if (adicionalesInvalidos.Count > 0)
            {
                MostrarErrorEnBanner(string.Format(t.GetTexto("FC_val_adicionales_invalidos"),
                                                   string.Join(", ", adicionalesInvalidos)));
                return false;
            }

            int dni = int.Parse(txtDni_790MY.Text.Trim());
            string apellido = txtApellidos_790MY.Text.Trim();
            string nombre = txtNombres_790MY.Text.Trim();
            string email = txtEmail_790MY.Text.Trim();
            string celular = txtCelular_790MY.Text.Trim();
            string direccion = txtDireccion_790MY.Text.Trim();

            try
            {
                if (_modo == ModoEdicion.Alta)
                {
                    _clienteBll.RegistrarCliente(dni, nombre, apellido, email, celular, direccion,
                                                  _emailsAdicionalesEdicion, _telefonosAdicionalesEdicion);

                    // Alta (E2-D4): la ficha queda limpia y lista para el siguiente cliente.
                    // Se recarga la grilla en reposo para que la selección no dispare
                    // el aviso de «Guardar o descartar».
                    _modo = ModoEdicion.Reposo;
                    CargarGrilla(null);
                    ToastHost_790MY.Mostrar(dgvClientes_790MY,
                        string.Format(t.GetTexto("msg_fc_registrado_ok"), nombre, apellido));
                    LimpiarCampos();
                    EstablecerModo(ModoEdicion.Alta);
                    _huellaInicial = HuellaFicha();
                    txtDni_790MY.Focus();
                    return true;
                }

                // Modificación (E2-D4): la ficha conserva los datos guardados.
                var cliente = new Cliente_790MY(dni, nombre, apellido, email, celular, direccion)
                {
                    EmailsAdicionales = _emailsAdicionalesEdicion,
                    TelefonosAdicionales = _telefonosAdicionalesEdicion
                };

                _clienteBll.ActualizarCliente(cliente);

                _modo = ModoEdicion.Reposo;
                CargarGrilla(dni);
                EstablecerModo(ModoEdicion.Reposo);
                ToastHost_790MY.Mostrar(dgvClientes_790MY, t.GetTexto("msg_fc_modificado_ok"));
                return true;
            }
            catch (ClienteDniDuplicadoException_790MY)
            {
                // Exists() cuenta también a los clientes dados de baja.
                string mensaje = string.Format(t.GetTexto("FC_err_dni_duplicado"), dni);
                MostrarErrorEnBanner(mensaje);
                errValidacion_790MY.SetError(txtDni_790MY, mensaje);
                txtDni_790MY.Focus();
            }
            catch (ArgumentException ex)
            {
                MostrarErrorEnBanner(string.Format(t.GetTexto("FC_err_bll_guardar"), ex.Message));
            }
            catch (Exception)
            {
                MostrarErrorFatal(t.GetTexto("FC_fatal_guardar"));
            }
            return false;
        }

        private void btnCancelar_790MY_Click(object sender, EventArgs e)
        {
            int? dni = _modo == ModoEdicion.Modificacion ? DniEnFoco() : null;
            SalirDeEdicion();
            _ignorarSeleccion = true;
            dgvClientes_790MY.ClearSelection();
            if (dni.HasValue) SeleccionarFila(dni.Value);
            _ignorarSeleccion = false;
            MostrarFicha();
            ActualizarHabilitacionBotonesAccion();
        }

        private void SalirDeEdicion()
        {
            EstablecerModo(ModoEdicion.Reposo);
        }

        private void btnGuardarCambios_790MY_Click(object sender, EventArgs e)
        {
            int? destino = _dniPendiente;
            bool irAOtro = _hayDestinoPendiente;
            if (Guardar() && irAOtro)
            {
                // Tras un alta la ficha queda lista para otro cliente; acá el usuario ya eligió otra fila.
                SalirDeEdicion();
                IrAPendiente(destino);
            }
        }

        private void btnDescartarCambios_790MY_Click(object sender, EventArgs e)
        {
            int? destino = _dniPendiente;
            SalirDeEdicion();
            IrAPendiente(destino);
        }

        private void IrAPendiente(int? dni)
        {
            pnlAvisoCambios_790MY.Visible = false;
            _hayDestinoPendiente = false;
            _dniPendiente = null;
            _ignorarSeleccion = true;
            dgvClientes_790MY.ClearSelection();
            if (dni.HasValue) SeleccionarFila(dni.Value);
            _ignorarSeleccion = false;
            MostrarFicha();
            ActualizarHabilitacionBotonesAccion();
        }
        #endregion

        private void btnSalir_790MY_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Serializacion XML (plegada, P17)
        private void btnPlegarSerializacion_790MY_Click(object sender, EventArgs e)
        {
            grpSerializacion_790MY.Visible = !grpSerializacion_790MY.Visible;
            pnlSerializacion_790MY.Height = grpSerializacion_790MY.Visible ? 290 : 44;
            ActualizarTextoPliegue();
        }

        private void ActualizarTextoPliegue()
        {
            btnPlegarSerializacion_790MY.Text = (grpSerializacion_790MY.Visible ? "▾  " : "▸  ")
                + TraductorManager_08YS.Instance.GetTexto("FC_btnSerializacion");
        }

        private void btnExaminarSerializar_790MY_Click(object sender, EventArgs e)
        {
            using (var dialogo = new SaveFileDialog
            {
                Filter       = TraductorManager_08YS.Instance.GetTexto("FC_filtro_xml"),
                DefaultExt   = "xml",
                AddExtension = true,
                FileName     = "Clientes.xml"
            })
            {
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                {
                    txtRutaSerializar_790MY.Text = dialogo.FileName;
                    errValidacion_790MY.SetError(txtRutaSerializar_790MY, string.Empty);
                }
            }
        }

        private void btnExaminarDeserializar_790MY_Click(object sender, EventArgs e)
        {
            using (var dialogo = new OpenFileDialog
            {
                Filter = TraductorManager_08YS.Instance.GetTexto("FC_filtro_xml")
            })
            {
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                {
                    txtRutaDeserializar_790MY.Text = dialogo.FileName;
                    errValidacion_790MY.SetError(txtRutaDeserializar_790MY, string.Empty);
                }
            }
        }

        private void btnSerializar_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;

            // Se exportan exactamente las filas seleccionadas (1 o N). Sin selección
            // no se exporta nada: se pide al usuario que elija al menos un registro.
            List<Cliente_790MY> aSerializar = dgvClientes_790MY.SelectedRows
                .Cast<DataGridViewRow>()
                .OrderBy(fila => fila.Index)              // mismo orden que en la grilla
                .Select(fila => fila.DataBoundItem as Cliente_790MY)
                .Where(cliente => cliente != null)
                .ToList();

            errValidacion_790MY.SetError(txtRutaSerializar_790MY, string.Empty);
            if (aSerializar.Count == 0)
            {
                MostrarErrorEnBanner(t.GetTexto("msg_fc_seleccionar_para_exportar"), TipoBanner_790MY.Aviso);
                dgvClientes_790MY.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtRutaSerializar_790MY.Text))
            {
                errValidacion_790MY.SetError(txtRutaSerializar_790MY, t.GetTexto("msg_fc_falta_ruta_serializar"));
                btnExaminarSerializar_790MY.Focus();
                return;
            }

            try
            {
                string ruta = txtRutaSerializar_790MY.Text.Trim();
                _clienteBll.SerializarArchivoXML(aSerializar, ruta);
                bnrAvisos_790MY.Ocultar();
                ToastHost_790MY.Mostrar(dgvClientes_790MY,
                    string.Format(t.GetTexto("msg_fc_serializacion_ok"), aSerializar.Count));
            }
            catch (Exception ex) when (EsErrorDeArchivo(ex))
            {
                MostrarErrorEnBanner(string.Format(t.GetTexto("msg_fc_error_serializar"), ex.Message));
            }
            catch (Exception ex)
            {
                MostrarErrorFatal(string.Format(t.GetTexto("msg_fc_error_serializar"), ex.Message));
            }
        }

        // Archivo inválido, inexistente, sin permiso o en uso: el usuario puede elegir otro.
        private static bool EsErrorDeArchivo(Exception ex)
            => ex is ArgumentException || ex is IOException || ex is UnauthorizedAccessException;

        private void btnDeserializar_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;

            errValidacion_790MY.SetError(txtRutaDeserializar_790MY, string.Empty);
            if (string.IsNullOrWhiteSpace(txtRutaDeserializar_790MY.Text))
            {
                errValidacion_790MY.SetError(txtRutaDeserializar_790MY, t.GetTexto("msg_fc_falta_ruta_deserializar"));
                btnExaminarDeserializar_790MY.Focus();
                return;
            }

            try
            {
                string ruta = txtRutaDeserializar_790MY.Text.Trim();
                List<Cliente_790MY> resultado = _clienteBll.DeserializarArchivoXML(ruta);

                bnrAvisos_790MY.Ocultar();
                lstDeserializados_790MY.Items.Clear();

                if (resultado.Count == 0)
                {
                    lstDeserializados_790MY.Items.Add(t.GetTexto("msg_fc_archivo_sin_clientes"));
                    return;
                }

                foreach (var cliente in resultado)
                {
                    lstDeserializados_790MY.Items.Add(
                        $"DNI {cliente.DNI} - {cliente.Nombre} {cliente.Apellido} - {cliente.Email}");
                }
            }
            catch (Exception ex) when (EsErrorDeArchivo(ex))
            {
                MostrarErrorEnBanner(string.Format(t.GetTexto("msg_fc_error_deserializar"), ex.Message));
            }
            catch (Exception ex)
            {
                MostrarErrorFatal(string.Format(t.GetTexto("msg_fc_error_deserializar"), ex.Message));
            }
        }

        private void btnLimpiarSerializacion_790MY_Click(object sender, EventArgs e)
        {
            errValidacion_790MY.SetError(txtRutaSerializar_790MY, string.Empty);
            errValidacion_790MY.SetError(txtRutaDeserializar_790MY, string.Empty);
            txtRutaSerializar_790MY.Clear();
            txtRutaDeserializar_790MY.Clear();
            lstDeserializados_790MY.Items.Clear();
        }

        #endregion

        #region i18n
        public void UpdateIdioma()
        {
            var t = TraductorManager_08YS.Instance;
            TraducirControles(this);

            // Encabezados de columnas del DGV (no son Controls, deben traducirse explícitamente)
            colDni_790MY.HeaderText       = t.GetTexto("FC_colDni");
            colApellidos_790MY.HeaderText = t.GetTexto("FC_colApellidos");
            colNombres_790MY.HeaderText   = t.GetTexto("FC_colNombres");
            colEmail_790MY.HeaderText     = t.GetTexto("FC_colEmail");
            colCelular_790MY.HeaderText   = t.GetTexto("FC_colCelular");
            colDireccion_790MY.HeaderText = t.GetTexto("FC_colDireccion");

            T.PonerMarcaDeAgua(txtBuscar_790MY, t.GetTexto("FC_marca_buscar"));
            T.PonerMarcaDeAgua(txtEmailAdicional_790MY, t.GetTexto("FC_marca_email_adicional"));
            T.PonerMarcaDeAgua(txtCelularAdicional_790MY, t.GetTexto("FC_marca_celular_adicional"));

            this.Text = t.GetTexto("FC_titulo");
            ActualizarTextoPliegue();
            ActualizarResumen(dgvClientes_790MY.Rows.Count);
            ActualizarTituloFicha();
            DibujarEtiquetas();
            ActualizarValidacion();
        }

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                // Omitir TextBox y RichTextBox: son entradas editables por el usuario
                if (c is TextBox || c is RichTextBox)
                    continue;

                if (c.Tag is string clave && !string.IsNullOrWhiteSpace(clave))
                    c.Text = TraductorManager_08YS.Instance.GetTexto(clave);

                if (c.HasChildren)
                    TraducirControles(c);
            }
        }
        #endregion

        #region Ayuda contextual (Etapa 5)
        /// <summary>Sugerencias del Chef (F1): solo datos; el globo y el resalte los dibuja AyudaSugerencias_790MY.</summary>
        public IReadOnlyList<PistaAyuda_790MY> ObtenerPistas() => new[]
        {
            new PistaAyuda_790MY(nameof(txtBuscar_790MY), "AY_sug_cli1_t", "FC_marca_buscar"),
            new PistaAyuda_790MY(nameof(dgvClientes_790MY), "AY_sug_cli2_t", "AY_sug_cli2_x"),
            new PistaAyuda_790MY(nameof(btnAnadir_790MY), "AY_sug_cli3_t", "FC_val_dni_formato"),
        };
        #endregion
    }
}
