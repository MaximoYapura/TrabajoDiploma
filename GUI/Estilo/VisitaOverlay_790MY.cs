using System.Drawing;
using System.Windows.Forms;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Capa de la Visita Guiada (Etapa 5, Fase 7, técnica (a) del spike B2): control opaco que
    /// pinta una captura de la ventana, la oscurece y deja un hueco nítido sobre el elemento
    /// que se explica. WinForms no compone transparencia sobre hermanos (C8), así que el
    /// «transparente» se simula dentro de un único control. Absorbe los clics.
    /// </summary>
    public class VisitaOverlay_790MY : Control
    {
        private static readonly Color Sombra = Color.FromArgb(170, 20, 10, 10);
        private const int Borde = 3;

        private Bitmap _captura;
        private Rectangle _hueco;

        public VisitaOverlay_790MY()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.Opaque, true);
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
        }

        /// <summary>La captura pasa a ser de este control (se libera al reemplazarla).</summary>
        public Bitmap Captura
        {
            get => _captura;
            set
            {
                if (ReferenceEquals(_captura, value)) return;
                _captura?.Dispose();
                _captura = value;
                Invalidate();
            }
        }

        public Rectangle Hueco
        {
            get => _hueco;
            set { _hueco = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            if (_captura != null) g.DrawImageUnscaled(_captura, 0, 0);
            else g.Clear(T.Crema);

            using (var region = new Region(ClientRectangle))
            using (var sombra = new SolidBrush(Sombra))
            {
                if (!_hueco.IsEmpty) region.Exclude(_hueco);
                g.FillRegion(sombra, region);
            }
            if (!_hueco.IsEmpty)
            {
                using (var lapiz = new Pen(T.Oro, Borde))
                    g.DrawRectangle(lapiz, Rectangle.Inflate(_hueco, Borde / 2 + 1, Borde / 2 + 1));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _captura?.Dispose(); _captura = null; }
            base.Dispose(disposing);
        }
    }
}
