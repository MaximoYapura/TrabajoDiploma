using BE_08YS;
using BLL_08YS;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace GUI_08YS.RF1
{
    public class MesaVisualControl_790MY : Control
    {
        // ── Colors ────────────────────────────────────────────────────────
        private static readonly Color ClrRecomendada  = Color.FromArgb(39, 174, 96);
        private static readonly Color ClrDisponible   = Color.FromArgb(80, 140, 70);
        private static readonly Color ClrOcupada      = Color.FromArgb(120, 25, 25);
        private static readonly Color ClrIncompatible = Color.FromArgb(160, 155, 148);
        private static readonly Color ClrSeleccionada = Color.FromArgb(211, 84, 0);
        private static readonly Color ClrHover        = Color.FromArgb(230, 130, 20);
        private static readonly Color ClrTextoBlanco  = Color.FromArgb(255, 255, 255);
        private static readonly Color ClrTextoOscuro  = Color.FromArgb(45, 42, 40);

        // Sillas: tono madera oscura / caoba, en armonía con el piso del salón
        private static readonly Color ClrSillaFondo = Color.FromArgb(92, 44, 22);   // #5C2C16 caoba cálido
        private static readonly Color ClrSillaBorde = Color.FromArgb(50, 24, 10);   // borde sutil más oscuro

        // ── Constants ─────────────────────────────────────────────────────
        private const int Radio   = 10;
        private const int SillaW  = 14;
        private const int SillaH  = 9;
        private const int SillaGap = 5;
        public  const int MargenCtrl = 20;

        // ── Fields ────────────────────────────────────────────────────────
        private bool _hover;
        private bool _seleccionada;

        // ── Properties ────────────────────────────────────────────────────
        public MesaMapaDTO_790MY Datos { get; }

        public bool Seleccionada
        {
            get => _seleccionada;
            set { _seleccionada = value; Invalidate(); }
        }

        // ── Event ─────────────────────────────────────────────────────────
        public event EventHandler<MesaMapaDTO_790MY> MesaClick;

        // ── Dimensions helper ─────────────────────────────────────────────
        /// <summary>
        /// Devuelve (ancho, alto) en píxeles según la capacidad de la mesa.
        /// ≤2p → 70×70 (circular), 3-4p → 85×65, 5-8p → 120×65.
        /// </summary>
        public static (int ancho, int alto) GetDimensiones(int capacidad)
        {
            if (capacidad <= 2) return (70, 70);
            if (capacidad <= 4) return (85, 65);
            return (120, 65);
        }

        // ── Constructor ───────────────────────────────────────────────────
        public MesaVisualControl_790MY(MesaMapaDTO_790MY datos)
        {
            Datos = datos;
            var (ancho, alto) = GetDimensiones(datos.Mesa.Capacidad);
            Size   = new Size(ancho + MargenCtrl * 2, alto + MargenCtrl * 2);
            Cursor = datos.EsSeleccionable ? Cursors.Hand : Cursors.Default;

            // IMPORTANTE: habilitar SupportsTransparentBackColor y deshabilitar
            // Opaque ANTES de asignar BackColor = Transparent, o WinForms lanza
            // "El control no admite colores de fondo transparentes".
            SetStyle(
                ControlStyles.AllPaintingInWmPaint        |
                ControlStyles.UserPaint                   |
                ControlStyles.OptimizedDoubleBuffer        |
                ControlStyles.ResizeRedraw                 |
                ControlStyles.SupportsTransparentBackColor,
                true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();

            BackColor = Color.Transparent;   // proyecta la imagen de fondo del salón
        }

        // ── Mouse ─────────────────────────────────────────────────────────
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (Datos.EsSeleccionable) { _hover = true; Invalidate(); }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hover = false;
            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (Datos.EsSeleccionable)
                MesaClick?.Invoke(this, Datos);
        }

        // ── Transparent background ─────────────────────────────────────────
        /// <summary>
        /// Simula la transparencia dibujando la región correspondiente del
        /// BackgroundImage del panel padre antes de pintar el contenido propio.
        /// Esto garantiza que las sombras de las mesas proyecten sobre la madera.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Parent != null && Parent.BackgroundImage != null)
            {
                var g   = e.Graphics;
                var img = Parent.BackgroundImage;

                // Calcular la región del BackgroundImage (Stretch) que corresponde
                // a la posición de este control dentro del panel padre.
                float scaleX = (float)img.Width  / Math.Max(1, Parent.ClientSize.Width);
                float scaleY = (float)img.Height / Math.Max(1, Parent.ClientSize.Height);

                var srcRect = new RectangleF(
                    Left   * scaleX,
                    Top    * scaleY,
                    Width  * scaleX,
                    Height * scaleY);

                g.DrawImage(img, new Rectangle(0, 0, Width, Height), srcRect, GraphicsUnit.Pixel);
            }
            else
            {
                base.OnPaintBackground(e);
            }
        }

        // ── Painting ──────────────────────────────────────────────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            // No g.Clear() — el fondo ya fue pintado en OnPaintBackground

            var (ancho, alto) = GetDimensiones(Datos.Mesa.Capacidad);
            var tablero = new Rectangle(MargenCtrl, MargenCtrl, ancho, alto);

            // 1. Sillas (detrás de la mesa)
            DibujarSillas(g, tablero, Datos.Mesa.Capacidad);

            // 2. Color principal
            Color colorPrincipal = ObtenerColorPrincipal();

            // 3. Borde
            float grosorBorde = _seleccionada ? 3f : 1.5f;
            Color colorBorde  = _seleccionada
                ? ClrSeleccionada
                : _hover
                    ? ClrHover
                    : PintarMasOscuro(colorPrincipal, 0.18f);

            bool esCircular = Datos.EsCircular;

            // 4. Sombra
            var sombra = new Rectangle(tablero.X + 2, tablero.Y + 3, tablero.Width, tablero.Height);
            using (var brSombra = new SolidBrush(Color.FromArgb(45, 0, 0, 0)))
            {
                if (esCircular) g.FillEllipse(brSombra, sombra);
                else
                {
                    using (var pathSombra = ConstruirRoundedRect(sombra, Radio))
                        g.FillPath(brSombra, pathSombra);
                }
            }

            // 5. Relleno del tablero
            using (var brFill   = new SolidBrush(colorPrincipal))
            using (var penBorde = new Pen(colorBorde, grosorBorde))
            {
                if (esCircular)
                {
                    g.FillEllipse(brFill, tablero);
                    g.DrawEllipse(penBorde, tablero);
                }
                else
                {
                    using (var path = ConstruirRoundedRect(tablero, Radio))
                    {
                        g.FillPath(brFill, path);
                        g.DrawPath(penBorde, path);
                    }
                }
            }

            // 6. Borde interior si está seleccionada
            if (_seleccionada)
            {
                var inset = new Rectangle(tablero.X + 5, tablero.Y + 5,
                                          tablero.Width - 10, tablero.Height - 10);
                using (var penInner = new Pen(Color.FromArgb(160, ClrSeleccionada), 1.5f))
                {
                    if (esCircular) g.DrawEllipse(penInner, inset);
                    else
                    {
                        using (var pathInner = ConstruirRoundedRect(inset, Radio - 3))
                            g.DrawPath(penInner, pathInner);
                    }
                }
            }

            // 7. Número de mesa
            Color colorTexto = Datos.EstadoMapa == EstadoMapa_790MY.Incompatible
                ? ClrTextoOscuro : ClrTextoBlanco;

            using (var fntNumero = new Font("Segoe UI", 11f, FontStyle.Bold))
            {
                string txtNumero = "#" + Datos.Mesa.NroMesa;
                var sfCenter = new StringFormat
                {
                    Alignment     = StringAlignment.Center,
                    LineAlignment = StringAlignment.Near
                };
                int cy = tablero.Top + tablero.Height / 2 - 14;
                g.DrawString(txtNumero, fntNumero, new SolidBrush(colorTexto),
                             new RectangleF(tablero.X, cy, tablero.Width, 24), sfCenter);
            }

            // 8. Capacidad
            using (var fntCap = new Font("Segoe UI", 7.5f))
            {
                string txtCap = Datos.Mesa.Capacidad + "p";
                var sfCenter = new StringFormat
                {
                    Alignment     = StringAlignment.Center,
                    LineAlignment = StringAlignment.Near
                };
                Color clrCap = Color.FromArgb(200,
                    colorTexto.R, colorTexto.G, colorTexto.B);
                int cy = tablero.Top + tablero.Height / 2 + 2;
                g.DrawString(txtCap, fntCap, new SolidBrush(clrCap),
                             new RectangleF(tablero.X, cy, tablero.Width, 16), sfCenter);
            }

            // 9. Estrella si es recomendada
            if (Datos.EstadoMapa == EstadoMapa_790MY.DisponibleRecomendada)
                DibujarEstrella(g, tablero.Right - 14, tablero.Top + 5, 7f, Color.FromArgb(255, 220, 60));

            // 10. Candado si está ocupada
            if (Datos.EstadoMapa == EstadoMapa_790MY.Ocupada)
                DibujarCandado(g, tablero);
        }

        // ── Sillas ────────────────────────────────────────────────────────
        private void DibujarSillas(Graphics g, Rectangle tablero, int capacidad)
        {
            var (arr, ab, izq, der) = DistribuirPorLado(capacidad);
            DibujarSillasEnLado(g, tablero, arr, 0);
            DibujarSillasEnLado(g, tablero, ab,  1);
            DibujarSillasEnLado(g, tablero, izq, 2);
            DibujarSillasEnLado(g, tablero, der, 3);
        }

        private void DibujarSillasEnLado(Graphics g, Rectangle tablero, int cantidad, int lado)
        {
            if (cantidad == 0) return;

            for (int i = 0; i < cantidad; i++)
            {
                float t = (cantidad == 1) ? 0.5f
                    : 0.2f + 0.6f * (i / (cantidad - 1.0f));

                Rectangle r;
                switch (lado)
                {
                    case 0: // top
                        r = new Rectangle(
                            tablero.X + (int)(t * tablero.Width) - SillaW / 2,
                            tablero.Y - SillaH - SillaGap,
                            SillaW, SillaH);
                        break;
                    case 1: // bottom
                        r = new Rectangle(
                            tablero.X + (int)(t * tablero.Width) - SillaW / 2,
                            tablero.Bottom + SillaGap,
                            SillaW, SillaH);
                        break;
                    case 2: // left
                        r = new Rectangle(
                            tablero.X - SillaH - SillaGap,
                            tablero.Y + (int)(t * tablero.Height) - SillaW / 2,
                            SillaH, SillaW);
                        break;
                    default: // right
                        r = new Rectangle(
                            tablero.Right + SillaGap,
                            tablero.Y + (int)(t * tablero.Height) - SillaW / 2,
                            SillaH, SillaW);
                        break;
                }

                using (var path   = ConstruirRoundedRect(r, 3))
                using (var brFill = new SolidBrush(ClrSillaFondo))
                using (var penB   = new Pen(ClrSillaBorde, 1f))
                {
                    g.FillPath(brFill, path);
                    g.DrawPath(penB, path);
                }
            }
        }

        private static (int arr, int ab, int izq, int der) DistribuirPorLado(int cap)
        {
            switch (cap)
            {
                case 2:  return (1, 1, 0, 0);
                case 3:  return (1, 1, 1, 0);
                case 4:  return (1, 1, 1, 1);
                case 5:  return (2, 2, 1, 0);
                case 6:  return (2, 2, 1, 1);
                case 7:  return (2, 2, 2, 1);
                default: return (2, 2, 2, 2);
            }
        }

        // ── Decorations ───────────────────────────────────────────────────
        private static void DibujarEstrella(Graphics g, float cx, float cy, float r, Color color)
        {
            const int puntas = 5;
            float rInner = r * 0.42f;
            var pts = new PointF[puntas * 2];

            for (int i = 0; i < puntas * 2; i++)
            {
                float angle  = (float)(Math.PI / puntas * i) - (float)(Math.PI / 2);
                float radius = (i % 2 == 0) ? r : rInner;
                pts[i] = new PointF(
                    cx + radius * (float)Math.Cos(angle),
                    cy + radius * (float)Math.Sin(angle));
            }

            using (var br = new SolidBrush(color))
                g.FillPolygon(br, pts);
        }

        private static void DibujarCandado(Graphics g, Rectangle tablero)
        {
            float cx = tablero.Right - 13f;
            float cy = tablero.Top  + 13f;
            float r  = 11f;

            using (var brSombra = new SolidBrush(Color.FromArgb(45, 0, 0, 0)))
                g.FillEllipse(brSombra, cx - r + 1f, cy - r + 2f, r * 2f, r * 2f);

            using (var brBadge = new SolidBrush(Color.FromArgb(252, 250, 247)))
                g.FillEllipse(brBadge, cx - r, cy - r, r * 2f, r * 2f);

            using (var penBorder = new Pen(Color.FromArgb(120, 25, 25), 1.5f))
                g.DrawEllipse(penBorder, cx - r, cy - r, r * 2f, r * 2f);

            using (var penAsa = new Pen(Color.FromArgb(120, 25, 25), 2f))
                g.DrawArc(penAsa, cx - 4f, cy - 7f, 8f, 7f, 180, 180);

            var body = new RectangleF(cx - 5f, cy - 1.5f, 10f, 8f);
            using (var bodyPath = ConstruirRoundedRect(
                       new Rectangle((int)body.X, (int)body.Y, (int)body.Width, (int)body.Height), 2))
            using (var brBody = new SolidBrush(Color.FromArgb(120, 25, 25)))
                g.FillPath(brBody, bodyPath);

            using (var brKey = new SolidBrush(Color.FromArgb(252, 250, 247)))
            {
                g.FillEllipse(brKey, cx - 1.8f, cy + 0.5f, 3.6f, 3.2f);
                g.FillRectangle(brKey, cx - 1f, cy + 3f, 2f, 2.5f);
            }
        }

        // ── Helpers ───────────────────────────────────────────────────────
        private Color ObtenerColorPrincipal()
        {
            if (_seleccionada) return ClrSeleccionada;
            if (_hover)        return ClrHover;

            switch (Datos.EstadoMapa)
            {
                case EstadoMapa_790MY.DisponibleRecomendada: return ClrRecomendada;
                case EstadoMapa_790MY.Disponible:  return ClrDisponible;
                case EstadoMapa_790MY.Ocupada:     return ClrOcupada;
                default:                           return ClrIncompatible;
            }
        }

        private static Color PintarMasOscuro(Color c, float factor)
        {
            return Color.FromArgb(
                c.A,
                Math.Max(0, (int)(c.R * (1f - factor))),
                Math.Max(0, (int)(c.G * (1f - factor))),
                Math.Max(0, (int)(c.B * (1f - factor))));
        }

        internal static GraphicsPath ConstruirRoundedRect(Rectangle r, int radio)
        {
            int d = radio * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X,          r.Y,          d, d, 180, 90);
            path.AddArc(r.Right - d,  r.Y,          d, d, 270, 90);
            path.AddArc(r.Right - d,  r.Bottom - d, d, d,   0, 90);
            path.AddArc(r.X,          r.Bottom - d, d, d,  90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
