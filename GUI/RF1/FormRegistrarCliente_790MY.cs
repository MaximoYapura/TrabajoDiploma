using BE_08YS;
using BLL_08YS;
using BLL_08YS.Exceptions;
using GUI_08YS.Maestros;
using Service_08YS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using GUI_08YS.Estilo;

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

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
        }

        private void FormRegistrarCliente_790MY_Load(object sender, EventArgs e)
        {
            UpdateIdioma();
            ActualizarContadoresAdicionales();
            _altoSinBanner = Height;
            txtApellidos_790MY.Focus();
        }

        // Línea divisoria superior del pie, en el tono de borde de la paleta.
        private void pnlFooter_790MY_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(ColorBordeSuave))
                e.Graphics.DrawLine(pen, 0, 0, pnlFooter_790MY.Width, 0);
        }

        // Hover del botón principal (el Designer no expone color de hover).
        private void btnAplicar_790MY_MouseEnter(object sender, EventArgs e)
        {
            btnAplicar_790MY.BackColor = ColorBordoHover;
        }

        private void btnAplicar_790MY_MouseLeave(object sender, EventArgs e)
        {
            btnAplicar_790MY.BackColor = ColorBordo;
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

        #region Validación en línea (Etapa 2, mismas reglas que el Maestro de Clientes)

        // DNI de 7 u 8 dígitos; email y celular obligatorios (los exige ClienteBLL_790MY);
        // dirección opcional. La BLL sigue siendo la autoridad al guardar (E2-H3).
        private static readonly Regex _regexDni     = new Regex(@"^\d{7,8}$");
        private static readonly Regex _regexEmail   = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        private static readonly Regex _regexCelular = new Regex(@"^\d+$");

        // Un campo muestra su error recién al salir de él; el resumen del banner aparece
        // al intentar guardar y se va solo cuando ya no hay errores.
        private readonly HashSet<Control> _camposTocados = new HashSet<Control>();
        private bool _resumenVisible;
        private int _altoSinBanner;

        private IEnumerable<TextBox> CamposFicha()
        {
            yield return txtDni_790MY;
            yield return txtApellidos_790MY;
            yield return txtNombres_790MY;
            yield return txtEmail_790MY;
            yield return txtCelular_790MY;
            yield return txtDireccion_790MY;
        }

        private Dictionary<TextBox, string> ValidarFicha()
        {
            var t = TraductorManager_08YS.Instance;
            var errores = new Dictionary<TextBox, string>();

            string dni = txtDni_790MY.Text.Trim();
            if (!_regexDni.IsMatch(dni) || int.Parse(dni) <= 0)
                errores[txtDni_790MY] = t.GetTexto("RC_val_dni_precargado");

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

        private void Campo_TextChanged(object sender, EventArgs e)
        {
            // Un error devuelto por la BLL (DNI duplicado, etc.) se da por atendido al editar.
            if (!_resumenVisible) bnrAvisos_790MY.Ocultar();
            ActualizarValidacion();
        }

        private void Campo_Leave(object sender, EventArgs e)
        {
            _camposTocados.Add((Control)sender);
            ActualizarValidacion();
        }

        private void ActualizarValidacion()
        {
            Dictionary<TextBox, string> errores = ValidarFicha();
            foreach (TextBox caja in CamposFicha())
            {
                bool mostrar = errores.ContainsKey(caja) && _camposTocados.Contains(caja);
                errValidacion_790MY.SetError(caja, mostrar ? errores[caja] : string.Empty);
            }

            if (!_resumenVisible) return;
            if (errores.Count == 0)
            {
                _resumenVisible = false;
                bnrAvisos_790MY.Ocultar();
                return;
            }

            var renglones = errores.Select(par => "•  " + NombreCampo(par.Key) + ": " + par.Value);
            bnrAvisos_790MY.Mostrar(TraductorManager_08YS.Instance.GetTexto("FC_banner_revisar")
                + Environment.NewLine + string.Join(Environment.NewLine, renglones),
                TipoBanner_790MY.Error, cerrable: false);
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

        // El diálogo tiene tamaño fijo: crece mientras se ve el banner para no apretar los campos.
        private void bnrAvisos_790MY_TamanioCambiado(object sender, EventArgs e)
        {
            if (_altoSinBanner == 0) return;
            Height = _altoSinBanner + (bnrAvisos_790MY.Visible ? bnrAvisos_790MY.Height : 0);
        }

        private void MostrarErrorEnBanner(string mensaje)
        {
            _resumenVisible = false;
            bnrAvisos_790MY.Mostrar(mensaje);
        }

        #endregion

        private void btnAplicar_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;

            Dictionary<TextBox, string> errores = ValidarFicha();
            if (errores.Count > 0)
            {
                foreach (TextBox caja in CamposFicha()) _camposTocados.Add(caja);
                _resumenVisible = true;
                ActualizarValidacion();
                errores.Keys.First().Focus();
                return;
            }

            int dni = int.Parse(txtDni_790MY.Text.Trim());
            string apellido  = txtApellidos_790MY.Text.Trim();
            string nombre    = txtNombres_790MY.Text.Trim();
            string email     = txtEmail_790MY.Text.Trim();
            string celular   = txtCelular_790MY.Text.Trim();
            string direccion = txtDireccion_790MY.Text.Trim();

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
                // Exists() cuenta también a los clientes dados de baja.
                string mensaje = string.Format(t.GetTexto("FC_err_dni_duplicado"), dni);
                MostrarErrorEnBanner(mensaje);
                errValidacion_790MY.SetError(txtDni_790MY, mensaje);
            }
            catch (ArgumentException ex)
            {
                MostrarErrorEnBanner(string.Format(t.GetTexto("FC_err_bll_guardar"), ex.Message));
            }
            catch (Exception)
            {
                // Error inesperado (base de datos, excepción no prevista): MessageBox nativo (E2-D1).
                MessageBox.Show(this, t.GetTexto("FC_fatal_guardar"), t.GetTexto("error"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
