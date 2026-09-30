using BE_08YS;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Service_08YS;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace BLL_08YS
{
    /// <summary>
    /// Comprobante de Reserva: generación del PDF en memoria y envío por email.
    /// Todos los textos salen de TraductorManager_08YS (idiomas.json), por lo que el
    /// documento y el correo se emiten en el idioma activo de la sesión.
    /// </summary>
    public partial class ReservaBLL_790MY
    {
        #region Constantes y formato compartido (PDF, email y vista previa)

        /// <summary>Duración de un turno de reserva (los turnos del sistema son de 2 horas).</summary>
        public static readonly TimeSpan DuracionTurno = TimeSpan.FromHours(2);

        /// <summary>Devuelve el turno como "20:00 - 22:00" (el fin de 22:00 se muestra como 24:00).</summary>
        public static string FormatearTurno(TimeSpan inicio)
        {
            TimeSpan fin = inicio + DuracionTurno;
            return $"{(int)inicio.TotalHours:00}:{inicio.Minutes:00} - {(int)fin.TotalHours:00}:{fin.Minutes:00}";
        }

        /// <summary>Número de reserva con ceros a la izquierda (ej. 000123).</summary>
        public static string FormatearNumeroReserva(int reservaId) => reservaId.ToString("D6");

        /// <summary>Nombre sugerido para el archivo PDF del comprobante.</summary>
        public static string NombreArchivoComprobante(Reserva_790MY reserva)
            => $"Comprobante_Reserva_{FormatearNumeroReserva(reserva.ReservaID)}_{reserva.Fecha:yyyyMMdd}.pdf";

        #endregion

        #region Paleta El Fogón del Sur

        private static readonly BaseColor PdfBordo      = new BaseColor(120, 25, 25);
        private static readonly BaseColor PdfDorado     = new BaseColor(235, 195, 140);
        private static readonly BaseColor PdfTexto      = new BaseColor(45, 42, 40);
        private static readonly BaseColor PdfGris       = new BaseColor(110, 100, 95);
        private static readonly BaseColor PdfTile       = new BaseColor(248, 243, 238);
        private static readonly BaseColor PdfTileBorde  = new BaseColor(230, 220, 205);
        private static readonly BaseColor PdfLinea      = new BaseColor(235, 228, 218);
        private static readonly BaseColor PdfNotaFondo  = new BaseColor(252, 246, 236);
        private static readonly BaseColor PdfVerde      = new BaseColor(30, 110, 60);
        private static readonly BaseColor PdfVerdeFondo = new BaseColor(225, 242, 230);

        #endregion

        #region Generación del PDF

        /// <summary>
        /// Genera el comprobante de la reserva como PDF (A5 vertical) completamente en
        /// memoria. No escribe en disco: la GUI decide si lo guarda (SaveFileDialog) y
        /// EnviarComprobanteEmail lo adjunta al correo.
        /// </summary>
        public byte[] GenerarComprobantePDF(Reserva_790MY reserva, Cliente_790MY cliente)
        {
            if (reserva == null) throw new ArgumentNullException(nameof(reserva));
            if (cliente == null) throw new ArgumentNullException(nameof(cliente));

            var t       = TraductorManager_08YS.Instance;
            var cultura = t.CulturaActual;
            string nro  = FormatearNumeroReserva(reserva.ReservaID);

            var fLabel      = new Font(Font.FontFamily.HELVETICA, 7.5f, Font.BOLD, PdfGris);
            var fNumero     = new Font(Font.FontFamily.HELVETICA, 24f,  Font.BOLD, PdfBordo);
            var fChip       = new Font(Font.FontFamily.HELVETICA, 7.5f, Font.BOLD, PdfVerde);
            var fSeccion    = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD, PdfBordo);
            var fTileValor  = new Font(Font.FontFamily.HELVETICA, 13f,  Font.BOLD, PdfTexto);
            var fTileExtra  = new Font(Font.FontFamily.HELVETICA, 8f,   Font.NORMAL, PdfGris);
            var fClave      = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.NORMAL, PdfGris);
            var fValor      = new Font(Font.FontFamily.HELVETICA, 10f,  Font.NORMAL, PdfTexto);
            var fNota       = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.ITALIC, PdfTexto);
            var fBandaTit   = new Font(Font.FontFamily.HELVETICA, 19f,  Font.BOLD, BaseColor.WHITE);
            var fBandaSub   = new Font(Font.FontFamily.HELVETICA, 9f,   Font.NORMAL, PdfDorado);
            var fPie        = new Font(Font.FontFamily.HELVETICA, 7.5f, Font.NORMAL, PdfGris);
            var fGracias    = new Font(Font.FontFamily.HELVETICA, 9f,   Font.BOLDITALIC, PdfBordo);

            using (var ms = new MemoryStream())
            {
                // A5 vertical: formato de ticket/comprobante. El margen superior deja
                // lugar a la banda de marca y el inferior al pie absoluto.
                var doc    = new Document(PageSize.A5, 30f, 30f, 108f, 70f);
                var writer = PdfWriter.GetInstance(doc, ms);
                writer.CloseStream = false;

                // El pie se dibuja en OnEndPage: queda anclado al borde inferior de
                // cada página aunque un dato largo (p. ej. un email extenso) haga
                // crecer el contenido.
                writer.PageEvent = new PieComprobante(
                    string.Format(t.GetTexto("CR_doc_emitido"), DateTime.Now.ToString("dd/MM/yyyy HH:mm"), UsuarioEmisor()),
                    t.GetTexto("CR_doc_gracias"), fPie, fGracias);
                doc.AddTitle($"{t.GetTexto("CR_doc_titulo")} {nro}");
                doc.AddAuthor(t.GetTexto("CR_doc_restaurante"));
                doc.Open();

                float ancho = doc.PageSize.Width;
                float alto  = doc.PageSize.Height;
                PdfContentByte cb = writer.DirectContent;

                // ── Banda de marca ───────────────────────────────────────────────
                const float altoBanda = 92f;
                cb.SaveState();
                cb.SetColorFill(PdfBordo);
                cb.Rectangle(0, alto - altoBanda, ancho, altoBanda);
                cb.Fill();
                cb.SetColorFill(PdfDorado);
                cb.Rectangle(0, alto - altoBanda - 2.5f, ancho, 2.5f);
                cb.Fill();

                // Logo sobre una baldosa blanca para que se lea sobre el bordó.
                cb.SetColorFill(BaseColor.WHITE);
                cb.RoundRectangle(30f, alto - altoBanda + 14f, 64f, 64f, 8f);
                cb.Fill();
                cb.RestoreState();

                byte[] logo = ObtenerLogoPng();
                if (logo != null)
                {
                    var img = Image.GetInstance(logo);
                    img.ScaleToFit(56f, 56f);
                    float lx = 30f + (64f - img.ScaledWidth) / 2f;
                    float ly = alto - altoBanda + 14f + (64f - img.ScaledHeight) / 2f;
                    img.SetAbsolutePosition(lx, ly);
                    cb.AddImage(img);
                }

                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT,
                    new Phrase(t.GetTexto("CR_doc_restaurante"), fBandaTit), 108f, alto - 44f, 0);
                var chunkSub = new Chunk(t.GetTexto("CR_doc_titulo").ToUpper(cultura), fBandaSub);
                chunkSub.SetCharacterSpacing(1.2f);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(chunkSub), 108f, alto - 62f, 0);

                // ── N° de reserva + estado + código de barras ────────────────────
                var tablaNro = new PdfPTable(2) { WidthPercentage = 100f };
                tablaNro.SetWidths(new[] { 1.25f, 1f });

                var celdaIzq = new PdfPCell { Border = Rectangle.NO_BORDER, PaddingLeft = 0f, VerticalAlignment = Element.ALIGN_MIDDLE };
                var chunkLbl = new Chunk(t.GetTexto("CR_doc_nro"), fLabel);
                chunkLbl.SetCharacterSpacing(1f);
                celdaIzq.AddElement(new Paragraph(chunkLbl) { SpacingAfter = 0f });
                celdaIzq.AddElement(new Paragraph(nro, fNumero) { Leading = 26f, SpacingAfter = 5f });

                var chip = new PdfPTable(1) { TotalWidth = 78f, LockedWidth = true, HorizontalAlignment = Element.ALIGN_LEFT };
                chip.AddCell(new PdfPCell(new Phrase(t.GetTexto("CR_doc_estado_confirmada"), fChip))
                {
                    BackgroundColor     = PdfVerdeFondo,
                    Border              = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    PaddingTop          = 3f,
                    PaddingBottom       = 4f
                });
                celdaIzq.AddElement(chip);
                tablaNro.AddCell(celdaIzq);

                var barcode = new Barcode128
                {
                    Code      = "RES-" + nro,
                    CodeType  = Barcode.CODE128,
                    BarHeight = 30f,
                    X         = 0.85f,
                    Size      = 7f
                };
                var imgBarcode = barcode.CreateImageWithBarcode(cb, PdfTexto, PdfTexto);
                imgBarcode.ScaleToFit(150f, 46f);
                tablaNro.AddCell(new PdfPCell(imgBarcode, false)
                {
                    Border              = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment   = Element.ALIGN_MIDDLE,
                    PaddingRight        = 0f
                });
                doc.Add(tablaNro);

                doc.Add(SeparadorPunteado(6f, 8f));

                // ── Detalle de la reserva (2 x 2 baldosas) ───────────────────────
                doc.Add(TituloSeccion(t.GetTexto("CR_doc_sec_detalle"), fSeccion, cultura));

                string diaSemana = Capitalizar(reserva.Fecha.ToString("dddd", cultura), cultura);
                var tiles = new PdfPTable(3) { WidthPercentage = 100f };
                tiles.SetWidths(new[] { 1f, 0.06f, 1f });

                tiles.AddCell(Baldosa(t.GetTexto("CR_doc_fecha"), reserva.Fecha.ToString("dd/MM/yyyy"), diaSemana,
                                      fLabel, fTileValor, fTileExtra, cultura));
                tiles.AddCell(CeldaVacia());
                tiles.AddCell(Baldosa(t.GetTexto("CR_doc_turno"), FormatearTurno(reserva.Hora), null,
                                      fLabel, fTileValor, fTileExtra, cultura));

                tiles.AddCell(new PdfPCell { Colspan = 3, FixedHeight = 6f, Border = Rectangle.NO_BORDER });

                tiles.AddCell(Baldosa(t.GetTexto("CR_doc_mesa"),
                                      string.Format(t.GetTexto("CR_doc_mesa_valor"), reserva.MesaNumero), null,
                                      fLabel, fTileValor, fTileExtra, cultura));
                tiles.AddCell(CeldaVacia());
                tiles.AddCell(Baldosa(t.GetTexto("CR_doc_comensales"), reserva.CantidadComensales.ToString(), null,
                                      fLabel, fTileValor, fTileExtra, cultura));
                doc.Add(tiles);

                // ── Datos del cliente ────────────────────────────────────────────
                var tituloCliente = TituloSeccion(t.GetTexto("CR_doc_sec_cliente"), fSeccion, cultura);
                tituloCliente.SpacingBefore = 12f;
                doc.Add(tituloCliente);

                var tablaCliente = new PdfPTable(2) { WidthPercentage = 100f };
                tablaCliente.SetWidths(new[] { 1f, 2.6f });
                string sinDato = t.GetTexto("CR_doc_sin_dato");
                FilaCliente(tablaCliente, t.GetTexto("CR_doc_cliente_nombre"), $"{cliente.Nombre} {cliente.Apellido}".Trim(), fClave, fValor, sinDato);
                FilaCliente(tablaCliente, t.GetTexto("CR_doc_cliente_dni"),      cliente.DNI.ToString(), fClave, fValor, sinDato);
                FilaCliente(tablaCliente, t.GetTexto("CR_doc_cliente_email"),    cliente.Email,          fClave, fValor, sinDato);
                FilaCliente(tablaCliente, t.GetTexto("CR_doc_cliente_telefono"), cliente.Telefono,       fClave, fValor, sinDato);
                doc.Add(tablaCliente);

                // ── Nota ─────────────────────────────────────────────────────────
                var nota = new PdfPTable(1) { WidthPercentage = 100f, SpacingBefore = 12f };
                nota.AddCell(new PdfPCell(new Phrase(t.GetTexto("CR_doc_nota"), fNota))
                {
                    BackgroundColor = PdfNotaFondo,
                    Border          = Rectangle.LEFT_BORDER,
                    BorderColorLeft = PdfBordo,
                    BorderWidthLeft = 2.5f,
                    Padding         = 9f,
                    PaddingLeft     = 11f
                });
                doc.Add(nota);

                doc.Close();
                return ms.ToArray();
            }
        }

        /// <summary>Pie del comprobante: línea punteada, datos de emisión y agradecimiento.</summary>
        private sealed class PieComprobante : PdfPageEventHelper
        {
            private readonly string _emitido;
            private readonly string _gracias;
            private readonly Font _fPie;
            private readonly Font _fGracias;

            public PieComprobante(string emitido, string gracias, Font fPie, Font fGracias)
            {
                _emitido  = emitido;
                _gracias  = gracias;
                _fPie     = fPie;
                _fGracias = fGracias;
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfContentByte cb = writer.DirectContent;
                float ancho = document.PageSize.Width;

                cb.SaveState();
                cb.SetColorStroke(PdfTileBorde);
                cb.SetLineWidth(1f);
                cb.SetLineDash(1.5f, 3f, 0f);
                cb.MoveTo(30f, 58f);
                cb.LineTo(ancho - 30f, 58f);
                cb.Stroke();
                cb.RestoreState();

                ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(_emitido, _fPie), ancho / 2f, 42f, 0);
                ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(_gracias, _fGracias), ancho / 2f, 26f, 0);
            }
        }

        private static Paragraph TituloSeccion(string texto, Font fuente, CultureInfo cultura)
        {
            var chunk = new Chunk(texto.ToUpper(cultura), fuente);
            chunk.SetCharacterSpacing(1.2f);
            return new Paragraph(chunk) { SpacingAfter = 7f };
        }

        private static Paragraph SeparadorPunteado(float antes, float despues)
        {
            var linea = new DottedLineSeparator { LineColor = PdfTileBorde, Gap = 3f, LineWidth = 1f };
            return new Paragraph(new Chunk(linea)) { SpacingBefore = antes, SpacingAfter = despues };
        }

        private static PdfPCell CeldaVacia() => new PdfPCell { Border = Rectangle.NO_BORDER };

        private static PdfPCell Baldosa(string etiqueta, string valor, string extra,
                                        Font fEtiqueta, Font fValor, Font fExtra, CultureInfo cultura)
        {
            var celda = new PdfPCell
            {
                BackgroundColor = PdfTile,
                BorderColor     = PdfTileBorde,
                BorderWidth     = 0.75f,
                Padding         = 9f,
                PaddingTop      = 7f,
                MinimumHeight   = 52f
            };

            var chunkEtiqueta = new Chunk(etiqueta.ToUpper(cultura), fEtiqueta);
            chunkEtiqueta.SetCharacterSpacing(1f);
            celda.AddElement(new Paragraph(chunkEtiqueta));
            celda.AddElement(new Paragraph(valor, fValor) { Leading = 17f });
            if (!string.IsNullOrEmpty(extra))
                celda.AddElement(new Paragraph(extra, fExtra) { Leading = 11f });
            return celda;
        }

        private static void FilaCliente(PdfPTable tabla, string clave, string valor, Font fClave, Font fValor, string sinDato)
        {
            string texto = string.IsNullOrWhiteSpace(valor) ? sinDato : valor.Trim();

            tabla.AddCell(new PdfPCell(new Phrase(clave, fClave))
            {
                Border = Rectangle.BOTTOM_BORDER, BorderColor = PdfLinea,
                PaddingTop = 5f, PaddingBottom = 6f, PaddingLeft = 0f
            });
            tabla.AddCell(new PdfPCell(new Phrase(texto, fValor))
            {
                Border = Rectangle.BOTTOM_BORDER, BorderColor = PdfLinea,
                PaddingTop = 5f, PaddingBottom = 6f
            });
        }

        private static string Capitalizar(string texto, CultureInfo cultura)
            => string.IsNullOrEmpty(texto) ? texto : char.ToUpper(texto[0], cultura) + texto.Substring(1);

        private static string UsuarioEmisor()
        {
            var actual = SessionManager_08YS.Instance.Current;
            return actual != null ? $"{actual.Nombre} {actual.Apellido}".Trim() : "-";
        }

        // El logo original pesa ~1 MB; se reescala una sola vez a 256 px para que el
        // PDF adjunto al email quede liviano.
        private static byte[] _logoCache;
        private static readonly object _logoLock = new object();

        private static byte[] ObtenerLogoPng()
        {
            lock (_logoLock)
            {
                if (_logoCache != null) return _logoCache;

                string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo_fogon.png");
                if (!File.Exists(ruta)) return null;

                try
                {
                    using (var original = System.Drawing.Image.FromFile(ruta))
                    {
                        const int lado = 256;
                        float escala = Math.Min((float)lado / original.Width, (float)lado / original.Height);
                        int w = Math.Max(1, (int)(original.Width * escala));
                        int h = Math.Max(1, (int)(original.Height * escala));

                        using (var bmp = new System.Drawing.Bitmap(w, h))
                        using (var g = System.Drawing.Graphics.FromImage(bmp))
                        using (var salida = new MemoryStream())
                        {
                            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode     = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            g.DrawImage(original, 0, 0, w, h);
                            bmp.Save(salida, System.Drawing.Imaging.ImageFormat.Png);
                            _logoCache = salida.ToArray();
                        }
                    }
                }
                catch (Exception)
                {
                    // Si el logo no puede leerse, el comprobante se emite igual sin imagen.
                    _logoCache = null;
                }

                return _logoCache;
            }
        }

        #endregion

        #region Envío por email

        /// <summary>
        /// Indica si el envío de emails está configurado: la sección
        /// &lt;system.net&gt;&lt;mailSettings&gt; del App.config existe (no está comentada)
        /// y define un host SMTP y un remitente. Nunca lanza excepciones.
        /// </summary>
        public static bool EmailConfigurado()
        {
            try
            {
                SmtpSection seccion = LeerSeccionSmtp();
                return seccion != null
                    && !string.IsNullOrWhiteSpace(seccion.Network?.Host)
                    && !string.IsNullOrWhiteSpace(seccion.From);
            }
            catch (Exception)
            {
                // Sección mailSettings mal formada o ilegible: se considera no configurado.
                return false;
            }
        }

        // Lee <system.net><mailSettings><smtp> del App.config (null si está comentada).
        private static SmtpSection LeerSeccionSmtp()
            => ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;

        /// <summary>
        /// Envía el comprobante en PDF adjunto al email principal del cliente usando
        /// System.Net.Mail. La configuración SMTP (host, puerto, SSL, credenciales y
        /// remitente) se toma de la sección &lt;system.net&gt;&lt;mailSettings&gt; del App.config.
        /// <para>
        /// Tolerante a fallos: NUNCA lanza excepciones. Si falta el email del cliente,
        /// si la configuración SMTP no existe o está incompleta, o si el servidor
        /// rechaza la conexión o las credenciales, devuelve <c>false</c> y describe el
        /// motivo en <paramref name="mensaje"/> (ya traducido cuando es un motivo conocido).
        /// </para>
        /// Es sincrónico a propósito: la GUI lo invoca en segundo plano con Task.Run.
        /// </summary>
        /// <returns><c>true</c> si el correo se entregó al servidor SMTP.</returns>
        public bool EnviarComprobanteEmail(Cliente_790MY cliente, Reserva_790MY reserva, out string mensaje)
        {
            mensaje = null;
            var t = TraductorManager_08YS.Instance;

            if (cliente == null || reserva == null)
            {
                mensaje = t.GetTexto("CR_email_sin_datos");
                return false;
            }

            if (string.IsNullOrWhiteSpace(cliente.Email))
            {
                mensaje = t.GetTexto("CR_email_sin_destinatario");
                return false;
            }

            if (!EmailConfigurado())
            {
                mensaje = t.GetTexto("CR_email_no_configurado");
                return false;
            }

            try
            {
                byte[] pdf = GenerarComprobantePDF(reserva, cliente);
                string nro = FormatearNumeroReserva(reserva.ReservaID);

                // SmtpClient() toma host, puerto, SSL y credenciales de mailSettings;
                // el remitente se asigna explícitamente desde la misma sección.
                using (var smtp = new SmtpClient { Timeout = 30000 })
                using (var correo = new MailMessage())
                {
                    correo.From = new MailAddress(LeerSeccionSmtp().From);

                    string nombreCompleto = $"{cliente.Nombre} {cliente.Apellido}".Trim();
                    correo.To.Add(new MailAddress(cliente.Email.Trim(), nombreCompleto));
                    correo.Subject         = string.Format(t.GetTexto("CR_mail_asunto"), nro);
                    correo.SubjectEncoding = Encoding.UTF8;
                    correo.Body            = ConstruirCuerpoHtml(cliente, reserva);
                    correo.BodyEncoding    = Encoding.UTF8;
                    correo.IsBodyHtml      = true;

                    // MailMessage.Dispose libera el adjunto y, con él, el MemoryStream.
                    correo.Attachments.Add(new Attachment(new MemoryStream(pdf),
                                                          NombreArchivoComprobante(reserva),
                                                          MediaTypeNames.Application.Pdf));
                    smtp.Send(correo);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Conexión rechazada, credenciales inválidas, timeout, email con formato
                // inválido, etc. SmtpException suele traer la causa real en InnerException.
                mensaje = ex is SmtpException && ex.InnerException != null
                    ? ex.InnerException.Message
                    : ex.Message;
                return false;
            }
        }

        private static string H(string s) => WebUtility.HtmlEncode(s ?? string.Empty);

        private static string FilaHtml(string clave, string valor) =>
            "<tr>" +
            $"<td style=\"padding:9px 18px;font-size:11px;letter-spacing:1px;color:#6E645F;text-transform:uppercase;width:38%;\">{H(clave)}</td>" +
            $"<td style=\"padding:9px 18px;font-size:15px;font-weight:bold;color:#2D2A28;\">{H(valor)}</td>" +
            "</tr>";

        private static string ConstruirCuerpoHtml(Cliente_790MY cliente, Reserva_790MY reserva)
        {
            var t       = TraductorManager_08YS.Instance;
            var cultura = t.CulturaActual;

            string fecha = Capitalizar(reserva.Fecha.ToString(cultura.DateTimeFormat.LongDatePattern, cultura), cultura);

            var sb = new StringBuilder();
            sb.Append("<!DOCTYPE html><html><head><meta charset=\"utf-8\"></head>");
            sb.Append("<body style=\"margin:0;padding:0;background:#EFE9E1;\">");
            sb.Append("<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"background:#EFE9E1;padding:24px 0;\"><tr><td align=\"center\">");
            sb.Append("<table role=\"presentation\" width=\"560\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width:560px;width:100%;background:#FFFFFF;border:1px solid #E6DCCD;font-family:'Segoe UI',Arial,sans-serif;color:#2D2A28;\">");

            // Banda de marca
            sb.Append("<tr><td style=\"background:#781919;padding:22px 28px;border-bottom:3px solid #EBC38C;\">");
            sb.Append($"<div style=\"font-size:22px;font-weight:bold;color:#FFFFFF;\">{H(t.GetTexto("CR_doc_restaurante"))}</div>");
            sb.Append($"<div style=\"font-size:12px;letter-spacing:1.5px;color:#EBC38C;text-transform:uppercase;margin-top:4px;\">{H(t.GetTexto("CR_doc_titulo"))}</div>");
            sb.Append("</td></tr>");

            // Saludo
            sb.Append("<tr><td style=\"padding:26px 28px 6px;\">");
            sb.Append($"<p style=\"font-size:16px;margin:0 0 8px;\">{H(string.Format(t.GetTexto("CR_mail_saludo"), cliente.Nombre))}</p>");
            sb.Append($"<p style=\"font-size:14px;line-height:1.5;margin:0;color:#4A4441;\">{H(t.GetTexto("CR_mail_cuerpo"))}</p>");
            sb.Append("</td></tr>");

            // Tarjeta con el detalle
            sb.Append("<tr><td style=\"padding:18px 28px;\">");
            sb.Append("<table role=\"presentation\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"background:#F8F3EE;border:1px solid #E6DCCD;\">");
            sb.Append("<tr><td colspan=\"2\" style=\"padding:14px 18px;border-bottom:1px dashed #D8C8B4;\">");
            sb.Append($"<div style=\"font-size:11px;letter-spacing:1px;color:#6E645F;\">{H(t.GetTexto("CR_doc_nro"))}</div>");
            sb.Append($"<div style=\"font-size:26px;font-weight:bold;color:#781919;\">{H(FormatearNumeroReserva(reserva.ReservaID))}</div>");
            sb.Append("</td></tr>");
            sb.Append(FilaHtml(t.GetTexto("CR_doc_fecha"), fecha));
            sb.Append(FilaHtml(t.GetTexto("CR_doc_turno"), FormatearTurno(reserva.Hora)));
            sb.Append(FilaHtml(t.GetTexto("CR_doc_mesa"), string.Format(t.GetTexto("CR_doc_mesa_valor"), reserva.MesaNumero)));
            sb.Append(FilaHtml(t.GetTexto("CR_doc_comensales"), reserva.CantidadComensales.ToString()));
            sb.Append("</table>");
            sb.Append("</td></tr>");

            // Pie y firma
            sb.Append("<tr><td style=\"padding:4px 28px 24px;font-size:13px;color:#6E645F;line-height:1.5;\">");
            sb.Append($"{H(t.GetTexto("CR_mail_pie"))}<br><br><b style=\"color:#781919;\">{H(t.GetTexto("CR_mail_firma"))}</b>");
            sb.Append("</td></tr>");
            sb.Append("<tr><td style=\"background:#FAF8F5;border-top:1px solid #E6DCCD;padding:14px 28px;font-size:11px;color:#8A7F78;text-align:center;\">");
            sb.Append(H(t.GetTexto("CR_doc_nota")));
            sb.Append("</td></tr>");

            sb.Append("</table></td></tr></table></body></html>");
            return sb.ToString();
        }

        #endregion
    }
}
