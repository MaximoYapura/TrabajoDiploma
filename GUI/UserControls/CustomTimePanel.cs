using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
    // ══════════════════════════════════════════════════════════════════════
    //  CustomTimePanel
    //
    //  Selector de hora pintado a mano. Se usa como popup de
    //  IconDateTimePicker cuando Format = DateTimePickerFormat.Time,
    //  en lugar de mostrar un calendario que no tiene sentido en ese modo.
    //
    //  ESTRUCTURA VISUAL:
    //    ┌───────────────────────────┐
    //    │     Seleccionar hora      │  ← Header
    //    ├───────────────────────────┤
    //    │   ▲       ▲      (▲)      │
    //    │  HH   :  MM  : (SS)       │  ← Spinners (segundos opcional)
    //    │   ▼       ▼      (▼)      │
    //    ├───────────────────────────┤
    //    │   [ Ahora ]  [ Aceptar ]  │  ← Footer
    //    └───────────────────────────┘
    //
    //  • Click en ▲/▼ incrementa/decrementa con wrap (0-23 / 0-59)
    //  • Rueda del mouse sobre un segmento también incrementa/decrementa
    //  • "Ahora" carga la hora actual del sistema
    //  • "Aceptar" dispara TimeAccepted y cierra el popup (lo maneja el padre)
    // ══════════════════════════════════════════════════════════════════════

    [ToolboxItem(false)]
    public class CustomTimePanel : Control
    {
        // ──────────────────────────────────────────────────────────────────
        // Estado
        // ──────────────────────────────────────────────────────────────────
        private int _hour = 0;
        private int _minute = 0;
        private int _second = 0;
        private bool _showSeconds = true;   // ✅ ahora true por defecto

        private int _hoverSegment = -1;
        private bool _hoverUp = false;
        private bool _hoverDown = false;
        private bool _hoverNow = false;
        private bool _hoverAccept = false;

        // Segmento seleccionado para edición por teclado (0=H, 1=M, 2=S)
        private int _selectedSegment = 0;
        private string _editBuffer = string.Empty;

        // ──────────────────────────────────────────────────────────────────
        // Colores
        // ──────────────────────────────────────────────────────────────────
        private Color _titleBackColor = Color.FromArgb(0, 120, 215);
        private Color _titleForeColor = Color.White;
        private Color _separatorColor = Color.FromArgb(200, 200, 200);
        private Color _accentColor = Color.FromArgb(0, 120, 215);
        private Color _accentForeColor = Color.White;
        private Color _hoverBackColor = Color.FromArgb(220, 235, 252);
        private Color _selectedBorderColor = Color.FromArgb(0, 120, 215);

        // ──────────────────────────────────────────────────────────────────
        // Métricas
        // ──────────────────────────────────────────────────────────────────
        private int _headerH;
        private int _spinnerAreaH;
        private int _footerH;
        private int _colW;
        private int _arrowH;
        private int _numH;

        private Rectangle[] _upRects = new Rectangle[3];
        private Rectangle[] _downRects = new Rectangle[3];
        private Rectangle[] _numRects = new Rectangle[3];
        private Rectangle _nowBtn;
        private Rectangle _acceptBtn;

        // ══════════════════════════════════════════════════════════════════
        // Evento
        // ══════════════════════════════════════════════════════════════════
        public event EventHandler TimeAccepted;

        // ══════════════════════════════════════════════════════════════════
        // Constructor
        // ══════════════════════════════════════════════════════════════════
        public CustomTimePanel()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.Selectable, true);

            // ✅ FIX clicks rápidos: sin esto, Windows fusiona clicks
            // consecutivos en WM_LBUTTONDBLCLK y se "pierden" eventos.
            SetStyle(ControlStyles.StandardDoubleClick, false);

            TabStop = true;

            Font = new Font("Segoe UI", 9.5f);
            BackColor = SystemColors.Window;
            ForeColor = SystemColors.WindowText;

            var now = DateTime.Now;
            _hour = now.Hour;
            _minute = now.Minute;
            _second = now.Second;

            RecalcLayout();
            Size = PreferredSize;
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES PÚBLICAS ──────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        public int Hour => _hour;
        public int Minute => _minute;
        public int Second => _second;

        public bool ShowSeconds
        {
            get => _showSeconds;
            set { _showSeconds = value; RecalcLayout(); Size = PreferredSize; Invalidate(); }
        }

        public Color TitleBackColor { get => _titleBackColor; set { _titleBackColor = value; Invalidate(); } }
        public Color TitleForeColor { get => _titleForeColor; set { _titleForeColor = value; Invalidate(); } }
        public Color SeparatorColor { get => _separatorColor; set { _separatorColor = value; Invalidate(); } }
        public Color AccentColor { get => _accentColor; set { _accentColor = value; _selectedBorderColor = value; Invalidate(); } }
        public Color AccentForeColor { get => _accentForeColor; set { _accentForeColor = value; Invalidate(); } }
        public Color HoverBackColor { get => _hoverBackColor; set { _hoverBackColor = value; Invalidate(); } }

        public void SetTime(DateTime time)
        {
            _hour = time.Hour;
            _minute = time.Minute;
            _second = time.Second;
            _selectedSegment = 0;
            _editBuffer = string.Empty;
            Invalidate();
        }

        public override Size GetPreferredSize(Size proposedSize) => PreferredSize;

        public new Size PreferredSize
        {
            get
            {
                RecalcLayout();
                int cols = _showSeconds ? 3 : 2;
                int w = _colW * cols + 24;
                int h = _headerH + _spinnerAreaH + _footerH;
                return new Size(Math.Max(w, 220), h);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── LAYOUT ────────────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        private void RecalcLayout()
        {
            using (var g = Graphics.FromHwnd(Handle == IntPtr.Zero ? IntPtr.Zero : Handle))
            {
                SizeF sample = g.MeasureString("00", new Font(Font.FontFamily, Font.Size + 4, FontStyle.Bold));
                _numH = (int)Math.Ceiling(sample.Height) + 8;
                _colW = (int)Math.Ceiling(sample.Width) + 24;
            }

            _arrowH = 22;
            _headerH = 30;
            _spinnerAreaH = _arrowH * 2 + _numH + 8;
            _footerH = 38;

            int cols = _showSeconds ? 3 : 2;
            int totalW = Math.Max(_colW * cols + 24, 220);
            int colGap = 14;
            int startX = (totalW - (_colW * cols + colGap * (cols - 1))) / 2;

            int y = _headerH + 4;
            for (int i = 0; i < cols; i++)
            {
                int x = startX + i * (_colW + colGap);
                _upRects[i] = new Rectangle(x, y, _colW, _arrowH);
                _numRects[i] = new Rectangle(x, y + _arrowH, _colW, _numH);
                _downRects[i] = new Rectangle(x, y + _arrowH + _numH, _colW, _arrowH);
            }

            int footerY = _headerH + _spinnerAreaH;
            int btnW = (totalW - 24) / 2;
            int btnH = _footerH - 12;
            _nowBtn = new Rectangle(8, footerY + 6, btnW, btnH);
            _acceptBtn = new Rectangle(8 + btnW + 8, footerY + 6, btnW, btnH);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            RecalcLayout();
            Size = PreferredSize;
            Invalidate();
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PINTURA ───────────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            g.FillRectangle(new SolidBrush(BackColor), ClientRectangle);

            DrawHeader(g);

            using (var pen = new Pen(_separatorColor))
                g.DrawLine(pen, 0, _headerH, Width, _headerH);

            DrawSpinners(g);

            int footerY = _headerH + _spinnerAreaH;
            using (var pen = new Pen(_separatorColor))
                g.DrawLine(pen, 0, footerY, Width, footerY);

            DrawFooter(g);
        }

        private void DrawHeader(Graphics g)
        {
            g.FillRectangle(new SolidBrush(_titleBackColor), new Rectangle(0, 0, Width, _headerH));
            var rect = new Rectangle(0, 0, Width, _headerH);
            TextRenderer.DrawText(g, "Seleccionar hora", new Font(Font, FontStyle.Bold),
                                  rect, _titleForeColor,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawSpinners(Graphics g)
        {
            int cols = _showSeconds ? 3 : 2;
            int[] values = { _hour, _minute, _second };
            var bigFont = new Font(Font.FontFamily, Font.Size + 4, FontStyle.Bold);

            for (int i = 0; i < cols; i++)
            {
                bool upHover = _hoverSegment == i && _hoverUp;
                bool downHover = _hoverSegment == i && _hoverDown;

                DrawArrow(g, _upRects[i], true, upHover);
                DrawArrow(g, _downRects[i], false, downHover);

                // ✅ Resaltar el segmento seleccionado para edición por teclado
                bool isSelected = i == _selectedSegment && Focused;
                if (isSelected)
                {
                    using (var path = RoundedRect(Inset(_numRects[i], 1), 4))
                    {
                        Color fillColor = _editBuffer.Length > 0
                            ? Color.FromArgb(60, _selectedBorderColor)
                            : Color.FromArgb(25, _selectedBorderColor);
                        using (var br = new SolidBrush(fillColor))
                            g.FillPath(br, path);
                        using (var pen = new Pen(_selectedBorderColor, 1.5f))
                            g.DrawPath(pen, path);
                    }
                }

                TextRenderer.DrawText(g, values[i].ToString("D2"), bigFont,
                                      _numRects[i], ForeColor,
                                      TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            for (int i = 0; i < cols - 1; i++)
            {
                int xMid = (_upRects[i].Right + _upRects[i + 1].Left) / 2;
                var rect = new Rectangle(xMid - 8, _numRects[i].Y, 16, _numRects[i].Height);
                TextRenderer.DrawText(g, ":", new Font(Font, FontStyle.Bold), rect, ForeColor,
                                      TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private void DrawArrow(Graphics g, Rectangle rect, bool up, bool hover)
        {
            if (hover)
                using (var br = new SolidBrush(_hoverBackColor))
                    g.FillRectangle(br, rect);

            int cx = rect.X + rect.Width / 2;
            int cy = rect.Y + rect.Height / 2;
            int s = 5;

            Point[] pts = up
                ? new[] { new Point(cx - s, cy + 2), new Point(cx + s, cy + 2), new Point(cx, cy - s + 1) }
                : new[] { new Point(cx - s, cy - 2), new Point(cx + s, cy - 2), new Point(cx, cy + s - 1) };

            using (var brush = new SolidBrush(ForeColor))
                g.FillPolygon(brush, pts);
        }

        private void DrawFooter(Graphics g)
        {
            DrawButton(g, _nowBtn, "Ahora", _hoverNow, false);
            DrawButton(g, _acceptBtn, "Aceptar", _hoverAccept, true);
        }

        private void DrawButton(Graphics g, Rectangle rect, string text, bool hover, bool accent)
        {
            Color back = accent ? _accentColor : BackColor;
            Color fore = accent ? _accentForeColor : ForeColor;

            if (hover)
                back = accent ? ControlPaint.Light(_accentColor, 0.15f) : _hoverBackColor;

            using (var path = RoundedRect(rect, 4))
            {
                using (var br = new SolidBrush(back))
                    g.FillPath(br, path);
                if (!accent)
                    using (var pen = new Pen(_separatorColor))
                        g.DrawPath(pen, path);
            }

            TextRenderer.DrawText(g, text, Font, rect, fore,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // ══════════════════════════════════════════════════════════════════
        // ── INTERACCIÓN ───────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int newSeg = -1;
            bool newUp = false;
            bool newDown = false;
            int cols = _showSeconds ? 3 : 2;

            for (int i = 0; i < cols; i++)
            {
                if (_upRects[i].Contains(e.Location)) { newSeg = i; newUp = true; }
                if (_downRects[i].Contains(e.Location)) { newSeg = i; newDown = true; }
            }

            bool newNowHover = _nowBtn.Contains(e.Location);
            bool newAcceptHover = _acceptBtn.Contains(e.Location);

            if (newSeg != _hoverSegment || newUp != _hoverUp || newDown != _hoverDown
                || newNowHover != _hoverNow || newAcceptHover != _hoverAccept)
            {
                _hoverSegment = newSeg;
                _hoverUp = newUp;
                _hoverDown = newDown;
                _hoverNow = newNowHover;
                _hoverAccept = newAcceptHover;
                Cursor = (newSeg >= 0 || newNowHover || newAcceptHover) ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hoverSegment = -1;
            _hoverUp = _hoverDown = _hoverNow = _hoverAccept = false;
            Cursor = Cursors.Default;
            Invalidate();
        }

        // ✅ FIX puntos 2 y 3: toda la interacción se maneja en MouseDown.
        // Con StandardDoubleClick desactivado, cada click —por rápido que
        // sea— llega aquí como un evento independiente, sin perderse.
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;

            Focus();

            int cols = _showSeconds ? 3 : 2;

            for (int i = 0; i < cols; i++)
            {
                if (_upRects[i].Contains(e.Location))
                {
                    ChangeSegment(i, +1);
                    _selectedSegment = i;
                    _editBuffer = string.Empty;
                    return;
                }
                if (_downRects[i].Contains(e.Location))
                {
                    ChangeSegment(i, -1);
                    _selectedSegment = i;
                    _editBuffer = string.Empty;
                    return;
                }
                if (_numRects[i].Contains(e.Location))
                {
                    // ✅ Punto 3: seleccionar el segmento para edición por teclado
                    _selectedSegment = i;
                    _editBuffer = string.Empty;
                    Invalidate();
                    return;
                }
            }

            if (_nowBtn.Contains(e.Location))
            {
                var now = DateTime.Now;
                _hour = now.Hour; _minute = now.Minute; _second = now.Second;
                Invalidate();
                return;
            }

            if (_acceptBtn.Contains(e.Location))
            {
                TimeAccepted?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            int cols = _showSeconds ? 3 : 2;
            for (int i = 0; i < cols; i++)
            {
                if (_numRects[i].Contains(e.Location) ||
                    _upRects[i].Contains(e.Location) ||
                    _downRects[i].Contains(e.Location))
                {
                    ChangeSegment(i, e.Delta > 0 ? +1 : -1);
                    _selectedSegment = i;
                    _editBuffer = string.Empty;
                    return;
                }
            }
        }

        // ✅ Punto 3: navegación y edición por teclado
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            int cols = _showSeconds ? 3 : 2;

            switch (e.KeyCode)
            {
                case Keys.Return:
                    TimeAccepted?.Invoke(this, EventArgs.Empty);
                    e.Handled = true;
                    break;

                case Keys.Up:
                    ChangeSegment(_selectedSegment, +1);
                    _editBuffer = string.Empty;
                    e.Handled = true;
                    break;

                case Keys.Down:
                    ChangeSegment(_selectedSegment, -1);
                    _editBuffer = string.Empty;
                    e.Handled = true;
                    break;

                case Keys.Left:
                    _selectedSegment = Wrap(_selectedSegment - 1, 0, cols - 1);
                    _editBuffer = string.Empty;
                    Invalidate();
                    e.Handled = true;
                    break;

                case Keys.Right:
                    _selectedSegment = Wrap(_selectedSegment + 1, 0, cols - 1);
                    _editBuffer = string.Empty;
                    Invalidate();
                    e.Handled = true;
                    break;

                case Keys.Escape:
                    _editBuffer = string.Empty;
                    Invalidate();
                    e.Handled = true;
                    break;
            }
        }

        // ✅ Punto 3: escribir dígitos directamente sobre el segmento
        // seleccionado, con auto-avance al siguiente segmento — igual
        // que el comportamiento nativo del DateTimePicker de Windows.
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (!char.IsDigit(e.KeyChar)) { return; }

            int digit = e.KeyChar - '0';
            int max = SegmentMax(_selectedSegment);

            if (_editBuffer.Length == 0)
            {
                _editBuffer = digit.ToString();
                SetSegmentValue(_selectedSegment, digit);

                // Si ya no entra un segundo dígito sin pasarse del máximo,
                // confirmamos directamente y avanzamos (ej: hora "5" → no
                // puede ser "5X" porque 50+ > 23, así que pasa directo).
                if (digit * 10 > max)
                    AdvanceSegment();
            }
            else
            {
                int combined = int.Parse(_editBuffer) * 10 + digit;
                if (combined <= max)
                {
                    SetSegmentValue(_selectedSegment, combined);
                    AdvanceSegment();
                }
                else
                {
                    // El segundo dígito no entra: empieza un valor nuevo
                    _editBuffer = digit.ToString();
                    SetSegmentValue(_selectedSegment, digit);
                    if (digit * 10 > max)
                        AdvanceSegment();
                }
            }

            Invalidate();
            e.Handled = true;
        }

        // ══════════════════════════════════════════════════════════════════
        // ── HELPERS ───────────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        private int SegmentMax(int segment) => segment == 0 ? 23 : 59;

        private void SetSegmentValue(int segment, int value)
        {
            switch (segment)
            {
                case 0: _hour = value; break;
                case 1: _minute = value; break;
                case 2: _second = value; break;
            }
        }

        private void AdvanceSegment()
        {
            int cols = _showSeconds ? 3 : 2;
            _selectedSegment = (_selectedSegment + 1) % cols;
            _editBuffer = string.Empty;
        }

        private void ChangeSegment(int segment, int delta)
        {
            switch (segment)
            {
                case 0: _hour = Wrap(_hour + delta, 0, 23); break;
                case 1: _minute = Wrap(_minute + delta, 0, 59); break;
                case 2: _second = Wrap(_second + delta, 0, 59); break;
            }
            Invalidate();
        }

        private static int Wrap(int value, int min, int max)
        {
            int range = max - min + 1;
            if (value < min) value += range;
            if (value > max) value -= range;
            return value;
        }

        private static GraphicsPath RoundedRect(Rectangle rect, int radius)
        {
            float d = radius * 2f;
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static Rectangle Inset(Rectangle rect, int margin) =>
            new Rectangle(rect.X + margin, rect.Y + margin,
                          rect.Width - margin * 2, rect.Height - margin * 2);
    }
}