using System;
using System.Windows.Forms;
using Service_08YS;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Diálogo modal de confirmación con TemaFogon_790MY (Etapa 2, E2-D1): reemplaza al
    /// MessageBox nativo en bajas, eliminaciones y demás preguntas antes de una acción.
    /// </summary>
    public partial class ConfirmModal_790MY : Form
    {
        public ConfirmModal_790MY()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Pregunta antes de ejecutar <paramref name="textoAccion"/>. Devuelve true solo si el
        /// usuario eligió la acción; Esc, cerrar la ventana o Cancelar devuelven false.
        /// El foco arranca en Cancelar para que un Enter accidental no confirme una baja.
        /// </summary>
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
