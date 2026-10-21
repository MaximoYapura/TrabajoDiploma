using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Un dato de prueba de la Ficha de Ingredientes (Etapa 5, Fase 3): título, detalle,
    /// valor de solo lectura y botón «Copiar». El aviso (toast o banner) lo muestra la Ficha.
    /// </summary>
    public partial class ValorCopiable_790MY : UserControl
    {
        /// <summary>Resultado de pulsar «Copiar»: true si el portapapeles aceptó el valor.</summary>
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

        /// <summary>Si el portapapeles falla, el valor queda seleccionado para Ctrl+C.</summary>
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
