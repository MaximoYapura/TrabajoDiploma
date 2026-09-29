using BE_08YS;
using BLL_08YS;
using Service_08YS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI_08YS.RF1
{
    public partial class FormSeleccionarMesa_790MY : Form, IIdiomaObserver_08YS
    {
        // ── Palette ──────────────────────────────────────────────────────
        private static readonly Color ClrBordoProfundo = Color.FromArgb(56, 17, 17);
        private static readonly Color ClrBordoPrimario = Color.FromArgb(120, 25, 25);
        private static readonly Color ClrCrema         = Color.FromArgb(250, 248, 245);
        private static readonly Color ClrCremaOscura   = Color.FromArgb(238, 233, 225);
        private static readonly Color ClrCobre         = Color.FromArgb(211, 84, 0);
        private static readonly Color ClrTextoOscuro   = Color.FromArgb(45, 42, 40);
        private static readonly Color ClrTextoDorado   = Color.FromArgb(253, 246, 227);
        private static readonly Color ClrVerde1        = Color.FromArgb(39, 174, 96);
        private static readonly Color ClrVerde2        = Color.FromArgb(80, 140, 70);
        private static readonly Color ClrGris          = Color.FromArgb(160, 155, 148);

        // ── BLL & state ──────────────────────────────────────────────────
        private readonly MesaBLL_790MY _mesaBll;
        private readonly DateTime      _fecha;
        private readonly TimeSpan      _hora;
        private readonly int           _comensales;

        private MesaVisualControl_790MY _ctrlSeleccionado;
        public Mesa_790MY MesaSeleccionada { get; private set; }

        // ── Programmatic controls ─────────────────────────────────────────
        private DoubleBufferedPanel _pnlSalon;
        private Panel  _pnlDetalle;
        private Label  _lblTituloHeader;
        private Label  _lblSubtitulo;
        private Label  _lblLeyendaTitulo;
        private Label[] _lblsLeyenda;
        private Label  _lblInfoTituloDetalle;
        private Label  _lblInfoNumero;
        private Label  _lblInfoCapacidad;
        private Label  _lblInfoEstado;
        private Label  _lblPista;
        private Button _btnAceptar;
        private Button _btnCancelar;
        private Panel  _pnlFooter;
        private Label  _lblFooterInfo;

        // ── Constructor ───────────────────────────────────────────────────
        public FormSeleccionarMesa_790MY(DateTime fecha, TimeSpan hora, int comensales)
        {
            _fecha      = fecha;
            _hora       = hora;
            _comensales = comensales;
            _mesaBll    = BLLFactory_790MY.CreateMesaBLL();

            InitializeComponent();

            // Icono corporativo (seguro: el recurso puede no existir en la Build actual)
            try
            {
                var icon = Properties.Resources.ResourceManager
                               .GetObject("FogonIcon") as System.Drawing.Icon;
                if (icon != null) this.Icon = icon;
            }
            catch { }
            ConstruirLayout();

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
            this.Load  += FormSeleccionarMesa_790MY_Load;
            this.Shown += (s, e) => CargarMesasMapa();
            UpdateIdioma();
        }

        private void FormSeleccionarMesa_790MY_Load(object sender, EventArgs e)
        {
            UpdateIdioma();
            CargarMesasMapa();
        }

        // ── Layout ────────────────────────────────────────────────────────
        private void ConstruirLayout()
        {
            // ── Dimensiones del formulario modal ──────────────────────────
            this.Size          = new Size(1180, 820);
            this.MinimumSize   = new Size(1180, 820);
            this.StartPosition = FormStartPosition.CenterParent;

            // ── Header bordó corporativo (#4A151B) ────────────────────────
            var pnlHeader = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 60,
                BackColor = ColorTranslator.FromHtml("#4A151B")
            };

            // Logo corporativo (izquierda, 62×62)
            var pbLogo = new PictureBox
            {
                Size      = new Size(62, 62),
                Location  = new Point(10, 9),
                SizeMode  = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            try { pbLogo.Image = Properties.Resources.LogoElFogon; } catch { }
            pnlHeader.Controls.Add(pbLogo);

            // Divisor vertical semitransparente
            var pnlDivisor = new Panel
            {
                BackColor = Color.FromArgb(90, 255, 255, 255),
                Size      = new Size(1, 52),
                Location  = new Point(78, 14)
            };
            pnlHeader.Controls.Add(pnlDivisor);

            // Título (blanco)
            _lblTituloHeader = new Label
            {
                Text      = TraductorManager_08YS.Instance.GetTexto("msg_seleccionar_mesa_titulo")
                            ?? "Seleccionar Mesa",
                Font      = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize  = true,
                Location  = new Point(88, 12)
            };
            pnlHeader.Controls.Add(_lblTituloHeader);

            // Subtítulo estático (dorado)
            _lblSubtitulo = new Label
            {
                Text      = TraductorManager_08YS.Instance.GetTexto("msg_subtitulo_seleccionar_mesa")
                            ?? "Elegí la mesa que mejor se adapte a tu reserva",
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                ForeColor = Color.FromArgb(253, 215, 128),
                AutoSize  = true,
                Location  = new Point(90, 44)
            };
            pnlHeader.Controls.Add(_lblSubtitulo);

            this.Controls.Add(pnlHeader);
            pnlHeader.SendToBack();   // el header queda al fondo del z-order del form

            // ── Footer bar (Bottom) con datos del turno ───────────────────
            _pnlFooter = new Panel
            {
                Dock      = DockStyle.Bottom,
                Height    = 35,
                BackColor = ColorTranslator.FromHtml("#1A1A1A")
            };
            _lblFooterInfo = new Label
            {
                Text      = FormatarSubtitulo(),
                Font      = new Font("Segoe UI", 8.5f),
                ForeColor = Color.FromArgb(220, 200, 170),
                AutoSize  = true,
                Location  = new Point(15, 8)
            };
            _pnlFooter.Controls.Add(_lblFooterInfo);
            this.Controls.Add(_pnlFooter);
            _pnlFooter.SendToBack();  // el footer queda al fondo del z-order del form

            // Body panel (Fill — debe agregarse después de Top y Bottom)
            var pnlBody = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = ClrCrema
            };
            this.Controls.Add(pnlBody);

            // Detail panel (right)
            _pnlDetalle = new Panel
            {
                Dock      = DockStyle.Right,
                Width     = 240,
                Padding   = new Padding(10),
                BackColor = Color.White
            };
            _pnlDetalle.Paint += (s, e) =>
            {
                using (var pen = new Pen(ClrCremaOscura, 1f))
                    e.Graphics.DrawLine(pen, 0, 0, 0, _pnlDetalle.Height);
            };

            // Leyenda title
            _lblLeyendaTitulo = new Label
            {
                Text      = TraductorManager_08YS.Instance.GetTexto("SM_leyenda_titulo") ?? "Leyenda",
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = ClrTextoOscuro,
                AutoSize  = true,
                Location  = new Point(15, 15)
            };
            _pnlDetalle.Controls.Add(_lblLeyendaTitulo);

            var leyendaColors = new Color[]
            {
                ClrVerde1,
                ClrVerde2,
                Color.FromArgb(120, 25, 25),
                ClrGris,
                ClrCobre
            };
            var t18n = TraductorManager_08YS.Instance;
            var leyendaTextos = new string[]
            {
                t18n.GetTexto("SM_leyenda_recomendada")  ?? "⭐ Recomendada",
                t18n.GetTexto("SM_leyenda_disponible")   ?? "Disponible",
                t18n.GetTexto("SM_leyenda_ocupada")      ?? "Ocupada / Reservada",
                t18n.GetTexto("SM_leyenda_incompatible") ?? "Incompatible",
                t18n.GetTexto("SM_leyenda_seleccionada") ?? "Seleccionada",
            };

            _lblsLeyenda = new Label[leyendaColors.Length];
            for (int i = 0; i < leyendaColors.Length; i++)
            {
                int y = 45 + i * 25;
                var dot = new Panel
                {
                    BackColor = leyendaColors[i],
                    Size      = new Size(10, 10),
                    Location  = new Point(15, y + 2)
                };
                var lbl = new Label
                {
                    Text      = leyendaTextos[i],
                    Font      = new Font("Segoe UI", 8f),
                    ForeColor = ClrTextoOscuro,
                    AutoSize  = true,
                    Location  = new Point(30, y)
                };
                _pnlDetalle.Controls.Add(dot);
                _pnlDetalle.Controls.Add(lbl);
                _lblsLeyenda[i] = lbl;
            }

            var pnlSep = new Panel
            {
                BackColor = ClrCremaOscura,
                Size      = new Size(200, 1),
                Location  = new Point(15, 170)
            };
            _pnlDetalle.Controls.Add(pnlSep);

            _lblInfoTituloDetalle = new Label
            {
                Text      = TraductorManager_08YS.Instance.GetTexto("SM_info_mesa_titulo") ?? "Mesa seleccionada:",
                Font      = new Font("Segoe UI", 8f, FontStyle.Bold),
                ForeColor = ClrTextoOscuro,
                AutoSize  = true,
                Location  = new Point(15, 185)
            };
            _pnlDetalle.Controls.Add(_lblInfoTituloDetalle);

            _lblInfoNumero = new Label
            {
                Text      = "—",
                Font      = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = ClrCobre,
                AutoSize  = true,
                Location  = new Point(15, 203)
            };
            _pnlDetalle.Controls.Add(_lblInfoNumero);

            _lblInfoCapacidad = new Label
            {
                Text      = "",
                Font      = new Font("Segoe UI", 9f),
                ForeColor = ClrTextoOscuro,
                AutoSize  = true,
                Location  = new Point(15, 233)
            };
            _pnlDetalle.Controls.Add(_lblInfoCapacidad);

            _lblInfoEstado = new Label
            {
                Text      = "",
                Font      = new Font("Segoe UI", 8f, FontStyle.Italic),
                ForeColor = ClrTextoOscuro,
                AutoSize  = true,
                Location  = new Point(15, 251)
            };
            _pnlDetalle.Controls.Add(_lblInfoEstado);

            _lblPista = new Label
            {
                Text      = TraductorManager_08YS.Instance.GetTexto("msg_sm_falta_seleccion") ?? "Seleccioná una mesa del plano",
                Font      = new Font("Segoe UI", 7.5f, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize  = false,
                Width     = 200,
                Location  = new Point(15, 273)
            };
            _pnlDetalle.Controls.Add(_lblPista);

            _btnAceptar = new Button
            {
                Location  = new Point(15, _pnlDetalle.Height - 90),
                Size      = new Size(200, 36),
                BackColor = ClrBordoPrimario,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Tag       = "btn_aceptar",
                Text      = "Confirmar Mesa",
                Enabled   = false
            };
            _btnAceptar.FlatAppearance.BorderSize = 0;
            _btnAceptar.Click += _btnAceptar_Click;
            _pnlDetalle.Controls.Add(_btnAceptar);

            _btnCancelar = new Button
            {
                Location              = new Point(15, _pnlDetalle.Height - 40),
                Size                  = new Size(200, 36),
                Tag                   = "btn_cancelar",
                Text                  = "Cancelar",
                UseVisualStyleBackColor = true
            };
            _btnCancelar.Click += _btnCancelar_Click;
            _pnlDetalle.Controls.Add(_btnCancelar);

            _pnlDetalle.Resize += (s, e) =>
            {
                _btnAceptar.Location  = new Point(15, _pnlDetalle.Height - 130);
                _btnCancelar.Location = new Point(15, _pnlDetalle.Height - 80);
            };

            pnlBody.Controls.Add(_pnlDetalle);
            _pnlDetalle.BringToFront();   // el panel lateral queda sobre el fondo

            // ── Salon panel (fill) con imagen de fondo ────────────────────
            // AutoScroll = false: la grilla 4×3 cabe en 660 px (3 filas × 155 px + 40 px inicial).
            // Con AutoScroll activo, WinForms estira BackgroundImage sobre la DisplayRectangle
            // (área virtual del scroll) en lugar del ClientRectangle visible, provocando recorte.
            _pnlSalon = new DoubleBufferedPanel
            {
                Dock      = DockStyle.Fill,
                BackColor = ClrCrema
            };

            // Cargar imagen de fondo desde recursos embebidos.
            // DoubleBufferedPanel.OnPaintBackground la dibuja sobre ClientRectangle completo,
            // asegurando que la textura cubre homogéneamente el ancho del salón.
            try
            {
                _pnlSalon.BackgroundImage = Properties.Resources.FondoRestauran;
                // No asignar BackgroundImageLayout: la pintura manual de OnPaintBackground
                // reemplaza el escalado automático de WinForms (que causaba el recorte).
            }
            catch
            {
                // Fallback: grilla geométrica si el recurso no está disponible
                _pnlSalon.BackColor = Color.FromArgb(248, 245, 240);
                _pnlSalon.Paint    += PintarFondoSalon;
            }

            pnlBody.Controls.Add(_pnlSalon);
            _pnlSalon.BringToFront();     // el salón ocupa el fill y queda encima
        }

        private string FormatarSubtitulo()
        {
            var t       = TraductorManager_08YS.Instance;
            string lCom = t.GetTexto("SM_footer_comensales") ?? "Comensales";
            string lFec = t.GetTexto("SM_footer_fecha")       ?? "Fecha";
            string lHor = t.GetTexto("SM_footer_hora")        ?? "Hora";
            return string.Format("{0}: {1}  |  {2}: {3}  |  {4}: {5}",
                lCom, _comensales,
                lFec, _fecha.ToString("dd/MM/yyyy"),
                lHor, _hora.ToString(@"hh\:mm"));
        }

        // ── Floor background (fallback sin imagen) ────────────────────────
        private void PintarFondoSalon(object sender, PaintEventArgs e)
        {
            var g     = e.Graphics;
            var panel = (Panel)sender;

            using (var pen = new Pen(Color.FromArgb(60, ClrCremaOscura), 1f))
            {
                for (int x = 0; x < panel.Width; x += 60)
                    g.DrawLine(pen, x, 0, x, panel.Height);
                for (int y = 0; y < panel.Height; y += 60)
                    g.DrawLine(pen, 0, y, panel.Width, y);
            }
        }

        // ── Data loading ──────────────────────────────────────────────────
        private void CargarMesasMapa()
        {
            _pnlSalon.Controls.Clear();
            _ctrlSeleccionado   = null;
            MesaSeleccionada    = null;
            _btnAceptar.Enabled = false;
            ActualizarDetalle(null);

            // 1. Llamada a BLL — puede lanzar ArgumentException por regla de negocio
            List<MesaMapaDTO_790MY> mesas;
            try
            {
                mesas = _mesaBll.ObtenerMesasMapa(_fecha, _hora, _comensales);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(
                    ex.Message,
                    TraductorManager_08YS.Instance.GetTexto("aviso_titulo") ?? "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // 2. Instanciación de controles — cada mesa se protege individualmente
            //    para que un fallo aislado no interrumpa el renderizado de las demás.
            //    Auto-Grid Layout: 3 columnas a X=50+col*185 px, filas cada 150 px desde Y=40.
            // Grilla 4×3 (hasta 12 mesas): X = 50+col*195 px, Y = 40+row*155 px
            int idx = 0;
            foreach (var dto in mesas)
            {
                try
                {
                    int col = idx % 4;
                    int row = idx / 4;
                    int px  = 110 + col * 180;
                    int py  = 120 + row * 200;   // Y=25 para la primera fila (relativo a _pnlSalon)

                    var ctrl = new MesaVisualControl_790MY(dto)
                    {
                        Location = new Point(
                            px - MesaVisualControl_790MY.MargenCtrl,
                            py - MesaVisualControl_790MY.MargenCtrl)
                    };
                    ctrl.MesaClick += CtrlMesa_Click;
                    _pnlSalon.Controls.Add(ctrl);
                }
                catch
                {
                    // Error aislado en la mesa #{dto.Mesa.NroMesa} — se omite sin bloquear el resto
                }
                idx++;
            }
        }

        // ── Selection ─────────────────────────────────────────────────────
        private void CtrlMesa_Click(object sender, MesaMapaDTO_790MY dto)
        {
            if (_ctrlSeleccionado != null)
                _ctrlSeleccionado.Seleccionada = false;

            var ctrl = (MesaVisualControl_790MY)sender;

            if (_ctrlSeleccionado == ctrl)
            {
                _ctrlSeleccionado   = null;
                MesaSeleccionada    = null;
                _btnAceptar.Enabled = false;
                ActualizarDetalle(null);
            }
            else
            {
                ctrl.Seleccionada   = true;
                _ctrlSeleccionado   = ctrl;
                MesaSeleccionada    = dto.Mesa;
                _btnAceptar.Enabled = true;
                ActualizarDetalle(dto);
            }
        }

        private void ActualizarDetalle(MesaMapaDTO_790MY dto)
        {
            if (dto == null)
            {
                _lblInfoNumero.Text    = "—";
                _lblInfoCapacidad.Text = "";
                _lblInfoEstado.Text    = "";
                _lblPista.Text         = TraductorManager_08YS.Instance.GetTexto("msg_sm_falta_seleccion")
                                         ?? "Seleccioná una mesa del plano";
            }
            else
            {
                var t = TraductorManager_08YS.Instance;
                _lblInfoNumero.Text    = "Mesa #" + dto.Mesa.NroMesa;
                _lblInfoCapacidad.Text = dto.Mesa.Capacidad + " " +
                                         (t.GetTexto("msg_personas") ?? "personas");
                _lblInfoEstado.Text    = ObtenerTextoEstado(dto.EstadoMapa);
                _lblPista.Text         = "";
            }
        }

        private static string ObtenerTextoEstado(EstadoMapa_790MY estado)
        {
            var t = TraductorManager_08YS.Instance;
            switch (estado)
            {
                case EstadoMapa_790MY.DisponibleRecomendada:
                    return t.GetTexto("msg_mejor_opcion") ?? "Mejor opción ⭐";
                case EstadoMapa_790MY.Disponible:
                    return t.GetTexto("SM_leyenda_disponible") ?? "Disponible";
                case EstadoMapa_790MY.Ocupada:
                    return t.GetTexto("SM_leyenda_ocupada") ?? "Ocupada";
                default:
                    return t.GetTexto("SM_leyenda_incompatible") ?? "Sin capacidad suficiente";
            }
        }

        // ── Buttons ───────────────────────────────────────────────────────
        private void _btnAceptar_Click(object sender, EventArgs e)
        {
            if (MesaSeleccionada == null)
            {
                MessageBox.Show(
                    TraductorManager_08YS.Instance.GetTexto("msg_sm_falta_seleccion")
                        ?? "Seleccioná una mesa del plano",
                    TraductorManager_08YS.Instance.GetTexto("aviso_titulo") ?? "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void _btnCancelar_Click(object sender, EventArgs e)
        {
            MesaSeleccionada = null;
            DialogResult     = DialogResult.Cancel;
            Close();
        }

        // ── i18n ─────────────────────────────────────────────────────────
        public void UpdateIdioma()
        {
            var t = TraductorManager_08YS.Instance;

            this.Text             = t.GetTexto("msg_seleccionar_mesa_titulo") ?? "Seleccionar Mesa";
            _lblTituloHeader.Text = t.GetTexto("msg_seleccionar_mesa_titulo") ?? "Seleccionar Mesa";
            _lblSubtitulo.Text    = t.GetTexto("msg_subtitulo_seleccionar_mesa")
                                    ?? "Elegí la mesa que mejor se adapte a tu reserva";
            _lblFooterInfo.Text   = FormatarSubtitulo();
            _btnAceptar.Text      = t.GetTexto("btn_aceptar")  ?? "Confirmar Mesa";
            _btnCancelar.Text     = t.GetTexto("btn_cancelar") ?? "Cancelar";

            _lblLeyendaTitulo.Text = t.GetTexto("SM_leyenda_titulo");
            if (_lblsLeyenda != null && _lblsLeyenda.Length == 5)
            {
                _lblsLeyenda[0].Text = t.GetTexto("SM_leyenda_recomendada");
                _lblsLeyenda[1].Text = t.GetTexto("SM_leyenda_disponible");
                _lblsLeyenda[2].Text = t.GetTexto("SM_leyenda_ocupada");
                _lblsLeyenda[3].Text = t.GetTexto("SM_leyenda_incompatible");
                _lblsLeyenda[4].Text = t.GetTexto("SM_leyenda_seleccionada");
            }
            _lblInfoTituloDetalle.Text = t.GetTexto("SM_info_mesa_titulo");

            if (MesaSeleccionada == null)
                _lblPista.Text = t.GetTexto("msg_sm_falta_seleccion")
                                 ?? "Seleccioná una mesa del plano";
        }

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                if (c is TextBox || c is RichTextBox)
                    continue;

                if (c.Tag is string clave && !string.IsNullOrWhiteSpace(clave))
                    c.Text = TraductorManager_08YS.Instance.GetTexto(clave);

                if (c.HasChildren)
                    TraducirControles(c);
            }
        }

        // ── Nested types ──────────────────────────────────────────────────
        /// <summary>
        /// Panel con DoubleBuffered y pintura de BackgroundImage sobre ClientRectangle.
        /// La sobreescritura de OnPaintBackground evita que WinForms estire la imagen
        /// sobre la DisplayRectangle (área virtual del scroll), que causaba recorte lateral.
        /// Al mantener BackgroundImage asignado, MesaVisualControl_790MY puede leer la
        /// propiedad para calcular la región correspondiente en su propia transparencia.
        /// </summary>
        private sealed class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
            {
                DoubleBuffered = true;
                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer,
                    true);
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                // Limpieza base primero (evita artefactos de re-pintado parcial).
                base.OnPaintBackground(e);

                if (BackgroundImage != null)
                {
                    // InterpolationMode.HighQualityBicubic garantiza calidad al escalar.
                    // DrawImage sobre ClientRectangle (área visible real) evita el recorte
                    // que causaba DisplayRectangle cuando AutoScroll estaba activo.
                    e.Graphics.InterpolationMode =
                        System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    e.Graphics.DrawImage(BackgroundImage, ClientRectangle);
                }
            }
        }
    }
}
