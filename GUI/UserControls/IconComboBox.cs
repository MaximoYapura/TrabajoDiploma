using FontAwesome.Sharp;
using GUI_08YS.UserControls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
    // ══════════════════════════════════════════════════════════════════════
    //  IconComboBox
    //
    //  El ComboBox nativo de WinForms tiene un problema conocido: al cambiar
    //  BackColor solo se pinta el campo de texto, mientras que el área de la
    //  flecha y el borde exterior quedan en el color del sistema (blanco/gris).
    //  No hay forma de solucionarlo con propiedades estándar.
    //
    //  Este control pinta TODA la cara a mano (UserPaint) y usa un ListBox
    //  con owner-draw en un ToolStripDropDown para el desplegable, dando
    //  control completo sobre todos los colores.
    //
    //  CARACTERÍSTICAS:
    //    1. Ícono FontAwesome.Sharp (izquierda o derecha, con evento IconClick)
    //    2. BackColor y ForeColor efectivos en toda la cara del control
    //    3. Borde personalizable: color normal/foco, grosor, esquinas redondeadas
    //    4. Desplegable con colores totalmente configurables (fondo, texto,
    //       highlight, borde)
    //    5. Items: añadir objetos arbitrarios; DisplayMember para binding simple
    //    6. SelectedIndex / SelectedItem / SelectedValue
    //    7. Navegación por teclado: ↑↓ sin abrir, F4/Enter/Space abre
    //    8. Evento SelectedIndexChanged
    // ══════════════════════════════════════════════════════════════════════
    [ToolboxItem(true)]
    [DefaultProperty(nameof(SelectedIndex))]
    [DefaultEvent("SelectedIndexChanged")]
    public class IconComboBox : UserControl
    {
        private const int ArrowAreaWidth = 20;

        // ──────────────────────────────────────────────────────────────────
        // Controles internos
        // ──────────────────────────────────────────────────────────────────
        private readonly PictureBox _iconPicture;
        private ListBox _listBox;
        private ToolStripDropDown _popup;

        // ──────────────────────────────────────────────────────────────────
        // Items y selección
        // ──────────────────────────────────────────────────────────────────
        private readonly ComboBoxItemCollection _items;
        private int _selectedIndex = -1;
        private int _hoverIndex = -1;
        private bool _allowDeselect = false;
        private string _displayMember = string.Empty;

        // ──────────────────────────────────────────────────────────────────
        // Campos — ícono
        // ──────────────────────────────────────────────────────────────────
        private IconChar _iconChar = IconChar.None;
        private IconFont _iconFont = IconFont.Auto;
        private Color _iconColor = Color.DimGray;
        private int _iconSize = 16;
        private IconTextBoxAlignment _iconAlign = IconTextBoxAlignment.Left;
        private int _iconPadding = 6;

        // ──────────────────────────────────────────────────────────────────
        // Campos — borde de la cara
        // ──────────────────────────────────────────────────────────────────
        private Color _borderColor = Color.FromArgb(180, 180, 180);
        private Color _borderFocusColor = Color.FromArgb(100, 149, 237);
        private int _borderWidth = 1;
        private int _cornerRadius = 0;
        private bool _isFocused = false;

        // ──────────────────────────────────────────────────────────────────
        // Campos — desplegable
        // ──────────────────────────────────────────────────────────────────
        private Color _dropBackColor = SystemColors.Window;
        private Color _dropForeColor = SystemColors.WindowText;
        private Color _dropHighlightBackColor = Color.FromArgb(0, 120, 215);
        private Color _dropHighlightForeColor = Color.White;
        private Color _dropBorderColor = Color.FromArgb(140, 140, 140);
        private int _dropMaxHeight = 200;
        private int _dropItemHeight = 0;   // 0 = automático (Font.Height + 6)
        private Font _dropDownFont = null;

        // ──────────────────────────────────────────────────────────────────
        // Campos — DataSource
        // ──────────────────────────────────────────────────────────────────
        private object _dataSource = null;
        private string _valueMember = string.Empty;
        private bool _refreshingDataSource = false;

        // ══════════════════════════════════════════════════════════════════
        // Eventos
        // ══════════════════════════════════════════════════════════════════
        public event EventHandler IconClick;
        public event EventHandler SelectedIndexChanged;

        // ══════════════════════════════════════════════════════════════════
        // Constructor
        // ══════════════════════════════════════════════════════════════════
        public IconComboBox()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            _items = new ComboBoxItemCollection(this);

            _iconPicture = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.CenterImage,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Visible = false
            };
            _iconPicture.Click += (_, e) =>
            {
                IconClick?.Invoke(this, e);
                if (IconClick == null) ToggleDropdown();
            };
            Controls.Add(_iconPicture);

            BackColor = SystemColors.Window;
            ForeColor = SystemColors.WindowText;
            Size = new Size(200, 32);
            Cursor = Cursors.Hand;
            TabStop = true;

            RefreshIcon();
            UpdateLayout();
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: ITEMS Y SELECCIÓN ───────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        /// <summary>Colección de ítems del ComboBox.</summary>
        [Category("Data")]
        [Description("Colección de elementos del ComboBox.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ComboBoxItemCollection Items => _items;

        /// <summary>Índice del elemento seleccionado (-1 = ninguno).</summary>
        [Category("Data")]
        [DefaultValue(-1)]
        [Browsable(false)]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                int clamped = (value < -1 || value >= _items.Count) ? -1 : value;
                if (clamped == _selectedIndex) return;
                _selectedIndex = clamped;
                SyncListBoxSelection();
                Invalidate();
                SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>Elemento actualmente seleccionado (null si ninguno).</summary>
        [Browsable(false)]
        public object SelectedItem
        {
            get => _selectedIndex >= 0 && _selectedIndex < _items.Count
                   ? _items[_selectedIndex] : null;
            set
            {
                int idx = _items.IndexOf(value);
                SelectedIndex = idx; // -1 si no encontrado → deselecciona
            }
        }

        [Category("Behavior")]
        [Description("Permite deseleccionar mostrando una opción vacía al inicio de la lista.")]
        [DefaultValue(false)]
        public bool AllowDeselect
        {
            get => _allowDeselect;
            set { _allowDeselect = value; RefreshListBoxItems(); }
        }

        /// <summary>
        /// Propiedad del objeto que se muestra como texto.
        /// String vacío = usa ToString() del objeto.
        /// </summary>
        [Category("Data")]
        [DefaultValue("")]
        [Description("Nombre de la propiedad del objeto que se muestra como texto.")]
        public string DisplayMember
        {
            get => _displayMember;
            set { _displayMember = value ?? string.Empty; Invalidate(); }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: ÍCONO ────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Category("Icon")]
        [DefaultValue(IconChar.None)]
        [Description("Ícono de FontAwesome.Sharp.")]
        public IconChar IconChar
        {
            get => _iconChar;
            set { _iconChar = value; RefreshIcon(); UpdateLayout(); Invalidate(); }
        }

        [Category("Icon")]
        [DefaultValue(IconFont.Auto)]
        [Description("Estilo FontAwesome: Auto, Solid, Regular, Brands…")]
        public IconFont IconFont
        {
            get => _iconFont;
            set { _iconFont = value; RefreshIcon(); }
        }

        [Category("Icon")]
        [Description("Color del ícono.")]
        public Color IconColor
        {
            get => _iconColor;
            set { _iconColor = value; RefreshIcon(); }
        }

        [Category("Icon")]
        [DefaultValue(16)]
        [Description("Tamaño del ícono en píxeles.")]
        public int IconSize
        {
            get => _iconSize;
            set { _iconSize = Math.Max(8, value); RefreshIcon(); UpdateLayout(); Invalidate(); }
        }

        [Category("Icon")]
        [DefaultValue(IconTextBoxAlignment.Left)]
        [Description("Posición del ícono: Left o Right.")]
        public IconTextBoxAlignment IconAlignment
        {
            get => _iconAlign;
            set { _iconAlign = value; UpdateLayout(); Invalidate(); }
        }

        [Category("Icon")]
        [DefaultValue(6)]
        [Description("Espacio horizontal a cada lado del ícono.")]
        public int IconPadding
        {
            get => _iconPadding;
            set { _iconPadding = Math.Max(0, value); UpdateLayout(); Invalidate(); }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: BORDE DE LA CARA ────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Category("Border")]
        [Description("Color del borde sin foco.")]
        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Border")]
        [Description("Color del borde con foco / desplegado.")]
        public Color BorderFocusColor
        {
            get => _borderFocusColor;
            set { _borderFocusColor = value; Invalidate(); }
        }

        [Category("Border")]
        [DefaultValue(1)]
        [Description("Grosor del borde en píxeles.")]
        public int BorderWidth
        {
            get => _borderWidth;
            set { _borderWidth = Math.Max(1, value); UpdateLayout(); Invalidate(); }
        }

        [Category("Border")]
        [DefaultValue(0)]
        [Description("Radio de las esquinas redondeadas (0 = cuadradas).")]
        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = Math.Max(0, value); Invalidate(); }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: DESPLEGABLE ─────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        [Category("DropDown")]
        [Description("Color de fondo de la lista desplegable.")]
        public Color DropDownBackColor
        {
            get => _dropBackColor;
            set { _dropBackColor = value; ApplyDropColors(); }
        }

        [Category("DropDown")]
        [Description("Color de texto de la lista desplegable.")]
        public Color DropDownForeColor
        {
            get => _dropForeColor;
            set { _dropForeColor = value; ApplyDropColors(); }
        }

        [Category("DropDown")]
        [Description("Color de fondo del ítem seleccionado / hover.")]
        public Color DropDownHighlightBackColor
        {
            get => _dropHighlightBackColor;
            set { _dropHighlightBackColor = value; _listBox?.Invalidate(); }
        }

        [Category("DropDown")]
        [Description("Color de texto del ítem seleccionado / hover.")]
        public Color DropDownHighlightForeColor
        {
            get => _dropHighlightForeColor;
            set { _dropHighlightForeColor = value; _listBox?.Invalidate(); }
        }

        [Category("DropDown")]
        [Description("Color del borde de la lista desplegable.")]
        public Color DropDownBorderColor
        {
            get => _dropBorderColor;
            set { _dropBorderColor = value; }
        }

        [Category("DropDown")]
        [DefaultValue(200)]
        [Description("Altura máxima de la lista desplegable en píxeles.")]
        public int DropDownMaxHeight
        {
            get => _dropMaxHeight;
            set { _dropMaxHeight = Math.Max(40, value); }
        }

        [Category("DropDown")]
        [DefaultValue(0)]
        [Description("Altura de cada ítem en píxeles. 0 = automático (Font.Height + 6).")]
        public int DropDownItemHeight
        {
            get => _dropItemHeight;
            set { _dropItemHeight = Math.Max(0, value); }
        }

        [Category("DropDown")]
        [Description("Fuente de los ítems desplegados. Null = usa la fuente del control.")]
        [DefaultValue(null)]
        public Font DropDownFont
        {
            get => _dropDownFont;
            set
            {
                _dropDownFont = value;
                if (_listBox != null)
                {
                    _listBox.Font = _dropDownFont ?? Font;
                    _listBox.ItemHeight = _dropItemHeight > 0
                        ? _dropItemHeight
                        : (_dropDownFont ?? Font).Height + 6;
                    _listBox.Invalidate();
                }
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PROPIEDADES: DATASOURCE ──────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Fuente de datos del ComboBox. Acepta IList, IListSource (DataTable/DataSet),
        /// IBindingList y cualquier IEnumerable — idéntico al ComboBox nativo.
        /// </summary>
        [Category("Data")]
        [Description("Fuente de datos. Acepta List<T>, DataTable, arrays, etc.")]
        [DefaultValue(null)]
        [AttributeProvider(typeof(IListSource))]
        public object DataSource
        {
            get => _dataSource;
            set
            {
                if (_dataSource == value) return;
                DetachDataSource();
                _dataSource = value;
                AttachDataSource();
                RefreshDataItems();
            }
        }

        /// <summary>
        /// Propiedad/columna cuyo valor se usa como clave (para SelectedValue).
        /// </summary>
        [Category("Data")]
        [Description("Propiedad/columna del objeto que se usa como valor clave.")]
        [DefaultValue("")]
        public string ValueMember
        {
            get => _valueMember;
            set { _valueMember = value ?? string.Empty; }
        }

        /// <summary>
        /// Valor del ítem seleccionado según ValueMember.
        /// Asignar busca el ítem cuyo ValueMember coincida.
        /// </summary>
        [Browsable(false)]
        public object SelectedValue
        {
            get
            {
                var item = SelectedItem;
                if (item == null) return null;
                return string.IsNullOrEmpty(_valueMember)
                    ? item
                    : GetMemberValue(item, _valueMember);
            }
            set
            {
                if (string.IsNullOrEmpty(_valueMember)) return;
                for (int i = 0; i < _items.Count; i++)
                {
                    if (Equals(GetMemberValue(_items[i], _valueMember), value))
                    {
                        SelectedIndex = i;
                        return;
                    }
                }
                SelectedIndex = -1;
            }
        }

        // ══════════════════════════════════════════════════════════════════
        // ── PINTURA DE LA CARA ────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 1. Fondo completo (BackColor configurable)
            using (var bg = new SolidBrush(BackColor))
                g.FillRectangle(bg, ClientRectangle);

            // 2. Texto del ítem seleccionado
            PaintSelectedText(g);

            // 3. Flecha ▼
            PaintArrow(g);

            // 4. Borde
            Color borderColor = _isFocused ? _borderFocusColor : _borderColor;
            var borderRect = new Rectangle(0, 0, Width - 1, Height - 1);
            if (_cornerRadius > 0)
                PaintRoundedBorder(g, borderRect, borderColor);
            else
                PaintSquareBorder(g, borderRect, borderColor);
        }

        private void PaintSelectedText(Graphics g)
        {
            int bw = _borderWidth;
            int padX = bw + 3;
            int iconW = _iconChar != IconChar.None ? (_iconSize + _iconPadding * 2) : 0;

            int textX = padX + (_iconAlign == IconTextBoxAlignment.Left ? iconW : 0);
            int textW = Width - padX * 2 - iconW - ArrowAreaWidth - bw;

            string text = GetDisplayText(_selectedIndex);
            var rect = new Rectangle(textX, 0, textW, Height);
            var flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left
                         | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding;

            TextRenderer.DrawText(g, text, Font, rect, ForeColor, flags);
        }

        private void PaintArrow(Graphics g)
        {
            int bw = _borderWidth;
            int arrowX = Width - ArrowAreaWidth - bw;

            // Separador
            Color sepColor = _isFocused ? _borderFocusColor : _borderColor;
            using (var pen = new Pen(sepColor))
                g.DrawLine(pen, arrowX, bw + 2, arrowX, Height - bw - 3);

            // Triángulo ▼
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

                for (int i = 0; i < _borderWidth; i++)
                    using (var pen = new Pen(color))
                        g.DrawPath(pen, path);
            }

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
        // ── INTERACCIÓN ───────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            // Ignorar clics en el ícono (ya los maneja _iconPicture.Click)
            ToggleDropdown();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.F4:
                case Keys.Space:
                    ToggleDropdown();
                    e.Handled = true;
                    break;

                case Keys.Down:
                    if (_selectedIndex < _items.Count - 1)
                        SelectedIndex = _selectedIndex + 1;
                    e.Handled = true;
                    break;

                case Keys.Up:
                    if (_selectedIndex > 0)
                        SelectedIndex = _selectedIndex - 1;
                    e.Handled = true;
                    break;

                case Keys.Home:
                    if (_items.Count > 0) SelectedIndex = 0;
                    e.Handled = true;
                    break;

                case Keys.End:
                    if (_items.Count > 0) SelectedIndex = _items.Count - 1;
                    e.Handled = true;
                    break;

                case Keys.Delete:
                case Keys.Back:
                    SelectedIndex = -1;
                    e.Handled = true;
                    break;
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
            Invalidate();
        }

        // ══════════════════════════════════════════════════════════════════
        // ── POPUP DESPLEGABLE ─────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        private void ToggleDropdown()
        {
            if (_items.Count == 0) return;

            if (_popup == null || _popup.IsDisposed)
                BuildPopup();
            else
                RefreshListBoxItems();

            if (_popup.Visible)
            {
                _popup.Close();
            }
            else
            {
                SyncListBoxSelection();
                ResizePopup();

                // Posición: debajo del control; si no cabe, encima
                Point screenPt = PointToScreen(new Point(0, Height));
                if (screenPt.Y + _popup.Height > Screen.FromControl(this).WorkingArea.Bottom)
                    screenPt = PointToScreen(new Point(0, -_popup.Height));

                _popup.Show(screenPt);
                _listBox.Focus();
                _isFocused = true;
                Invalidate();
            }
        }

        private void BuildPopup()
        {
            Font dropFont = _dropDownFont ?? Font;
            int itemH = _dropItemHeight > 0 ? _dropItemHeight : dropFont.Height + 6;

            _listBox = new ListBox
            {
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = itemH,
                BorderStyle = BorderStyle.None,
                BackColor = _dropBackColor,
                ForeColor = _dropForeColor,
                Font = dropFont,
                IntegralHeight = false
            };

            _listBox.DrawItem += OnListBoxDrawItem;
            _listBox.MouseMove += OnListBoxMouseMove;
            _listBox.MouseLeave += (_, __) => { _hoverIndex = -1; _listBox.Invalidate(); };

            _listBox.MouseClick += (_, e) =>
            {
                int idx = _listBox.IndexFromPoint(e.Location);
                if (idx < 0) return;

                if (_allowDeselect && idx == 0)
                {
                    SelectedIndex = -1;
                    _popup.Close();
                    return;
                }

                SelectedIndex = _allowDeselect ? idx - 1 : idx;
                _popup.Close();
            };

            _listBox.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Space)
                {
                    if (_listBox.SelectedIndex >= 0)
                    {
                        SelectedIndex = _listBox.SelectedIndex;
                        _popup.Close();
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    _popup.Close();
                }
            };

            RefreshListBoxItems();

            // Panel con borde de color para el desplegable
            var borderPanel = new Panel
            {
                BackColor = _dropBorderColor,
                Padding = new Padding(1)
            };
            _listBox.Dock = DockStyle.Fill;
            borderPanel.Controls.Add(_listBox);

            var host = new ToolStripControlHost(borderPanel)
            {
                Padding = Padding.Empty,
                Margin = Padding.Empty,
                AutoSize = false
            };

            _popup = new ToolStripDropDown
            {
                Padding = Padding.Empty,
                AutoClose = true
            };
            _popup.Items.Add(host);

            _popup.Closed += (_, __) =>
            {
                _isFocused = false;
                _hoverIndex = -1;
                Focus();       // devolver foco al ComboBox
                Invalidate();
            };
        }

        private void RefreshListBoxItems()
        {
            if (_listBox == null) return;
            _listBox.Items.Clear();

            if (_allowDeselect)
                _listBox.Items.Add(""); // ← entrada vacía como primer ítem

            foreach (var item in _items)
                _listBox.Items.Add(GetDisplayText(item));
        }

        private void SyncListBoxSelection()
        {
            if (_listBox == null) return;
            if (_selectedIndex >= 0 && _selectedIndex < _listBox.Items.Count)
                _listBox.SelectedIndex = _selectedIndex;
            else
                _listBox.ClearSelected();
        }

        private void ResizePopup()
        {
            if (_popup == null || _listBox == null) return;

            int itemH = _listBox.ItemHeight;
            int listH = Math.Min(_items.Count * itemH, _dropMaxHeight);
            int listW = Math.Max(Width - 2, MeasureMaxItemWidth() + 10);

            _listBox.Width = listW;
            _listBox.Height = listH;

            var borderPanel = _listBox.Parent as Panel;
            if (borderPanel != null)
            {
                borderPanel.Size = new Size(listW + 2, listH + 2);
                var host = _popup.Items[0] as ToolStripControlHost;
                if (host != null) host.Size = borderPanel.Size;
            }
        }

        private int MeasureMaxItemWidth()
        {
            int max = 0;   // ← antes era Width, lo que garantizaba siempre Width+12
            using (var g = CreateGraphics())
                foreach (var item in _items)
                {
                    int w = TextRenderer.MeasureText(g, GetDisplayText(item), Font).Width;
                    if (w > max) max = w;
                }
            return max;
        }

        private void ApplyDropColors()
        {
            if (_listBox == null) return;
            _listBox.BackColor = _dropBackColor;
            _listBox.ForeColor = _dropForeColor;
            _listBox.Invalidate();
        }

        // ──────────────────────────────────────────────────────────────────
        // Owner-draw del ListBox — control total del color de cada ítem
        // ──────────────────────────────────────────────────────────────────

        private void OnListBoxDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= _listBox.Items.Count) return;

            bool isHover = e.Index == _hoverIndex;
            bool highlight = isHover;

            Color back = highlight ? _dropHighlightBackColor : _dropBackColor;
            Color fore = highlight ? _dropHighlightForeColor : _dropForeColor;

            using (var bg = new SolidBrush(back))
                e.Graphics.FillRectangle(bg, e.Bounds);

            var textRect = new Rectangle(e.Bounds.X + 6, e.Bounds.Y,
                                          e.Bounds.Width - 6, e.Bounds.Height);
            var textFlags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left
                          | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding;

            TextRenderer.DrawText(e.Graphics,
                                  _listBox.Items[e.Index]?.ToString() ?? string.Empty,
                                  _listBox.Font, textRect, fore, textFlags);
        }

        private void OnListBoxMouseMove(object sender, MouseEventArgs e)
        {
            int idx = _listBox.IndexFromPoint(e.Location);
            if (idx != _hoverIndex)
            {
                _hoverIndex = idx;
                _listBox.Invalidate();
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
            if (!hasIcon) { _iconPicture.Visible = false; return; }

            int padX = _borderWidth + 3;
            int iconW = _iconSize + _iconPadding * 2;
            _iconPicture.Size = new Size(iconW, Height);

            _iconPicture.Location = _iconAlign == IconTextBoxAlignment.Left
                ? new Point(padX - 2, 0)
                : new Point(Width - padX - iconW - ArrowAreaWidth + 2, 0);
        }

        // ══════════════════════════════════════════════════════════════════
        // ── HELPERS ───────────────────────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        // ══════════════════════════════════════════════════════════════
        // ── DATASOURCE: BIND / REFRESH ────────────────────────────────────
        // ══════════════════════════════════════════════════════════════════

        private void AttachDataSource()
        {
            if (_dataSource is System.ComponentModel.IBindingList bl)
                bl.ListChanged += OnDataSourceListChanged;
        }

        private void DetachDataSource()
        {
            if (_dataSource is System.ComponentModel.IBindingList bl)
                bl.ListChanged -= OnDataSourceListChanged;
        }

        private void OnDataSourceListChanged(object sender,
            System.ComponentModel.ListChangedEventArgs e)
        {
            RefreshDataItems();
        }

        /// <summary>
        /// Repopula _items desde _dataSource.
        /// Soporta IListSource (DataTable), IList (List&lt;T&gt;, arrays) e IEnumerable.
        /// </summary>
        private void RefreshDataItems()
        {
            _refreshingDataSource = true;
            try
            {
                _items.Clear();

                if (_dataSource == null) return;

                System.Collections.IList list = null;

                if (_dataSource is System.ComponentModel.IListSource ls)
                    list = ls.GetList();
                else if (_dataSource is System.Collections.IList ilist)
                    list = ilist;

                if (list != null)
                {
                    foreach (var item in list)
                        _items.Add(item);
                }
                else if (_dataSource is System.Collections.IEnumerable en)
                {
                    foreach (var item in en)
                        _items.Add(item);
                }
            }
            finally
            {
                _refreshingDataSource = false;
            }

            // Ajustar selección y refrescar UI una sola vez al final
            if (_selectedIndex >= _items.Count)
                _selectedIndex = _items.Count > 0 ? 0 : -1;

            RefreshListBoxItems();
            Invalidate();
        }

        /// <summary>
        /// Obtiene el valor de una propiedad/columna de un objeto.
        /// Compatible con DataRowView (DataTable) y objetos POCO.
        /// </summary>
        private static object GetMemberValue(object item, string member)
        {
            if (item == null || string.IsNullOrEmpty(member)) return item;

            // DataTable binding
            if (item is System.Data.DataRowView drv)
            {
                try { return drv[member]; } catch { return item; }
            }

            // POCO via reflection
            var prop = item.GetType().GetProperty(member);
            return prop?.GetValue(item) ?? item;
        }

        internal void OnItemsChanged()
        {
            // Durante RefreshDataItems() las notificaciones individuales se omiten;
            // el método hace un único refresh al final para evitar trabajo redundante.
            if (_refreshingDataSource) return;

            if (_selectedIndex >= _items.Count)
                _selectedIndex = _items.Count - 1;

            RefreshListBoxItems();
            Invalidate();
        }

        private string GetDisplayText(int index)
        {
            if (index < 0 || index >= _items.Count) return string.Empty;
            return GetDisplayText(_items[index]);
        }

        private string GetDisplayText(object item)
        {
            if (item == null) return string.Empty;
            if (string.IsNullOrEmpty(_displayMember)) return item.ToString();

            // DataTable / DataView binding: los ítems son DataRowView
            if (item is System.Data.DataRowView drv)
            {
                try { return drv[_displayMember]?.ToString() ?? string.Empty; }
                catch { return item.ToString(); }
            }

            // POCO: reflection
            var prop = item.GetType().GetProperty(_displayMember);
            return prop?.GetValue(item)?.ToString() ?? item.ToString();
        }

        // ══════════════════════════════════════════════════════════════════
        // ── FONT / FORECOLOR / BACKCOLOR propagados ───────────────────────
        // ══════════════════════════════════════════════════════════════════

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (_listBox != null && _dropDownFont == null)
                _listBox.Font = Font;
        }

        // ══════════════════════════════════════════════════════════════════
        // Dispose
        // ══════════════════════════════════════════════════════════════════
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _popup?.Dispose();  // el popup sí se libera, el bitmap no
            }
            base.Dispose(disposing);
        }
    }

    // ══════════════════════════════════════════════════════════════════════
    //  ComboBoxItemCollection
    //  Colección observable que notifica al IconComboBox cuando cambian los
    //  ítems (Add, Remove, Clear, Insert, indexador).
    // ══════════════════════════════════════════════════════════════════════
    public class ComboBoxItemCollection : Collection<object>
    {
        private readonly IconComboBox _owner;

        internal ComboBoxItemCollection(IconComboBox owner) { _owner = owner; }

        /// <summary>Agrega varios ítems de una vez.</summary>
        public void AddRange(object[] items)
        {
            foreach (var item in items) Add(item);
        }

        protected override void InsertItem(int index, object item)
        {
            base.InsertItem(index, item);
            _owner.OnItemsChanged();
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            _owner.OnItemsChanged();
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            _owner.OnItemsChanged();
        }

        protected override void SetItem(int index, object item)
        {
            base.SetItem(index, item);
            _owner.OnItemsChanged();
        }
    }
}