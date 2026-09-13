using FontAwesome.Sharp;
using GUI_08YS.UserControls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
    // ══════════════════════════════════════════════════════════════════════
    //  IconDateTimePicker  (v2 — calendario pintado a mano)
    //
    //  Igual que la versión anterior pero usa CustomCalendarPanel en lugar
    //  de MonthCalendar, resolviendo el problema de colores en Windows 10/11
    //  con EnableVisualStyles activo.
    // ══════════════════════════════════════════════════════════════════════
    [ToolboxItem(true)]
    [DefaultProperty(nameof(Value))]
    [DefaultEvent("ValueChanged")]
    public class IconDateTimePicker : UserControl
    {
        private const int ArrowAreaWidth = 20;
        private const int CheckBoxAreaWidth = 20;

        // ──────────────────────────────────────────────────────────────────
        // Controles internos
        // ──────────────────────────────────────────────────────────────────
        private PictureBox _iconPicture;
        private CustomCalendarPanel _calendar;    // ← reemplaza MonthCalendar
        private CustomTimePanel _timePanel;
        private ToolStripDropDown _popup;
        private ToolStripDropDown _timePopup;

        // ──────────────────────────────────────────────────────────────────
        // Valor
        // ──────────────────────────────────────────────────────────────────
        private DateTime? _value = null;
        private DateTime _minDate = new DateTime(1753, 1, 1);
        private DateTime _maxDate = new DateTime(9998, 12, 31);
        private DateTimePickerFormat _format = DateTimePickerFormat.Short;
        private string _customFormat = "dd/MM/yyyy";

        // ──────────────────────────────────────────────────────────────────
        // Ícono
        // ──────────────────────────────────────────────────────────────────
        private IconChar _iconChar = IconChar.None;
        private IconFont _iconFont = IconFont.Auto;
        private Color _iconColor = Color.DimGray;
        private int _iconSize = 16;
        private IconTextBoxAlignment _iconAlign = IconTextBoxAlignment.Left;
        private int _iconPadding = 6;

        // ──────────────────────────────────────────────────────────────────
        // Borde
        // ──────────────────────────────────────────────────────────────────
        private Color _borderColor = Color.FromArgb(180, 180, 180);
        private Color _borderFocusColor = Color.FromArgb(100, 149, 237);
        private int _borderWidth = 1;
        private int _cornerRadius = 0;
        private bool _isFocused = false;
        private bool _calendarOpen = false;

        // ──────────────────────────────────────────────────────────────────
        // Checked
        // ──────────────────────────────────────────────────────────────────
        private bool _showCheckBox = false;
        private bool _checked = true;

        // ──────────────────────────────────────────────────────────────────
        // Colores del calendario — ahora sí funcionan porque pintamos a mano
        // ──────────────────────────────────────────────────────────────────
        private Color _calBackColor = SystemColors.Window;
        private Color _calForeColor = SystemColors.WindowText;
        private Color _calTitleBackColor = Color.FromArgb(0, 120, 215);
        private Color _calTitleForeColor = Color.White;
        private Color _calTrailingForeColor = Color.Silver;

        // ══════════════════════════════════════════════════════════════════
        // Eventos
        // ══════════════════════════════════════════════════════════════════
        public event EventHandler ValueChanged;
        public event EventHandler CheckedChanged;

        // ══════════════════════════════════════════════════════════════════
        // Constructor
        // ══════════════════════════════════════════════════════════════════
        public IconDateTimePicker()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            _iconPicture = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Visible = false
            };
            _iconPicture.Click += (s, e) => ToggleCalendar();
            Controls.Add(_iconPicture);

            BackColor = SystemColors.Window;
            ForeColor = SystemColors.WindowText;
            Size = new Size(220, 32);
            Cursor = Cursors.Hand;
            TabStop = true;

            RefreshIcon();
            UpdateLayout();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateLayout();
            UpdateRegion();
            Invalidate();
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: VALOR ────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Category("Data")]
        [Description("Fecha seleccionada. Null si no hay valor. En Format=Time, la fecha siempre se normaliza a hoy.")]
        public DateTime? Value
        {
            get
            {
                // En modo Time la fecha no importa: siempre se devuelve con la fecha de hoy,
                // así el consumidor del control nunca tiene que pensar en eso.
                if (_format == DateTimePickerFormat.Time && _value.HasValue)
                    return DateTime.Today.Add(_value.Value.TimeOfDay);
                return _value;
            }
            set
            {
                DateTime? incoming = value;

                // Si estamos en modo Time, descartamos cualquier fecha que venga
                // y nos quedamos solo con la hora, pegada a la fecha de hoy.
                if (_format == DateTimePickerFormat.Time && incoming.HasValue)
                    incoming = DateTime.Today.Add(incoming.Value.TimeOfDay);

                DateTime? clamped = incoming.HasValue
                    ? Clamp(incoming.Value, _minDate, _maxDate)
                    : (DateTime?)null;
                if (clamped == _value) return;
                _value = clamped;
                _checked = _value.HasValue;
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Category("Data")]
        public DateTime MinDate
        {
            get => _minDate;
            set
            {
                _minDate = value;
                if (_value.HasValue) _value = Clamp(_value.Value, _minDate, _maxDate);
                if (_calendar != null) _calendar.MinDate = _minDate;
                Invalidate();
            }
        }

        [Category("Data")]
        public DateTime MaxDate
        {
            get => _maxDate;
            set
            {
                _maxDate = value;
                if (_value.HasValue) _value = Clamp(_value.Value, _minDate, _maxDate);
                if (_calendar != null) _calendar.MaxDate = _maxDate;
                Invalidate();
            }
        }

        [Category("Data")]
        [DefaultValue(DateTimePickerFormat.Short)]
        public DateTimePickerFormat Format
        {
            get => _format;
            set
            {
                _format = value;

                // Si pasamos a modo Time y ya había un valor con una fecha real
                // cargada (ej. proviene de Format=Short), descartamos esa fecha
                // y dejamos solo la hora, pegada a hoy.
                if (_format == DateTimePickerFormat.Time && _value.HasValue)
                    _value = DateTime.Today.Add(_value.Value.TimeOfDay);

                Invalidate();
            }
        }

        [Category("Data")]
        [DefaultValue("dd/MM/yyyy")]
        public string CustomFormat
        {
            get => _customFormat;
            set { _customFormat = value ?? "dd/MM/yyyy"; Invalidate(); }
        }

        /// <summary>
        /// Solo la porción de hora/minuto/segundo del valor actual, sin fecha.
        /// Pensada para usar cuando Format = Time. Null si no hay valor.
        /// </summary>
        [Browsable(false)]
        public TimeSpan? TimeValue
        {
            get => Value?.TimeOfDay;
            set
            {
                if (!value.HasValue)
                {
                    Value = null;
                    return;
                }
                Value = DateTime.Today.Add(value.Value);
            }
        }

        /// <summary>Hora formateada como "HH:mm:ss". String.Empty si no hay valor.</summary>
        [Browsable(false)]
        public string TimeText => Value.HasValue ? Value.Value.ToString("HH:mm:ss") : string.Empty;

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: ÍCONO ────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Category("Icon")]
        [DefaultValue(IconChar.None)]
        public IconChar IconChar
        {
            get => _iconChar;
            set { _iconChar = value; RefreshIcon(); UpdateLayout(); Invalidate(); }
        }

        [Category("Icon")]
        [DefaultValue(IconFont.Auto)]
        public IconFont IconFont
        {
            get => _iconFont;
            set { _iconFont = value; RefreshIcon(); }
        }

        [Category("Icon")]
        public Color IconColor
        {
            get => _iconColor;
            set { _iconColor = value; RefreshIcon(); }
        }

        [Category("Icon")]
        [DefaultValue(16)]
        public int IconSize
        {
            get => _iconSize;
            set { _iconSize = Math.Max(8, value); RefreshIcon(); UpdateLayout(); Invalidate(); }
        }

        [Category("Icon")]
        [DefaultValue(IconTextBoxAlignment.Left)]
        public IconTextBoxAlignment IconAlignment
        {
            get => _iconAlign;
            set { _iconAlign = value; UpdateLayout(); Invalidate(); }
        }

        [Category("Icon")]
        [DefaultValue(6)]
        public int IconPadding
        {
            get => _iconPadding;
            set { _iconPadding = Math.Max(0, value); UpdateLayout(); Invalidate(); }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: BORDE ────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Category("Border")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Border")]
        public Color BorderFocusColor
        {
            get => _borderFocusColor;
            set { _borderFocusColor = value; Invalidate(); }
        }

        [Category("Border")]
        [DefaultValue(1)]
        public int BorderWidth
        {
            get => _borderWidth;
            set { _borderWidth = Math.Max(1, value); UpdateLayout(); Invalidate(); }
        }

        [Category("Border")]
        [DefaultValue(0)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(0, value); UpdateRegion(); Invalidate(); }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: CHECKED ──────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool ShowCheckBox
        {
            get => _showCheckBox;
            set { _showCheckBox = value; Invalidate(); }
        }

        [Category("Behavior")]
        [DefaultValue(true)]
        [Bindable(true)]
        public bool Checked
        {
            get => _value.HasValue;
            set
            {
                if (_checked == value) return;
                _checked = value;
                if (!value)
                    _value = null;
                else if (_value == null)
                    _value = DateTime.Now;
                Invalidate();
                CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: COLORES DEL CALENDARIO ───────────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Category("Calendar")]
        public Color CalendarBackColor
        {
            get => _calBackColor;
            set { _calBackColor = value; ApplyCalendarColors(); }
        }

        [Category("Calendar")]
        public Color CalendarForeColor
        {
            get => _calForeColor;
            set { _calForeColor = value; ApplyCalendarColors(); }
        }

        [Category("Calendar")]
        public Color CalendarTitleBackColor
        {
            get => _calTitleBackColor;
            set { _calTitleBackColor = value; ApplyCalendarColors(); }
        }

        [Category("Calendar")]
        public Color CalendarTitleForeColor
        {
            get => _calTitleForeColor;
            set { _calTitleForeColor = value; ApplyCalendarColors(); }
        }

        [Category("Calendar")]
        public Color CalendarTrailingForeColor
        {
            get => _calTrailingForeColor;
            set { _calTrailingForeColor = value; ApplyCalendarColors(); }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PINTURA ───────────────────────────════════════════════════════
        // ══════════════════════════════════════════════════════════════════

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var bg = new SolidBrush(BackColor))
                g.FillRectangle(bg, ClientRectangle);

            if (_showCheckBox)
                PaintCheckBox(g);

            PaintDateText(g);
            PaintArrow(g);

            Color borderColor = (_isFocused || _calendarOpen) ? _borderFocusColor : _borderColor;
            var borderRect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (_cornerRadius > 0)
                PaintRoundedBorder(g, borderRect, borderColor);
            else
                PaintSquareBorder(g, borderRect, borderColor);
        }

        private void PaintCheckBox(Graphics g)
        {
            int size = 13;
            int x = _borderWidth + 4;
            int y = (Height - size) / 2;
            var rect = new Rectangle(x, y, size, size);

            using (var pen = new Pen(_borderColor))
                g.DrawRectangle(pen, rect);

            if (_checked)
            {
                using (var pen = new Pen(ForeColor, 2f) { LineJoin = LineJoin.Round })
                {
                    g.DrawLines(pen, new[]
                    {
                        new Point(x + 2,  y + 6),
                        new Point(x + 5,  y + 10),
                        new Point(x + 11, y + 3)
                    });
                }
            }
        }

        private void PaintDateText(Graphics g)
        {
            int padX = _borderWidth + 3;
            int iconW = _iconChar != IconChar.None ? (_iconSize + _iconPadding * 2) : 0;
            int checkW = _showCheckBox ? CheckBoxAreaWidth : 0;
            int leftOffset = (_iconAlign == IconTextBoxAlignment.Left) ? iconW : 0;
            int rightOffset = (_iconAlign == IconTextBoxAlignment.Right) ? iconW : 0;

            int textX = padX + checkW + leftOffset;
            int textW = Width - textX - ArrowAreaWidth - rightOffset - padX;

            Color textColor = Checked ? ForeColor : Color.Gray;
            var rect = new Rectangle(textX, 0, textW, Height);
            var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left
                      | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding;

            TextRenderer.DrawText(g, GetDisplayText(), Font, rect, textColor, flags);
        }

        private void PaintArrow(Graphics g)
        {
            int arrowX = Width - ArrowAreaWidth - _borderWidth;
            Color sepColor = (_isFocused || _calendarOpen) ? _borderFocusColor : _borderColor;

            using (var pen = new Pen(sepColor))
                g.DrawLine(pen, arrowX, _borderWidth + 2, arrowX, Height - _borderWidth - 3);

            int cx = arrowX + ArrowAreaWidth / 2;
            int cy = Height / 2;
            int s = 4;
            var pts = new[] {
                new Point(cx - s, cy - 2),
                new Point(cx + s, cy - 2),
                new Point(cx,     cy + s - 1)
            };
            using (var brush = new SolidBrush(ForeColor))
                g.FillPolygon(brush, pts);
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

                using (var pen = new Pen(color, _borderWidth))
                {
                    pen.Alignment = PenAlignment.Inset;
                    g.DrawPath(pen, path);
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── INTERACCIÓN ───────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (_showCheckBox && e is MouseEventArgs me)
            {
                int checkX = _borderWidth + 4;
                int checkSize = 13;
                var checkRect = new Rectangle(checkX, (Height - checkSize) / 2,
                                              checkSize + 4, Height);
                if (checkRect.Contains(me.Location))
                {
                    Checked = !_checked;
                    return;
                }
            }
            ToggleCalendar();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.F4 || e.KeyCode == Keys.Return || e.KeyCode == Keys.Space)
            {
                ToggleCalendar();
                e.Handled = true;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            _isFocused = true;
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            _isFocused = false;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateLayout();
            UpdateRegion();
            Invalidate();
        }

        // ══════════════════════════════════════════════════════════════════
        // ── POPUP ─────────────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        private void ToggleCalendar()
        {
            if (_showCheckBox && !_checked) return;

            // Si el formato es Time, usar el selector de hora, no el calendario
            if (_format == DateTimePickerFormat.Time)
            {
                ToggleTimePopup();
                return;
            }

            if (_popup == null || _popup.IsDisposed)
                BuildPopup();

            if (_popup.Visible)
            {
                _popup.Close();
            }
            else
            {
                // Sincronizar fecha antes de abrir
                DateTime safeDate = Clamp(
                    _value.HasValue ? _value.Value : DateTime.Today,
                    _minDate, _maxDate);
                _calendar.SetDate(safeDate);
                if (_value.HasValue)
                    _calendar.SelectedDate = _value;

                // Posición: debajo; si no cabe, arriba
                Point showAt = PointToScreen(new Point(0, Height));
                int calH = _calendar.Height + 4;
                if (showAt.Y + calH > Screen.FromControl(this).WorkingArea.Bottom)
                    showAt = PointToScreen(new Point(0, -calH));

                _popup.Show(showAt);
                _calendar.Focus();
                _calendarOpen = true;
                _isFocused = true;
                Invalidate();
            }
        }

        private void ToggleTimePopup()
        {
            if (_timePopup == null || _timePopup.IsDisposed)
                BuildTimePopup();

            if (_timePopup.Visible)
            {
                _timePopup.Close();
            }
            else
            {
                DateTime safeTime = _value ?? DateTime.Now;
                _timePanel.SetTime(safeTime);

                Point showAt = PointToScreen(new Point(0, Height));
                int popH = _timePanel.Height + 4;
                if (showAt.Y + popH > Screen.FromControl(this).WorkingArea.Bottom)
                    showAt = PointToScreen(new Point(0, -popH));

                _timePopup.Show(showAt);
                _timePanel.Focus();
                _calendarOpen = true;
                _isFocused = true;
                Invalidate();
            }
        }

        private void BuildPopup()
        {
            _calendar = new CustomCalendarPanel
            {
                MinDate = _minDate,
                MaxDate = _maxDate,
                Font = new Font("Segoe UI", 9f)
            };
            ApplyCalendarColors();

            _calendar.DateSelected += (s, e) =>
            {
                _value = e.Start;
                _checked = true;
                _popup.Close();
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            };

            _calendar.SizeChanged2 += (s, e) =>
            {
                var newSize = _calendar.PreferredSize;
                _calendar.Size = newSize;

                var calHost = _popup.Items[0] as ToolStripControlHost; // ← renombrado
                if (calHost != null)
                {
                    calHost.Size = newSize;
                    calHost.Control.Size = newSize;
                }
                _popup.Size = newSize;
            };

            var host = new ToolStripControlHost(_calendar)
            {
                Padding = Padding.Empty,
                Margin = Padding.Empty,
                AutoSize = false,
                Size = _calendar.PreferredSize
            };

            _popup = new ToolStripDropDown
            {
                Padding = Padding.Empty,
                AutoSize = false,
                Size = _calendar.PreferredSize,
                BackColor = _calBackColor
            };
            _popup.Items.Add(host);

            _popup.Closed += (s, e) =>
            {
                _calendarOpen = false;
                _isFocused = ContainsFocus;
                Invalidate();
            };
        }

        private void BuildTimePopup()
        {
            _timePanel = new CustomTimePanel
            {
                Font = new Font("Segoe UI", 9f)
            };
            ApplyCalendarColors(); // ya propaga colores al _timePanel (ver paso 6)

            _timePanel.TimeAccepted += (s, e) =>
            {
                DateTime baseDate = _value?.Date ?? DateTime.Today;
                DateTime newValue = baseDate
                    .AddHours(_timePanel.Hour)
                    .AddMinutes(_timePanel.Minute)
                    .AddSeconds(_timePanel.ShowSeconds ? _timePanel.Second : 0);

                _value = newValue;
                _checked = true;
                _timePopup.Close();
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            };

            var host = new ToolStripControlHost(_timePanel)
            {
                Padding = Padding.Empty,
                Margin = Padding.Empty,
                AutoSize = false,
                Size = _timePanel.PreferredSize
            };

            _timePopup = new ToolStripDropDown
            {
                Padding = Padding.Empty,
                AutoSize = false,
                Size = _timePanel.PreferredSize,
                BackColor = _calBackColor
            };
            _timePopup.Items.Add(host);

            _timePopup.Closed += (s, e) =>
            {
                _calendarOpen = false;
                _isFocused = ContainsFocus;
                Invalidate();
            };
        }

        private void ApplyCalendarColors()
        {
            if (_calendar != null)
            {
                _calendar.BackColor = _calBackColor;
                _calendar.ForeColor = _calForeColor;
                _calendar.TitleBackColor = _calTitleBackColor;
                _calendar.TitleForeColor = _calTitleForeColor;
                _calendar.TrailingForeColor = _calTrailingForeColor;
                _calendar.DayForeColor = _calForeColor;
                _calendar.FooterForeColor = _calForeColor;
                _calendar.DayNameForeColor = Color.FromArgb(
                    Math.Max(0, _calForeColor.R - 40),
                    Math.Max(0, _calForeColor.G - 40),
                    Math.Max(0, _calForeColor.B - 40));
                _calendar.SeparatorColor = Color.FromArgb(
                    Math.Min(255, _calBackColor.R + 30),
                    Math.Min(255, _calBackColor.G + 30),
                    Math.Min(255, _calBackColor.B + 30));

                if (_popup != null)
                    _popup.BackColor = _calBackColor;
            }

            if (_timePanel != null)
            {
                _timePanel.BackColor = _calBackColor;
                _timePanel.ForeColor = _calForeColor;
                _timePanel.TitleBackColor = _calTitleBackColor;
                _timePanel.TitleForeColor = _calTitleForeColor;
                _timePanel.AccentColor = _calTitleBackColor;
                _timePanel.AccentForeColor = _calTitleForeColor;
                _timePanel.SeparatorColor = Color.FromArgb(
                    Math.Min(255, _calBackColor.R + 30),
                    Math.Min(255, _calBackColor.G + 30),
                    Math.Min(255, _calBackColor.B + 30));

                if (_timePopup != null)
                    _timePopup.BackColor = _calBackColor;
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── ÍCONO ─────────────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        private void RefreshIcon()
        {
            if (_iconChar == IconChar.None)
            {
                _iconPicture.Visible = false;
                _iconPicture.Image = null;
                return;
            }
            _iconPicture.Image = IconCache.Get(_iconChar, _iconFont, _iconSize, _iconColor);
            _iconPicture.Visible = true;
        }

        private void UpdateLayout()
        {
            bool hasIcon = _iconChar != IconChar.None;
            int iconW = hasIcon ? (_iconSize + _iconPadding * 2) : 0;
            int bw = _borderWidth;
            int padX = bw + 3;

            if (!hasIcon)
            {
                _iconPicture.Visible = false;
                return;
            }

            int innerHeight = Height - bw * 2;
            int iconH = Math.Min(_iconSize + _iconPadding * 2, innerHeight);
            int iconY = bw + (innerHeight - iconH) / 2;

            _iconPicture.Size = new Size(iconW, iconH);
            _iconPicture.Location = _iconAlign == IconTextBoxAlignment.Left
                ? new Point(padX - 2, iconY)
                : new Point(Width - padX - iconW - ArrowAreaWidth + 2, iconY);
        }

        private void UpdateRegion()
        {
            if (_cornerRadius <= 0) { Region = null; return; }

            float r2 = Math.Max(0, _cornerRadius - _borderWidth);
            float d2 = r2 * 2f;
            int bw = _borderWidth;
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
        // ── HELPERS ───────────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        private string GetDisplayText()
        {
            if (!_value.HasValue) return string.Empty;
            switch (_format)
            {
                case DateTimePickerFormat.Long:   return _value.Value.ToLongDateString();
                case DateTimePickerFormat.Short:  return _value.Value.ToShortDateString();
                case DateTimePickerFormat.Time: return _value.Value.ToString("HH:mm:ss");
                case DateTimePickerFormat.Custom: return _value.Value.ToString(_customFormat);
                default: return _value.Value.ToShortDateString();
            }
        }

        private static DateTime Clamp(DateTime val, DateTime min, DateTime max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        // ══════════════════════════════════════════════════════════════════
        // Dispose
        // ══════════════════════════════════════════════════════════════════
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _popup?.Dispose();
                _timePopup?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}