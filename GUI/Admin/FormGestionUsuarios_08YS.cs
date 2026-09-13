using BLL_08YS;
using GUI_08YS;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace GUI
{
    public partial class FormGestionUsuarios_08YS : Form, IIdiomaObserver_08YS
    {
        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnCrear),       Permisos.CrearUsuario      },
                { nameof(btnModificar),   Permisos.ModificarUsuario  },
                { nameof(btnDesbloquear), Permisos.DesbloquearUsuario},
                { nameof(btnActDes),      Permisos.DesActivarUsuario },
            };

        private UserBLL_08YS _bll = BLLFactory_08YS.CreateUserBLL();
        private RolBLL_08YS _bllRol = BLLFactory_08YS.CreateRolBLL();

        public FormGestionUsuarios_08YS()
        {
            InitializeComponent();
        }

        #region Idioma
        public void UpdateIdioma()
        {
            TraducirControles(this);
            TraducirColumnas();
        }

        private void TraducirColumnas()
        {
            if (dgvUsuarios.Columns.Count > 0)
            {
                if (dgvUsuarios.Columns.Contains("Rol")) dgvUsuarios.Columns["Rol"].HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaRol");
                if (dgvUsuarios.Columns.Contains("Nombre")) dgvUsuarios.Columns["Nombre"].HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaNombre");
                if (dgvUsuarios.Columns.Contains("Apellido")) dgvUsuarios.Columns["Apellido"].HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaApellido");
                if (dgvUsuarios.Columns.Contains("Activo")) dgvUsuarios.Columns["Activo"].HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaActivo");
                if (dgvUsuarios.Columns.Contains("Bloqueado")) dgvUsuarios.Columns["Bloqueado"].HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaBloqueado");
            }
        }
        

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                // Si el control tiene un Tag asignado, buscamos su traducción
                if (c.Tag != null && !string.IsNullOrWhiteSpace(c.Tag.ToString()))
                {
                    c.Text = TraductorManager_08YS.Instance.GetTexto(c.Tag.ToString());
                }

                // Si el control tiene hijos (como un Panel o GroupBox), hacemos recursividad
                if (c.HasChildren)
                {
                    TraducirControles(c);
                }
            }
        }
        #endregion

        private enum EstadoUI { Consulta, Insertando, Editando }
        private EstadoUI _estadoActual;

        private void FormGestionUsuarios_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);

            cmbRol.ValueMember = "RolID";       // La propiedad ID del objeto Rol_08YS
            cmbRol.DisplayMember = "Nombre";    // La propiedad con el texto a mostrar en el objeto Rol_08YS
            cmbRol.DataSource = _bllRol.GetAllPlano(); // Trae los roles disponibles del sistema
            CargarGrilla();
           
            CambiarEstado(EstadoUI.Consulta); // Inicia en modo lectura
            UpdateIdioma();
        }

        #region Manejo de Interfaz y Estados

        private void CambiarEstado(EstadoUI nuevoEstado)
        {
            _estadoActual = nuevoEstado;

            switch (nuevoEstado)
            {
                case EstadoUI.Consulta:
                    SetCamposReadOnly(true);
                    btnCrear.Enabled = true;
                    btnModificar.Enabled = dgvUsuarios.CurrentRow != null;
                    btnAplicar.Enabled = false;
                    btnCancelar.Enabled = false;
                    btnDesbloquear.Enabled = ValidarSiEstaBloqueado();
                    txtMensaje.Text = TraductorManager_08YS.Instance.GetTexto("modo_consulta_msg");
                    break;

                case EstadoUI.Insertando:
                    LimpiarCampos();
                    SetCamposReadOnly(false);
                    btnCrear.Enabled = false;
                    btnModificar.Enabled = false;
                    btnDesbloquear.Enabled = false;
                    btnAplicar.Enabled = true;
                    btnCancelar.Enabled = true;
                    btnActDes.Enabled = false;
                    btnActDes.BackColor=Color.White;
                    btnActDes.ForeColor = Color.Black;
                    txtLogin.Enabled = false;
                    txtBloqueado.Enabled = false;
                    txtActivo.Enabled = false;
                    txtMensaje.Text = TraductorManager_08YS.Instance.GetTexto("modo_insercion_msg");
                    txtDNI.Focus();
                    break;
                case EstadoUI.Editando:
                    SetCamposReadOnly(false);
                    btnCrear.Enabled = false;
                    btnModificar.Enabled = false;
                    btnDesbloquear.Enabled = false;
                    btnAplicar.Enabled = true;
                    btnCancelar.Enabled = true;
                    btnActDes.Enabled = false;
                    btnActDes.BackColor = Color.White;
                    btnActDes.ForeColor = Color.Black;
                    txtLogin.Enabled = false;
                    txtBloqueado.Enabled = false;
                    txtActivo.Enabled = false;
                    txtEmail.Enabled = true;
                    cmbRol.Enabled = true;
                    txtMensaje.Text = TraductorManager_08YS.Instance.GetTexto("modo_edicion_msg");
                    txtEmail.Focus();
                    break;
            }
        }

        private void SetCamposReadOnly(bool status)
        {
            // Si status es true (Consulta), todo bloqueado.
            // Si status es false, depende de si es Inserción o Edición.
            bool esEdicion = _estadoActual == EstadoUI.Editando;

            // Campos que NUNCA se editan (DNI, Nombres, Apellidos, Username)
            // Se bloquean si estamos en Consulta O si estamos en Edición.
            txtDNI.ReadOnly = status || esEdicion;
            txtNombres.ReadOnly = status || esEdicion;
            txtApellidos.ReadOnly = status || esEdicion;
            txtLogin.ReadOnly = status || esEdicion;

            // Campos que SÍ se pueden editar (Email y Rol)
            // Solo se bloquean si status es true (Consulta).
            txtEmail.ReadOnly = status;
            cmbRol.Enabled = !status;

            // Campos de estado (Bloqueado/Activo) - Siempre solo lectura para el usuario
            txtBloqueado.ReadOnly = true;
            txtActivo.ReadOnly = true;
        }

        private void LimpiarCampos()
        {
            txtDNI.Clear();
            txtApellidos.Clear();
            txtNombres.Clear();
            txtEmail.Clear();
            txtLogin.Clear();
            cmbRol.SelectedIndex = -1;
            txtActivo.Clear();
            txtBloqueado.Clear();
        }

        private bool ValidarSiEstaBloqueado()
        {
            if (dgvUsuarios.CurrentRow == null) return false;
            var user = (User_08YS)dgvUsuarios.CurrentRow.DataBoundItem;
            return user.Bloqueado;
        }

        #endregion

        #region Validaciones
        private bool ValidarDNI(string dni)
        {
            string patron = @"^\d+$";
            return Regex.IsMatch(dni, patron);
        }
        private bool ValidarCorreo(string email)
        {
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, patron);
        }
        private bool ValidarCampos()
        {
            return !string.IsNullOrEmpty(txtDNI.Text) &&
                   !string.IsNullOrEmpty(txtApellidos.Text) &&
                   !string.IsNullOrEmpty(txtNombres.Text) &&
                   !string.IsNullOrEmpty(txtEmail.Text) &&
                   cmbRol.SelectedIndex != -1;
        }
        #endregion

        #region Grilla
        private void CargarGrilla()
        {
            dgvUsuarios.DataSource = null;
            var listaCompleta = _bll.GetAll();
            //var listaCompleta = UserBLL_08YS._usuariosLocal;

            // Aplicamos el filtro según el RadioButton seleccionado
            if (rbBloqueados.Checked)
            {
                // Solo los que tienen Bloqueado == true
                dgvUsuarios.DataSource = null;
                dgvUsuarios.DataSource = listaCompleta.Where(u => u.Bloqueado == true).ToList();
            }
            else if (rbActivos.Checked)
            {
                // Solo los que tienen Activo == true
                dgvUsuarios.DataSource = null;
                dgvUsuarios.DataSource = listaCompleta.Where(u => u.Activo == true).ToList();
            }
            else
            { 
                // Todos los usuarios
                dgvUsuarios.DataSource = null;
                dgvUsuarios.DataSource = listaCompleta;

            }

          
            int cantidadMostrada = dgvUsuarios.Rows.Count;
            string textoBase = TraductorManager_08YS.Instance.GetTexto(lblCantUsuarios.Tag.ToString());
            lblCantUsuarios.Text = $"{textoBase}{cantidadMostrada}";
            TraducirColumnas();
        }
      
      
        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (_estadoActual != EstadoUI.Consulta)
            {
                return;
            }

            if (dgvUsuarios.CurrentRow != null)
            {
         
                // Obtenemos el objeto vinculado a la fila
                var user = (User_08YS)dgvUsuarios.CurrentRow.DataBoundItem;

                txtDNI.Text = user.DNI.ToString();
                txtApellidos.Text = user.Apellido;
                txtNombres.Text = user.Nombre;
                txtEmail.Text = user.Email;
                if (user.Rol != null)
                {
                    cmbRol.SelectedValue = user.Rol.RolID;
                }
                txtLogin.Text = user.Username;
                txtBloqueado.Text = user.Bloqueado.ToString();
                txtActivo.Text = user.Activo.ToString();

                btnModificar.Enabled = true;
                btnDesbloquear.Enabled = user.Bloqueado;

                if (user.Activo)
                {
                  
                    btnActDes.Tag = "btnDesactivar";
                    btnActDes.BackColor = Color.LightCoral; // Un tono rojo suave
                    btnActDes.ForeColor = Color.White;      // Texto blanco para contraste
                }
                else
                {
                   
                    btnActDes.Tag = "btnActivar";
                    btnActDes.BackColor = Color.MediumSeaGreen; // Un tono verde suave
                    btnActDes.ForeColor = Color.White;
                }
                btnActDes.Text = TraductorManager_08YS.Instance.GetTexto(btnActDes.Tag.ToString());
                btnActDes.Enabled = true;
            }
        }
        #endregion

        #region Botones
        private void btnCrear_Click(object sender, EventArgs e)
        {
            CambiarEstado(EstadoUI.Insertando);
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            CambiarEstado(EstadoUI.Editando);
        }
        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_seleccionar_usuario"),
                                                TraductorManager_08YS.Instance.GetTexto("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var user = (User_08YS)dgvUsuarios.CurrentRow.DataBoundItem;

            // Usamos string.Format para incrustar el nombre dinámico dentro de la traducción
            string pregunta = string.Format(TraductorManager_08YS.Instance.GetTexto("msg_desbloquear_preg"), user.Nombre);
            DialogResult resp = MessageBox.Show(pregunta, TraductorManager_08YS.Instance.GetTexto("confirmar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                try
                {
                    _bll.DesbloquearUsuario(user.Username);

                    string exito = string.Format(TraductorManager_08YS.Instance.GetTexto("msg_desbloquear_exito"), user.Nombre);
                    MessageBox.Show(exito);
                    CargarGrilla();
                    CambiarEstado(EstadoUI.Consulta);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("error") + ": " + ex.Message);
                }
            }
        }
        
        private void btnActDes_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null) return;

            var user = (User_08YS)dgvUsuarios.CurrentRow.DataBoundItem;

            // Traducimos el verbo dinámico primero
            string accion = user.Activo
                ? TraductorManager_08YS.Instance.GetTexto("desactivar")
                : TraductorManager_08YS.Instance.GetTexto("activar");

            string pregunta = string.Format(TraductorManager_08YS.Instance.GetTexto("msg_alternar_activo_preg"), accion, user.Username);
            DialogResult resp = MessageBox.Show(pregunta, TraductorManager_08YS.Instance.GetTexto("confirmar_estado"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resp == DialogResult.Yes)
            {
                try
                {
                    _bll.AlternarEstado(user.Username);

                    string exito = string.Format(TraductorManager_08YS.Instance.GetTexto("msg_actualizado_exito"), user.Username);
                    MessageBox.Show(exito);

                    CargarGrilla();
                    CambiarEstado(EstadoUI.Consulta);
                }
                catch (UserAutoEstadoException_08YS)
                {
                    MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_error_auto_estado"),
                                    TraductorManager_08YS.Instance.GetTexto("operacion_invalida"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{TraductorManager_08YS.Instance.GetTexto("msg_error_inesperado")}{ex.Message}",
                                    TraductorManager_08YS.Instance.GetTexto("error_critico"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnAplicar_Click(object sender, EventArgs e)
        {
            var validaciones = new (bool condicion, string mensaje)[]
            {
                (!ValidarCampos(), TraductorManager_08YS.Instance.GetTexto("msg_completar_campos")),
                (!ValidarCorreo(txtEmail.Text), TraductorManager_08YS.Instance.GetTexto("msg_email_invalido")),
                (!ValidarDNI(txtDNI.Text), TraductorManager_08YS.Instance.GetTexto("msg_dni_invalido"))
            };

            var fallo = validaciones.FirstOrDefault(v => v.condicion);

            if (fallo.mensaje != null)
            {
                MessageBox.Show(fallo.mensaje, TraductorManager_08YS.Instance.GetTexto("error_validacion"));
                return;
            }

            try
            {
                if (_estadoActual == EstadoUI.Insertando) EjecutarAlta();
                else if (_estadoActual == EstadoUI.Editando) EjecutarModificacion();

                CargarGrilla();
                CambiarEstado(EstadoUI.Consulta);
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_operacion_exito"));
            }
            catch (UserDniDuplicadoException_08YS)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_user_dni_duplicado"),
                                TraductorManager_08YS.Instance.GetTexto("error_validacion"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (UserAutoModificacionException_08YS)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_user_auto_modificacion"),
                                TraductorManager_08YS.Instance.GetTexto("operacion_invalida"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{TraductorManager_08YS.Instance.GetTexto("msg_error_inesperado")}{ex.Message}",
                                TraductorManager_08YS.Instance.GetTexto("error_critico"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CambiarEstado(EstadoUI.Consulta);
        }
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void EjecutarAlta()
        {
            // 1. Obtener datos de los campos
            int dni = int.Parse(txtDNI.Text);
            string nombre = txtNombres.Text.Trim();
            string apellido = txtApellidos.Text.Trim();
            string email = txtEmail.Text.Trim();
            Rol_08YS rol = (Rol_08YS)cmbRol.SelectedItem;

            // 2. Enviar a la BLL
            _bll.CrearUsuario(dni,nombre,apellido,email,rol);
        }
        private void EjecutarModificacion()
        {
            string username = txtLogin.Text; // Usamos el login como ID
            string email = txtEmail.Text;
            Rol_08YS rol = (Rol_08YS)cmbRol.SelectedItem;

            // Llamada a la BLL
            _bll.ModificarUsuario(username, email, rol);
        }

        #region Filtros
        private void rbTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTodos.Checked)
            {
                CargarGrilla();
            }
        }

        private void rbBloqueados_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBloqueados.Checked)
            {
                CargarGrilla();
            }
        }
        private void rbActivos_CheckedChanged(object sender, EventArgs e)
        {
            if (rbActivos.Checked)
            {
                CargarGrilla();
            }
        }




        #endregion

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }
    }
}
