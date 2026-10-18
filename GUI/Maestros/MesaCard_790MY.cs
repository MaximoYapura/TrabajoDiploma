using BE_08YS;
using GUI_08YS.RF2;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Maestros
{
    /// <summary>
    /// Tarjeta de una mesa en el tablero del Maestro de Mesas (P11, P12, P14).
    /// Muestra solo datos estructurales (H12): número, capacidad y estado (Disponible o
    /// Fuera de servicio). El estado se lee por superficie suave, ícono y texto, nunca
    /// solo por color (P04).
    /// Control nuevo: no reemplaza ni modifica MesaVisualControl_790MY.
    /// </summary>
    public class MesaCard_790MY : Control
    {
        private Mesa_790MY _mesa;
        private bool _seleccionada;
        private bool _hover;
        private string _textoMesa = "Mesa";
        private string _textoEstado = string.Empty;
        private string _textoCapacidad = string.Empty;

        public MesaCard_790MY()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable |
                     ControlStyles.SupportsTransparentBackColor, true);
            Size = new Size(156, 136);
            Margin = new Padding(0, 0, T.Espacio3, T.Espacio3);
            Cursor = Cursors.Hand;
            TabStop = true;
            Font = new Font(T.Familia, 9F);
        }

        public Mesa_790MY Mesa
        {
            get { return _mesa; }
            set { _mesa = value; Invalidate(); }
        }

        public bool Seleccionada
        {
            get { return _seleccionada; }
            set { if (_seleccionada == value) return; _seleccionada = value; Invalidate(); }
        }

        /// <summary>Textos ya traducidos (los arma el formulario con idiomas.json).</summary>
        public void EstablecerTextos(string textoMesa, string textoEstado, string textoCapacidad)
        {
            _textoMesa = textoMesa ?? string.Empty;
            _textoEstado = textoEstado ?? string.Empty;
            _textoCapacidad = textoCapacidad ?? string.Empty;
            AccessibleName = $"{_textoMesa} {_mesa?.NroMesa} · {_textoEstado} · {_textoCapacidad}";
            Invalidate();
        }

        #region Interacción
        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }
        protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) Focus();
            base.OnMouseDown(e);
        }

        protected override bool IsInputKey(Keys keyData)
            => keyData == Keys.Space || keyData == Keys.Enter || base.IsInputKey(keyData);

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                OnClick(EventArgs.Empty);
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }
        #endregion

        #region Dibujo
        // El maestro distingue solo dos estados (decisión de Etapa 1): Fuera de servicio
        // o Disponible. Ocupada/Reservada son estados operativos de otros módulos y aquí
        // se muestran como Disponible.
        private bool FueraDeServicio => _mesa?.Estado == EstadoMesa_790MY.FueraDeServicio;

        private void ColoresEstado(out Color superficie, out Color borde, out Color tinta)
        {
            if (FueraDeServicio)
            {
                superficie = T.FueraFondo; borde = T.FueraBorde; tinta = T.FueraTexto;
            }
            else
            {
                superficie = T.OkSuave; borde = T.OkBorde; tinta = T.OkTexto;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Parent?.BackColor ?? T.Crema);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (_mesa == null) return;

            ColoresEstado(out Color superficie, out Color borde, out Color tinta);
            bool fuera = FueraDeServicio;

            var rect = new RectangleF(1.5f, 1.5f, Width - 4f, Height - 4f);
            using (GraphicsPath ruta = PanelRedondeado_790MY.CrearRuta(rect, T.RadioPanel))
            {
                using (var relleno = new SolidBrush(_seleccionada ? T.AccionSuave : superficie))
                    g.FillPath(relleno, ruta);

                Color colorBorde = _seleccionada ? T.Accion : (_hover && Enabled ? tinta : borde);
                float grosor = _seleccionada ? 2.5f : 1.5f;
                using (var lapiz = new Pen(colorBorde, grosor))
                {
                    if (fuera && !_seleccionada) lapiz.DashStyle = DashStyle.Dash;
                    g.DrawPath(lapiz, ruta);
                }
            }

            int x = T.Espacio3;
            int y = T.Espacio3 - 2;

            using (var fMesa = new Font(T.Familia, 8.25F, FontStyle.Bold))
            using (var fNumero = new Font(T.Familia, 22F, FontStyle.Bold))
            using (var fChip = new Font(T.Familia, 8.25F, FontStyle.Bold))
            using (var fCap = new Font(T.Familia, 8.25F, FontStyle.Bold))
            {
                TextRenderer.DrawText(g, _textoMesa, fMesa, new Point(x, y), T.TintaSuave, TextFormatFlags.NoPadding);
                y += TextRenderer.MeasureText(_textoMesa, fMesa, Size.Empty, TextFormatFlags.NoPadding).Height;

                string numero = _mesa.NroMesa.ToString();
                TextRenderer.DrawText(g, numero, fNumero, new Point(x - 2, y - 2), T.Tinta, TextFormatFlags.NoPadding);
                y += TextRenderer.MeasureText(numero, fNumero, Size.Empty, TextFormatFlags.NoPadding).Height + T.Espacio1;

                DibujarChipEstado(g, fChip, x, y, tinta);

                // Capacidad: asientos dibujados + texto, al pie de la tarjeta.
                Size medidaCap = TextRenderer.MeasureText(_textoCapacidad, fCap, Size.Empty, TextFormatFlags.NoPadding);
                int yCap = Height - T.Espacio3 - medidaCap.Height;
                TextRenderer.DrawText(g, _textoCapacidad, fCap, new Point(x, yCap), tinta, TextFormatFlags.NoPadding);
                DibujarAsientos(g, x, yCap - T.Espacio2 - 2, tinta);
            }

            if (_seleccionada) DibujarTilde(g);

            if (Focused && ShowFocusCues)
            {
                var rFoco = new RectangleF(4.5f, 4.5f, Width - 10f, Height - 10f);
                using (GraphicsPath ruta = PanelRedondeado_790MY.CrearRuta(rFoco, T.RadioTarjeta))
                using (var lapiz = new Pen(T.Foco, 1.5f) { DashStyle = DashStyle.Dot })
                    g.DrawPath(lapiz, ruta);
            }
        }

        private void DibujarChipEstado(Graphics g, Font fuente, int x, int y, Color tinta)
        {
            Size medida = TextRenderer.MeasureText(_textoEstado, fuente, Size.Empty, TextFormatFlags.NoPadding);
            int alto = Math.Max(20, medida.Height + 6);
            int icono = alto - 10;
            // El chip no pasa del ancho de la tarjeta: si el texto no entra, se corta con «…».
            int anchoMax = Width - 2 * x;
            int ancho = Math.Min(anchoMax, T.Espacio2 + icono + T.Espacio1 + medida.Width + T.Espacio2);
            var chip = new Rectangle(x, y, ancho, alto);

            Color fondoChip = FueraDeServicio ? Color.White : T.OkFondo;

            using (GraphicsPath ruta = PanelRedondeado_790MY.CrearRuta(chip, alto / 2))
            using (var relleno = new SolidBrush(fondoChip))
                g.FillPath(relleno, ruta);

            var rIcono = new Rectangle(chip.X + T.Espacio2, chip.Y + (alto - icono) / 2, icono, icono);
            DibujarIconoEstado(g, rIcono, tinta);

            var rTexto = new Rectangle(rIcono.Right + T.Espacio1, chip.Y, chip.Right - T.Espacio2 - rIcono.Right - T.Espacio1 + 2, alto);
            TextRenderer.DrawText(g, _textoEstado, fuente, rTexto, tinta,
                TextFormatFlags.NoPadding | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine);
        }

        private void DibujarIconoEstado(Graphics g, Rectangle r, Color color)
        {
            using (var lapiz = new Pen(color, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            {
                if (FueraDeServicio) // prohibido
                {
                    g.DrawEllipse(lapiz, r.X, r.Y, r.Width - 1, r.Height - 1);
                    g.DrawLine(lapiz, r.X + r.Width * 0.2f, r.Bottom - r.Height * 0.2f, r.Right - r.Width * 0.2f, r.Y + r.Height * 0.2f);
                }
                else // tilde
                {
                    g.DrawLines(lapiz, new[]
                    {
                        new PointF(r.X, r.Y + r.Height * 0.55f),
                        new PointF(r.X + r.Width * 0.38f, r.Bottom - r.Height * 0.1f),
                        new PointF(r.Right, r.Y + r.Height * 0.12f)
                    });
                }
            }
        }

        private void DibujarAsientos(Graphics g, int x, int y, Color color)
        {
            const int lado = 8, separacion = 3;
            int capacidad = Math.Max(0, _mesa.Capacidad);
            using (var relleno = new SolidBrush(Color.FromArgb(110, color)))
            {
                for (int i = 0; i < capacidad; i++)
                {
                    var r = new Rectangle(x + i * (lado + separacion), y, lado, lado);
                    if (r.Right > Width - T.Espacio3) break;
                    using (GraphicsPath ruta = PanelRedondeado_790MY.CrearRuta(r, 2))
                        g.FillPath(relleno, ruta);
                }
            }
        }

        private void DibujarTilde(Graphics g)
        {
            const int d = 20;
            var r = new Rectangle(Width - T.Espacio2 - d - 2, T.Espacio2 + 1, d, d);
            using (var relleno = new SolidBrush(T.Accion))
                g.FillEllipse(relleno, r);
            using (var lapiz = new Pen(Color.White, 2f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                g.DrawLines(lapiz, new[]
                {
                    new PointF(r.X + 5.5f, r.Y + 10.5f),
                    new PointF(r.X + 8.5f, r.Y + 13.5f),
                    new PointF(r.X + 14.5f, r.Y + 7f)
                });
        }
        #endregion
    }
}
