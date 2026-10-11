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
        // Los colores de los controles estáticos se configuran en el Designer;
        // aquí solo queda el que usan los manejadores de pintura.
        private static readonly Color ClrCremaOscura   = Color.FromArgb(238, 233, 225);

        // ── BLL & state ──────────────────────────────────────────────────
        private readonly MesaBLL_790MY _mesaBll;
        private readonly DateTime      _fecha;
        private readonly TimeSpan      _hora;
        private readonly int           _comensales;

        private MesaVisualControl_790MY _ctrlSeleccionado;
        public Mesa_790MY MesaSeleccionada { get; private set; }

        // ── Controles ─────────────────────────────────────────────────────
        // Todos los controles se declaran y configuran en
        // FormSeleccionarMesa_790MY.Designer.cs. Aquí solo se agrupan las
        // etiquetas de la leyenda para poder traducirlas en bloque.
        private Label[] _lblsLeyenda;

        // ── Constructor ───────────────────────────────────────────────────
        public FormSeleccionarMesa_790MY(DateTime fecha, TimeSpan hora, int comensales)
        {
            _fecha      = fecha;
            _hora       = hora;
            _comensales = comensales;
            _mesaBll    = BLLFactory_790MY.CreateMesaBLL();

            InitializeComponent();

            _lblsLeyenda = new[]
            {
                _lblLeyendaRecomendada,
                _lblLeyendaDisponible,
                _lblLeyendaOcupada,
                _lblLeyendaIncompatible,
                _lblLeyendaSeleccionada
            };

            // Fallback: grilla geométrica si la imagen de fondo no está disponible.
            if (_pnlSalon.BackgroundImage == null)
            {
                _pnlSalon.BackColor = Color.FromArgb(248, 245, 240);
                _pnlSalon.Paint    += PintarFondoSalon;
            }

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
            UpdateIdioma();
        }

        private void FormSeleccionarMesa_790MY_Load(object sender, EventArgs e)
        {
            UpdateIdioma();
            CargarMesasMapa();
        }

        private void FormSeleccionarMesa_790MY_Shown(object sender, EventArgs e)
        {
            CargarMesasMapa();
        }

        // ── Eventos visuales del panel lateral ────────────────────────────
        private void _pnlDetalle_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(ClrCremaOscura, 1f))
                e.Graphics.DrawLine(pen, 0, 0, 0, _pnlDetalle.Height);
        }

        private void _pnlDetalle_Resize(object sender, EventArgs e)
        {
            _btnAceptar.Location  = new Point(15, _pnlDetalle.Height - 130);
            _btnCancelar.Location = new Point(15, _pnlDetalle.Height - 80);
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
