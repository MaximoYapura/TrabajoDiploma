using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using GUI_08YS.Properties;
using Service_08YS;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Fila «Ayuda [F1]» de la barra lateral (Etapa 5, G1): sombrero del chef, texto y tecla
    /// dibujada. Se ve y se comporta como los IconButton de la barra: hover, presionado,
    /// foco con Tab + Enter/Espacio y estado deshabilitado (cuando el cobro bloquea la barra).
    /// Un clic en cualquier parte (imagen, texto o tecla) dispara <see cref="Control.Click"/>.
    /// </summary>
    public partial class FilaAyuda_790MY : UserControl, IIdiomaObserver_08YS
    {
        private bool _hover;
        private bool _presionado;
        private Image _imagen;
        private Image _imagenAtenuada;

        public FilaAyuda_790MY()
        {
            InitializeComponent();
            _imagen = ElegirSombrero(DeviceDpi);
            AplicarEstado();
        }

        /// <summary>Recurso nativo para la escala (32/40/48/64 px): reducir el maestro de 512 px pixela.</summary>
        private static Image ElegirSombrero(int dpi)
        {
            if (dpi <= 96) return Resources.SombreroAyuda32;
            if (dpi <= 120) return Resources.SombreroAyuda40;
            if (dpi <= 144) return Resources.SombreroAyuda48;
            return Resources.SombreroAyuda64;
        }

        public void UpdateIdioma()
        {
            var tr = TraductorManager_08YS.Instance;
            lblTexto_790MY.Text = tr.GetTexto("AY_fila_texto");
            AccessibleName = tr.GetTexto("AY_fila_accesible");
            string tip = tr.GetTexto("AY_fila_tooltip");
            ttAyuda_790MY.SetToolTip(this, tip);
            foreach (Control hijo in new Control[] { tlpFila_790MY, picSombrero_790MY, lblTexto_790MY, lblTecla_790MY })
                ttAyuda_790MY.SetToolTip(hijo, tip);
        }

        #region Estado visual

        private void AplicarEstado()
        {
            bool habilitado = Enabled;
            Color fondo = !habilitado ? T.BarraFondo
                        : _presionado ? T.BarraPresionado
                        : _hover ? T.BarraHover
                        : T.BarraFondo;
            Color tinta = habilitado ? T.BarraTinta : T.TextoDeshabilitado;

            if (BackColor != fondo)
            {
                BackColor = fondo;
                tlpFila_790MY.BackColor = fondo;
            }
            lblTexto_790MY.ForeColor = tinta;
            lblTecla_790MY.ForeColor = tinta;

            Image img = habilitado ? _imagen : ImagenAtenuada();
            if (!ReferenceEquals(picSombrero_790MY.Image, img))
                picSombrero_790MY.Image = img;
        }

        /// <summary>El sombrero recoloreado con TextoDeshabilitado (se conserva la transparencia).</summary>
        private Image ImagenAtenuada()
        {
            if (_imagenAtenuada != null) return _imagenAtenuada;
            var c = T.TextoDeshabilitado;
            var matriz = new ColorMatrix(new[]
            {
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { c.R / 255f, c.G / 255f, c.B / 255f, 0, 1 },
            });
            var bmp = new Bitmap(_imagen.Width, _imagen.Height);
            using (var g = Graphics.FromImage(bmp))
            using (var attr = new ImageAttributes())
            {
                attr.SetColorMatrix(matriz);
                g.DrawImage(_imagen, new Rectangle(0, 0, bmp.Width, bmp.Height),
                            0, 0, _imagen.Width, _imagen.Height, GraphicsUnit.Pixel, attr);
            }
            _imagenAtenuada = bmp;
            return bmp;
        }

        private bool CursorDentro() => ClientRectangle.Contains(PointToClient(Cursor.Position));

        private void Entrar()
        {
            if (_hover) return;
            _hover = true;
            AplicarEstado();
        }

        // Pasar de la imagen al texto dispara MouseLeave en uno y MouseEnter en otro: el hover
        // solo se quita si el cursor salió de la fila (sin parpadeo).
        private void Salir()
        {
            if (CursorDentro()) return;
            _hover = false;
            _presionado = false;
            AplicarEstado();
        }

        private void Presionar(MouseButtons boton)
        {
            if (boton != MouseButtons.Left) return;
            _presionado = true;
            AplicarEstado();
        }

        private void Soltar()
        {
            if (!_presionado) return;
            _presionado = false;
            _hover = CursorDentro();
            AplicarEstado();
        }

        #endregion

        #region Eventos de los hijos (suscritos en el Designer)

        private void Hijo_Click(object sender, EventArgs e) => OnClick(e);
        private void Hijo_MouseEnter(object sender, EventArgs e) => Entrar();
        private void Hijo_MouseLeave(object sender, EventArgs e) => Salir();
        private void Hijo_MouseDown(object sender, MouseEventArgs e) => Presionar(e.Button);
        private void Hijo_MouseUp(object sender, MouseEventArgs e) => Soltar();

        #endregion

        #region Eventos propios

        protected override void OnMouseEnter(EventArgs e) { Entrar(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { Salir(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { Presionar(e.Button); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { Soltar(); base.OnMouseUp(e); }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (!Enabled) { _hover = false; _presionado = false; }
            AplicarEstado();
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

        protected override bool IsInputKey(Keys keyData)
            => keyData == Keys.Enter || keyData == Keys.Space || base.IsInputKey(keyData);

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                OnClick(EventArgs.Empty);
                return;
            }
            base.OnKeyDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Borde de 1 px como el de los IconButton (FlatStyle.Flat toma el ForeColor).
            using (var lapiz = new Pen(T.BarraTinta))
                e.Graphics.DrawRectangle(lapiz, 0, 0, Width - 1, Height - 1);

            // Recuadro de foco visible en el margen libre (la tabla interna no lo tapa).
            if (Focused && ShowFocusCues && Enabled)
            {
                using (var foco = new Pen(T.Oro, 2))
                    e.Graphics.DrawRectangle(foco, 2, 2, Width - 4, Height - 4);
            }
        }

        #endregion
    }
}
