using BE_08YS;
using BLL_08YS;
using BLL_08YS.Exceptions;
using GUI_08YS.Maestros;
using Service_08YS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GUI_08YS.RF1
{
    /// <summary>
    /// Alta rápida de cliente (extend de Registrar Reserva). Diálogo modal compacto:
    /// se abre cuando el DNI buscado no existe, llega con el DNI precargado y, al
    /// aplicar, devuelve el cliente creado en <see cref="ClienteRegistrado"/>.
    /// </summary>
    public partial class FormRegistrarCliente_790MY : Form, IIdiomaObserver_08YS
    {
        private static readonly Color ColorBordo      = Color.FromArgb(120, 25, 25);
        private static readonly Color ColorBordoHover = Color.FromArgb(95, 18, 18);
        private static readonly Color ColorBordeSuave = Color.FromArgb(226, 216, 202);

        private readonly ClienteBLL_790MY _clienteBll;
        private List<string> _emailsAdicionales    = new List<string>();
        private List<string> _telefonosAdicionales = new List<string>();

        public Cliente_790MY ClienteRegistrado { get; private set; }

        public FormRegistrarCliente_790MY(int dniPrellenado)
        {
            InitializeComponent();
            _clienteBll = BLLFactory_790MY.CreateClienteBLL();

            txtDni_790MY.Text = dniPrellenado.ToString();

            // Hover del botón principal (el Designer no expone color de hover).
            btnAplicar_790MY.MouseEnter += (s, e) => btnAplicar_790MY.BackColor = ColorBordoHover;
            btnAplicar_790MY.MouseLeave += (s, e) => btnAplicar_790MY.BackColor = ColorBordo;

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
        }

        private void FormRegistrarCliente_790MY_Load(object sender, EventArgs e)
        {
            UpdateIdioma();
            ActualizarContadoresAdicionales();
            txtApellidos_790MY.Focus();
        }

        // Línea divisoria superior del pie, en el tono de borde de la paleta.
        private void pnlFooter_790MY_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(ColorBordeSuave))
                e.Graphics.DrawLine(pen, 0, 0, pnlFooter_790MY.Width, 0);
        }

        private void btnEmailsAdicionales_790MY_Click(object sender, EventArgs e)
        {
            using (var frm = new FormListaSimple_790MY("LS_titulo_emails", _emailsAdicionales))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    _emailsAdicionales = frm.Valores;
            }
            ActualizarContadoresAdicionales();
        }

        private void btnTelefonosAdicionales_790MY_Click(object sender, EventArgs e)
        {
            using (var frm = new FormListaSimple_790MY("LS_titulo_celulares", _telefonosAdicionales))
            {
                if (frm.ShowDialog(this) == DialogResult.OK)
                    _telefonosAdicionales = frm.Valores;
            }
            ActualizarContadoresAdicionales();
        }

        // Los botones "+" muestran cuántos valores adicionales hay cargados.
        private void ActualizarContadoresAdicionales()
        {
            btnEmailsAdicionales_790MY.Text    = _emailsAdicionales.Count    > 0 ? $"+{_emailsAdicionales.Count}"    : "+";
            btnTelefonosAdicionales_790MY.Text = _telefonosAdicionales.Count > 0 ? $"+{_telefonosAdicionales.Count}" : "+";
        }

        private void btnAplicar_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;

            if (!int.TryParse(txtDni_790MY.Text.Trim(), out int dni))
            {
                Avisar(t.GetTexto("msg_fc_dni_invalido"), txtDni_790MY);
                return;
            }

            string apellido = txtApellidos_790MY.Text.Trim();
            if (string.IsNullOrWhiteSpace(apellido))
            {
                Avisar(t.GetTexto("msg_fc_apellido_requerido"), txtApellidos_790MY);
                return;
            }

            string nombre = txtNombres_790MY.Text.Trim();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Avisar(t.GetTexto("msg_fc_nombre_requerido"), txtNombres_790MY);
                return;
            }

            string email = txtEmail_790MY.Text.Trim();
            if (!string.IsNullOrWhiteSpace(email) && !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Avisar(t.GetTexto("msg_fc_email_invalido"), txtEmail_790MY);
                return;
            }

            string celular = txtCelular_790MY.Text.Trim();
            if (!string.IsNullOrWhiteSpace(celular) && !Regex.IsMatch(celular, @"^\d+$"))
            {
                Avisar(t.GetTexto("msg_fc_celular_solo_numeros"), txtCelular_790MY);
                return;
            }

            string direccion = txtDireccion_790MY.Text.Trim();
            if (string.IsNullOrWhiteSpace(direccion))
            {
                Avisar(t.GetTexto("msg_fc_direccion_requerida"), txtDireccion_790MY);
                return;
            }

            try
            {
                _clienteBll.RegistrarCliente(dni, nombre, apellido, email, celular, direccion,
                                             _emailsAdicionales, _telefonosAdicionales);

                ClienteRegistrado = new Cliente_790MY(dni, nombre, apellido, email, celular, direccion)
                {
                    EmailsAdicionales    = _emailsAdicionales,
                    TelefonosAdicionales = _telefonosAdicionales
                };

                this.DialogResult = DialogResult.OK;
                this.Close();
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

        private void Avisar(string mensaje, Control foco)
        {
            MessageBox.Show(mensaje, TraductorManager_08YS.Instance.GetTexto("titulo_dato_invalido"),
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            foco.Focus();
        }

        private void btnCancelar_790MY_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #region i18n
        public void UpdateIdioma()
        {
            var t = TraductorManager_08YS.Instance;
            TraducirControles(this);

            this.Text = t.GetTexto("RC_titulo");
            tipAyuda_790MY.SetToolTip(btnEmailsAdicionales_790MY,    t.GetTexto("RC_tip_emails"));
            tipAyuda_790MY.SetToolTip(btnTelefonosAdicionales_790MY, t.GetTexto("RC_tip_telefonos"));
        }

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                // Las entradas del usuario no se traducen.
                if (c is TextBoxBase)
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
