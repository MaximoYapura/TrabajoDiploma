using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI_08YS.Estilo
{
    // Etiqueta con forma de tecla (borde redondeado). Se usa para el "F1" del botón de Ayuda.
    public class TeclaEtiqueta_790MY : Label
    {
        private const int Radio = 3;

        public TeclaEtiqueta_790MY()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var r = new Rectangle(0, 0, Width - 1, Height - 1);
            if (r.Width <= 2 * Radio || r.Height <= 2 * Radio) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var camino = new GraphicsPath())
            using (var lapiz = new Pen(ForeColor))
            {
                int d = Radio * 2;
                camino.AddArc(r.X, r.Y, d, d, 180, 90);
                camino.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                camino.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                camino.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                camino.CloseFigure();
                e.Graphics.DrawPath(lapiz, camino);
            }
        }
    }
}
