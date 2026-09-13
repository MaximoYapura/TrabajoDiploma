using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CustomControls
{
    /// <summary>
    /// TextBox con soporte de:
    ///   • Placeholder personalizable (color, texto)
    ///   • MaskedInput: placeholder visible, escritura enmascarada
    ///   • BorderColor / BorderFocusColor: color de borde independiente del ForeColor
    /// </summary>
    [ToolboxItem(true)]
    [DefaultProperty(nameof(PlaceholderText))]
    [DefaultEvent("TextChanged")]
    public class PlaceholderTextBox : TextBox
    {
        // ──────────────────────────────────────────────────────────────────
        // Win32
        // ──────────────────────────────────────────────────────────────────
        private const int  EM_SETCUEBANNER = 0x1501;
        private const int  WM_PAINT        = 0x000F;
        private const int  WM_NCPAINT      = 0x0085;
        private const uint RDW_INVALIDATE  = 0x0001;
        private const uint RDW_FRAME       = 0x0400;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern bool SendMessage(IntPtr hWnd, int msg, bool wParam, string lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprc, IntPtr hrgn, uint flags);

        // ──────────────────────────────────────────────────────────────────
        // Campos — Placeholder
        // ──────────────────────────────────────────────────────────────────
        private string _placeholderText  = string.Empty;
        private Color  _placeholderColor = Color.Silver;
        private bool   _maskedInput      = false;
        private char   _maskChar         = '●';
        private bool   _showingPlaceholder = false;
        private Color  _realForeColor;

        // ──────────────────────────────────────────────────────────────────
        // Campos — Borde
        // ──────────────────────────────────────────────────────────────────
        private Color _borderColor      = Color.Empty;   // Empty = color del sistema
        private Color _borderFocusColor = Color.Empty;   // Empty = igual a BorderColor
        private bool  _isFocused        = false;

        // ──────────────────────────────────────────────────────────────────
        // Constructor
        // ──────────────────────────────────────────────────────────────────
        public PlaceholderTextBox()
        {
            _realForeColor = ForeColor;
        }

        // ══════════════════════════════════════════════════════════════════
        // PROPIEDADES — BORDE
        // ══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Color del borde del TextBox.
        /// Permite tener un color de borde distinto al ForeColor del texto,
        /// algo imposible con el TextBox estándar.
        /// Color.Empty = usar el color del sistema (comportamiento por defecto).
        /// </summary>
        [Category("Border")]
        [Description("Color del borde. Color.Empty = color del sistema por defecto.")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; InvalidateBorder(); }
        }

        /// <summary>
        /// Color del borde cuando el control tiene foco.
        /// Color.Empty = usar BorderColor también en foco.
        /// </summary>
        [Category("Border")]
        [Description("Color del borde con foco. Color.Empty = igual a BorderColor.")]
        public Color BorderFocusColor
        {
            get => _borderFocusColor;
            set { _borderFocusColor = value; InvalidateBorder(); }
        }

        // ══════════════════════════════════════════════════════════════════
        // PROPIEDADES — PLACEHOLDER
        // ══════════════════════════════════════════════════════════════════

        [Category("Placeholder")]
        [Description("Texto de ayuda que se muestra cuando el campo está vacío.")]
        [DefaultValue("")]
        [Localizable(true)]
        public string PlaceholderText
        {
            get => _placeholderText;
            set { _placeholderText = value ?? string.Empty; ApplyPlaceholder(); }
        }

        [Category("Placeholder")]
        [Description("Color del texto de ayuda.")]
        public Color PlaceholderColor
        {
            get => _placeholderColor;
            set
            {
                _placeholderColor = value;
                if (_showingPlaceholder) base.ForeColor = _placeholderColor;
                Invalidate();
            }
        }

        [Category("Placeholder")]
        [Description("Placeholder visible en claro; escritura enmascarada con MaskChar.")]
        [DefaultValue(false)]
        public bool MaskedInput
        {
            get => _maskedInput;
            set
            {
                _maskedInput = value;

                if (_maskedInput)
                {
                    // Activar modo oculto
                    if (!_showingPlaceholder)
                    {
                        base.PasswordChar = MaskChar;
                    }
                }
                else
                {
                    // Desactivar modo oculto
                    if (_showingPlaceholder)
                    {
                        _showingPlaceholder = false;
                        base.Text = string.Empty;
                        base.ForeColor = _realForeColor;
                    }

                    base.PasswordChar = '\0';
                }

                ApplyPlaceholder();
            }
        }

        [Category("Placeholder")]
        [Description("Carácter de enmascaramiento cuando MaskedInput = true.")]
        [DefaultValue('●')]
        public char MaskChar
        {
            get => _maskChar;
            set { _maskChar = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string RealText => _showingPlaceholder ? string.Empty : base.Text;

        // ══════════════════════════════════════════════════════════════════
        // PROPIEDADES — TEXT / FORECOLOR (sobrescritas)
        // ══════════════════════════════════════════════════════════════════

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get => _showingPlaceholder ? string.Empty : base.Text;
            set
            {
                if (_maskedInput)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        if (!Focused) InternalShowPlaceholder();
                        else { _showingPlaceholder = false; base.PasswordChar = _maskChar; base.ForeColor = _realForeColor; base.Text = string.Empty; }
                    }
                    else { InternalHidePlaceholder(); base.Text = value; }
                }
                else { base.Text = value; }
            }
        }

        public override Color ForeColor
        {
            get => base.ForeColor;
            set { _realForeColor = value; if (!_showingPlaceholder) base.ForeColor = value; }
        }

        // ══════════════════════════════════════════════════════════════════
        // CICLO DE VIDA
        // ══════════════════════════════════════════════════════════════════

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            // Desactivar EM_SETCUEBANNER: usamos pintura propia para evitar duplicados
            SendMessage(Handle, EM_SETCUEBANNER, true, string.Empty);
            ApplyPlaceholder();
        }

        // ══════════════════════════════════════════════════════════════════
        // FOCO — placeholder + borde de foco
        // ══════════════════════════════════════════════════════════════════

        protected override void OnGotFocus(EventArgs e)
        {
            _isFocused = true;
            InvalidateBorder();

            if (_maskedInput && _showingPlaceholder)
                InternalHidePlaceholder();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _isFocused = false;
            InvalidateBorder();

            if (_maskedInput && string.IsNullOrEmpty(base.Text))
                InternalShowPlaceholder();

            base.OnLostFocus(e);
        }

        // ══════════════════════════════════════════════════════════════════
        // WndProc — borde coloreado (WM_NCPAINT) + placeholder (WM_PAINT)
        // ══════════════════════════════════════════════════════════════════

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WM_NCPAINT:
                    PaintBorder();
                    break;

                case WM_PAINT:
                    // Solo dibujar placeholder si tiene borde propio
                    // Con BorderStyle.None lo maneja el UserControl padre
                    if (BorderStyle != BorderStyle.None
                        && !_maskedInput && !Focused
                        && string.IsNullOrEmpty(base.Text)
                        && !string.IsNullOrEmpty(_placeholderText))
                    {
                        DrawPlaceholder();
                    }
                    break;
            }
        }

        // ──────────────────────────────────────────────────────────────────
        // Pintura del borde (área no cliente)
        // ──────────────────────────────────────────────────────────────────

        private void PaintBorder()
        {
            // Determinar el color activo
            Color active = Color.Empty;

            if (_isFocused && _borderFocusColor != Color.Empty)
                active = _borderFocusColor;
            else if (_borderColor != Color.Empty)
                active = _borderColor;

            if (active == Color.Empty) return; // sin color personalizado → borde del sistema

            // GetWindowDC obtiene el DC del área completa (cliente + no-cliente)
            IntPtr hdc = GetWindowDC(Handle);
            if (hdc == IntPtr.Zero) return;
            try
            {
                using (var g   = Graphics.FromHdc(hdc))
                using (var pen = new Pen(active))
                    g.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }
            finally
            {
                ReleaseDC(Handle, hdc);
            }
        }

        /// <summary>Fuerza el repintado del área no cliente (borde).</summary>
        private void InvalidateBorder()
        {
            if (IsHandleCreated)
                RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_INVALIDATE | RDW_FRAME);
        }

        // ──────────────────────────────────────────────────────────────────
        // Pintura del placeholder (área cliente)
        //
        // Usa TextRenderer.DrawText con TextFormatFlags.VerticalCenter para
        // reproducir exactamente la misma posición vertical que usa Windows
        // para el texto del TextBox nativo. Graphics.DrawString no es suficiente
        // porque su sistema de métricas difiere del TextBox subyacente.
        // Las coordenadas son relativas al área CLIENTE (ClientSize).
        // ──────────────────────────────────────────────────────────────────

        private void DrawPlaceholder()
        {
            using (var g = Graphics.FromHwnd(Handle))
            {
                if (Multiline)
                {
                    var rect = new Rectangle(2, 2, ClientSize.Width - 3, ClientSize.Height - 3);
                    var flags = TextFormatFlags.Top | TextFormatFlags.Left
                              | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis;
                    TextRenderer.DrawText(g, _placeholderText, Font, rect, _placeholderColor, flags);
                }
                else
                {
                    Size textSize = TextRenderer.MeasureText(g, "Agpqyj|", Font,
                        new Size(int.MaxValue, int.MaxValue),
                        TextFormatFlags.NoPadding);

                    // ✅ Ignorar ClientSize — usar el alto medido directamente
                    // textY = 0 dibuja desde arriba del área cliente
                    // Si textSize > ClientSize, parte del texto queda fuera del clip
                    // Necesitamos deshabilitar el clip de la ventana
                    g.ResetClip();
                    g.SetClip(new Rectangle(0, -10, ClientSize.Width, textSize.Height + 10));

                    int textY = (ClientSize.Height - textSize.Height) / 2; // será negativo: OK

                    var rect = new Rectangle(1, textY, ClientSize.Width - 2, textSize.Height);
                    var flags = TextFormatFlags.Left | TextFormatFlags.NoPadding
                              | TextFormatFlags.EndEllipsis;

                    TextRenderer.DrawText(g, _placeholderText, Font, rect, _placeholderColor, flags);
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // LÓGICA INTERNA — MaskedInput
        // ══════════════════════════════════════════════════════════════════

        private void ApplyPlaceholder()
        {
            if (!IsHandleCreated) return;

            if (!_maskedInput)
                Invalidate();
            else if (string.IsNullOrEmpty(base.Text) && !Focused)
                InternalShowPlaceholder();
            else if (_showingPlaceholder)
                InternalShowPlaceholder();
        }

        private void InternalShowPlaceholder()
        {
            if (_showingPlaceholder
                && base.Text == _placeholderText
                && base.ForeColor == _placeholderColor)
                return;

            _showingPlaceholder = true;
            base.PasswordChar   = '\0';
            base.ForeColor      = _placeholderColor;
            base.Text           = _placeholderText;
            SelectionStart      = 0;
            SelectionLength     = 0;
        }

        private void InternalHidePlaceholder()
        {
            if (!_showingPlaceholder) return;
            _showingPlaceholder = false;
            base.ForeColor      = _realForeColor;
            base.PasswordChar   = _maskChar;
            base.Text           = string.Empty;
        }
    }
}
