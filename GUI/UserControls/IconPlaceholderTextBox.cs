using FontAwesome.Sharp;
using GUI_08YS.UserControls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
    /// <summary>Posición del ícono izquierdo respecto al texto.</summary>
    public enum IconTextBoxAlignment { Left, Right }

    [ToolboxItem(true)]
    [DefaultProperty(nameof(PlaceholderText))]
    [DefaultEvent("TextChanged")]
    public class IconPlaceholderTextBox : UserControl
    {
        // ──────────────────────────────────────────────────────────────────
        // Controles internos
        // ──────────────────────────────────────────────────────────────────
        private readonly PictureBox _iconPicture;       // ícono principal (izq o der según IconAlignment)
        private readonly PictureBox _iconPictureRight;  // ícono derecho independiente
        private readonly PlaceholderTextBox _innerTextBox;

        // ──────────────────────────────────────────────────────────────────
        // Campos — ícono principal
        // ──────────────────────────────────────────────────────────────────
        private IconChar _iconChar = IconChar.None;
        private IconFont _iconFont = IconFont.Auto;
        private Color _iconColor = Color.DimGray;
        private int _iconSize = 18;
        private IconTextBoxAlignment _iconAlign = IconTextBoxAlignment.Left;
        private int _iconPadding = 6;

        // ──────────────────────────────────────────────────────────────────
        // Campos — ícono derecho independiente
        // ──────────────────────────────────────────────────────────────────
        private IconChar _iconCharRight = IconChar.None;
        private IconFont _iconFontRight = IconFont.Auto;
        private Color _iconColorRight = Color.DimGray;
        private int _iconSizeRight = 18;
        private int _iconPaddingRight = 6;

        // ──────────────────────────────────────────────────────────────────
        // Campos — borde
        // ──────────────────────────────────────────────────────────────────
        private Color _borderColor = Color.FromArgb(180, 180, 180);
        private Color _borderFocusColor = Color.FromArgb(100, 149, 237);
        private int _borderWidth = 1;
        private int _cornerRadius = 0;
        private bool _isFocused = false;

        // ──────────────────────────────────────────────────────────────────
        // Campos — layout
        // ──────────────────────────────────────────────────────────────────
        private int _textLeftPadding = 4;

        // ──────────────────────────────────────────────────────────────────
        // Eventos
        // ──────────────────────────────────────────────────────────────────
        /// <summary>
        /// Se dispara cuando el usuario hace clic sobre el ícono principal.
        /// Si no hay suscriptores, el clic pasa el foco al TextBox.
        /// </summary>
        public event EventHandler IconClick;

        /// <summary>
        /// Se dispara cuando el usuario hace clic sobre el ícono derecho independiente.
        /// </summary>
        public event EventHandler IconRightClick;

        // ══════════════════════════════════════════════════════════════════
        // Constructor
        // ══════════════════════════════════════════════════════════════════
        public IconPlaceholderTextBox()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            // ── Ícono principal ───────────────────────────────────────────
            _iconPicture = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Visible = false
            };
            _iconPicture.Click += OnIconPictureClick;

            // ── Ícono derecho independiente ───────────────────────────────
            _iconPictureRight = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Visible = false
            };
            _iconPictureRight.Click += OnIconRightPictureClick;

            // ── TextBox interno ───────────────────────────────────────────
            _innerTextBox = new PlaceholderTextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = SystemColors.Window
            };

            _innerTextBox.GotFocus += (_, e) =>
            {
                _isFocused = true;
                _innerTextBox.Visible = true;
                Invalidate();
                OnGotFocus(e);
            };

            _innerTextBox.LostFocus += (_, e) =>
            {
                _isFocused = false;
                if (string.IsNullOrEmpty(_innerTextBox.RealText))
                    _innerTextBox.Visible = false;
                Invalidate();
                OnLostFocus(e);
            };

            _innerTextBox.TextChanged += (_, e) =>
            {
                _innerTextBox.Visible = !string.IsNullOrEmpty(_innerTextBox.RealText) || _innerTextBox.Focused;
                Invalidate();
                OnTextChanged(e);
            };

            _innerTextBox.KeyDown += (_, e) => OnKeyDown(e);
            _innerTextBox.KeyPress += (_, e) => OnKeyPress(e);
            _innerTextBox.KeyUp += (_, e) => OnKeyUp(e);

            Controls.Add(_innerTextBox);
            Controls.Add(_iconPicture);
            Controls.Add(_iconPictureRight);

            BackColor = SystemColors.Window;
            Size = new Size(250, 32);
            TabStop = true;

            UpdateLayout();
            _innerTextBox.Visible = false;
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            _innerTextBox.Font = this.Font; // ← sincronizar antes de medir
            RefreshIcon();
            RefreshIconRight();
            UpdateLayout();
            Invalidate();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (IsHandleCreated)
            {
                _innerTextBox.Font = this.Font; // ← sincronizar antes de medir
                RefreshIcon();
                RefreshIconRight();
                UpdateLayout();
                Invalidate();
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: ÍCONO PRINCIPAL ──────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Category("Icon")]
        [Description("Ícono principal de FontAwesome.Sharp.")]
        [DefaultValue(IconChar.None)]
        public IconChar IconChar
        {
            get => _iconChar;
            set { _iconChar = value; RefreshIcon(); UpdateLayout(); }
        }

        [Category("Icon")]
        [Description("Estilo FontAwesome del ícono principal.")]
        [DefaultValue(IconFont.Auto)]
        public IconFont IconFont
        {
            get => _iconFont;
            set { _iconFont = value; RefreshIcon(); }
        }

        [Category("Icon")]
        [Description("Color del ícono principal.")]
        public Color IconColor
        {
            get => _iconColor;
            set { _iconColor = value; RefreshIcon(); }
        }

        [Category("Icon")]
        [Description("Tamaño del ícono principal en píxeles.")]
        [DefaultValue(18)]
        public int IconSize
        {
            get => _iconSize;
            set { _iconSize = Math.Max(8, value); RefreshIcon(); UpdateLayout(); }
        }

        [Category("Icon")]
        [Description("Posición del ícono principal: Left o Right respecto al texto.")]
        [DefaultValue(IconTextBoxAlignment.Left)]
        public IconTextBoxAlignment IconAlignment
        {
            get => _iconAlign;
            set { _iconAlign = value; UpdateLayout(); Invalidate(); }
        }

        [Category("Icon")]
        [Description("Espacio horizontal en píxeles a cada lado del ícono principal.")]
        [DefaultValue(6)]
        public int IconPadding
        {
            get => _iconPadding;
            set { _iconPadding = Math.Max(0, value); UpdateLayout(); }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: ÍCONO DERECHO INDEPENDIENTE ──────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Category("Icon Right")]
        [Description("Ícono derecho independiente. Siempre se posiciona a la derecha del texto.")]
        [DefaultValue(IconChar.None)]
        public IconChar IconCharRight
        {
            get => _iconCharRight;
            set { _iconCharRight = value; RefreshIconRight(); UpdateLayout(); }
        }

        [Category("Icon Right")]
        [Description("Estilo FontAwesome del ícono derecho.")]
        [DefaultValue(IconFont.Auto)]
        public IconFont IconFontRight
        {
            get => _iconFontRight;
            set { _iconFontRight = value; RefreshIconRight(); }
        }

        [Category("Icon Right")]
        [Description("Color del ícono derecho.")]
        public Color IconColorRight
        {
            get => _iconColorRight;
            set { _iconColorRight = value; RefreshIconRight(); }
        }

        [Category("Icon Right")]
        [Description("Tamaño del ícono derecho en píxeles.")]
        [DefaultValue(18)]
        public int IconSizeRight
        {
            get => _iconSizeRight;
            set { _iconSizeRight = Math.Max(8, value); RefreshIconRight(); UpdateLayout(); }
        }

        [Category("Icon Right")]
        [Description("Espacio horizontal en píxeles a cada lado del ícono derecho.")]
        [DefaultValue(6)]
        public int IconPaddingRight
        {
            get => _iconPaddingRight;
            set { _iconPaddingRight = Math.Max(0, value); UpdateLayout(); }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: BORDE ────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Category("Border")]
        [Description("Color del borde sin foco.")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Border")]
        [Description("Color del borde con foco.")]
        public Color BorderFocusColor
        {
            get => _borderFocusColor;
            set { _borderFocusColor = value; Invalidate(); }
        }

        [Category("Border")]
        [Description("Grosor del borde en píxeles.")]
        [DefaultValue(1)]
        public int BorderWidth
        {
            get => _borderWidth;
            set { _borderWidth = Math.Max(1, value); UpdateLayout(); Invalidate(); }
        }

        [Category("Border")]
        [Description("Radio de las esquinas (0 = esquinas cuadradas).")]
        [DefaultValue(0)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(0, value); Invalidate(); }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: PLACEHOLDER ──────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Category("Placeholder")]
        [Description("Texto de ayuda cuando el campo está vacío.")]
        [DefaultValue("")]
        [Localizable(true)]
        public string PlaceholderText
        {
            get => _innerTextBox.PlaceholderText;
            set => _innerTextBox.PlaceholderText = value;
        }

        [Category("Placeholder")]
        [Description("Color del texto de ayuda.")]
        public Color PlaceholderColor
        {
            get => _innerTextBox.PlaceholderColor;
            set => _innerTextBox.PlaceholderColor = value;
        }

        [Category("Placeholder")]
        [Description("true → placeholder visible, escritura enmascarada.")]
        [DefaultValue(false)]
        public bool MaskedInput
        {
            get => _innerTextBox.MaskedInput;
            set => _innerTextBox.MaskedInput = value;
        }

        [Category("Placeholder")]
        [Description("Carácter de enmascaramiento cuando MaskedInput = true.")]
        [DefaultValue('●')]
        public char MaskChar
        {
            get => _innerTextBox.MaskChar;
            set => _innerTextBox.MaskChar = value;
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: TEXTBOX ──────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get => _innerTextBox.Text;
            set => _innerTextBox.Text = value;
        }

        [Browsable(false)]
        public string RealText => _innerTextBox.RealText;

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (_innerTextBox != null)
            {
                _innerTextBox.Font = this.Font;
                UpdateLayout();
                Invalidate();
            }
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            if (_innerTextBox != null)
                _innerTextBox.ForeColor = base.ForeColor;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            if (_innerTextBox != null)
                _innerTextBox.BackColor = base.BackColor;
            Invalidate();
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get => _innerTextBox.ReadOnly;
            set => _innerTextBox.ReadOnly = value;
        }

        [DefaultValue(32767)]
        public int MaxLength
        {
            get => _innerTextBox.MaxLength;
            set => _innerTextBox.MaxLength = value;
        }

        [DefaultValue(false)]
        public bool Multiline
        {
            get => _innerTextBox.Multiline;
            set { _innerTextBox.Multiline = value; UpdateLayout(); }
        }

        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get => _innerTextBox.TextAlign;
            set => _innerTextBox.TextAlign = value;
        }

        [DefaultValue(true)]
        public bool WordWrap
        {
            get => _innerTextBox.WordWrap;
            set => _innerTextBox.WordWrap = value;
        }

        public ScrollBars ScrollBars
        {
            get => _innerTextBox.ScrollBars;
            set => _innerTextBox.ScrollBars = value;
        }

        [Browsable(false)]
        public TextBox InnerTextBox => _innerTextBox;

        [Category("Appearance")]
        [Description("Padding adicional a la izquierda del texto cuando no hay ícono izquierdo.")]
        [DefaultValue(4)]
        public int TextLeftPadding
        {
            get => _textLeftPadding;
            set { _textLeftPadding = Math.Max(0, value); UpdateLayout(); Invalidate(); }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── FOCO ──────────────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        private void OnIconPictureClick(object sender, EventArgs e)
        {
            IconClick?.Invoke(this, e);
            if (IconClick == null)
                _innerTextBox.Focus();
        }

        private void OnIconRightPictureClick(object sender, EventArgs e)
        {
            IconRightClick?.Invoke(this, e);
        }

        protected override void OnEnter(EventArgs e)
        {
            _innerTextBox.Visible = true;
            _innerTextBox.Focus();
            base.OnEnter(e);
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PINTURA ───────────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var bgBrush = new SolidBrush(BackColor))
                g.FillRectangle(bgBrush, ClientRectangle);

            if (!_innerTextBox.Multiline
                && !_innerTextBox.Focused
                && string.IsNullOrEmpty(_innerTextBox.RealText)
                && !string.IsNullOrEmpty(_innerTextBox.PlaceholderText))
            {
                PaintPlaceholder(g);
            }

            Color border = _isFocused ? _borderFocusColor : _borderColor;
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (_cornerRadius > 0)
                PaintRoundedBorder(g, rect, border);
            else
                PaintSquareBorder(g, rect, border);
        }

        private void PaintPlaceholder(Graphics g)
        {
            int bw = _borderWidth;
            int padX = bw + 3;
            bool hasIconLeft = _iconChar != IconChar.None && _iconAlign == IconTextBoxAlignment.Left;
            bool hasIconRight = _iconCharRight != IconChar.None;
            bool hasIconMainRight = _iconChar != IconChar.None && _iconAlign == IconTextBoxAlignment.Right;

            int iconWLeft = hasIconLeft ? (_iconSize + _iconPadding * 2) : 0;
            int iconWRight = hasIconRight ? (_iconSizeRight + _iconPaddingRight * 2) : 0;
            // ✅ ancho del ícono principal cuando está a la derecha, sin importar
            // si también hay ícono derecho independiente — los dos deben sumar espacio
            int iconWMainRight = hasIconMainRight ? (_iconSize + _iconPadding * 2) : 0;

            int leftOffset = hasIconLeft ? iconWLeft : _textLeftPadding;
            int rightReserved = iconWRight + iconWMainRight;
            
            int textX = padX + leftOffset;
            int textW = Width - textX - rightReserved - padX;

            var rect = new Rectangle(textX, 0, Math.Max(0, textW), Height);
            var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left
                      | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding;

            TextRenderer.DrawText(g, _innerTextBox.PlaceholderText, this.Font,
                      rect, _innerTextBox.PlaceholderColor, flags);
        }

        private void PaintSquareBorder(Graphics g, Rectangle rect, Color color)
        {
            for (int i = 0; i < _borderWidth; i++)
                using (var pen = new Pen(color))
                    g.DrawRectangle(pen, rect.X + i, rect.Y + i,
                                    rect.Width - i * 2, rect.Height - i * 2);
        }

        private void PaintRoundedBorder(Graphics g, Rectangle rect, Color color)
        {
            float d = _cornerRadius * 2f;

            using (var path = new GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();

                using (var bgBrush = new SolidBrush(BackColor))
                    g.FillPath(bgBrush, path);

                for (int i = 0; i < _borderWidth; i++)
                    using (var pen = new Pen(color))
                        g.DrawPath(pen, path);
            }

            int bw = _borderWidth;
            float r2 = Math.Max(0, _cornerRadius - bw);
            float d2 = r2 * 2f;

            using (var clip = new GraphicsPath())
            {
                clip.AddArc(bw, bw, d2, d2, 180, 90);
                clip.AddArc(Width - bw - d2, bw, d2, d2, 270, 90);
                clip.AddArc(Width - bw - d2, Height - bw - d2, d2, d2, 0, 90);
                clip.AddArc(bw, Height - bw - d2, d2, d2, 90, 90);
                clip.CloseFigure();
                Region = new Region(clip);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── ÍCONOS: refresh ───────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        private void RefreshIcon()
        {
            if (_iconChar == IconChar.None)
            {
                _iconPicture.Visible = false;
                _iconPicture.Image = null;  // no Dispose — bitmap es del caché
                return;
            }
            _iconPicture.Image = IconCache.Get(_iconChar, _iconFont, _iconSize, _iconColor);
            _iconPicture.Visible = true;
        }

        private void RefreshIconRight()
        {
            if (_iconCharRight == IconChar.None)
            {
                _iconPictureRight.Visible = false;
                _iconPictureRight.Image = null;
                return;
            }
            _iconPictureRight.Image = IconCache.Get(_iconCharRight, _iconFontRight, _iconSizeRight, _iconColorRight);
            _iconPictureRight.Visible = true;
        }

        // ══════════════════════════════════════════════════════════════════
        // ── LAYOUT ────────────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateLayout();
            Invalidate();
        }

        private void UpdateLayout()
        {
            int bw = _borderWidth;
            int padX = bw + 3;
            int padY = bw + 1;

            bool hasIconMain = _iconChar != IconChar.None;
            bool hasIconRight = _iconCharRight != IconChar.None;
            bool iconMainIsLeft = hasIconMain && _iconAlign == IconTextBoxAlignment.Left;
            bool iconMainIsRight = hasIconMain && _iconAlign == IconTextBoxAlignment.Right;

            int iconWMain = hasIconMain ? (_iconSize + _iconPadding * 2) : 0;
            int iconWRight = hasIconRight ? (_iconSizeRight + _iconPaddingRight * 2) : 0;

            int availW = Width - padX * 2;
            int availH = Height - padY * 2;
            if (availW <= 0 || availH <= 0) return;

            // ── Altura y centrado del TextBox ──────────────────────────────────
            // PreferredHeight es el alto real que WinForms asigna al TextBox nativo
            // con su fuente actual. Es la única referencia fiable para centrar.
            int tbH = _innerTextBox.Multiline ? availH : _innerTextBox.PreferredHeight;
            int tbTop = _innerTextBox.Multiline ? padY : Math.Max(padY, (Height - tbH) / 2);

            // No forzar Height en modo single-line: WinForms lo gestiona solo
            // y forzarlo puede causar conflictos con PreferredHeight
            if (_innerTextBox.Multiline)
                _innerTextBox.Height = availH;

            // ── Ícono principal ────────────────────────────────────────────────
            if (hasIconMain)
            {
                _iconPicture.Size = new Size(iconWMain, Height);
                _iconPicture.Location = iconMainIsLeft
                    ? new Point(padX - 2, 0)
                    : new Point(Width - padX - iconWMain + 2, 0);
            }

            // ── Ícono derecho independiente ────────────────────────────────────
            if (hasIconRight)
            {
                _iconPictureRight.Size = new Size(iconWRight, Height);
                _iconPictureRight.Location = new Point(Width - padX - iconWRight + 2, 0);

                if (iconMainIsRight && hasIconMain)
                    _iconPicture.Location = new Point(Width - padX - iconWRight - iconWMain + 2, 0);
            }

            // ── TextBox ────────────────────────────────────────────────────────
            int leftStart = iconMainIsLeft ? (padX - 2 + iconWMain) : (padX + _textLeftPadding);
            int rightReserved = iconWRight + (iconMainIsRight ? iconWMain : 0);
            int tbW = Width - leftStart - rightReserved - padX;

            _innerTextBox.Location = new Point(leftStart, tbTop);
            _innerTextBox.Width = Math.Max(1, tbW);
        }

        // ══════════════════════════════════════════════════════════════════
        // Dispose
        // ══════════════════════════════════════════════════════════════════
        protected override void Dispose(bool disposing)
        {
            // Los bitmaps son del caché estático — no se liberan aquí
            base.Dispose(disposing);
        }
    }
}