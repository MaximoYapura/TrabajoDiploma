using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace CustomControls
{
    // ══════════════════════════════════════════════════════════════════════
    //  CustomCalendarPanel
    //
    //  Calendario mensual pintado completamente a mano con tres vistas:
    //    • Días   (vista normal — muestra la grilla de días del mes)
    //    • Meses  (click en el título → elige el mes del año)
    //    • Años   (click en el título de la vista meses → elige el año)
    //
    //  NAVEGACIÓN POR VISTAS:
    //    Click en "Mes Año"  → vista de meses (12 meses del año)
    //    Click en "Año"      → vista de años  (12 años, centrados en el actual)
    //    Click en un mes     → vuelve a vista días con ese mes
    //    Click en un año     → vuelve a vista meses con ese año
    //    ◄ ► siempre navegan la unidad de la vista actual
    //       (días→mes, meses→año, años→década)
    // ══════════════════════════════════════════════════════════════════════
    [ToolboxItem(false)]
    public class CustomCalendarPanel : Control
    {
        // ──────────────────────────────────────────────────────────────────
        // Vistas
        // ──────────────────────────────────────────────────────────────────
        private enum CalView { Days, Months, Years }
        private CalView _view = CalView.Days;

        // ──────────────────────────────────────────────────────────────────
        // Constantes de layout
        // ──────────────────────────────────────────────────────────────────
        private const int Cols = 7;
        private const int MaxDayRows = 6;
        private const int GridCols = 4;   // columnas en vistas mes/año
        private const int GridRows = 3;   // filas en vistas mes/año

        // ──────────────────────────────────────────────────────────────────
        // Estado
        // ──────────────────────────────────────────────────────────────────
        private DateTime _viewDate;
        private DateTime? _selectedDate;
        private int _hoverCell = -1;
        private int _yearRangeStart;        // primer año mostrado en vista años

        // ──────────────────────────────────────────────────────────────────
        // Rango
        // ──────────────────────────────────────────────────────────────────
        private DateTime _minDate = new DateTime(1753, 1, 1);
        private DateTime _maxDate = new DateTime(9998, 12, 31);

        // ──────────────────────────────────────────────────────────────────
        // Colores
        // ──────────────────────────────────────────────────────────────────
        private Color _titleBackColor = Color.FromArgb(0, 120, 215);
        private Color _titleForeColor = Color.White;
        private Color _dayNameForeColor = Color.FromArgb(180, 180, 180);
        private Color _dayForeColor = SystemColors.WindowText;
        private Color _trailingForeColor = Color.Silver;
        private Color _todayHighlightColor = Color.FromArgb(0, 120, 215);
        private Color _selectedBackColor = Color.FromArgb(0, 120, 215);
        private Color _selectedForeColor = Color.White;
        private Color _hoverBackColor = Color.FromArgb(220, 235, 252);
        private Color _hoverForeColor = SystemColors.WindowText;
        private Color _disabledForeColor = Color.FromArgb(160, 160, 160);
        private Color _footerForeColor = SystemColors.WindowText;
        private Color _separatorColor = Color.FromArgb(200, 200, 200);
        private Color _navButtonHoverColor = Color.FromArgb(180, 210, 245);

        // ──────────────────────────────────────────────────────────────────
        // Opciones
        // ──────────────────────────────────────────────────────────────────
        private bool _showToday = true;
        private bool _showTodayHighlight = true;
        private bool _prevNavHover = false;
        private bool _nextNavHover = false;
        private bool _titleHover = false;

        // ──────────────────────────────────────────────────────────────────
        // Métricas
        // ──────────────────────────────────────────────────────────────────
        private int _cellW;
        private int _cellH;
        private int _headerH;
        private int _dayNameH;
        private int _footerH;
        private int _gridTop;
        private Rectangle _prevBtn;
        private Rectangle _nextBtn;
        private Rectangle _titleBtn;

        // Métricas de las vistas meses/años (celdas más anchas)
        private int _bigCellW;
        private int _bigCellH;

        private readonly CultureInfo _culture = CultureInfo.CurrentCulture;
        private readonly DayOfWeek _firstDayOfWeek;

        // ══════════════════════════════════════════════════════════════════
        // Evento
        // ══════════════════════════════════════════════════════════════════
        public event DateRangeEventHandler DateSelected;
        public event EventHandler SizeChanged2;

        // ══════════════════════════════════════════════════════════════════
        // Constructor
        // ══════════════════════════════════════════════════════════════════
        public CustomCalendarPanel()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            _firstDayOfWeek = _culture.DateTimeFormat.FirstDayOfWeek;
            _viewDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            _yearRangeStart = DateTime.Today.Year - 5;

            Font = new Font("Segoe UI", 9.5f);
            BackColor = SystemColors.Window;
            ForeColor = SystemColors.WindowText;

            RecalcLayout();
            Size = PreferredSize;
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES ───────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                if (value.HasValue)
                    _viewDate = new DateTime(value.Value.Year, value.Value.Month, 1);
                Invalidate();
            }
        }

        public void SetDate(DateTime date)
        {
            _viewDate = new DateTime(date.Year, date.Month, 1);
            Invalidate();
        }

        public DateTime SelectionStart => _selectedDate ?? DateTime.Today;

        public DateTime MinDate { get => _minDate; set { _minDate = value; Invalidate(); } }
        public DateTime MaxDate { get => _maxDate; set { _maxDate = value; Invalidate(); } }

        // Colores
        public Color TitleBackColor { get => _titleBackColor; set { _titleBackColor = value; Invalidate(); } }
        public Color TitleForeColor { get => _titleForeColor; set { _titleForeColor = value; Invalidate(); } }
        public Color DayNameForeColor { get => _dayNameForeColor; set { _dayNameForeColor = value; Invalidate(); } }
        public Color DayForeColor { get => _dayForeColor; set { _dayForeColor = value; Invalidate(); } }
        public Color TrailingForeColor { get => _trailingForeColor; set { _trailingForeColor = value; Invalidate(); } }
        public Color TodayHighlightColor { get => _todayHighlightColor; set { _todayHighlightColor = value; Invalidate(); } }
        public Color SelectedBackColor { get => _selectedBackColor; set { _selectedBackColor = value; Invalidate(); } }
        public Color SelectedForeColor { get => _selectedForeColor; set { _selectedForeColor = value; Invalidate(); } }
        public Color HoverBackColor { get => _hoverBackColor; set { _hoverBackColor = value; Invalidate(); } }
        public Color HoverForeColor { get => _hoverForeColor; set { _hoverForeColor = value; Invalidate(); } }
        public Color DisabledForeColor { get => _disabledForeColor; set { _disabledForeColor = value; Invalidate(); } }
        public Color SeparatorColor { get => _separatorColor; set { _separatorColor = value; Invalidate(); } }
        public Color FooterForeColor { get => _footerForeColor; set { _footerForeColor = value; Invalidate(); } }

        public bool ShowToday
        {
            get => _showToday;
            set { _showToday = value; RecalcLayout(); Size = PreferredSize; Invalidate(); }
        }
        public bool ShowTodayHighlight { get => _showTodayHighlight; set { _showTodayHighlight = value; Invalidate(); } }

        public override Size GetPreferredSize(Size proposedSize) => PreferredSize;

        public new Size PreferredSize
        {
            get
            {
                RecalcLayout();
                switch (_view)
                {
                    case CalView.Days:
                        return new Size(_cellW * Cols,
                                        _headerH + _dayNameH + _cellH * MaxDayRows + _footerH);

                    case CalView.Months:
                    case CalView.Years:
                        return new Size(_cellW * Cols,
                                        _headerH + 4 + _bigCellH * GridRows + 4);

                    default:
                        return new Size(_cellW * Cols,
                                        _headerH + _dayNameH + _cellH * MaxDayRows + _footerH);
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── LAYOUT ────────────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        private void RecalcLayout()
        {
            using (var g = Graphics.FromHwnd(Handle == IntPtr.Zero ? IntPtr.Zero : Handle))
            {
                // Celda días: más grande que la versión anterior
                SizeF sample = g.MeasureString("30", Font);
                _cellW = (int)Math.Ceiling(sample.Width) + 18;  // +18 en vez de +14
                _cellH = (int)Math.Ceiling(sample.Height) + 14;  // +14 en vez de +10

                // Celda meses/años: ocupa todo el ancho del calendario ÷ 4 columnas
                _bigCellW = (_cellW * Cols) / GridCols;
                _bigCellH = _cellH + 6;
            }

            _headerH = _cellH + 6;
            _dayNameH = _cellH - 4;
            _footerH = _showToday ? _cellH : 0;
            _gridTop = _headerH + _dayNameH;

            int btnSize = _headerH - 10;
            int btnY = (_headerH - btnSize) / 2;
            _prevBtn = new Rectangle(6, btnY, btnSize, btnSize);
            _nextBtn = new Rectangle(_cellW * Cols - btnSize - 6, btnY, btnSize, btnSize);
            _titleBtn = new Rectangle(_prevBtn.Right + 4, 0,
                                      _nextBtn.Left - _prevBtn.Right - 8, _headerH);
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

            switch (_view)
            {
                case CalView.Days:
                    DrawDayNames(g);
                    using (var pen = new Pen(_separatorColor))
                        g.DrawLine(pen, 0, _headerH + _dayNameH, Width, _headerH + _dayNameH);
                    DrawDays(g);
                    if (_showToday) DrawFooter(g);
                    break;

                case CalView.Months:
                    DrawMonthGrid(g);
                    break;

                case CalView.Years:
                    DrawYearGrid(g);
                    break;
            }
        }

        // ── Header ────────────────────────────────────────────────────────

        private void DrawHeader(Graphics g)
        {
            g.FillRectangle(new SolidBrush(_titleBackColor),
                            new Rectangle(0, 0, Width, _headerH));

            // Resaltar título cuando hay hover (indica que es clickeable)
            if (_titleHover && _view != CalView.Years)
            {
                using (var path = RoundedRect(Inset(_titleBtn, 2), 4))
                using (var br = new SolidBrush(_navButtonHoverColor))
                    g.FillPath(br, path);
            }

            string title = GetHeaderTitle();
            var boldFont = new Font(Font, FontStyle.Bold);
            TextRenderer.DrawText(g, title, boldFont, _titleBtn, _titleForeColor,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            DrawNavButton(g, _prevBtn, "◄", _prevNavHover, CanGoPrev());
            DrawNavButton(g, _nextBtn, "►", _nextNavHover, CanGoNext());
        }

        private string GetHeaderTitle()
        {
            switch (_view)
            {
                case CalView.Days:
                    return _culture.DateTimeFormat.GetMonthName(_viewDate.Month)
                           + " " + _viewDate.Year;
                case CalView.Months:
                    return _viewDate.Year.ToString();
                case CalView.Years:
                    return _yearRangeStart + " – " + (_yearRangeStart + 11);
                default:
                    return string.Empty;
            }
        }

        private void DrawNavButton(Graphics g, Rectangle rect, string text,
                                   bool hover, bool enabled)
        {
            if (hover && enabled)
            {
                using (var path = RoundedRect(rect, 3))
                using (var br = new SolidBrush(_navButtonHoverColor))
                    g.FillPath(br, path);
            }
            Color fore = enabled ? _titleForeColor
                                 : Color.FromArgb(80, _titleForeColor);
            TextRenderer.DrawText(g, text, Font, rect, fore,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // ── Vista días ────────────────────────────────────────────────────

        private void DrawDayNames(Graphics g)
        {
            int y = _headerH;
            for (int col = 0; col < Cols; col++)
            {
                DayOfWeek dow = (DayOfWeek)(((int)_firstDayOfWeek + col) % 7);
                string name = _culture.DateTimeFormat.GetShortestDayName(dow);
                var rect = new Rectangle(col * _cellW, y, _cellW, _dayNameH);
                TextRenderer.DrawText(g, name, Font, rect, _dayNameForeColor,
                                      TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private void DrawDays(Graphics g)
        {
            DateTime firstCell = GetFirstCellDate();
            DateTime today = DateTime.Today;

            for (int i = 0; i < Cols * MaxDayRows; i++)
            {
                DateTime date = firstCell.AddDays(i);
                int col = i % Cols;
                int row = i / Cols;
                var rect = new Rectangle(col * _cellW, _gridTop + row * _cellH, _cellW, _cellH);

                bool isCurrentMonth = date.Month == _viewDate.Month;
                bool isSelected = _selectedDate.HasValue && date.Date == _selectedDate.Value.Date;
                bool isToday = date.Date == today.Date;
                bool isHover = i == _hoverCell;
                bool isDisabled = date < _minDate || date > _maxDate;

                DrawDayCell(g, rect, date, isCurrentMonth, isSelected, isToday, isHover, isDisabled);
            }
        }

        private void DrawDayCell(Graphics g, Rectangle rect, DateTime date,
                                 bool currentMonth, bool selected,
                                 bool today, bool hover, bool disabled)
        {
            if (selected)
            {
                using (var path = RoundedRect(Inset(rect, 2), 4))
                using (var br = new SolidBrush(_selectedBackColor))
                    g.FillPath(br, path);
            }
            else if (hover && !disabled)
            {
                using (var path = RoundedRect(Inset(rect, 2), 4))
                using (var br = new SolidBrush(_hoverBackColor))
                    g.FillPath(br, path);
            }

            if (today && _showTodayHighlight && !selected)
            {
                using (var path = RoundedRect(Inset(rect, 2), 4))
                using (var pen = new Pen(_todayHighlightColor, 1.5f))
                    g.DrawPath(pen, path);
            }

            Color fore;
            if (selected) fore = _selectedForeColor;
            else if (disabled) fore = _disabledForeColor;
            else if (!currentMonth) fore = _trailingForeColor;
            else if (hover) fore = _hoverForeColor;
            else fore = _dayForeColor;

            TextRenderer.DrawText(g, date.Day.ToString(), Font, rect, fore,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void DrawFooter(Graphics g)
        {
            int y = _gridTop + MaxDayRows * _cellH;
            var rect = new Rectangle(0, y, Width, _footerH);
            string txt = "Hoy: " + DateTime.Today.ToString("dd/MM/yyyy");

            using (var pen = new Pen(_separatorColor))
                g.DrawLine(pen, 0, y, Width, y);

            TextRenderer.DrawText(g, txt, Font, rect, _footerForeColor,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // ── Vista meses ───────────────────────────────────────────────────

        private void DrawMonthGrid(Graphics g)
        {
            int gridTop = _headerH + 4;
            for (int i = 0; i < 12; i++)
            {
                int col = i % GridCols;
                int row = i / GridCols;
                var rect = new Rectangle(col * _bigCellW, gridTop + row * _bigCellH,
                                         _bigCellW, _bigCellH);

                bool isSelected = _selectedDate.HasValue &&
                                  _selectedDate.Value.Year == _viewDate.Year &&
                                  _selectedDate.Value.Month == i + 1;
                bool isHover = i == _hoverCell;
                bool isDisabled = !MonthInRange(i + 1, _viewDate.Year);
                bool isCurrent = DateTime.Today.Month == i + 1 &&
                                  DateTime.Today.Year == _viewDate.Year;

                DrawBigCell(g, rect,
                    _culture.DateTimeFormat.GetAbbreviatedMonthName(i + 1),
                    isSelected, isHover, isDisabled, isCurrent);
            }
        }

        // ── Vista años ────────────────────────────────────────────────────

        private void DrawYearGrid(Graphics g)
        {
            int gridTop = _headerH + 4;
            for (int i = 0; i < 12; i++)
            {
                int year = _yearRangeStart + i;
                int col = i % GridCols;
                int row = i / GridCols;
                var rect = new Rectangle(col * _bigCellW, gridTop + row * _bigCellH,
                                          _bigCellW, _bigCellH);

                bool isSelected = _selectedDate.HasValue && _selectedDate.Value.Year == year;
                bool isHover = i == _hoverCell;
                bool isDisabled = year < _minDate.Year || year > _maxDate.Year;
                bool isCurrent = year == DateTime.Today.Year;

                DrawBigCell(g, rect, year.ToString(),
                            isSelected, isHover, isDisabled, isCurrent);
            }
        }

        private void DrawBigCell(Graphics g, Rectangle rect, string text,
                                 bool selected, bool hover, bool disabled, bool isCurrent)
        {
            var inner = Inset(rect, 3);

            if (selected)
            {
                using (var path = RoundedRect(inner, 5))
                using (var br = new SolidBrush(_selectedBackColor))
                    g.FillPath(br, path);
            }
            else if (hover && !disabled)
            {
                using (var path = RoundedRect(inner, 5))
                using (var br = new SolidBrush(_hoverBackColor))
                    g.FillPath(br, path);
            }

            if (isCurrent && !selected)
            {
                using (var path = RoundedRect(inner, 5))
                using (var pen = new Pen(_todayHighlightColor, 1.5f))
                    g.DrawPath(pen, path);
            }

            Color fore;
            if (selected) fore = _selectedForeColor;
            else if (disabled) fore = _disabledForeColor;
            else if (hover) fore = _hoverForeColor;
            else fore = _dayForeColor;

            TextRenderer.DrawText(g, text, Font, rect, fore,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        // ══════════════════════════════════════════════════════════════════
        // ── INTERACCIÓN ───────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            bool prevP = _prevNavHover, nextP = _nextNavHover, titleP = _titleHover;
            _prevNavHover = _prevBtn.Contains(e.Location);
            _nextNavHover = _nextBtn.Contains(e.Location);
            _titleHover = _titleBtn.Contains(e.Location) && _view != CalView.Years;

            int newHover = HitTestCell(e.Location);

            if (newHover != _hoverCell || prevP != _prevNavHover ||
                nextP != _nextNavHover || titleP != _titleHover)
            {
                _hoverCell = newHover;
                Cursor = (_hoverCell >= 0 || _titleHover) ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hoverCell = -1;
            _prevNavHover = false;
            _nextNavHover = false;
            _titleHover = false;
            Cursor = Cursors.Default;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button != MouseButtons.Left) return;

            // ── Botones ◄ ►
            if (_prevBtn.Contains(e.Location) && CanGoPrev()) { NavigatePrev(); return; }
            if (_nextBtn.Contains(e.Location) && CanGoNext()) { NavigateNext(); return; }

            // ── Título → subir un nivel de vista
            if (_titleBtn.Contains(e.Location))
            {
                if (_view == CalView.Days)
                {
                    _view = CalView.Months;
                    _hoverCell = -1;
                    Size = PreferredSize;
                    SizeChanged2?.Invoke(this, EventArgs.Empty);
                    Invalidate();
                }
                else if (_view == CalView.Months)
                {
                    _yearRangeStart = _viewDate.Year - 5;
                    _view = CalView.Years;
                    _hoverCell = -1;
                    Size = PreferredSize;
                    SizeChanged2?.Invoke(this, EventArgs.Empty);
                    Invalidate();
                }
                return;
            }

            int idx = HitTestCell(e.Location);
            if (idx < 0) return;

            switch (_view)
            {
                case CalView.Days:
                    DateTime date = GetFirstCellDate().AddDays(idx);
                    if (date >= _minDate && date <= _maxDate)
                        SelectDate(date);
                    break;

                case CalView.Months:
                    int month = idx + 1;
                    if (MonthInRange(month, _viewDate.Year))
                    {
                        _viewDate = new DateTime(_viewDate.Year, month, 1);
                        _view = CalView.Days;
                        _hoverCell = -1;
                        Size = PreferredSize;
                        SizeChanged2?.Invoke(this, EventArgs.Empty);
                        Invalidate();
                    }
                    break;

                case CalView.Years:
                    int year = _yearRangeStart + idx;
                    if (year >= _minDate.Year && year <= _maxDate.Year)
                    {
                        _viewDate = new DateTime(year, _viewDate.Month, 1);
                        _view = CalView.Months;
                        _hoverCell = -1;
                        Size = PreferredSize;
                        SizeChanged2?.Invoke(this, EventArgs.Empty);
                        Invalidate();
                    }
                    break;
            }

            // Footer "Hoy"
            if (_view == CalView.Days && _showToday)
            {
                int footerY = _gridTop + MaxDayRows * _cellH;
                var footerRect = new Rectangle(0, footerY, Width, _footerH);
                if (footerRect.Contains(e.Location))
                    SelectDate(DateTime.Today);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (_view != CalView.Days) { e.Handled = true; return; }

            DateTime current = _selectedDate ?? DateTime.Today;
            switch (e.KeyCode)
            {
                case Keys.Left: TrySelect(current.AddDays(-1)); e.Handled = true; break;
                case Keys.Right: TrySelect(current.AddDays(+1)); e.Handled = true; break;
                case Keys.Up: TrySelect(current.AddDays(-7)); e.Handled = true; break;
                case Keys.Down: TrySelect(current.AddDays(+7)); e.Handled = true; break;
                case Keys.PageUp: NavigatePrev(); e.Handled = true; break;
                case Keys.PageDown: NavigateNext(); e.Handled = true; break;
                case Keys.Home:
                    TrySelect(new DateTime(current.Year, current.Month, 1));
                    e.Handled = true; break;
                case Keys.End:
                    TrySelect(new DateTime(current.Year, current.Month,
                        DateTime.DaysInMonth(current.Year, current.Month)));
                    e.Handled = true; break;
                case Keys.Return:
                case Keys.Space:
                    if (_selectedDate.HasValue)
                        FireDateSelected(_selectedDate.Value);
                    e.Handled = true; break;
                case Keys.Escape:
                    _view = CalView.Days; Invalidate();
                    e.Handled = true; break;
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── HELPERS ───────────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        private void NavigatePrev()
        {
            switch (_view)
            {
                case CalView.Days: _viewDate = _viewDate.AddMonths(-1); break;
                case CalView.Months: _viewDate = _viewDate.AddYears(-1); break;
                case CalView.Years: _yearRangeStart -= 12; break;
            }
            _hoverCell = -1;
            Invalidate();
        }

        private void NavigateNext()
        {
            switch (_view)
            {
                case CalView.Days: _viewDate = _viewDate.AddMonths(+1); break;
                case CalView.Months: _viewDate = _viewDate.AddYears(+1); break;
                case CalView.Years: _yearRangeStart += 12; break;
            }
            _hoverCell = -1;
            Invalidate();
        }

        private bool CanGoPrev()
        {
            switch (_view)
            {
                case CalView.Days: return _viewDate.AddDays(-1) >= _minDate;
                case CalView.Months: return _viewDate.Year - 1 >= _minDate.Year;
                case CalView.Years: return _yearRangeStart - 1 >= _minDate.Year;
                default: return false;
            }
        }

        private bool CanGoNext()
        {
            switch (_view)
            {
                case CalView.Days:
                    return _viewDate.AddMonths(1) <=
                           new DateTime(_maxDate.Year, _maxDate.Month, 1);
                case CalView.Months:
                    return _viewDate.Year + 1 <= _maxDate.Year;
                case CalView.Years:
                    return _yearRangeStart + 12 <= _maxDate.Year;
                default:
                    return false;
            }
        }

        /// <summary>Hit-test genérico: funciona para días, meses y años.</summary>
        private int HitTestCell(Point pt)
        {
            switch (_view)
            {
                case CalView.Days:
                    {
                        int x = pt.X / _cellW;
                        int y = (pt.Y - _gridTop) / _cellH;
                        if (x < 0 || x >= Cols || y < 0 || y >= MaxDayRows) return -1;
                        return y * Cols + x;
                    }
                case CalView.Months:
                case CalView.Years:
                    {
                        int gridTop = _headerH + 4;
                        int x = pt.X / _bigCellW;
                        int y = (pt.Y - gridTop) / _bigCellH;
                        if (x < 0 || x >= GridCols || y < 0 || y >= GridRows) return -1;
                        return y * GridCols + x;
                    }
                default: return -1;
            }
        }

        private DateTime GetFirstCellDate()
        {
            DateTime first = new DateTime(_viewDate.Year, _viewDate.Month, 1);
            int offset = ((int)first.DayOfWeek - (int)_firstDayOfWeek + 7) % 7;
            return first.AddDays(-offset);
        }

        private bool MonthInRange(int month, int year)
        {
            var first = new DateTime(year, month, 1);
            var last = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            return last >= _minDate && first <= _maxDate;
        }

        private void SelectDate(DateTime date)
        {
            _selectedDate = date;
            _viewDate = new DateTime(date.Year, date.Month, 1);
            Invalidate();
            FireDateSelected(date);
        }

        private void TrySelect(DateTime date)
        {
            if (date < _minDate || date > _maxDate) return;
            _selectedDate = date;
            _viewDate = new DateTime(date.Year, date.Month, 1);
            Invalidate();
        }

        private void FireDateSelected(DateTime date)
            => DateSelected?.Invoke(this, new DateRangeEventArgs(date, date));

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