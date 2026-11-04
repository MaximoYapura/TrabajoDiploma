using System;
using System.Windows.Forms;
using Service_08YS;

namespace GUI_08YS.Estilo
{
    // Ventana de confirmación con los colores de la casa. Se usa en lugar del MessageBox
    // antes de bajas y otras acciones que no se pueden deshacer.
    public partial class ConfirmModal_790MY : Form
    {
        public ConfirmModal_790MY()
        {
            InitializeComponent();
        }

        // Pregunta antes de ejecutar textoAccion. Devuelve true solo si el
        // usuario eligió la acción; Esc, cerrar la ventana o Cancelar devuelven false.
        // El foco arranca en Cancelar para que un Enter accidental no confirme una baja.
        public static bool Confirmar(IWin32Window owner, string titulo, string mensaje,
                                     string textoAccion, string textoCancelar = null)
        {
            using (var dialogo = new ConfirmModal_790MY())
            {
                dialogo.Text = titulo;
                dialogo.lblTitulo_790MY.Text = titulo;
                dialogo.lblMensaje_790MY.Text = mensaje;
                dialogo.btnAccion_790MY.Text = textoAccion;
                dialogo.btnCancelar_790MY.Text = textoCancelar
                    ?? TraductorManager_08YS.Instance.GetTexto("btn_Cancelar");
                return dialogo.ShowDialog(owner) == DialogResult.OK;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            btnCancelar_790MY.Focus();
        }
    }
}
