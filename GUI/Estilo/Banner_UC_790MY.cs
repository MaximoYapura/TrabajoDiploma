using System;
using System.Drawing;
using System.Windows.Forms;
using Service_08YS;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Estilo
{
    public enum TipoBanner_790MY { Error, Aviso, Info }

    /// <summary>
    /// Banner del formulario (Etapa 2, E2-D4): errores y avisos que permanecen en pantalla
    /// hasta que se corrigen o el usuario los cierra. Se acopla arriba (Dock = Top) y
    /// empuja el contenido hacia abajo, sin tapar controles.
    /// </summary>
    public partial class Banner_UC_790MY : UserControl
    {
        private Color _borde = T.Linea;

        public Banner_UC_790MY()
        {
            InitializeComponent();
        }

        /// <summary>Muestra <paramref name="mensaje"/>. Sin <paramref name="cerrable"/>, solo lo quita el código (al corregirse).</summary>
        public void Mostrar(string mensaje, TipoBanner_790MY tipo = TipoBanner_790MY.Error, bool cerrable = true)
        {
            Color fondo, texto;
            switch (tipo)
            {
                case TipoBanner_790MY.Aviso:
                    fondo = T.AvisoSuave; texto = T.AvisoTexto; _borde = T.AvisoBorde;
                    lblIcono_790MY.Text = "⚠";
                    break;
                case TipoBanner_790MY.Info:
                    fondo = T.Panel; texto = T.Tinta; _borde = T.Linea;
                    lblIcono_790MY.Text = "ⓘ";
                    break;
                default:
                    fondo = T.ErrorFondo; texto = T.ErrorTexto; _borde = T.ErrorTexto;
                    lblIcono_790MY.Text = "⚠";
                    break;
            }

            BackColor = fondo;
            tlpBanner_790MY.BackColor = fondo;
            lblIcono_790MY.ForeColor = texto;
            lblMensaje_790MY.ForeColor = texto;
            btnCerrar_790MY.ForeColor = texto;
            btnCerrar_790MY.BackColor = fondo;
            btnCerrar_790MY.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(fondo, 0.05f);
            btnCerrar_790MY.Visible = cerrable;
            btnCerrar_790MY.AccessibleName = TraductorManager_08YS.Instance.GetTexto("UI_cerrar_aviso");

            lblMensaje_790MY.Text = mensaje;
            AccessibleName = mensaje;
            // Sin BringToFront: cambiar el orden Z altera el acoplamiento y el banner
            // quedaría sin lugar debajo del panel con Dock = Fill.
            Visible = true;
            Invalidate();
        }

        public void Ocultar()
        {
            Visible = false;
            lblMensaje_790MY.Text = string.Empty;
        }

        public string Mensaje => lblMensaje_790MY.Text;

        private void btnCerrar_790MY_Click(object sender, EventArgs e)
        {
            Ocultar();
        }

        // El alto sigue al contenido (mensajes de varias líneas a cualquier escala de pantalla).
        private void tlpBanner_790MY_SizeChanged(object sender, EventArgs e)
        {
            int alto = tlpBanner_790MY.Height + Padding.Vertical;
            if (Height != alto) Height = alto;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var lapiz = new Pen(_borde))
                e.Graphics.DrawLine(lapiz, 0, Height - 1, Width, Height - 1);
        }
    }
}
