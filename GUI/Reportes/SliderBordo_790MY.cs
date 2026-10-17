using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI_08YS.Reportes
{
    /// <summary>
    /// Slider plano con la paleta de la casa (pista crema, tramo recorrido y perilla en
    /// bordó). Reemplaza al TrackBar del sistema, que no admite colores. Se maneja con
    /// el mouse (clic o arrastre) y con el teclado (flechas, RePág/AvPág, Inicio/Fin).
    /// </summary>
    public class SliderBordo_790MY : Control
    {
        private int _minimo;
        private int _maximo = 100;
        private int _valor = 40;
        private int _paso = 5;
        private bool _arrastrando;

        public SliderBordo_790MY()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable, true);
            Cursor = Cursors.Hand;
            Size = new Size(180, 24);
            TabStop = true;
        }

        public event EventHandler ValueChanged;

        public Color ColorPista { get; set; } = Color.FromArgb(236, 228, 218);
        public Color ColorRelleno { get; set; } = Color.FromArgb(107, 29, 47);

        [DefaultValue(0)]
        public int Minimum
        {
            get => _minimo;
            set { _minimo = value; Value = _valor; Invalidate(); }
        }

        [DefaultValue(100)]
        public int Maximum
        {
            get => _maximo;
            set { _maximo = Math.Max(_minimo, value); Value = _valor; Invalidate(); }
        }

        /// <summary>Incremento al que se ajusta el valor (mouse y teclado).</summary>
        [DefaultValue(5)]
        public int Step
        {
            get => _paso;
            set => _paso = Math.Max(1, value);
        }

        [DefaultValue(40)]
        public int Value
        {
            get => _valor;
            set
            {
                int nuevo = Math.Max(_minimo, Math.Min(_maximo, value));
                if (nuevo == _valor) return;
                _valor = nuevo;
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private const int RadioPerilla = 7;
        private const int AltoPista = 6;

        private float InicioPista => RadioPerilla + 1;
        private float AnchoPista => Math.Max(1, Width - 2 * (RadioPerilla + 1));

        private float PosicionDe(int valor)
            => InicioPista + (_maximo == _minimo ? 0 : AnchoPista * (valor - _minimo) / (float)(_maximo - _minimo));

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Parent?.BackColor ?? BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float cy = Height / 2f;
            float x = PosicionDe(_valor);
            var pista = new RectangleF(InicioPista, cy - AltoPista / 2f, AnchoPista, AltoPista);
            var recorrido = new RectangleF(InicioPista, pista.Y, Math.Max(0, x - InicioPista), AltoPista);

            using (var ruta = RutaRedondeada(pista))
            using (var pincel = new SolidBrush(ColorPista))
                g.FillPath(pincel, ruta);
            if (recorrido.Width > 0)
                using (var ruta = RutaRedondeada(recorrido))
                using (var pincel = new SolidBrush(ColorRelleno))
                    g.FillPath(pincel, ruta);

            var perilla = new RectangleF(x - RadioPerilla, cy - RadioPerilla, 2 * RadioPerilla, 2 * RadioPerilla);
            using (var pincel = new SolidBrush(ColorRelleno))
                g.FillEllipse(pincel, perilla);
            using (var borde = new Pen(Color.White, 2f))
                g.DrawEllipse(borde, perilla);
            if (Focused)
                using (var foco = new Pen(Color.FromArgb(90, ColorRelleno), 1f))
                    g.DrawEllipse(foco, perilla.X - 3, perilla.Y - 3, perilla.Width + 6, perilla.Height + 6);
        }

        private static GraphicsPath RutaRedondeada(RectangleF r)
        {
            var ruta = new GraphicsPath();
            float d = Math.Min(r.Height, r.Width);
            if (d <= 0) return ruta;
            ruta.AddArc(r.X, r.Y, d, d, 90, 180);
            ruta.AddArc(r.Right - d, r.Y, d, d, 270, 180);
            ruta.CloseFigure();
            return ruta;
        }

        private void AjustarA(int xPixel)
        {
            float proporcion = (xPixel - InicioPista) / AnchoPista;
            int crudo = _minimo + (int)Math.Round(proporcion * (_maximo - _minimo));
            Value = (int)(Math.Round(crudo / (double)_paso) * _paso);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            Focus();
            _arrastrando = true;
            AjustarA(e.X);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_arrastrando) AjustarA(e.X);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _arrastrando = false;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left: case Keys.Right: case Keys.Up: case Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Left: case Keys.Down: Value -= _paso; break;
                case Keys.Right: case Keys.Up: Value += _paso; break;
                case Keys.PageDown: Value -= _paso * 4; break;
                case Keys.PageUp: Value += _paso * 4; break;
                case Keys.Home: Value = _minimo; break;
                case Keys.End: Value = _maximo; break;
                default: return;
            }
            e.Handled = true;
        }

        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }
    }
}
