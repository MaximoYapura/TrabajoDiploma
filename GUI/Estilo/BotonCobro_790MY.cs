using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Estilo
{
    public enum EstadoBotonCobro_790MY { Disponible, Deshabilitado, Procesando }

    /// <summary>
    /// Botón de la acción principal de cobro (Etapa 4, E4-D2): bordó sólido con un ícono de
    /// confirmación. En «Procesando» muestra un indicador circular que gira (GDI+, timer de
    /// ~40 ms que corre solo en ese estado), cambia el texto y no acepta clics ni teclas.
    /// Con las animaciones de Windows desactivadas el indicador queda estático.
    /// Deriva de Button: conserva foco, Enter/Espacio, Tag de idioma y el filtro de permisos.
    /// </summary>
    public partial class BotonCobro_790MY : Button
    {
        private const int PasoGiroGrados = 30;

        private EstadoBotonCobro_790MY _estado = EstadoBotonCobro_790MY.Disponible;
        private string _textoProcesando = string.Empty;
        private int _angulo;
        private bool _hover;
        private bool _presionado;

        public BotonCobro_790MY()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        /// <summary>Disponible (acepta clic), Deshabilitado (datos incompletos) o Procesando (cobro en curso).</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public EstadoBotonCobro_790MY Estado
        {
            get => _estado;
            set
            {
                if (_estado == value) return;
                _estado = value;
                Enabled = value != EstadoBotonCobro_790MY.Deshabilitado;
                Cursor = value == EstadoBotonCobro_790MY.Disponible ? Cursors.Hand : Cursors.Default;
                AccessibleDescription = value == EstadoBotonCobro_790MY.Procesando ? _textoProcesando : null;

                if (value == EstadoBotonCobro_790MY.Procesando && SystemInformation.UIEffectsEnabled)
                {
                    _angulo = 0;
                    tmrGiro_790MY.Start();
                }
                else
                {
                    tmrGiro_790MY.Stop();
                }
                Invalidate();
            }
        }

        /// <summary>Texto mientras el cobro está en curso (lo asigna el formulario desde idiomas.json).</summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TextoProcesando
        {
            get => _textoProcesando;
            set { _textoProcesando = value ?? string.Empty; if (_estado == EstadoBotonCobro_790MY.Procesando) Invalidate(); }
        }

        private bool Procesando => _estado == EstadoBotonCobro_790MY.Procesando;

        // Durante el cobro no se dispara Click: ni el mouse ni Enter/Espacio inician un segundo cobro.
        protected override void OnClick(EventArgs e)
        {
            if (Procesando) return;
            base.OnClick(e);
        }

        private void tmrGiro_790MY_Tick(object sender, EventArgs e)
        {
            _angulo = (_angulo + PasoGiroGrados) % 360;
            Invalidate();
        }

        #region Mouse

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hover = true; Invalidate(); }

        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hover = false; _presionado = false; Invalidate(); }

        protected override void OnMouseDown(MouseEventArgs e) { base.OnMouseDown(e); _presionado = e.Button == MouseButtons.Left; Invalidate(); }

        protected override void OnMouseUp(MouseEventArgs e) { base.OnMouseUp(e); _presionado = false; Invalidate(); }

        #endregion

        #region Dibujo

        private float Escala => DeviceDpi / 96f;

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Parent?.BackColor ?? T.Tarjeta);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = ClientRectangle;
            r.Width -= 1; r.Height -= 1;
            if (r.Width <= 0 || r.Height <= 0) return;

            bool activo = Enabled && !Procesando;
            Color fondo = !Enabled ? T.Deshabilitado
                        : Procesando ? T.AccionHover
                        : (_hover || _presionado) ? T.AccionHover : T.Accion;
            Color tinta = Enabled ? Color.White : T.TextoDeshabilitado;

            int radio = (int)Math.Round(T.RadioControl * Escala);
            using (GraphicsPath ruta = Redondeado(r, radio))
            using (var pincel = new SolidBrush(fondo))
                g.FillPath(pincel, ruta);

            string texto = (Procesando ? _textoProcesando : Text)?.Trim() ?? string.Empty;
            int lado = Math.Max(14, (int)Math.Round(Font.Height * 1.05f));
            int separacion = (int)Math.Round(10 * Escala);
            Size medida = TextRenderer.MeasureText(g, texto, Font, Size.Empty, TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine);
            int anchoMax = r.Width - lado - separacion - (int)(24 * Escala);
            int anchoTexto = Math.Max(0, Math.Min(medida.Width, anchoMax));
            int x = r.X + (r.Width - (lado + separacion + anchoTexto)) / 2;
            var glifo = new Rectangle(x, r.Y + (r.Height - lado) / 2, lado, lado);

            if (Procesando) DibujarSpinner(g, glifo, tinta);
            else DibujarCheck(g, glifo, tinta);

            var areaTexto = new Rectangle(glifo.Right + separacion, r.Y, anchoTexto, r.Height);
            TextRenderer.DrawText(g, texto, Font, areaTexto, tinta,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis |
                TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);

            // Foco visible (P10): anillo dorado interior, solo cuando el botón acepta la acción.
            if (Focused && ShowFocusCues && activo)
            {
                Rectangle foco = r;
                int margen = Math.Max(3, (int)Math.Round(3 * Escala));
                foco.Inflate(-margen, -margen);
                using (GraphicsPath ruta = Redondeado(foco, Math.Max(2, radio - margen)))
                using (var lapiz = new Pen(T.Oro, Math.Max(2f, 2f * Escala)))
                    g.DrawPath(lapiz, ruta);
            }
        }

        private void DibujarSpinner(Graphics g, Rectangle area, Color tinta)
        {
            float grosor = Math.Max(2.5f, area.Width / 7f);
            RectangleF circulo = area;
            circulo.Inflate(-grosor / 2f, -grosor / 2f);
            using (var pista = new Pen(Color.FromArgb(70, tinta), grosor))
            using (var arco = new Pen(tinta, grosor) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                g.DrawEllipse(pista, circulo);
                g.DrawArc(arco, circulo, _angulo - 90, 100);
            }
        }

        private static void DibujarCheck(Graphics g, Rectangle area, Color tinta)
        {
            float grosor = Math.Max(2.5f, area.Width / 6.5f);
            using (var lapiz = new Pen(tinta, grosor) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            {
                g.DrawLines(lapiz, new[]
                {
                    new PointF(area.X + area.Width * 0.12f, area.Y + area.Height * 0.55f),
                    new PointF(area.X + area.Width * 0.40f, area.Y + area.Height * 0.82f),
                    new PointF(area.X + area.Width * 0.90f, area.Y + area.Height * 0.22f)
                });
            }
        }

        private static GraphicsPath Redondeado(Rectangle r, int radio)
        {
            var ruta = new GraphicsPath();
            int d = Math.Max(1, Math.Min(radio * 2, Math.Min(r.Width, r.Height)));
            ruta.AddArc(r.X, r.Y, d, d, 180, 90);
            ruta.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            ruta.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            ruta.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            ruta.CloseFigure();
            return ruta;
        }

        #endregion
    }
}
