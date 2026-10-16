using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI_08YS.RF2
{
    /// <summary>
    /// Panel con esquinas redondeadas y borde configurable (sólido o punteado).
    /// Lo usan las tarjetas de RF2: reservas, camareros, productos, mesas y comandas.
    /// El relleno es BackColor; fuera de las esquinas se pinta el fondo del contenedor.
    /// </summary>
    public class PanelRedondeado_790MY : Panel
    {
        private int _radio = 10;
        private Color _colorBorde = Color.FromArgb(226, 216, 202);
        private float _grosorBorde = 1f;
        private DashStyle _estiloBorde = DashStyle.Solid;

        public PanelRedondeado_790MY()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        [DefaultValue(10)]
        public int Radio
        {
            get { return _radio; }
            set { _radio = value; Invalidate(); }
        }

        public Color ColorBorde
        {
            get { return _colorBorde; }
            set { _colorBorde = value; Invalidate(); }
        }

        [DefaultValue(1f)]
        public float GrosorBorde
        {
            get { return _grosorBorde; }
            set { _grosorBorde = value; Invalidate(); }
        }

        [DefaultValue(DashStyle.Solid)]
        public DashStyle EstiloBorde
        {
            get { return _estiloBorde; }
            set { _estiloBorde = value; Invalidate(); }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent?.BackColor ?? SystemColors.Control);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            float mitad = _grosorBorde / 2f;
            var rect = new RectangleF(mitad, mitad, Width - _grosorBorde - 1, Height - _grosorBorde - 1);

            using (GraphicsPath ruta = CrearRuta(rect, _radio))
            {
                using (var relleno = new SolidBrush(BackColor))
                    e.Graphics.FillPath(relleno, ruta);

                if (_grosorBorde > 0 && _colorBorde.A > 0)
                    using (var lapiz = new Pen(_colorBorde, _grosorBorde) { DashStyle = _estiloBorde })
                        e.Graphics.DrawPath(lapiz, ruta);
            }
        }

        internal static GraphicsPath CrearRuta(RectangleF r, int radio)
        {
            var ruta = new GraphicsPath();
            float d = System.Math.Max(1, System.Math.Min(radio * 2, System.Math.Min(r.Width, r.Height)));
            ruta.AddArc(r.X, r.Y, d, d, 180, 90);
            ruta.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            ruta.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            ruta.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            ruta.CloseFigure();
            return ruta;
        }
    }

    /// <summary>
    /// Etiqueta tipo "píldora" (fondo redondeado) para estados y contadores:
    /// "Disp. 9", "# 229 · Listo", "3 pendientes", etc.
    /// </summary>
    public class EtiquetaChip_790MY : Label
    {
        private Color _colorBorde = Color.Transparent;

        public EtiquetaChip_790MY()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            AutoSize = true;
            Padding = new Padding(8, 3, 8, 3);
            TextAlign = ContentAlignment.MiddleCenter;
        }

        public Color ColorBorde
        {
            get { return _colorBorde; }
            set { _colorBorde = value; Invalidate(); }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent?.BackColor ?? SystemColors.Control);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new RectangleF(0.5f, 0.5f, Width - 2, Height - 2);
            using (GraphicsPath ruta = PanelRedondeado_790MY.CrearRuta(rect, Height / 2))
            {
                using (var relleno = new SolidBrush(BackColor))
                    e.Graphics.FillPath(relleno, ruta);
                if (_colorBorde.A > 0)
                    using (var lapiz = new Pen(_colorBorde))
                        e.Graphics.DrawPath(lapiz, ruta);
            }
        }
    }
}
