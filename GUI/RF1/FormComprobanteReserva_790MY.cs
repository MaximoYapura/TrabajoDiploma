using BE_08YS;
using BLL_08YS;
using Service_08YS;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_08YS.RF1
{
    /// <summary>
    /// Modal que se abre al confirmar una reserva: muestra la vista previa del
    /// comprobante con la estética de El Fogón del Sur, lo envía por email al
    /// cliente en segundo plano y permite guardarlo en PDF.
    /// </summary>
    public partial class FormComprobanteReserva_790MY : Form, IIdiomaObserver_08YS
    {
        private enum EstadoEmail { Enviando, Enviado, Error, NoEnviado }

        // Paleta El Fogón del Sur
        private static readonly Color Bordo       = Color.FromArgb(120, 25, 25);
        private static readonly Color BordoHover  = Color.FromArgb(95, 18, 18);
        private static readonly Color Dorado      = Color.FromArgb(235, 195, 140);
        private static readonly Color Texto       = Color.FromArgb(45, 42, 40);
        private static readonly Color Gris        = Color.FromArgb(110, 100, 95);
        private static readonly Color Tile        = Color.FromArgb(248, 243, 238);
        private static readonly Color TileBorde   = Color.FromArgb(230, 220, 205);
        private static readonly Color Linea       = Color.FromArgb(235, 228, 218);
        private static readonly Color Verde       = Color.FromArgb(30, 110, 60);
        private static readonly Color VerdeFondo  = Color.FromArgb(225, 242, 230);
        private static readonly Color Ambar       = Color.FromArgb(140, 90, 20);
        private static readonly Color Rojo        = Color.FromArgb(160, 40, 30);
        private static readonly Color FondoHost   = Color.FromArgb(236, 230, 222);

        private readonly Reserva_790MY _reserva;
        private readonly Cliente_790MY _cliente;
        private readonly ReservaBLL_790MY _reservaBll;
        private readonly DateTime _emitido = DateTime.Now;
        private readonly Image _logo;

        private EstadoEmail _estadoEmail = EstadoEmail.NoEnviado;
        private string _detalleEmail;   // mensaje de error / motivo de no envío

        private readonly Font _fTitulo       = new Font("Segoe UI", 15f, FontStyle.Bold);
        private readonly Font _fSubtitulo    = new Font("Segoe UI", 8.25f, FontStyle.Bold);
        private readonly Font _fEtiqueta     = new Font("Segoe UI", 7.5f, FontStyle.Bold);
        private readonly Font _fNumero       = new Font("Segoe UI", 22f, FontStyle.Bold);
        private readonly Font _fChip         = new Font("Segoe UI", 8f, FontStyle.Bold);
        private readonly Font _fSeccion      = new Font("Segoe UI", 8.25f, FontStyle.Bold);
        private readonly Font _fValor        = new Font("Segoe UI", 11.25f, FontStyle.Bold);
        private readonly Font _fExtra        = new Font("Segoe UI", 7.5f);
        private readonly Font _fClave        = new Font("Segoe UI", 8.5f);
        private readonly Font _fValorCliente = new Font("Segoe UI", 9.25f);
        private readonly Font _fNota         = new Font("Segoe UI", 8.25f, FontStyle.Italic);
        private readonly Font _fPie          = new Font("Segoe UI", 7.5f);
        private readonly Font _fGracias      = new Font("Segoe UI", 9f, FontStyle.Bold | FontStyle.Italic);

        public FormComprobanteReserva_790MY(Reserva_790MY reserva, Cliente_790MY cliente, ReservaBLL_790MY reservaBll)
        {
            InitializeComponent();

            _reserva    = reserva    ?? throw new ArgumentNullException(nameof(reserva));
            _cliente    = cliente    ?? throw new ArgumentNullException(nameof(cliente));
            _reservaBll = reservaBll ?? throw new ArgumentNullException(nameof(reservaBll));

            try { _logo = Properties.Resources.LogoElFogon; }
            catch (Exception) { _logo = null; }

            btnGuardarPdf_790MY.MouseEnter += (s, e) => btnGuardarPdf_790MY.BackColor = BordoHover;
            btnGuardarPdf_790MY.MouseLeave += (s, e) => btnGuardarPdf_790MY.BackColor = Bordo;

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) =>
            {
                TraductorManager_08YS.Instance.Desuscribir(this);
                LiberarRecursosGraficos();
            };
        }

        private void FormComprobanteReserva_790MY_Load(object sender, EventArgs e)
        {
            UpdateIdioma();
        }

        // El envío arranca recién cuando la ventana ya está visible, así el usuario
        // ve la vista previa de inmediato y el estado del email se actualiza en vivo.
        private async void FormComprobanteReserva_790MY_Shown(object sender, EventArgs e)
        {
            //await EnviarEmailAsync();
        }

        #region Email en segundo plano

        private async Task EnviarEmailAsync()
        {
            var t = TraductorManager_08YS.Instance;
            btnReenviar_790MY.Visible = false;

            if (string.IsNullOrWhiteSpace(_cliente.Email))
            {
                _detalleEmail = t.GetTexto("CR_email_sin_destinatario");
                MostrarEstadoEmail(EstadoEmail.NoEnviado);
                return;
            }

            MostrarEstadoEmail(EstadoEmail.Enviando);

            try
            {
                await Task.Run(() => _reservaBll.EnviarComprobanteEmail(_cliente, _reserva));
                if (IsDisposed) return;
                MostrarEstadoEmail(EstadoEmail.Enviado);
            }
            catch (InvalidOperationException ex)
            {
                // Falta de configuración SMTP: reintentar no cambia nada.
                if (IsDisposed) return;
                _detalleEmail = ex.Message;
                MostrarEstadoEmail(EstadoEmail.NoEnviado);
            }
            catch (Exception ex)
            {
                if (IsDisposed) return;
                _detalleEmail = ex is SmtpException && ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;
                MostrarEstadoEmail(EstadoEmail.Error);
                btnReenviar_790MY.Visible = true;
            }
        }

        private void MostrarEstadoEmail(EstadoEmail estado)
        {
            var t = TraductorManager_08YS.Instance;
            _estadoEmail = estado;

            switch (estado)
            {
                case EstadoEmail.Enviando:
                    lblEstadoEmail_790MY.Text      = string.Format(t.GetTexto("CR_email_enviando"), _cliente.Email);
                    lblEstadoEmail_790MY.ForeColor = Gris;
                    break;
                case EstadoEmail.Enviado:
                    lblEstadoEmail_790MY.Text      = string.Format(t.GetTexto("CR_email_enviado"), _cliente.Email);
                    lblEstadoEmail_790MY.ForeColor = Verde;
                    break;
                case EstadoEmail.Error:
                    lblEstadoEmail_790MY.Text      = string.Format(t.GetTexto("CR_email_error"), _detalleEmail);
                    lblEstadoEmail_790MY.ForeColor = Rojo;
                    break;
                default:
                    lblEstadoEmail_790MY.Text      = _detalleEmail ?? string.Empty;
                    lblEstadoEmail_790MY.ForeColor = Ambar;
                    break;
            }

            // El texto completo queda disponible como tooltip por si se trunca.
            tipEstado_790MY.SetToolTip(lblEstadoEmail_790MY, lblEstadoEmail_790MY.Text);
        }

        private async void btnReenviar_790MY_Click(object sender, EventArgs e)
        {
            await EnviarEmailAsync();
        }

        #endregion

        #region Guardar PDF

        private void btnGuardarPdf_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;

            using (var dlg = new SaveFileDialog
            {
                Title           = t.GetTexto("CR_guardar_titulo"),
                Filter          = t.GetTexto("CR_filtro_pdf"),
                FileName        = ReservaBLL_790MY.NombreArchivoComprobante(_reserva),
                DefaultExt      = "pdf",
                AddExtension    = true,
                OverwritePrompt = true
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    Cursor = Cursors.WaitCursor;
                    byte[] pdf = _reservaBll.GenerarComprobantePDF(_reserva, _cliente);
                    File.WriteAllBytes(dlg.FileName, pdf);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(t.GetTexto("CR_msg_error_guardar"), ex.Message),
                                    t.GetTexto("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    Cursor = Cursors.Default;
                }

                var abrir = MessageBox.Show(string.Format(t.GetTexto("CR_msg_guardado"), dlg.FileName),
                                            t.GetTexto("exito"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (abrir == DialogResult.Yes)
                {
                    try { Process.Start(new ProcessStartInfo(dlg.FileName) { UseShellExecute = true }); }
                    catch (Exception) { /* sin visor de PDF asociado: el archivo ya quedó guardado */ }
                }
            }
        }

        private void btnCerrar_790MY_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        #region Vista previa del comprobante (GDI+)

        private void pnlFooter_790MY_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(TileBorde))
                e.Graphics.DrawLine(pen, 0, 0, pnlFooter_790MY.Width, 0);
        }

        private void pnlTicket_790MY_Paint(object sender, PaintEventArgs e)
        {
            var g       = e.Graphics;
            var t       = TraductorManager_08YS.Instance;
            var cultura = t.CulturaActual;
            int W       = pnlTicket_790MY.Width;
            int H       = pnlTicket_790MY.Height;
            const TextFormatFlags sinPadding = TextFormatFlags.NoPadding;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            using (var borde = new Pen(TileBorde))
                g.DrawRectangle(borde, 0, 0, W - 1, H - 1);

            // ── Banda de marca ───────────────────────────────────────────────
            using (var b = new SolidBrush(Bordo))  g.FillRectangle(b, 0, 0, W, 78);
            using (var b = new SolidBrush(Dorado)) g.FillRectangle(b, 0, 78, W, 3);

            using (var baldosaLogo = RectRedondeado(new Rectangle(16, 11, 56, 56), 8))
            using (var b = new SolidBrush(Color.White))
                g.FillPath(b, baldosaLogo);
            if (_logo != null)
                DibujarImagenAjustada(g, _logo, new Rectangle(20, 15, 48, 48));

            TextRenderer.DrawText(g, t.GetTexto("CR_doc_restaurante"), _fTitulo, new Point(84, 14), Color.White, sinPadding);
            TextRenderer.DrawText(g, t.GetTexto("CR_doc_titulo").ToUpper(cultura), _fSubtitulo, new Point(86, 46), Dorado, sinPadding);

            // ── N° de reserva + estado ───────────────────────────────────────
            TextRenderer.DrawText(g, t.GetTexto("CR_doc_nro"), _fEtiqueta, new Point(20, 96), Gris, sinPadding);
            TextRenderer.DrawText(g, ReservaBLL_790MY.FormatearNumeroReserva(_reserva.ReservaID), _fNumero,
                                  new Point(17, 108), Bordo, sinPadding);

            string estado = t.GetTexto("CR_doc_estado_confirmada");
            Size tamEstado = TextRenderer.MeasureText(estado, _fChip);
            var chip = new Rectangle(W - 20 - tamEstado.Width - 18, 116, tamEstado.Width + 18, 24);
            using (var pathChip = RectRedondeado(chip, 12))
            using (var b = new SolidBrush(VerdeFondo))
                g.FillPath(b, pathChip);
            TextRenderer.DrawText(g, estado, _fChip, chip, Verde,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            DibujarPerforacion(g, 160, W);

            // ── Detalle de la reserva (2 x 2) ────────────────────────────────
            TextRenderer.DrawText(g, t.GetTexto("CR_doc_sec_detalle").ToUpper(cultura), _fSeccion,
                                  new Point(20, 174), Bordo, sinPadding);

            int tw = (W - 40 - 10) / 2;
            const int th = 56;
            string dia = _reserva.Fecha.ToString("dddd", cultura);
            if (dia.Length > 0) dia = char.ToUpper(dia[0], cultura) + dia.Substring(1);

            DibujarBaldosa(g, new Rectangle(20, 196, tw, th), t.GetTexto("CR_doc_fecha"),
                           _reserva.Fecha.ToString("dd/MM/yyyy"), dia, cultura);
            DibujarBaldosa(g, new Rectangle(30 + tw, 196, tw, th), t.GetTexto("CR_doc_turno"),
                           ReservaBLL_790MY.FormatearTurno(_reserva.Hora), null, cultura);
            DibujarBaldosa(g, new Rectangle(20, 262, tw, th), t.GetTexto("CR_doc_mesa"),
                           string.Format(t.GetTexto("CR_doc_mesa_valor"), _reserva.MesaNumero), null, cultura);
            DibujarBaldosa(g, new Rectangle(30 + tw, 262, tw, th), t.GetTexto("CR_doc_comensales"),
                           _reserva.CantidadComensales.ToString(), null, cultura);

            // ── Datos del cliente ────────────────────────────────────────────
            TextRenderer.DrawText(g, t.GetTexto("CR_doc_sec_cliente").ToUpper(cultura), _fSeccion,
                                  new Point(20, 332), Bordo, sinPadding);

            string sinDato = t.GetTexto("CR_doc_sin_dato");
            int y = 354;
            DibujarFilaCliente(g, W, ref y, t.GetTexto("CR_doc_cliente_nombre"), $"{_cliente.Nombre} {_cliente.Apellido}", sinDato);
            DibujarFilaCliente(g, W, ref y, t.GetTexto("CR_doc_cliente_dni"), _cliente.DNI.ToString(), sinDato);
            DibujarFilaCliente(g, W, ref y, t.GetTexto("CR_doc_cliente_email"), _cliente.Email, sinDato);
            DibujarFilaCliente(g, W, ref y, t.GetTexto("CR_doc_cliente_telefono"), _cliente.Telefono, sinDato);

            DibujarPerforacion(g, 462, W);

            // ── Pie ──────────────────────────────────────────────────────────
            var centrado = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                           TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding;
            TextRenderer.DrawText(g, t.GetTexto("CR_doc_nota"), _fNota, new Rectangle(16, 470, W - 32, 20), Texto, centrado);
            TextRenderer.DrawText(g, string.Format(t.GetTexto("CR_doc_emitido"), _emitido.ToString("dd/MM/yyyy HH:mm"), UsuarioEmisor()),
                                  _fPie, new Rectangle(16, 492, W - 32, 16), Gris, centrado);
            TextRenderer.DrawText(g, t.GetTexto("CR_doc_gracias"), _fGracias, new Rectangle(16, 510, W - 32, 18), Bordo, centrado);
        }

        private void DibujarBaldosa(Graphics g, Rectangle r, string etiqueta, string valor, string extra,
                                    System.Globalization.CultureInfo cultura)
        {
            using (var b = new SolidBrush(Tile)) g.FillRectangle(b, r);
            using (var p = new Pen(TileBorde))   g.DrawRectangle(p, r.X, r.Y, r.Width - 1, r.Height - 1);

            TextRenderer.DrawText(g, etiqueta.ToUpper(cultura), _fEtiqueta, new Point(r.X + 10, r.Y + 8), Gris, TextFormatFlags.NoPadding);
            if (!string.IsNullOrEmpty(extra))
                TextRenderer.DrawText(g, extra, _fExtra, new Rectangle(r.X, r.Y + 8, r.Width - 10, 14), Gris,
                                      TextFormatFlags.Right | TextFormatFlags.NoPadding);
            TextRenderer.DrawText(g, valor, _fValor, new Rectangle(r.X + 10, r.Y + 24, r.Width - 16, 24), Texto,
                                  TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        }

        private void DibujarFilaCliente(Graphics g, int W, ref int y, string clave, string valor, string sinDato)
        {
            string texto = string.IsNullOrWhiteSpace(valor) ? sinDato : valor.Trim();

            TextRenderer.DrawText(g, clave, _fClave, new Point(20, y + 2), Gris, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(g, texto, _fValorCliente, new Rectangle(118, y, W - 138, 20), Texto,
                                  TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            using (var p = new Pen(Linea))
                g.DrawLine(p, 20, y + 21, W - 20, y + 21);
            y += 24;
        }

        // Línea punteada con muescas semicirculares a los costados (ticket troquelado).
        private static void DibujarPerforacion(Graphics g, int y, int W)
        {
            using (var p = new Pen(TileBorde) { DashStyle = DashStyle.Dash })
                g.DrawLine(p, 18, y, W - 18, y);
            using (var b = new SolidBrush(FondoHost))
            {
                g.FillEllipse(b, -9, y - 9, 18, 18);
                g.FillEllipse(b, W - 9, y - 9, 18, 18);
            }
        }

        private static void DibujarImagenAjustada(Graphics g, Image img, Rectangle destino)
        {
            float escala = Math.Min((float)destino.Width / img.Width, (float)destino.Height / img.Height);
            int w = (int)(img.Width * escala);
            int h = (int)(img.Height * escala);
            var r = new Rectangle(destino.X + (destino.Width - w) / 2, destino.Y + (destino.Height - h) / 2, w, h);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, r);
        }

        private static GraphicsPath RectRedondeado(Rectangle r, int radio)
        {
            int d = radio * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static string UsuarioEmisor()
        {
            var actual = SessionManager_08YS.Instance.Current;
            return actual != null ? $"{actual.Nombre} {actual.Apellido}".Trim() : "-";
        }

        private void LiberarRecursosGraficos()
        {
            foreach (var f in new[] { _fTitulo, _fSubtitulo, _fEtiqueta, _fNumero, _fChip, _fSeccion, _fValor,
                                      _fExtra, _fClave, _fValorCliente, _fNota, _fPie, _fGracias })
                f.Dispose();
            _logo?.Dispose();
        }

        #endregion

        #region i18n
        public void UpdateIdioma()
        {
            var t = TraductorManager_08YS.Instance;
            TraducirControles(this);

            this.Text = t.GetTexto("CR_titulo_ventana");
            lblBannerExito_790MY.Text = string.Format(t.GetTexto("CR_banner_exito"),
                                                      ReservaBLL_790MY.FormatearNumeroReserva(_reserva.ReservaID));
            MostrarEstadoEmail(_estadoEmail);
            pnlTicket_790MY.Invalidate();
        }

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                if (c.Tag is string clave && !string.IsNullOrWhiteSpace(clave))
                    c.Text = TraductorManager_08YS.Instance.GetTexto(clave);
                if (c.HasChildren)
                    TraducirControles(c);
            }
        }
        #endregion
    }

    /// <summary>
    /// Panel con doble buffer para dibujar la vista previa del comprobante sin parpadeo.
    /// </summary>
    public class TicketPreviewPanel_790MY : Panel
    {
        public TicketPreviewPanel_790MY()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);
        }
    }
}
