using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GUI_08YS.Estilo
{
    // Un dato de la Ficha de ingredientes: título, detalle, el valor y el botón "Copiar".
    public partial class ValorCopiable_790MY : UserControl
    {
        // Resultado de pulsar «Copiar»: true si el portapapeles aceptó el valor.
        public event EventHandler<bool> CopiaIntentada;

        private string _textoCopiar = "Copiar";

        public ValorCopiable_790MY()
        {
            InitializeComponent();
        }

        public string Valor => txtValor_790MY.Text;

        public void Mostrar(string titulo, string detalle, string valor)
        {
            lblTitulo_790MY.Text = titulo ?? string.Empty;
            lblDetalle_790MY.Text = detalle ?? string.Empty;
            txtValor_790MY.Text = valor ?? string.Empty;
            txtValor_790MY.AccessibleName = lblTitulo_790MY.Text;
            ActualizarAccesible();
            Visible = true;
        }

        public void ActualizarTextos(string textoCopiar)
        {
            _textoCopiar = textoCopiar;
            ttCopiar_790MY.SetToolTip(btnCopiar_790MY, textoCopiar);
            ActualizarAccesible();
        }

        private void ActualizarAccesible()
            => btnCopiar_790MY.AccessibleName = $"{_textoCopiar}: {lblTitulo_790MY.Text} ({txtValor_790MY.Text})";

        // Si el portapapeles falla, el valor queda seleccionado para Ctrl+C.
        public void SeleccionarValor()
        {
            txtValor_790MY.Focus();
            txtValor_790MY.SelectAll();
        }

        private void btnCopiar_790MY_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtValor_790MY.Text)) return;
            bool ok;
            try
            {
                Clipboard.SetText(txtValor_790MY.Text);
                ok = true;
            }
            catch (ExternalException)
            {
                // Otro programa tiene el portapapeles abierto (CLIPBRD_E_CANT_OPEN).
                ok = false;
            }
            CopiaIntentada?.Invoke(this, ok);
        }
    }
}
