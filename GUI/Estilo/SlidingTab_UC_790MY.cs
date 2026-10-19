using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Service_08YS;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Selector deslizante (Etapa 4, E4-D7): una pista con N opciones y un indicador bordó
    /// que se desliza hasta la opción elegida (~200 ms; sin movimiento si Windows tiene las
    /// animaciones desactivadas). Se maneja con clic/toque o, con foco, con las flechas,
    /// Inicio y Fin. Los textos salen de idiomas.json a partir de <see cref="ClavesIdioma"/>.
    /// Todas las medidas se derivan de la fuente y del DPI del control.
    /// </summary>
    public partial class SlidingTab_UC_790MY : UserControl
    {
        private const int DuracionAnimacionMs = 200;

        private string[] _claves = new string[0];
        private string[] _textos = new string[0];
        private int _indice;
        private int _indiceHover = -1;

        // Animación del indicador: posición en "unidades de opción" (0 = primera opción).
        private float _posicion;
        private float _posicionOrigen;
        private readonly Stopwatch _reloj = new Stopwatch();

        public SlidingTab_UC_790MY()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable, true);
        }

        /// <summary>Se dispara cuando el usuario (o el código) cambia la opción elegida.</summary>
        public event EventHandler SeleccionCambiada;

        /// <summary>Claves de idiomas.json de cada opción, en orden.</summary>
        [Category("Fogon"), Description("Claves de idiomas.json de cada opción, en orden.")]
        public string[] ClavesIdioma
        {
            get => _claves;
            set
            {
                _claves = value ?? new string[0];
                ActualizarTextos();
                if (_indice >= _claves.Length) _indice = Math.Max(0, _claves.Length - 1);
                _posicion = _indice;
                Invalidate();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int IndiceSeleccionado
        {
            get => _indice;
            set
            {
                if (_claves.Length == 0) return;
                int nuevo = Math.Max(0, Math.Min(_claves.Length - 1, value));
                if (nuevo == _indice) return;
                _indice = nuevo;
                IniciarDeslizamiento();
                AccessibilityNotifyClients(AccessibleEvents.Selection, _indice);
                SeleccionCambiada?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>Vuelve a leer los textos de idiomas.json (llamar desde UpdateIdioma del formulario).</summary>
        public void ActualizarTextos()
        {
            _textos = _claves.Select(Traducir).ToArray();
            AccessibleName = _textos.Length > 0 ? _textos[_indice] : string.Empty;
            Invalidate();
        }

        // En el diseñador de Visual Studio no hay idiomas.json cargado: se muestra la clave.
        private string Traducir(string clave)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return clave;
            try { return TraductorManager_08YS.Instance.GetTexto(clave)?.Trim() ?? clave; }
            catch (Exception) { return clave; }
        }

        #region Animación

        private void IniciarDeslizamiento()
        {
            AccessibleName = _textos.Length > _indice ? _textos[_indice] : string.Empty;
            if (!SystemInformation.UIEffectsEnabled || !IsHandleCreated || !Visible)
            {
                tmrDeslizar_790MY.Stop();
                _posicion = _indice;
                Invalidate();
                return;
            }
            _posicionOrigen = _posicion;
            _reloj.Restart();
            tmrDeslizar_790MY.Start();
        }

        private void tmrDeslizar_790MY_Tick(object sender, EventArgs e)
        {
            float t = Math.Min(1f, _reloj.ElapsedMilliseconds / (float)DuracionAnimacionMs);
            float suavizado = 1f - (1f - t) * (1f - t) * (1f - t);   // ease-out cúbico
            _posicion = _posicionOrigen + (_indice - _posicionOrigen) * suavizado;
            if (t >= 1f)
            {
                _posicion = _indice;
                tmrDeslizar_790MY.Stop();
                _reloj.Reset();
            }
            Invalidate();
        }

        #endregion

        #region Teclado y mouse

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                case Keys.Home:
                case Keys.End:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                    IndiceSeleccionado = _indice - 1; e.Handled = true; break;
                case Keys.Right:
                case Keys.Down:
                    IndiceSeleccionado = _indice + 1; e.Handled = true; break;
                case Keys.Home:
                    IndiceSeleccionado = 0; e.Handled = true; break;
                case Keys.End:
                    IndiceSeleccionado = _claves.Length - 1; e.Handled = true; break;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            Focus();
            int indice = IndiceEn(e.Location);
            if (indice >= 0) IndiceSeleccionado = indice;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int indice = IndiceEn(e.Location);
            if (indice == _indiceHover) return;
            _indiceHover = indice;
            Cursor = indice >= 0 && indice != _indice ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _indiceHover = -1;
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }

        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }

        protected override void OnEnabledChanged(EventArgs e) { base.OnEnabledChanged(e); Invalidate(); }

        protected override void OnFontChanged(EventArgs e) { base.OnFontChanged(e); Invalidate(); }

        private int IndiceEn(Point p)
        {
            Rectangle pista = Pista();
            if (_claves.Length == 0 || !pista.Contains(p)) return -1;
            float ancho = pista.Width / (float)_claves.Length;
            return Math.Max(0, Math.Min(_claves.Length - 1, (int)((p.X - pista.X) / ancho)));
        }

        #endregion

        #region Dibujo

        private float Escala => DeviceDpi / 96f;

        private Rectangle Pista()
        {
            Rectangle r = ClientRectangle;
            r.Inflate(-1, -1);
            return r;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Parent?.BackColor ?? T.Tarjeta);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle pista = Pista();
            if (pista.Width <= 0 || pista.Height <= 0 || _claves.Length == 0) return;

            int radio = (int)Math.Round(T.RadioControl * Escala);
            int interior = Math.Max(2, (int)Math.Round(3 * Escala));
            float anchoOpcion = pista.Width / (float)_claves.Length;

            Color fondoPista = Enabled ? T.Panel : T.Deshabilitado;
            Color bordePista = Enabled ? T.Linea : T.Deshabilitado;
            Color indicador = Enabled ? T.Accion : T.Linea;

            using (GraphicsPath ruta = Redondeado(pista, radio))
            using (var pincel = new SolidBrush(fondoPista))
            using (var lapiz = new Pen(bordePista))
            {
                g.FillPath(pincel, ruta);
                g.DrawPath(lapiz, ruta);
            }

            // Hover de una opción no elegida: superficie suave.
            if (Enabled && _indiceHover >= 0 && _indiceHover != _indice)
            {
                RectangleF hover = RectOpcion(pista, anchoOpcion, _indiceHover, interior);
                using (GraphicsPath ruta = Redondeado(Rectangle.Round(hover), radio))
                using (var pincel = new SolidBrush(T.AccionSuave))
                    g.FillPath(pincel, ruta);
            }

            RectangleF rectIndicador = RectOpcion(pista, anchoOpcion, _posicion, interior);
            using (GraphicsPath ruta = Redondeado(Rectangle.Round(rectIndicador), radio))
            using (var pincel = new SolidBrush(indicador))
                g.FillPath(pincel, ruta);

            for (int i = 0; i < _claves.Length; i++)
            {
                Rectangle celda = Rectangle.Round(RectOpcion(pista, anchoOpcion, i, interior));
                bool elegida = i == _indice;
                Color texto = !Enabled ? (elegida ? T.Tinta : T.TextoDeshabilitado)
                                       : (elegida ? Color.White : T.Accion);
                string etiqueta = i < _textos.Length ? _textos[i] : _claves[i];
                TextRenderer.DrawText(g, etiqueta, Font, celda, texto,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
            }

            // Foco visible (P10): anillo dorado sobre la opción elegida.
            if (Focused && ShowFocusCues && Enabled)
            {
                Rectangle foco = Rectangle.Round(RectOpcion(pista, anchoOpcion, _indice, interior));
                int separacion = Math.Max(2, (int)Math.Round(2 * Escala));
                foco.Inflate(-separacion, -separacion);
                using (GraphicsPath ruta = Redondeado(foco, Math.Max(2, radio - separacion)))
                using (var lapiz = new Pen(T.Oro, Math.Max(2f, 2f * Escala)))
                    g.DrawPath(lapiz, ruta);
            }
        }

        private static RectangleF RectOpcion(Rectangle pista, float anchoOpcion, float posicion, int interior)
            => new RectangleF(pista.X + posicion * anchoOpcion + interior, pista.Y + interior,
                              anchoOpcion - 2 * interior, pista.Height - 2 * interior);

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

        protected override AccessibleObject CreateAccessibilityInstance() => new SlidingTabAccesible(this);

        /// <summary>Expone el selector como lista de pestañas, con la opción elegida como valor.</summary>
        private sealed class SlidingTabAccesible : ControlAccessibleObject
        {
            private readonly SlidingTab_UC_790MY _dueno;

            public SlidingTabAccesible(SlidingTab_UC_790MY dueno) : base(dueno) { _dueno = dueno; }

            public override AccessibleRole Role => AccessibleRole.PageTabList;

            public override string Value
                => _dueno._textos.Length > _dueno._indice ? _dueno._textos[_dueno._indice] : string.Empty;
        }
    }
}
