using BLL_08YS;
using CustomControls;
using FontAwesome.Sharp;
using Service_08YS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_08YS
{
    public partial class FormCambiarContraseña_08YS : Form,IIdiomaObserver_08YS
    {
        private UserBLL_08YS _userBLL;
        private bool _syncingPasswords = false;

        // Hint Strings de traducción correspondientes
        private string _hintActual = "";
        private string _hintNueva = "";
        private string _hintConfirmar = "";

        // BANDERAS DE ESTADO: Eliminan fallos de sincronización por comparación de strings literales
        private bool _actualTienePlaceholder = true;
        private bool _nuevaTienePlaceholder = true;
        private bool _confirmarTienePlaceholder = true;

        public FormCambiarContraseña_08YS()
        {
            InitializeComponent();
            _userBLL = BLLFactory_08YS.CreateUserBLL();

            txtContraseñaActual.IconClick += (s, e) =>
            {
                var tb = (IconPlaceholderTextBox)s;
                ApplyPasswordToggle(tb, !tb.MaskedInput);
            };

            txtNuevaContraseña.IconClick += OnLinkedPasswordToggle;
            txtConfirmarContraseña.IconClick += OnLinkedPasswordToggle;

            AsignarEventosPlaceholder();
            TraductorManager_08YS.Instance.Suscribir(this);
            UpdateIdioma();
        }

        public void UpdateIdioma()
        {
            // 1. Cargamos las traducciones vigentes desde el diccionario de recursos
            _hintActual = TraductorManager_08YS.Instance.GetTexto("txtPwdActual_hint");
            _hintNueva = TraductorManager_08YS.Instance.GetTexto("txtPwdNueva_hint");
            _hintConfirmar = TraductorManager_08YS.Instance.GetTexto("txtPwdConfirmar_hint");

            // 2. Traducimos etiquetas y botones usando Tags
            TraducirControles(this);

            // 3. Forzamos refresco visual de campos según su estado booleano
            RefrescarEstadoPlaceholder(txtContraseñaActual, _hintActual, _actualTienePlaceholder);
            RefrescarEstadoPlaceholder(txtNuevaContraseña, _hintNueva, _nuevaTienePlaceholder);
            RefrescarEstadoPlaceholder(txtConfirmarContraseña, _hintConfirmar, _confirmarTienePlaceholder);
        }

        private void RefrescarEstadoPlaceholder(IconPlaceholderTextBox txt, string placeholder, bool tienePlaceholder)
        {
            if (tienePlaceholder)
            {
                txt.Text = placeholder;
                txt.ForeColor = Color.DarkGray;
                txt.MaskedInput = false; // Desactivar máscara para que se lea el hint informativo
            }
        }

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                // Ignoramos la propiedad Text directa de los cuadros de texto
                if (c is TextBox || c is IconPlaceholderTextBox)
                {
                    continue;
                }

                if (c.Tag != null && !string.IsNullOrWhiteSpace(c.Tag.ToString()))
                {
                    c.Text = TraductorManager_08YS.Instance.GetTexto(c.Tag.ToString());
                }

                if (c.HasChildren)
                {
                    TraducirControles(c);
                }
            }
        }

        #region Lógica Controladora de Placeholders Basada en Estados

        private void AsignarEventosPlaceholder()
        {
            // Contraseña Actual
            txtContraseñaActual.Enter += (s, e) => {
                if (_actualTienePlaceholder)
                {
                    txtContraseñaActual.Text = "";
                    txtContraseñaActual.ForeColor = Color.White;
                    txtContraseñaActual.MaskedInput = true;
                    _actualTienePlaceholder = false;
                }
            };
            txtContraseñaActual.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtContraseñaActual.Text))
                {
                    txtContraseñaActual.Text = _hintActual;
                    txtContraseñaActual.ForeColor = Color.DarkGray;
                    txtContraseñaActual.MaskedInput = false;
                    _actualTienePlaceholder = true;
                }
            };

            // Nueva Contraseña
            txtNuevaContraseña.Enter += (s, e) => {
                if (_nuevaTienePlaceholder)
                {
                    txtNuevaContraseña.Text = "";
                    txtNuevaContraseña.ForeColor = Color.White;
                    txtNuevaContraseña.MaskedInput = true;
                    _nuevaTienePlaceholder = false;
                }
            };
            txtNuevaContraseña.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtNuevaContraseña.Text))
                {
                    txtNuevaContraseña.Text = _hintNueva;
                    txtNuevaContraseña.ForeColor = Color.DarkGray;
                    txtNuevaContraseña.MaskedInput = false;
                    _nuevaTienePlaceholder = true;
                }
            };

            // Confirmar Contraseña
            txtConfirmarContraseña.Enter += (s, e) => {
                if (_confirmarTienePlaceholder)
                {
                    txtConfirmarContraseña.Text = "";
                    txtConfirmarContraseña.ForeColor = Color.White;
                    txtConfirmarContraseña.MaskedInput = true;
                    _confirmarTienePlaceholder = false;
                }
            };
            txtConfirmarContraseña.Leave += (s, e) => {
                if (string.IsNullOrWhiteSpace(txtConfirmarContraseña.Text))
                {
                    txtConfirmarContraseña.Text = _hintConfirmar;
                    txtConfirmarContraseña.ForeColor = Color.DarkGray;
                    txtConfirmarContraseña.MaskedInput = false;
                    _confirmarTienePlaceholder = true;
                }
            };
        }

        #endregion
        private void OnLinkedPasswordToggle(object sender, EventArgs e)
        {
            if (_nuevaTienePlaceholder || _confirmarTienePlaceholder) return;
            if (_syncingPasswords) return;

            _syncingPasswords = true;
            try
            {
                bool nowMasked = !((IconPlaceholderTextBox)sender).MaskedInput;
                ApplyPasswordToggle(txtNuevaContraseña, nowMasked);
                ApplyPasswordToggle(txtConfirmarContraseña, nowMasked);
            }
            finally
            {
                _syncingPasswords = false;
            }
        }

        private static void ApplyPasswordToggle(IconPlaceholderTextBox tb, bool masked)
        {
            tb.MaskedInput = masked;
            tb.IconChar = masked ? IconChar.EyeSlash : IconChar.Eye;
        }
        private void btnCambiarContraseña_Click(object sender, EventArgs e)
        {
            // Extraemos los valores reales ingresados basándonos en la bandera de estado
            string passActualInput = _actualTienePlaceholder ? "" : txtContraseñaActual.Text;
            string passNuevaInput = _nuevaTienePlaceholder ? "" : txtNuevaContraseña.Text;
            string passConfirmarInput = _confirmarTienePlaceholder ? "" : txtConfirmarContraseña.Text;
            if (!ValidarCampos()) return;
            try
            {
                _userBLL.CambiarContraseña(txtContraseñaActual.Text, txtNuevaContraseña.Text);
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_pwd_changed"));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (PwdActualIncorrectaException_08YS)
            {
                // Buscamos la clave de traducción exacta para este error de negocio
                string mensajeTraducido = TraductorManager_08YS.Instance.GetTexto("msg_pwd_actual_incorrecta");
                string tituloError = TraductorManager_08YS.Instance.GetTexto("error_validacion");

                MessageBox.Show(mensajeTraducido, tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Error inesperado del sistema
                string msgInesperado = TraductorManager_08YS.Instance.GetTexto("msg_error_inesperado");
                string tituloError = TraductorManager_08YS.Instance.GetTexto("error");

                MessageBox.Show($"{msgInesperado}: {ex.Message}", tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtContraseñaActual.Text) || string.IsNullOrWhiteSpace(txtNuevaContraseña.Text) || string.IsNullOrWhiteSpace(txtConfirmarContraseña.Text))
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_completar_campos"));
                return false;
            }
            if (txtNuevaContraseña.Text != txtConfirmarContraseña.Text)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_pwd_no_coinciden"));
                return false;
            }
            if (txtNuevaContraseña.Text == txtContraseñaActual.Text)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_pwd_igual_actual"));
                return false;
            }
            return true;
        }

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
