using BE_08YS;
using BLL_08YS;
using BLL_08YS.Exceptions;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Service_08YS;
using System.Xml.Serialization;

namespace GUI_08YS.Maestros
{
    public partial class FormClientes_790MY : Form, IIdiomaObserver_08YS
    {
        private enum ModoEdicion { Reposo, Alta, Modificacion }

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnAnadir_790MY), Permisos.CrearCliente },
                { nameof(btnModificar_790MY), Permisos.CrearCliente },
                { nameof(btnEliminar_790MY), Permisos.CrearCliente }
            };

        private readonly ClienteBLL_790MY _clienteBll;

        // Modo "registro rapido": Caso de Uso distinto del Maestro (ABM-C). Se activa
        // solo cuando Registrar Reserva abre este formulario via ShowDialog porque el
        // DNI buscado no existe. Su unico objetivo es dar de alta al cliente y devolver
        // el resultado; no expone la grilla ni las acciones de Modificar/Eliminar/Salir
        // del Maestro completo, ni la seccion de Serializacion.
        private readonly bool _modoRegistroRapido;
        public Cliente_790MY ClienteRegistrado { get; private set; }

        private ModoEdicion _modo = ModoEdicion.Reposo;
        private List<string> _emailsAdicionalesEdicion = new List<string>();
        private List<string> _telefonosAdicionalesEdicion = new List<string>();

        public FormClientes_790MY()
        {
            InitializeComponent();
            _clienteBll = BLLFactory_790MY.CreateClienteBLL();
            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
        }

        // Se mantiene por compatibilidad con el flujo de Registrar Reserva: cuando el
        // DNI buscado no existe, se abre este formulario en modo modal para dar de
        // alta al cliente sobre la marcha. En ese caso arranca directo en modo Alta
        // con el DNI precargado, y al confirmar cierra devolviendo DialogResult.OK.
        public FormClientes_790MY(int dniPrellenado) : this()
        {
            _modoRegistroRapido = true;
            txtDni_790MY.Text = dniPrellenado.ToString();
        }

        private void FormClientes_790MY_Load(object sender, EventArgs e)
        {
            UpdateIdioma(); // Aplicar traducciones al abrir (modo normal y modal)
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);

            if (_modoRegistroRapido)
            {
                this.Text = TraductorManager_08YS.Instance.GetTexto("FC_titulo_registro_rapido");

                // Este Caso de Uso (Alta Rapida, extend de Registrar Reserva) no es el
                // Maestro: se ocultan la grilla, las acciones que no le corresponden y
                // toda la seccion de Serializacion. Se hace DESPUES de
                // PermissionFilter_08YS.Aplicar a proposito: ese filtro podria volver a
                // mostrar estos botones segun el permiso del usuario, y acá se fuerza
                // que queden ocultos sin importar el permiso.
                dgvClientes_790MY.Visible = false;
                btnAnadir_790MY.Visible = false;
                btnModificar_790MY.Visible = false;
                btnEliminar_790MY.Visible = false;
                btnSalir_790MY.Visible = false;
                grpSerializacion_790MY.Visible = false;

                EstablecerModo(ModoEdicion.Alta);
                txtDni_790MY.Enabled = false; // ya se busco este DNI en Registrar Reserva; no se cambia acá
            }
            else
            {
                CargarGrilla();
                EstablecerModo(ModoEdicion.Reposo);
            }
        }

        private void CargarGrilla()
        {
            dgvClientes_790MY.DataSource = null;
            dgvClientes_790MY.DataSource = _clienteBll.GetAll();
        }

        private void dgvClientes_790MY_SelectionChanged(object sender, EventArgs e)
        {
            if (_modo == ModoEdicion.Reposo)
                ActualizarHabilitacionBotonesAccion();
        }

        private void ActualizarHabilitacionBotonesAccion()
        {
            bool haySeleccion = dgvClientes_790MY.CurrentRow?.DataBoundItem is Cliente_790MY;
            btnModificar_790MY.Enabled = haySeleccion;
            btnEliminar_790MY.Enabled = haySeleccion;
        }

        private void EstablecerModo(ModoEdicion modo)
        {
            _modo = modo;
            bool editando = modo != ModoEdicion.Reposo;

            txtDni_790MY.Enabled = modo == ModoEdicion.Alta;
            txtApellidos_790MY.Enabled = editando;
            txtNombres_790MY.Enabled = editando;
            txtEmail_790MY.Enabled = editando;
            txtCelular_790MY.Enabled = editando;
            txtDireccion_790MY.Enabled = editando;
            btnEmailsAdicionales_790MY.Enabled = editando;
            btnTelefonosAdicionales_790MY.Enabled = editando;

            btnAplicar_790MY.Enabled = editando;
            btnCancelar_790MY.Enabled = editando;

            dgvClientes_790MY.Enabled = !editando;
            btnAnadir_790MY.Enabled = !editando;

            // La sección de Serialización opera sobre la lista ya cargada en la grilla,
            // asi que se deshabilita junto con ella mientras se está dando de alta o
            // modificando (evita serializar datos a medio confirmar).
            btnSerializar_790MY.Enabled = !editando;
            btnDeserializar_790MY.Enabled = !editando;
            btnExaminarSerializar_790MY.Enabled = !editando;
            btnExaminarDeserializar_790MY.Enabled = !editando;

            if (!editando)
                ActualizarHabilitacionBotonesAccion();
            else
                btnModificar_790MY.Enabled = btnEliminar_790MY.Enabled = false;
        }

        private void LimpiarCampos()
        {
            txtDni_790MY.Clear();
            txtApellidos_790MY.Clear();
            txtNombres_790MY.Clear();
            txtEmail_790MY.Clear();
            txtCelular_790MY.Clear();
            txtDireccion_790MY.Clear();
            _emailsAdicionalesEdicion = new List<string>();
            _telefonosAdicionalesEdicion = new List<string>();
        }

        private void CargarCamposDesde(Cliente_790MY cliente)
        {
            txtDni_790MY.Text = cliente.DNI.ToString();
            txtApellidos_790MY.Text = cliente.Apellido;
            txtNombres_790MY.Text = cliente.Nombre;
            txtEmail_790MY.Text = cliente.Email;
            txtCelular_790MY.Text = cliente.Telefono;
            txtDireccion_790MY.Text = cliente.Direccion;
            _emailsAdicionalesEdicion = new List<string>(cliente.EmailsAdicionales ?? new List<string>());
            _telefonosAdicionalesEdicion = new List<string>(cliente.TelefonosAdicionales ?? new List<string>());
        }

        private void btnAnadir_790MY_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            EstablecerModo(ModoEdicion.Alta);
            txtDni_790MY.Focus();
        }

        private void btnModificar_790MY_Click(object sender, EventArgs e)
        {
            if (!(dgvClientes_790MY.CurrentRow?.DataBoundItem is Cliente_790MY seleccionado))
                return;

            CargarCamposDesde(seleccionado);
            EstablecerModo(ModoEdicion.Modificacion);
        }

        private void btnEliminar_790MY_Click(object sender, EventArgs e)
        {
            if (!(dgvClientes_790MY.CurrentRow?.DataBoundItem is Cliente_790MY seleccionado))
                return;

            var t = TraductorManager_08YS.Instance;
            var respuesta = MessageBox.Show(
                string.Format(t.GetTexto("msg_fc_confirmar_baja"),
                              seleccionado.Nombre, seleccionado.Apellido, seleccionado.DNI),
                t.GetTexto("titulo_confirmar_baja"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            try
            {
                _clienteBll.EliminarCliente(seleccionado.DNI);
                MessageBox.Show(t.GetTexto("msg_fc_baja_ok"), t.GetTexto("exito"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrilla();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, t.GetTexto("no_se_puede_eliminar"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show(t.GetTexto("msg_fc_error_eliminar"), t.GetTexto("error"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEmailsAdicionales_790MY_Click(object sender, EventArgs e)
        {
            using (var frm = new FormListaSimple_790MY("LS_titulo_emails", _emailsAdicionalesEdicion))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    _emailsAdicionalesEdicion = frm.Valores;
            }
        }

        private void btnTelefonosAdicionales_790MY_Click(object sender, EventArgs e)
        {
            using (var frm = new FormListaSimple_790MY("LS_titulo_celulares", _telefonosAdicionalesEdicion))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    _telefonosAdicionalesEdicion = frm.Valores;
            }
        }

        private void btnAplicar_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;

            if (!int.TryParse(txtDni_790MY.Text.Trim(), out int dni))
            {
                MessageBox.Show(t.GetTexto("msg_fc_dni_invalido"), t.GetTexto("titulo_dato_invalido"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string apellido = txtApellidos_790MY.Text.Trim();
            if (string.IsNullOrWhiteSpace(apellido))
            {
                MessageBox.Show(t.GetTexto("msg_fc_apellido_requerido"), t.GetTexto("titulo_dato_invalido"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombre = txtNombres_790MY.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show(t.GetTexto("msg_fc_nombre_requerido"), t.GetTexto("titulo_dato_invalido"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string email = txtEmail_790MY.Text.Trim();
            if (!string.IsNullOrWhiteSpace(email) &&
                !System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show(t.GetTexto("msg_fc_email_invalido"), t.GetTexto("titulo_dato_invalido"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string celular = txtCelular_790MY.Text.Trim();
            if (!string.IsNullOrWhiteSpace(celular) &&
                !System.Text.RegularExpressions.Regex.IsMatch(celular, @"^\d+$"))
            {
                MessageBox.Show(t.GetTexto("msg_fc_celular_solo_numeros"), t.GetTexto("titulo_dato_invalido"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string direccion = txtDireccion_790MY.Text.Trim();
            if (string.IsNullOrWhiteSpace(direccion))
            {
                MessageBox.Show(t.GetTexto("msg_fc_direccion_requerida"), t.GetTexto("titulo_dato_invalido"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_modo == ModoEdicion.Alta)
                {
                    _clienteBll.RegistrarCliente(dni, nombre, apellido, email, celular, direccion,
                                                  _emailsAdicionalesEdicion, _telefonosAdicionalesEdicion);

                    if (_modoRegistroRapido)
                    {
                        ClienteRegistrado = new Cliente_790MY(dni, nombre, apellido, email, celular, direccion)
                        {
                            EmailsAdicionales = _emailsAdicionalesEdicion,
                            TelefonosAdicionales = _telefonosAdicionalesEdicion
                        };
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }

                    MessageBox.Show(t.GetTexto("msg_fc_registrado_ok"), t.GetTexto("exito"),
                                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_modo == ModoEdicion.Modificacion)
                {
                    var cliente = new Cliente_790MY(dni, nombre, apellido, email, celular, direccion)
                    {
                        EmailsAdicionales = _emailsAdicionalesEdicion,
                        TelefonosAdicionales = _telefonosAdicionalesEdicion
                    };

                    _clienteBll.ActualizarCliente(cliente);

                    MessageBox.Show(t.GetTexto("msg_fc_modificado_ok"), t.GetTexto("exito"),
                                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LimpiarCampos();
                CargarGrilla();
                EstablecerModo(ModoEdicion.Reposo);
            }
            catch (ClienteDniDuplicadoException_790MY)
            {
                MessageBox.Show(t.GetTexto("msg_dni_duplicado_cliente"), t.GetTexto("titulo_operacion_no_permitida"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException)
            {
                MessageBox.Show(t.GetTexto("msg_fc_error_validacion_bll"), t.GetTexto("titulo_datos_invalidos"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show(t.GetTexto("msg_fc_error_guardar"), t.GetTexto("error"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_790MY_Click(object sender, EventArgs e)
        {
            if (_modoRegistroRapido)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }

            LimpiarCampos();
            EstablecerModo(ModoEdicion.Reposo);
        }

        private void btnSalir_790MY_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Serializacion XML

        private void btnExaminarSerializar_790MY_Click(object sender, EventArgs e)
        {
            using (var dialogo = new SaveFileDialog { Filter = "Archivos XML (*.xml)|*.xml", FileName = "Clientes.xml" })
            {
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                    txtRutaSerializar_790MY.Text = dialogo.FileName;
            }
        }

        private void btnExaminarDeserializar_790MY_Click(object sender, EventArgs e)
        {
            using (var dialogo = new OpenFileDialog { Filter = "Archivos XML (*.xml)|*.xml" })
            {
                if (dialogo.ShowDialog(this) == DialogResult.OK)
                    txtRutaDeserializar_790MY.Text = dialogo.FileName;
            }
        }

        private void btnSerializar_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;

            if (string.IsNullOrWhiteSpace(txtRutaSerializar_790MY.Text))
            {
                MessageBox.Show(t.GetTexto("msg_fc_falta_ruta_serializar"), t.GetTexto("titulo_falta_ruta"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si hay una fila seleccionada se serializa solo esa (la grilla es de
            // seleccion unica); si no hay seleccion, se serializa la lista completa
            // que está cargada actualmente.
            List<Cliente_790MY> aSerializar;
            if (dgvClientes_790MY.CurrentRow?.DataBoundItem is Cliente_790MY seleccionado)
                aSerializar = new List<Cliente_790MY> { seleccionado };
            else if (dgvClientes_790MY.DataSource is List<Cliente_790MY> todos)
                aSerializar = todos;
            else
                aSerializar = new List<Cliente_790MY>();

            try
            {
                var serializer = new XmlSerializer(typeof(List<Cliente_790MY>));
                using (var writer = new StreamWriter(txtRutaSerializar_790MY.Text))
                {
                    serializer.Serialize(writer, aSerializar);
                }

                MessageBox.Show(
                    string.Format(t.GetTexto("msg_fc_serializacion_ok"), aSerializar.Count),
                    t.GetTexto("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(t.GetTexto("msg_fc_error_serializar"), ex.Message),
                    t.GetTexto("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeserializar_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;

            if (string.IsNullOrWhiteSpace(txtRutaDeserializar_790MY.Text))
            {
                MessageBox.Show(t.GetTexto("msg_fc_falta_ruta_deserializar"), t.GetTexto("titulo_falta_ruta"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var serializer = new XmlSerializer(typeof(List<Cliente_790MY>));
                List<Cliente_790MY> resultado;

                using (var reader = new StreamReader(txtRutaDeserializar_790MY.Text))
                {
                    resultado = (List<Cliente_790MY>)serializer.Deserialize(reader);
                }

                lstDeserializados_790MY.Items.Clear();

                if (resultado == null || resultado.Count == 0)
                {
                    lstDeserializados_790MY.Items.Add(TraductorManager_08YS.Instance.GetTexto("msg_fc_archivo_sin_clientes"));
                    return;
                }

                foreach (var cliente in resultado)
                {
                    lstDeserializados_790MY.Items.Add(
                        $"DNI {cliente.DNI} - {cliente.Nombre} {cliente.Apellido} - {cliente.Email}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(t.GetTexto("msg_fc_error_deserializar"), ex.Message),
                    t.GetTexto("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiarSerializacion_790MY_Click(object sender, EventArgs e)
        {
            txtRutaSerializar_790MY.Clear();
            txtRutaDeserializar_790MY.Clear();
            lstDeserializados_790MY.Items.Clear();
        }

        #endregion

        #region i18n
        public void UpdateIdioma()
        {
            TraducirControles(this);

            // Título dinámico en modo registro rápido
            if (_modoRegistroRapido)
                this.Text = TraductorManager_08YS.Instance.GetTexto("FC_titulo_registro_rapido");

            // Encabezados de columnas del DGV (no son Controls, deben traducirse explícitamente)
            colDni_790MY.HeaderText       = TraductorManager_08YS.Instance.GetTexto("FC_colDni");
            colApellidos_790MY.HeaderText = TraductorManager_08YS.Instance.GetTexto("FC_colApellidos");
            colNombres_790MY.HeaderText   = TraductorManager_08YS.Instance.GetTexto("FC_colNombres");
            colEmail_790MY.HeaderText     = TraductorManager_08YS.Instance.GetTexto("FC_colEmail");
            colCelular_790MY.HeaderText   = TraductorManager_08YS.Instance.GetTexto("FC_colCelular");
            colDireccion_790MY.HeaderText = TraductorManager_08YS.Instance.GetTexto("FC_colDireccion");
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
    }
}
