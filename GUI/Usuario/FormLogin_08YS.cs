using BLL_08YS;
using CustomControls;
using FontAwesome.Sharp;
using GUI_08YS.Admin;
using Service_08YS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_08YS
{
    public partial class FormLogin_08YS : Form,IIdiomaObserver_08YS
    {
        private UserBLL_08YS _userBLL;
        private readonly bool ModoRelogin;
        private string _userPlaceholder = "";
        private string _passPlaceholder = "";
        private bool _userTienePlaceholder = true;
        private bool _passTienePlaceholder = true;

        public FormLogin_08YS(bool modoRelogin = false)
        {
            InitializeComponent();
            ModoRelogin = modoRelogin;
            _userBLL = BLLFactory_08YS.CreateUserBLL();
            ConfigurarCombo();
            if (ModoRelogin)
                IdiomaCombobox.Visible = false;
            TraductorManager_08YS.Instance.Suscribir(this);

            AsignarEventosPlaceholder();

            // 2. Ejecutamos la carga inicial de traducciones
            UpdateIdioma();

        }

        #region Idioma
        private void ConfigurarCombo()
        {
            IdiomaCombobox.Items.Clear();

            // En lugar de hardcodear textos, manejamos el índice inicial por defecto (0 = Español)
            IdiomaCombobox.SelectedIndex = 0;
        }

        public void UpdateIdioma()
        {
            _userPlaceholder = TraductorManager_08YS.Instance.GetTexto("txtUsername_hint");
            _passPlaceholder = TraductorManager_08YS.Instance.GetTexto("txtPassword_hint");

            // Traducimos el resto de la interfaz (Labels, botones, etc.)
            TraducirControles(this);

            // Refrescamos los placeholders basándonos en la bandera de estado, no en el texto literal anterior
            RefrescarPlaceholderTraduccion(txtUsername, _userPlaceholder, _userTienePlaceholder, false);
            RefrescarPlaceholderTraduccion(txtPassword, _passPlaceholder, _passTienePlaceholder, true);

            ActualizarContenidoComboIdioma();
        }

        private void RefrescarPlaceholderTraduccion(IconPlaceholderTextBox txt, string placeholder, bool tienePlaceholder, bool esPassword)
        {
            // Si el control está actualmente en modo placeholder, actualizamos su texto al nuevo idioma de inmediato
            if (tienePlaceholder)
            {
                txt.Text = placeholder;
                txt.ForeColor = Color.DarkGray;
                if (esPassword) txt.MaskedInput = false;
            }
        }

        private void ActualizarContenidoComboIdioma()
        {
            // Salvamos el índice seleccionado actualmente para que no se resetee la vista al usuario
            int indexTemporal = IdiomaCombobox.SelectedIndex;

            // Desenganchamos temporalmente el evento para evitar ejecuciones cíclicas en cascada
            IdiomaCombobox.SelectedIndexChanged -= IdiomaComboBox_SelectedIndexChanged;

            IdiomaCombobox.Items.Clear();

            // Insertamos las traducciones dinámicas directo desde el archivo de traducción activo
            IdiomaCombobox.Items.Add(TraductorManager_08YS.Instance.GetTexto("idioma_es")); // "Español" o "Spanish"
            IdiomaCombobox.Items.Add(TraductorManager_08YS.Instance.GetTexto("idioma_en")); // "Inglés" o "English"

            // Restauramos la selección previa de manera segura
            if (indexTemporal >= 0)
            {
                IdiomaCombobox.SelectedIndex = indexTemporal;
            }
            else
            {
                // Fallback por si la carga inicial viene vacía
                IdiomaCombobox.SelectedIndex = 0;
            }

            // Volvemos a dar de alta el manejador de eventos del ComboBox
            IdiomaCombobox.SelectedIndexChanged += IdiomaComboBox_SelectedIndexChanged;
        }

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                // CONDICIÓN CRÍTICA: Si es un TextBox o tu control personalizado de texto, 
                // NO traduzcas su propiedad .Text, solo hazlo si es un Label, Button, GroupBox, etc.
                if (c is TextBox || c is IconPlaceholderTextBox)
                {
                    // Ignoramos la traducción del .Text directo para que no se rompa el input
                }
                else if (c.Tag != null && !string.IsNullOrWhiteSpace(c.Tag.ToString()))
                {
                    c.Text = TraductorManager_08YS.Instance.GetTexto(c.Tag.ToString());
                }

                if (c.HasChildren)
                {
                    TraducirControles(c);
                }
            }
        }
        #endregion

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            string usernameInput = _userTienePlaceholder ? "" : txtUsername.Text.Trim();
            string passwordInput = _passTienePlaceholder ? "" : txtPassword.Text.Trim();
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_completar_campos"),
                                TraductorManager_08YS.Instance.GetTexto("error_validacion"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            try
            {
                User_08YS user = _userBLL.ValidarLogin(username, password);

                if(!DVManager_08YS.VerificarConsistencia())
                {
                    if (_userBLL.ManejaInconsistencias(user))
                    {
                        var formDV = new FormInconsistenciaDB_08YS();
                        formDV.ShowDialog(this);
                    }
                    else MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_dv_inconsistente"),
                                         TraductorManager_08YS.Instance.GetTexto("error_critico"),
                                         MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                _userBLL.Login(user, out bool passwordDefault);

                if (passwordDefault)
                {
                    MessageBox.Show(
                        TraductorManager_08YS.Instance.GetTexto("msg_pwd_cambio_req"),
                        TraductorManager_08YS.Instance.GetTexto("pwd_cambio_req"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    FormCambiarContraseña_08YS formCambiarContraseña = new FormCambiarContraseña_08YS();
                    formCambiarContraseña.ShowDialog();

                    if (formCambiarContraseña.DialogResult == DialogResult.OK)
                    {
                        SessionManager_08YS.Instance.CerrarSesion();
                        ResetearCamposAForcePlaceholder();
                        MessageBox.Show(
                            TraductorManager_08YS.Instance.GetTexto("msg_sesion_reiniciada"),
                            TraductorManager_08YS.Instance.GetTexto("sesion_reiniciada"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        return;
                    }
                    else
                    {
                        SessionManager_08YS.Instance.CerrarSesion();
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(user.Idioma))
                    TraductorManager_08YS.Instance.CambiarIdioma(user.Idioma);

                FormMDI_08YS formMDI = new FormMDI_08YS();
                this.Hide();

                formMDI.CerrarSesion += () =>
                {
                    try
                    {
                        _userBLL.Logout();
                    }
                    catch (LogoutPersistenceException_08YS)
                    {
                        MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_logout_fallido"),
                                        TraductorManager_08YS.Instance.GetTexto("error_critico"),
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    this.Show();
                };

                formMDI.Show();
                formMDI.Activate();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_ya_hay_login"),
                                TraductorManager_08YS.Instance.GetTexto("error_sesion"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (AuthenticationException)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_error_credenciales"),
                                TraductorManager_08YS.Instance.GetTexto("error_autenticacion"),
                                MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            catch (UserBloqueadoException_08YS)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_user_bloqueado"),
                                TraductorManager_08YS.Instance.GetTexto("acceso_denegado"),
                                MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (UserNoRegistradoException_08YS)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_user_no_registrado"),
                                TraductorManager_08YS.Instance.GetTexto("usuario_invalido"),
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (UserDesactivadoException_08YS)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_user_desactivado"),
                                TraductorManager_08YS.Instance.GetTexto("usuario_suspendido"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                string msgBase = TraductorManager_08YS.Instance.GetTexto("msg_error_inesperado");
                MessageBox.Show($"{msgBase}{ex.Message}",
                                TraductorManager_08YS.Instance.GetTexto("error_critico"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResetearCamposAForcePlaceholder()
        {
            // Forzar el vaciado y re-inyección de Placeholders limpios al desloguearse o resetear
            _userTienePlaceholder = true;
            _passTienePlaceholder = true;
            UpdateIdioma();
        }

        #region BarraSuperior

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void button1_Click_1(object sender, EventArgs e)
        {
            if(ModoRelogin)
                this.Close();
            else Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        #endregion

        private void txtPassword_IconRightClick(object sender, EventArgs e)
        {
            if (txtPassword.Text != _passPlaceholder)
            {
                var tb = (IconPlaceholderTextBox)sender;
                tb.MaskedInput = !tb.MaskedInput;
                tb.IconCharRight = tb.MaskedInput ? IconChar.EyeSlash : IconChar.Eye;
            }
        }

        private void IdiomaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idiomaSeleccionado = IdiomaCombobox.SelectedIndex == 1 ? "en" : "es";

            // 1. Le avisamos al Manager para que muten todas las pantallas abiertas por el Observer
            TraductorManager_08YS.Instance.CambiarIdioma(idiomaSeleccionado);

            // 2. Si hay un usuario logueado en el SessionManager, impactamos su perfil en la BD
            if (SessionManager_08YS.Instance.IsLogged)
            {
                User_08YS usuarioActual = SessionManager_08YS.Instance.Current;
                _userBLL.CambiarIdiomaUsuario(idiomaSeleccionado);
            }
        }

        #region Lógica Efecto Placeholder Avanzada

        private void AsignarEventosPlaceholder()
        {
            // Eventos de Foco - Username
            txtUsername.Enter += (s, e) => {
                if (_userTienePlaceholder)
                {
                    txtUsername.Text = "";
                    txtUsername.ForeColor = Color.White;
                    _userTienePlaceholder = false;
                }
            };
            txtUsername.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    txtUsername.Text = _userPlaceholder;
                    txtUsername.ForeColor = Color.DarkGray;
                    _userTienePlaceholder = true;
                }
            };

            // Eventos de Foco - Password
            txtPassword.Enter += (s, e) => {
                if (_passTienePlaceholder)
                {
                    txtPassword.Text = "";
                    txtPassword.ForeColor = Color.White;
                    txtPassword.MaskedInput = true; // Activamos máscara al escribir
                    _passTienePlaceholder = false;
                }
            };
            txtPassword.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    txtPassword.Text = _passPlaceholder;
                    txtPassword.ForeColor = Color.DarkGray;
                    txtPassword.MaskedInput = false; // Desactivamos máscara para leer el hint
                    _passTienePlaceholder = true;
                }
            };
        }

        #endregion
    }
}
