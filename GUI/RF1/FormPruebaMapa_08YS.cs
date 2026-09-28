// FormPruebaMapa_08YS.cs
// ============================================================
//  PROTOTIPO VISUAL — Plano Interactivo del Salón
//  El Fogón del Sur  |  Trabajo de Diploma
// ============================================================
//  Archivo autocontenido: no depende de BLL/BE/DAL.
//  Datos simulados por MockDTO interno.
//
//  Para probarlo directamente:
//    1. Agregá este archivo al proyecto GUI en VS.
//    2. En Program.cs cambiá Application.Run(new FormPruebaMapa_08YS());
//    3. Presioná F5.
//
//  En producción reemplazás MockDTO → MesaMapaDTO_790MY
//  y los datos hardcodeados → MesaBLL.ObtenerMesasMapa().
// ============================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace GUI_08YS.RF1
{
    // ════════════════════════════════════════════════════════════════════════
    //  MOCK — en producción vendrán de BLL_08YS
    // ════════════════════════════════════════════════════════════════════════

    internal enum EstadoMapaPrueba
    {
        DisponibleRecomendada,
        Disponible,
        Ocupada,
        Incompatible
    }

    internal class MesaMockDTO
    {
        public int             NroMesa    { get; set; }
        public int             Capacidad  { get; set; }
        public EstadoMapaPrueba EstadoMapa { get; set; }
        public int             PosicionX  { get; set; }
        public int             PosicionY  { get; set; }
        public int             Ancho      { get; set; }
        public int             Alto       { get; set; }

        public bool EsSeleccionable =>
            EstadoMapa == EstadoMapaPrueba.DisponibleRecomendada ||
            EstadoMapa == EstadoMapaPrueba.Disponible;
    }

    // ════════════════════════════════════════════════════════════════════════
    //  CONTROL VISUAL DE MESA  —  renderizado 100% GDI+
    // ════════════════════════════════════════════════════════════════════════

    internal sealed class MesaVisualControl : Control
    {
        // ── Paleta El Fogón del Sur ─────────────────────────────────────────
        private static readonly Color ClrRecomendada  = Color.FromArgb( 39, 174,  96);
        private static readonly Color ClrDisponible   = Color.FromArgb( 80, 140,  70);
        private static readonly Color ClrOcupada      = Color.FromArgb(120,  25,  25);
        private static readonly Color ClrIncompatible = Color.FromArgb(160, 155, 148);
        private static readonly Color ClrSeleccionada = Color.FromArgb(211,  84,   0);
        private static readonly Color ClrHover        = Color.FromArgb(230, 130,  20);
        private static readonly Color ClrCrema        = Color.FromArgb(250, 248, 245);
        private static readonly Color ClrSillaFondo   = Color.FromArgb(218, 212, 202);
        private static readonly Color ClrSillaBorde   = Color.FromArgb(170, 162, 150);
        private static readonly Color ClrTextoBlanco  = Color.FromArgb(255, 255, 255);
        private static readonly Color ClrTextoOscuro  = Color.FromArgb( 45,  42,  40);

        // ── Geometría ────────────────────────────────────────────────────────
        private const int Radio      = 10;   // radio esquinas redondeadas del tablero
        private const int SillaW     = 14;   // ancho de la silla
        private const int SillaH     = 9;    // alto de la silla
        private const int SillaGap   = 5;    // separación tablero–silla
        public  const int MargenCtrl = 20;   // margen extra del control para alojar sillas

        // ── Estado ───────────────────────────────────────────────────────────
        private bool _hover;
        private bool _seleccionada;

        public MesaMockDTO  Datos { get; }

        public bool Seleccionada
        {
            get => _seleccionada;
            set { _seleccionada = value; Invalidate(); }
        }

        public event EventHandler<MesaMockDTO> MesaClick;

        // ── Constructor ──────────────────────────────────────────────────────
        public MesaVisualControl(MesaMockDTO datos)
        {
            Datos = datos;

            // El control es más grande que el tablero para que las sillas
            // queden dentro del área de pintura (no recortadas).
            Size      = new Size(datos.Ancho + MargenCtrl * 2,
                                 datos.Alto  + MargenCtrl * 2);
            BackColor = ClrCrema;
            Cursor    = datos.EsSeleccionable ? Cursors.Hand : Cursors.Default;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint  |
                ControlStyles.UserPaint             |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
        }

        // ── Mouse ─────────────────────────────────────────────────────────────
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!Datos.EsSeleccionable) return;
            _hover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hover = false;
            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (Datos.EsSeleccionable)
                MesaClick?.Invoke(this, Datos);
        }

        // ── Pintura ───────────────────────────────────────────────────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode     = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            g.Clear(BackColor);

            // Rectángulo del tablero dentro del control (desplazado por el margen)
            var tablero = new Rectangle(MargenCtrl, MargenCtrl, Datos.Ancho, Datos.Alto);

            // 1. Sillas (detrás del tablero)
            DibujarSillas(g, tablero, Datos.Capacidad);

            // 2. Color según estado + interacción
            Color colorPrincipal = ObtenerColorPrincipal();
            float grosorBorde    = _seleccionada ? 3f : 1.5f;
            Color colorBorde     = _seleccionada ? ClrSeleccionada
                                 : _hover        ? ClrHover
                                 : PintarMasOscuro(colorPrincipal, 0.18f);

            // 3. Sombra suave
            using (var shadowPath = ConstruirRoundedRect(
                       new Rectangle(tablero.X + 2, tablero.Y + 3,
                                     tablero.Width, tablero.Height), Radio))
            using (var shadowBrush = new SolidBrush(Color.FromArgb(45, 0, 0, 0)))
                g.FillPath(shadowBrush, shadowPath);

            // 4. Tablero
            using (var path = ConstruirRoundedRect(tablero, Radio))
            {
                using (var fill = new SolidBrush(colorPrincipal))
                    g.FillPath(fill, path);

                using (var pen = new Pen(colorBorde, grosorBorde))
                    g.DrawPath(pen, path);
            }

            // 5. Borde interior cuando está seleccionada (cobre)
            if (_seleccionada)
            {
                var inner = new Rectangle(tablero.X + 5, tablero.Y + 5,
                                          tablero.Width - 10, tablero.Height - 10);
                using (var ip = ConstruirRoundedRect(inner, Radio - 3))
                using (var pen = new Pen(Color.FromArgb(160, ClrSeleccionada), 1.5f))
                    g.DrawPath(pen, ip);
            }

            // 6. Número de mesa
            Color colorTexto = (Datos.EstadoMapa == EstadoMapaPrueba.Incompatible)
                               ? ClrTextoOscuro : ClrTextoBlanco;

            using (var fntNum = new Font("Segoe UI", 11f, FontStyle.Bold, GraphicsUnit.Point))
            {
                string numStr = "#" + Datos.NroMesa;
                SizeF  sz     = g.MeasureString(numStr, fntNum);
                float  nx     = tablero.X + (tablero.Width  - sz.Width)  / 2f;
                float  ny     = tablero.Y + (tablero.Height - sz.Height) / 2f - 7f;
                using (var b = new SolidBrush(colorTexto))
                    g.DrawString(numStr, fntNum, b, nx, ny);
            }

            // 7. Capacidad (sub-texto)
            using (var fntCap = new Font("Segoe UI", 7.5f, FontStyle.Regular, GraphicsUnit.Point))
            {
                string capStr = Datos.Capacidad + "p";
                SizeF  sz     = g.MeasureString(capStr, fntCap);
                float  cx     = tablero.X + (tablero.Width  - sz.Width)  / 2f;
                float  cy     = tablero.Y + (tablero.Height - sz.Height) / 2f + 8f;
                using (var b = new SolidBrush(Color.FromArgb(200, colorTexto)))
                    g.DrawString(capStr, fntCap, b, cx, cy);
            }

            // 8. Estrella para recomendada
            if (Datos.EstadoMapa == EstadoMapaPrueba.DisponibleRecomendada)
            {
                DibujarEstrella(g,
                    tablero.Right - 14,
                    tablero.Top   + 5,
                    7, Color.FromArgb(255, 220, 60));
            }

            // 9. Icono de candado para ocupada
            if (Datos.EstadoMapa == EstadoMapaPrueba.Ocupada)
            {
                DibujarCandado(g, tablero.Right - 16, tablero.Top + 5);
            }
        }

        // ── Sillas ────────────────────────────────────────────────────────────

        private void DibujarSillas(Graphics g, Rectangle tablero, int capacidad)
        {
            (int arr, int ab, int izq, int der) = DistribuirPorLado(capacidad);

            DibujarSillasEnLado(g, tablero, arr, 0);   // Arriba
            DibujarSillasEnLado(g, tablero, ab,  1);   // Abajo
            DibujarSillasEnLado(g, tablero, izq, 2);   // Izquierda
            DibujarSillasEnLado(g, tablero, der, 3);   // Derecha
        }

        // lado: 0=arriba, 1=abajo, 2=izq, 3=der
        private void DibujarSillasEnLado(Graphics g, Rectangle tablero, int cantidad, int lado)
        {
            if (cantidad == 0) return;

            for (int i = 0; i < cantidad; i++)
            {
                float t = (cantidad == 1) ? 0.5f : 0.2f + 0.6f * ((float)i / (cantidad - 1));
                Rectangle r;

                switch (lado)
                {
                    case 0: // Arriba
                        r = new Rectangle(
                            tablero.X + (int)(tablero.Width * t) - SillaW / 2,
                            tablero.Y - SillaH - SillaGap,
                            SillaW, SillaH);
                        break;
                    case 1: // Abajo
                        r = new Rectangle(
                            tablero.X + (int)(tablero.Width * t) - SillaW / 2,
                            tablero.Bottom + SillaGap,
                            SillaW, SillaH);
                        break;
                    case 2: // Izquierda
                        r = new Rectangle(
                            tablero.X - SillaH - SillaGap,
                            tablero.Y + (int)(tablero.Height * t) - SillaW / 2,
                            SillaH, SillaW);
                        break;
                    default: // Derecha
                        r = new Rectangle(
                            tablero.Right + SillaGap,
                            tablero.Y + (int)(tablero.Height * t) - SillaW / 2,
                            SillaH, SillaW);
                        break;
                }

                using (var path = ConstruirRoundedRect(r, 3))
                {
                    using (var b = new SolidBrush(ClrSillaFondo))
                        g.FillPath(b, path);
                    using (var p = new Pen(ClrSillaBorde, 1f))
                        g.DrawPath(p, path);
                }
            }
        }

        private static (int arr, int ab, int izq, int der) DistribuirPorLado(int cap)
        {
            switch (cap)
            {
                case 2:  return (1, 1, 0, 0);
                case 3:  return (1, 1, 1, 0);
                case 4:  return (1, 1, 1, 1);
                case 5:  return (2, 2, 1, 0);
                case 6:  return (2, 2, 1, 1);
                case 7:  return (2, 2, 2, 1);
                default: return (2, 2, 2, 2); // 8+
            }
        }

        // ── Íconos decorativos ────────────────────────────────────────────────

        private static void DibujarEstrella(Graphics g, float cx, float cy, float r, Color color)
        {
            // Polígono de 5 puntas
            const int puntas = 5;
            var pts = new PointF[puntas * 2];
            double paso = Math.PI / puntas;
            for (int i = 0; i < puntas * 2; i++)
            {
                double angulo = i * paso - Math.PI / 2;
                float radio  = (i % 2 == 0) ? r : r * 0.42f;
                pts[i] = new PointF(
                    cx + (float)(radio * Math.Cos(angulo)),
                    cy + (float)(radio * Math.Sin(angulo)));
            }
            using (var b = new SolidBrush(color))
                g.FillPolygon(b, pts);
        }

        private static void DibujarCandado(Graphics g, float x, float y)
        {
            // Arco superior
            using (var p = new Pen(Color.FromArgb(200, 255, 255, 255), 1.5f))
                g.DrawArc(p, x, y, 8, 6, 180, 180);
            // Cuerpo
            using (var b = new SolidBrush(Color.FromArgb(180, 255, 255, 255)))
                g.FillRectangle(b, x - 1, y + 4, 10, 8);
            using (var p = new Pen(Color.FromArgb(180, 255, 255, 255), 1f))
                g.DrawRectangle(p, x - 1, y + 4, 10, 8);
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private Color ObtenerColorPrincipal()
        {
            if (_seleccionada) return ClrSeleccionada;
            if (_hover)        return ClrHover;

            switch (Datos.EstadoMapa)
            {
                case EstadoMapaPrueba.DisponibleRecomendada: return ClrRecomendada;
                case EstadoMapaPrueba.Disponible:            return ClrDisponible;
                case EstadoMapaPrueba.Ocupada:               return ClrOcupada;
                default:                                     return ClrIncompatible;
            }
        }

        private static Color PintarMasOscuro(Color c, float factor)
        {
            return Color.FromArgb(
                c.A,
                Math.Max(0, (int)(c.R * (1f - factor))),
                Math.Max(0, (int)(c.G * (1f - factor))),
                Math.Max(0, (int)(c.B * (1f - factor))));
        }

        internal static GraphicsPath ConstruirRoundedRect(Rectangle r, int radio)
        {
            int d    = radio * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X,          r.Y,           d, d, 180, 90);
            path.AddArc(r.Right - d,  r.Y,           d, d, 270, 90);
            path.AddArc(r.Right - d,  r.Bottom - d,  d, d,   0, 90);
            path.AddArc(r.X,          r.Bottom - d,  d, d,  90, 90);
            path.CloseFigure();
            return path;
        }
    }

    // ════════════════════════════════════════════════════════════════════════
    //  FORMULARIO PROTOTIPO
    // ════════════════════════════════════════════════════════════════════════

    public sealed class FormPruebaMapa_08YS : Form
    {
        // ── Paleta ──────────────────────────────────────────────────────────
        private static readonly Color ClrBordoProfundo = Color.FromArgb( 56,  17,  17);
        private static readonly Color ClrBordoPrimario = Color.FromArgb(120,  25,  25);
        private static readonly Color ClrCrema         = Color.FromArgb(250, 248, 245);
        private static readonly Color ClrCremaOscura   = Color.FromArgb(238, 233, 225);
        private static readonly Color ClrCobre         = Color.FromArgb(211,  84,   0);
        private static readonly Color ClrTextoOscuro   = Color.FromArgb( 45,  42,  40);
        private static readonly Color ClrTextoDorado   = Color.FromArgb(253, 246, 227);

        // ── Mock data ────────────────────────────────────────────────────────
        private static readonly List<MesaMockDTO> MockMesas = new List<MesaMockDTO>
        {
            new MesaMockDTO { NroMesa=1, Capacidad=2, EstadoMapa=EstadoMapaPrueba.Disponible,            PosicionX= 30, PosicionY= 30, Ancho= 85, Alto=60 },
            new MesaMockDTO { NroMesa=2, Capacidad=4, EstadoMapa=EstadoMapaPrueba.DisponibleRecomendada, PosicionX=205, PosicionY= 30, Ancho=105, Alto=70 },
            new MesaMockDTO { NroMesa=3, Capacidad=6, EstadoMapa=EstadoMapaPrueba.Ocupada,               PosicionX= 45, PosicionY=200, Ancho=120, Alto=80 },
            new MesaMockDTO { NroMesa=4, Capacidad=8, EstadoMapa=EstadoMapaPrueba.Disponible,            PosicionX=240, PosicionY=200, Ancho=145, Alto=90 },
            new MesaMockDTO { NroMesa=5, Capacidad=4, EstadoMapa=EstadoMapaPrueba.Incompatible,          PosicionX=150, PosicionY=370, Ancho=105, Alto=70 },
        };

        // ── Controles ────────────────────────────────────────────────────────
        private Panel  _pnlSalon;
        private Panel  _pnlDetalle;
        private Label  _lblNumeroMesa;
        private Label  _lblCapacidad;
        private Label  _lblEstadoDesc;
        private Label  _lblPista;
        private Button _btnConfirmar;
        private Button _btnCancelar;

        // ── Estado ───────────────────────────────────────────────────────────
        private MesaVisualControl _ctrlSeleccionado;
        public  int?              NroMesaSeleccionada { get; private set; }

        // ════════════════════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ════════════════════════════════════════════════════════════════════

        public FormPruebaMapa_08YS()
        {
            SuspendLayout();
            Text          = "Seleccionar Mesa  –  El Fogón del Sur";
            Size          = new Size(980, 640);
            MinimumSize   = new Size(860, 560);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor     = ClrBordoProfundo;
            Font          = new Font("Segoe UI", 9.5f, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox   = false;

            ConstruirLayout();
            CargarMesas();
            ActualizarDetalle(null);

            ResumeLayout(false);
            PerformLayout();
        }

        // ════════════════════════════════════════════════════════════════════
        //  UI
        // ════════════════════════════════════════════════════════════════════

        private void ConstruirLayout()
        {
            // ── Header ──────────────────────────────────────────────────────
            var header = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 56,
                BackColor = ClrBordoProfundo
            };

            var lblTitulo = new Label
            {
                Text      = "Plano del Salón",
                Font      = new Font("Segoe UI", 16f, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = ClrTextoDorado,
                AutoSize  = true,
                Location  = new Point(20, 10)
            };
            var lblSub = new Label
            {
                Text      = "Seleccioná una mesa disponible (verde) para continuar la reserva",
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Italic, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(175, 155, 110),
                AutoSize  = true,
                Location  = new Point(22, 36)
            };
            header.Controls.Add(lblTitulo);
            header.Controls.Add(lblSub);
            Controls.Add(header);

            // ── Body ────────────────────────────────────────────────────────
            var body = new Panel { Dock = DockStyle.Fill, BackColor = ClrCrema };

            // Panel derecho – detalle
            _pnlDetalle = new Panel
            {
                Dock      = DockStyle.Right,
                Width     = 250,
                BackColor = Color.White,
                Padding   = new Padding(0)
            };
            _pnlDetalle.Paint += (s, e) =>
            {
                using (var pen = new Pen(ClrCremaOscura, 1f))
                    e.Graphics.DrawLine(pen, 0, 0, 0, _pnlDetalle.Height);
            };
            ConstruirPanelDetalle();
            body.Controls.Add(_pnlDetalle);

            // Panel salón – plano interactivo
            _pnlSalon = new Panel
            {
                Dock       = DockStyle.Fill,
                BackColor  = ClrCrema,
                AutoScroll = true
            };
            _pnlSalon.Paint += PintarFondoSalon;
            body.Controls.Add(_pnlSalon);

            Controls.Add(body);
        }

        private void ConstruirPanelDetalle()
        {
            // ── Encabezado del panel ─────────────────────────────────────────
            var encabezado = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 48,
                BackColor = ClrBordoPrimario
            };
            var lblTitDet = new Label
            {
                Text      = "Detalle de Mesa",
                Font      = new Font("Segoe UI", 10.5f, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.White,
                AutoSize  = true,
                Location  = new Point(16, 14)
            };
            encabezado.Controls.Add(lblTitDet);
            _pnlDetalle.Controls.Add(encabezado);

            // ── Área de información ──────────────────────────────────────────
            var info = new Panel
            {
                Location  = new Point(0, 48),
                Size      = new Size(250, 200),
                BackColor = Color.White,
                Padding   = new Padding(16, 12, 16, 0)
            };

            // Número grande
            _lblNumeroMesa = new Label
            {
                Font      = new Font("Segoe UI", 28f, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = ClrBordoPrimario,
                AutoSize  = true,
                Location  = new Point(16, 12)
            };
            info.Controls.Add(_lblNumeroMesa);

            // Separador
            var sep = new Panel
            {
                Location  = new Point(16, 68),
                Size      = new Size(218, 1),
                BackColor = ClrCremaOscura
            };
            info.Controls.Add(sep);

            // Capacidad
            _lblCapacidad = new Label
            {
                Font      = new Font("Segoe UI", 9.5f, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = ClrTextoOscuro,
                AutoSize  = true,
                Location  = new Point(16, 78)
            };
            info.Controls.Add(_lblCapacidad);

            // Estado
            _lblEstadoDesc = new Label
            {
                Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold, GraphicsUnit.Point),
                AutoSize  = true,
                Location  = new Point(16, 102)
            };
            info.Controls.Add(_lblEstadoDesc);

            _pnlDetalle.Controls.Add(info);

            // ── Pista / instrucción ──────────────────────────────────────────
            _lblPista = new Label
            {
                Text      = "Hacé clic sobre una\nmesa verde del plano",
                Font      = new Font("Segoe UI", 8.5f, FontStyle.Italic, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(160, 145, 130),
                AutoSize  = false,
                Size      = new Size(218, 40),
                Location  = new Point(16, 260),
                TextAlign = ContentAlignment.MiddleCenter
            };
            _pnlDetalle.Controls.Add(_lblPista);

            // ── Botón Confirmar ──────────────────────────────────────────────
            _btnConfirmar = new Button
            {
                Text      = "Confirmar Mesa",
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold, GraphicsUnit.Point),
                BackColor = ClrBordoPrimario,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size      = new Size(218, 44),
                Location  = new Point(16, 310),
                Enabled   = false
            };
            _btnConfirmar.FlatAppearance.BorderSize = 0;
            _btnConfirmar.FlatAppearance.MouseOverBackColor = ClrCobre;
            _btnConfirmar.Click += BtnConfirmar_Click;
            _pnlDetalle.Controls.Add(_btnConfirmar);

            // ── Botón Cancelar ───────────────────────────────────────────────
            _btnCancelar = new Button
            {
                Text      = "Cancelar",
                Font      = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = Color.White,
                ForeColor = ClrTextoOscuro,
                FlatStyle = FlatStyle.Flat,
                Size      = new Size(218, 36),
                Location  = new Point(16, 362)
            };
            _btnCancelar.FlatAppearance.BorderColor = ClrCremaOscura;
            _btnCancelar.FlatAppearance.BorderSize  = 1;
            _btnCancelar.FlatAppearance.MouseOverBackColor = ClrCremaOscura;
            _btnCancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            _pnlDetalle.Controls.Add(_btnCancelar);

            // ── Leyenda ──────────────────────────────────────────────────────
            ConstruirLeyenda(420);
        }

        private void ConstruirLeyenda(int topY)
        {
            var (colores, textos) = (
                new Color[]  {
                    Color.FromArgb( 39,174, 96),
                    Color.FromArgb( 80,140, 70),
                    Color.FromArgb(120, 25, 25),
                    Color.FromArgb(160,155,148),
                    Color.FromArgb(211, 84,  0)
                },
                new string[] {
                    "Recomendada ★",
                    "Disponible",
                    "Ocupada",
                    "Insuficiente",
                    "Seleccionada"
                }
            );

            var lblTitLey = new Label
            {
                Text      = "REFERENCIAS",
                Font      = new Font("Segoe UI", 7.5f, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.FromArgb(150, 140, 130),
                AutoSize  = true,
                Location  = new Point(16, topY)
            };
            _pnlDetalle.Controls.Add(lblTitLey);
            topY += 20;

            for (int i = 0; i < textos.Length; i++)
            {
                var color = colores[i];
                var dot   = new Panel { Size = new Size(12, 12), Location = new Point(16, topY + 2) };
                var cc    = color; // captura para el closure
                dot.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.Clear(dot.Parent?.BackColor ?? Color.White);
                    using (var b = new SolidBrush(cc))
                        e.Graphics.FillEllipse(b, 0, 0, 11, 11);
                };

                var lbl = new Label
                {
                    Text      = textos[i],
                    Font      = new Font("Segoe UI", 8.5f, FontStyle.Regular, GraphicsUnit.Point),
                    ForeColor = ClrTextoOscuro,
                    AutoSize  = true,
                    Location  = new Point(34, topY)
                };

                _pnlDetalle.Controls.Add(dot);
                _pnlDetalle.Controls.Add(lbl);
                topY += 22;
            }
        }

        // ════════════════════════════════════════════════════════════════════
        //  CARGA DE MESAS
        // ════════════════════════════════════════════════════════════════════

        private void CargarMesas()
        {
            foreach (var dto in MockMesas)
            {
                var ctrl = new MesaVisualControl(dto)
                {
                    // La posición del control es la del tablero menos el margen de sillas.
                    // Así las coordenadas X,Y del DTO apuntan a la esquina del tablero.
                    Left      = dto.PosicionX - MesaVisualControl.MargenCtrl + 24,
                    Top       = dto.PosicionY - MesaVisualControl.MargenCtrl + 28,
                    BackColor = ClrCrema
                };
                ctrl.MesaClick += Ctrl_MesaClick;
                _pnlSalon.Controls.Add(ctrl);
            }
        }

        // ════════════════════════════════════════════════════════════════════
        //  EVENTOS
        // ════════════════════════════════════════════════════════════════════

        private void Ctrl_MesaClick(object sender, MesaMockDTO datos)
        {
            var ctrl = (MesaVisualControl)sender;

            if (_ctrlSeleccionado != null)
                _ctrlSeleccionado.Seleccionada = false;

            if (_ctrlSeleccionado == ctrl)
            {
                // Clic en la misma mesa → deseleccionar (toggle)
                _ctrlSeleccionado  = null;
                NroMesaSeleccionada = null;
                ActualizarDetalle(null);
            }
            else
            {
                ctrl.Seleccionada   = true;
                _ctrlSeleccionado   = ctrl;
                NroMesaSeleccionada = datos.NroMesa;
                ActualizarDetalle(datos);
            }
        }

        private void BtnConfirmar_Click(object sender, EventArgs e)
        {
            if (NroMesaSeleccionada == null) return;
            MessageBox.Show(
                "Mesa #" + NroMesaSeleccionada + " confirmada.\n\n" +
                "(En producción: DialogResult = OK y se cierra el formulario.)",
                "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }

        // ════════════════════════════════════════════════════════════════════
        //  DETALLE
        // ════════════════════════════════════════════════════════════════════

        private void ActualizarDetalle(MesaMockDTO datos)
        {
            bool haySeleccion = datos != null;

            _lblNumeroMesa.Text  = haySeleccion ? "Mesa #" + datos.NroMesa : "—";
            _lblCapacidad.Text   = haySeleccion ? "Capacidad: " + datos.Capacidad + " personas" : "";
            _lblEstadoDesc.Text  = haySeleccion ? DescripcionEstado(datos.EstadoMapa)           : "";
            _lblEstadoDesc.ForeColor = haySeleccion ? ColorEstado(datos.EstadoMapa) : ClrTextoOscuro;
            _lblPista.Visible    = !haySeleccion;
            _btnConfirmar.Enabled = haySeleccion;
        }

        private static string DescripcionEstado(EstadoMapaPrueba estado)
        {
            switch (estado)
            {
                case EstadoMapaPrueba.DisponibleRecomendada: return "★ Recomendada";
                case EstadoMapaPrueba.Disponible:            return "✓ Disponible";
                case EstadoMapaPrueba.Ocupada:               return "✗ Ocupada (este turno)";
                default:                                     return "✗ Capacidad insuficiente";
            }
        }

        private static Color ColorEstado(EstadoMapaPrueba estado)
        {
            switch (estado)
            {
                case EstadoMapaPrueba.DisponibleRecomendada: return Color.FromArgb( 39,174, 96);
                case EstadoMapaPrueba.Disponible:            return Color.FromArgb( 80,140, 70);
                case EstadoMapaPrueba.Ocupada:               return Color.FromArgb(120, 25, 25);
                default:                                     return Color.FromArgb(140,130,120);
            }
        }

        // ════════════════════════════════════════════════════════════════════
        //  FONDO DEL SALÓN
        // ════════════════════════════════════════════════════════════════════

        private void PintarFondoSalon(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Grilla de referencia (estilo blueprint suave)
            using (var pen = new Pen(Color.FromArgb(22, ClrTextoOscuro), 1f))
            {
                for (int x = 0; x < _pnlSalon.Width; x += 40)
                    g.DrawLine(pen, x, 0, x, _pnlSalon.Height);
                for (int y = 0; y < _pnlSalon.Height; y += 40)
                    g.DrawLine(pen, 0, y, _pnlSalon.Width, y);
            }

            // Etiqueta del plano
            using (var fnt = new Font("Segoe UI", 7.5f, FontStyle.Bold, GraphicsUnit.Point))
            using (var b   = new SolidBrush(Color.FromArgb(90, ClrBordoPrimario)))
                g.DrawString("SALÓN PRINCIPAL  –  EL FOGÓN DEL SUR", fnt, b, 20f, 8f);

            // Silueta de paredes del salón (decorativa)
            var salon = new Rectangle(10, 22, _pnlSalon.Width - 280, _pnlSalon.Height - 36);
            using (var pen = new Pen(Color.FromArgb(40, ClrBordoPrimario), 2f))
            using (var path = MesaVisualControl.ConstruirRoundedRect(salon, 14))
                g.DrawPath(pen, path);
        }

        // ════════════════════════════════════════════════════════════════════
        //  ENTRY POINT — para prueba directa (F5)
        //  En producción: eliminá este Main() y usá el Program.cs del proyecto.
        // ════════════════════════════════════════════════════════════════════
        
    }
}
