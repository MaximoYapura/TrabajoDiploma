﻿using BE_08YS;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Service_08YS;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BLL_08YS
{
    /// <summary>
    /// Representación de la Factura B (ticket de FormComprobanteFactura_790MY y PDF del
    /// Arqueo de Caja): datos del emisor y formatos compartidos, para no duplicarlos entre
    /// la GUI y el PDF. Datos fiscales ficticios: el sistema no se conecta con ARCA.
    /// </summary>
    public partial class FacturaBLL_790MY
    {
        #region Emisor y formato compartido (ticket y PDF)

        public const string EmisorRazonSocial = "El Fogón del Sur S.R.L.";
        public const string EmisorCuit = "30-12345678-9";
        public const string EmisorIngresosBrutos = "901-123456-7";
        public const string EmisorInicioActividades = "01/03/2024";
        public const string EmisorDomicilio = "Av. del Fogón 1234 · Buenos Aires";

        /// <summary>Días de vigencia del CAE simulado desde la emisión.</summary>
        public const int DiasVigenciaCae = 10;

        /// <summary>Módulos por lado del QR simulado.</summary>
        public const int ModulosQrSimulado = 25;

        /// <summary>Punto de venta con 5 dígitos, como el modelo oficial ("00001").</summary>
        public static string FormatearPuntoVenta()
            => Factura_790MY.PuntoVenta.ToString("00000", CultureInfo.InvariantCulture);

        /// <summary>
        /// Número del comprobante con 8 dígitos ("00000234"). Factura_790MY.NumeroComprobante
        /// no cambia: entra en el hash de las facturas ya emitidas.
        /// </summary>
        public static string FormatearNumeroComprobante(Factura_790MY factura)
            => FormatearNumeroComprobante(factura.NroFactura);

        public static string FormatearNumeroComprobante(int nroFactura)
            => nroFactura.ToString("00000000", CultureInfo.InvariantCulture);

        /// <summary>Punto de venta y número: "00001-00000234" (grilla del arqueo).</summary>
        public static string FormatearComprobante(int nroFactura)
            => FormatearPuntoVenta() + "-" + FormatearNumeroComprobante(nroFactura);

        /// <summary>Nombre sugerido para el PDF: "Factura_B_00001-00000234.pdf".</summary>
        public static string NombreArchivoFacturaPdf(int nroFactura)
            => "Factura_B_" + FormatearComprobante(nroFactura) + ".pdf";

        /// <summary>Fecha de emisión que se imprime: la del cobro si existe, si no la de la cuenta.</summary>
        public static DateTime FechaEmisionComprobante(Factura_790MY factura, Pago_790MY pago)
            => pago?.FechaHoraPago ?? factura.FechaEmision;

        /// <summary>
        /// CAE simulado de 14 dígitos derivado del hash del comprobante: no se persiste y
        /// sale igual en cada reimpresión.
        /// </summary>
        public static string CaeSimulado(string hash)
        {
            if (string.IsNullOrEmpty(hash) || hash.Length < 23) return new string('0', 14);
            ulong valor = Convert.ToUInt64(hash.Substring(8, 15), 16) % 10000000000000UL;
            return "7" + valor.ToString("D13", CultureInfo.InvariantCulture);
        }

        /// <summary>Vencimiento del CAE simulado: fecha de emisión + <see cref="DiasVigenciaCae"/> días.</summary>
        public static DateTime VencimientoCae(DateTime emision) => emision.Date.AddDays(DiasVigenciaCae);

        /// <summary>
        /// True si la factura se emitió con IVA contenido (total = subtotal − descuento).
        /// Las emitidas antes del cambio sumaban el IVA y se muestran con su cálculo original.
        /// </summary>
        public static bool TieneIvaContenido(Factura_790MY factura)
            => factura.MontoTotal == factura.Subtotal - factura.MontoDescuento;

        /// <summary>
        /// Patrón tipo QR derivado del hash (simulación visual, no decodificable), igual en el
        /// ticket y en el PDF: [fila, columna] = módulo lleno.
        /// </summary>
        public static bool[,] PatronQrSimulado(string hash)
        {
            const int modulos = ModulosQrSimulado;
            var patron = new bool[modulos, modulos];
            var semilla = new Random(Convert.ToInt32(hash.Substring(0, 7), 16));
            for (int f = 0; f < modulos; f++)
                for (int c = 0; c < modulos; c++)
                {
                    bool enPatron = (f < 8 && c < 8) || (f < 8 && c >= modulos - 8) || (f >= modulos - 8 && c < 8);
                    patron[f, c] = enPatron ? ModuloPatron(f, c, modulos) : semilla.Next(100) < 48;
                }
            return patron;
        }

        /// <summary>Patrones de posición (esquinas) de un QR: anillo 7x7 con centro 3x3.</summary>
        private static bool ModuloPatron(int fila, int col, int modulos)
        {
            int f = fila >= modulos - 8 ? fila - (modulos - 7) : fila;
            int c = col >= modulos - 8 ? col - (modulos - 7) : col;
            if (f < 0 || c < 0 || f > 6 || c > 6) return false;
            bool borde = f == 0 || f == 6 || c == 0 || c == 6;
            bool centro = f >= 2 && f <= 4 && c >= 2 && c <= 4;
            return borde || centro;
        }

        #endregion

        #region PDF A4 (Arqueo de Caja)

        private static readonly CultureInfo CulturaMoneda = CultureInfo.GetCultureInfo("es-AR");

        /// <summary>
        /// Representación gráfica de la Factura B en A4, con el mismo contenido fiscal que el
        /// ticket. Diagramación tomada del ejemplo de comprobante de ARCA: encabezado en dos
        /// columnas con la letra al centro. Se genera en memoria; la GUI decide dónde guardarlo.
        /// </summary>
        internal byte[] GenerarFacturaPDF(Factura_790MY factura, Pago_790MY pago)
        {
            if (factura == null) throw new ArgumentNullException(nameof(factura));
            if (pago == null) throw new ArgumentNullException(nameof(pago));

            var t = TraductorManager_08YS.Instance;
            string hash = CalcularHashComprobante(factura, pago);
            DateTime emision = FechaEmisionComprobante(factura, pago);
            bool ivaContenido = TieneIvaContenido(factura);

            var fMarca     = new Font(Font.FontFamily.HELVETICA, 15f,  Font.BOLD, ReservaBLL_790MY.PdfBordo);
            var fTitulo    = new Font(Font.FontFamily.HELVETICA, 16f,  Font.BOLD, ReservaBLL_790MY.PdfTexto);
            var fLetra     = new Font(Font.FontFamily.HELVETICA, 26f,  Font.BOLD, ReservaBLL_790MY.PdfTexto);
            var fCodigo    = new Font(Font.FontFamily.HELVETICA, 7f,   Font.BOLD, ReservaBLL_790MY.PdfGris);
            var fNegrita   = new Font(Font.FontFamily.HELVETICA, 9f,   Font.BOLD, ReservaBLL_790MY.PdfTexto);
            var fCuerpo    = new Font(Font.FontFamily.HELVETICA, 9f,   Font.NORMAL, ReservaBLL_790MY.PdfTexto);
            var fChica     = new Font(Font.FontFamily.HELVETICA, 8f,   Font.NORMAL, ReservaBLL_790MY.PdfGris);
            var fEncabezado = new Font(Font.FontFamily.HELVETICA, 8.5f, Font.BOLD, BaseColor.WHITE);
            var fTotal     = new Font(Font.FontFamily.HELVETICA, 13f,  Font.BOLD, ReservaBLL_790MY.PdfBordo);
            var fNota      = new Font(Font.FontFamily.HELVETICA, 7.5f, Font.ITALIC, ReservaBLL_790MY.PdfGris);
            var fPie       = new Font(Font.FontFamily.HELVETICA, 7.5f, Font.NORMAL, ReservaBLL_790MY.PdfGris);
            var fGracias   = new Font(Font.FontFamily.HELVETICA, 9f,   Font.BOLDITALIC, ReservaBLL_790MY.PdfBordo);

            using (var ms = new MemoryStream())
            {
                var doc = new Document(PageSize.A4, 36f, 36f, 36f, 72f);
                var writer = PdfWriter.GetInstance(doc, ms);
                writer.CloseStream = false;
                writer.PageEvent = new PieComprobante_790MY(
                    string.Format(t.GetTexto("CR_doc_emitido"), DateTime.Now.ToString("dd/MM/yyyy HH:mm"), ReservaBLL_790MY.UsuarioEmisor()),
                    t.GetTexto("CR_doc_gracias"), fPie, fGracias);
                doc.AddTitle(t.GetTexto("CJ_tk_factura") + " B " + FormatearComprobante(factura.NroFactura));
                doc.AddAuthor(EmisorRazonSocial);
                doc.Open();

                // ── Encabezado: emisor | letra | datos del comprobante ──────────────
                var encabezado = new PdfPTable(3) { WidthPercentage = 100f };
                encabezado.SetWidths(new[] { 1f, 0.22f, 1f });

                var emisor = new PdfPCell { Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER, Padding = 10f };
                byte[] logo = ReservaBLL_790MY.ObtenerLogoPng();
                if (logo != null)
                {
                    var img = Image.GetInstance(logo);
                    img.ScaleToFit(46f, 46f);
                    emisor.AddElement(img);
                }
                emisor.AddElement(new Paragraph("EL FOGÓN DEL SUR", fMarca) { SpacingBefore = 4f });
                emisor.AddElement(new Paragraph(t.GetTexto("CJ_tk_slogan"), fChica));
                emisor.AddElement(new Paragraph(EmisorRazonSocial, fNegrita) { SpacingBefore = 6f });
                emisor.AddElement(new Paragraph(EmisorDomicilio, fCuerpo));
                emisor.AddElement(new Paragraph(t.GetTexto("CJ_tk_iva_ri"), fNegrita));
                encabezado.AddCell(emisor);

                var centro = new PdfPCell { Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER, PaddingTop = 0f, HorizontalAlignment = Element.ALIGN_CENTER };
                var letra = new PdfPTable(1) { TotalWidth = 52f, LockedWidth = true, HorizontalAlignment = Element.ALIGN_CENTER };
                letra.AddCell(new PdfPCell(new Phrase("B", fLetra)) { HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1.2f, BorderWidthBottom = 0f, PaddingTop = 2f, PaddingBottom = 0f });
                letra.AddCell(new PdfPCell(new Phrase("COD. 06", fCodigo)) { HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 1.2f, BorderWidthTop = 0f, PaddingBottom = 5f });
                centro.AddElement(letra);
                encabezado.AddCell(centro);

                var datos = new PdfPCell { Border = Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER, Padding = 10f, PaddingLeft = 16f };
                datos.AddElement(new Paragraph(t.GetTexto("CJ_tk_factura"), fTitulo) { SpacingAfter = 6f });
                datos.AddElement(Dato(t.GetTexto("CJ_tk_punto_venta"), FormatearPuntoVenta(), fCuerpo, fNegrita));
                datos.AddElement(Dato(t.GetTexto("CJ_tk_comp_nro"), FormatearNumeroComprobante(factura), fCuerpo, fNegrita));
                datos.AddElement(Dato(t.GetTexto("CJ_tk_fecha_emision"), emision.ToString("dd/MM/yyyy HH:mm"), fCuerpo, fNegrita));
                datos.AddElement(new Paragraph(string.Format(t.GetTexto("CJ_tk_cuit_iibb"), EmisorCuit, EmisorIngresosBrutos), fCuerpo) { SpacingBefore = 6f });
                datos.AddElement(new Paragraph(string.Format(t.GetTexto("CJ_tk_inicio_actividades"), EmisorInicioActividades), fCuerpo));
                encabezado.AddCell(datos);
                doc.Add(encabezado);

                // ── A consumidor final + condición de venta ─────────────────────────
                bool efectivo = pago.MetodoPago == MetodoPago_790MY.Efectivo;
                var receptor = new PdfPTable(2) { WidthPercentage = 100f, SpacingBefore = 6f };
                receptor.SetWidths(new[] { 1f, 1f });
                receptor.AddCell(new PdfPCell(new Phrase(t.GetTexto("CJ_tk_consumidor_final_leyenda"), fNegrita)) { Padding = 7f, BorderWidthRight = 0f });
                receptor.AddCell(new PdfPCell(Dato(t.GetTexto("CJ_tk_condicion_venta"),
                    t.GetTexto(efectivo ? "CJ_tk_condicion_efectivo" : "CJ_tk_condicion_tarjeta"), fCuerpo, fNegrita))
                { Padding = 7f, BorderWidthLeft = 0f, HorizontalAlignment = Element.ALIGN_RIGHT });
                doc.Add(receptor);

                // ── Datos operativos del restaurante ───────────────────────────────
                var operativos = new PdfPTable(3) { WidthPercentage = 100f, SpacingBefore = 4f };
                operativos.AddCell(SinBorde(Dato(t.GetTexto("CJ_tk_mesa"), string.Format(t.GetTexto("CJ_tk_mesa_reserva"), factura.MesaNumero, factura.NroReserva), fChica, fChica)));
                operativos.AddCell(SinBorde(Dato(t.GetTexto("CJ_tk_camarero"), factura.CamareroNombre ?? "-", fChica, fChica)));
                operativos.AddCell(SinBorde(Dato(t.GetTexto("CJ_tk_comensales"), factura.CantidadComensales.ToString(), fChica, fChica)));
                doc.Add(operativos);

                // ── Ítems ───────────────────────────────────────────────────────────
                var items = new PdfPTable(4) { WidthPercentage = 100f, SpacingBefore = 10f, HeaderRows = 1 };
                items.SetWidths(new[] { 0.6f, 4f, 1.3f, 1.3f });
                items.AddCell(Encabezado(t.GetTexto("CJ_tk_cant"), fEncabezado, Element.ALIGN_LEFT));
                items.AddCell(Encabezado(t.GetTexto("CJ_tk_descripcion"), fEncabezado, Element.ALIGN_LEFT));
                items.AddCell(Encabezado(t.GetTexto("CJ_tk_punit"), fEncabezado, Element.ALIGN_RIGHT));
                items.AddCell(Encabezado(t.GetTexto("CJ_tk_subt"), fEncabezado, Element.ALIGN_RIGHT));
                foreach (var item in factura.Items.OrderBy(i => i.NroItem))
                {
                    items.AddCell(Renglon(item.Cantidad.ToString(), fCuerpo, Element.ALIGN_LEFT));
                    items.AddCell(Renglon(item.Descripcion, fCuerpo, Element.ALIGN_LEFT));
                    items.AddCell(Renglon(Numero(item.PrecioUnitario), fCuerpo, Element.ALIGN_RIGHT));
                    items.AddCell(Renglon(Numero(item.Subtotal), fCuerpo, Element.ALIGN_RIGHT));
                }
                doc.Add(items);
                doc.Add(new Paragraph(t.GetTexto(ivaContenido ? "CJ_tk_precios_finales" : "CJ_tk_importes_congelados"), fNota) { SpacingBefore = 3f });

                // ── Totales ─────────────────────────────────────────────────────────
                var totales = new PdfPTable(2) { WidthPercentage = 42f, HorizontalAlignment = Element.ALIGN_RIGHT, SpacingBefore = 6f };
                totales.SetWidths(new[] { 1.4f, 1f });
                FilaImporte(totales, t.GetTexto("CJ_tk_subtotal"), "$ " + Numero(factura.Subtotal), fCuerpo);
                FilaImporte(totales, string.Format(t.GetTexto("CJ_tk_descuento"), factura.PorcentajeDescuento.ToString("0.##", CulturaMoneda)),
                    factura.MontoDescuento == 0 ? "$ 0" : "- $ " + Numero(factura.MontoDescuento), fCuerpo);
                if (!ivaContenido)
                {
                    FilaImporte(totales, t.GetTexto("CJ_tk_subtotal_neto"), "$ " + Numero(factura.SubtotalNeto), fCuerpo);
                    FilaImporte(totales, t.GetTexto("CJ_tk_iva"), "$ " + Numero(factura.IVA), fCuerpo);
                }
                var rotuloTotal = new PdfPCell(new Phrase(t.GetTexto(ivaContenido ? "CJ_tk_total" : "CJ_tk_total_general"), fNegrita))
                { Border = Rectangle.TOP_BORDER, BorderWidthTop = 1.5f, PaddingTop = 6f, PaddingBottom = 4f, VerticalAlignment = Element.ALIGN_MIDDLE };
                var importeTotal = new PdfPCell(new Phrase("$ " + Numero(factura.MontoTotal), fTotal))
                { Border = Rectangle.TOP_BORDER, BorderWidthTop = 1.5f, PaddingTop = 4f, PaddingBottom = 4f, HorizontalAlignment = Element.ALIGN_RIGHT };
                totales.AddCell(rotuloTotal);
                totales.AddCell(importeTotal);
                doc.Add(totales);

                // ── Transparencia fiscal (izquierda) y detalle de pago (derecha) ────
                var bloques = new PdfPTable(3) { WidthPercentage = 100f, SpacingBefore = 14f };
                bloques.SetWidths(new[] { 1f, 0.06f, 1f });

                var transparencia = new PdfPTable(2) { WidthPercentage = 100f };
                transparencia.SetWidths(new[] { 2.4f, 1f });
                if (ivaContenido)
                {
                    transparencia.AddCell(new PdfPCell(new Phrase(t.GetTexto("CJ_tk_transparencia_titulo"), fNegrita)) { Colspan = 2, Border = Rectangle.NO_BORDER, PaddingBottom = 6f });
                    FilaImporte(transparencia, t.GetTexto("CJ_tk_iva_contenido"), "$ " + Numero(factura.IVA), fCuerpo);
                    FilaImporte(transparencia, t.GetTexto("CJ_tk_otros_impuestos"), "$ " + Numero(0m), fCuerpo);
                }
                bloques.AddCell(new PdfPCell(transparencia) { Padding = 9f, BorderWidth = ivaContenido ? 1f : 0f, VerticalAlignment = Element.ALIGN_TOP });
                bloques.AddCell(new PdfPCell { Border = Rectangle.NO_BORDER });

                var detalle = new PdfPTable(2) { WidthPercentage = 100f };
                detalle.SetWidths(new[] { 1.3f, 1f });
                detalle.AddCell(new PdfPCell(new Phrase(t.GetTexto("CJ_tk_detalle_pago"), fNegrita)) { Colspan = 2, Border = Rectangle.BOTTOM_BORDER, PaddingBottom = 5f, HorizontalAlignment = Element.ALIGN_CENTER });
                FilaImporte(detalle, t.GetTexto("CJ_tk_metodo"), (efectivo ? t.GetTexto("CJ_efectivo") : t.GetTexto("CJ_tarjeta")).Trim(), fCuerpo);
                if (efectivo)
                {
                    FilaImporte(detalle, t.GetTexto("CJ_tk_monto_recibido"), "$ " + Numero(pago.MontoRecibido ?? 0), fCuerpo);
                    FilaImporte(detalle, t.GetTexto("CJ_tk_cambio"), "$ " + Numero(pago.Cambio ?? 0), fCuerpo);
                }
                else
                {
                    string ultimos = string.IsNullOrEmpty(pago.NroTarjeta) ? "" : pago.NroTarjeta.Substring(pago.NroTarjeta.Length - 4);
                    FilaImporte(detalle, t.GetTexto("CJ_tk_tarjeta"), $"{pago.MarcaTarjeta} ···· {ultimos}", fCuerpo);
                    FilaImporte(detalle, t.GetTexto("CJ_tk_cuotas"), string.Format(t.GetTexto("CJ_tk_cuotas_detalle"), pago.CantidadCuotas ?? 1, Numero(pago.ImporteCuota ?? pago.MontoAbonado)), fCuerpo);
                    FilaImporte(detalle, t.GetTexto("CJ_tk_autorizacion"), pago.NroAutorizacion ?? "-", fCuerpo);
                }
                FilaImporte(detalle, t.GetTexto("CJ_tk_propina"), "$ " + Numero(pago.MontoPropina), fCuerpo);
                detalle.AddCell(new PdfPCell(new Phrase(t.GetTexto("CJ_tk_total_abonado"), fNegrita)) { Border = Rectangle.TOP_BORDER, PaddingTop = 5f });
                detalle.AddCell(new PdfPCell(new Phrase("$ " + Numero(pago.MontoAbonado), fNegrita)) { Border = Rectangle.TOP_BORDER, PaddingTop = 5f, HorizontalAlignment = Element.ALIGN_RIGHT });
                detalle.AddCell(new PdfPCell(new Phrase(t.GetTexto("CJ_tk_propina_nota"), fNota)) { Colspan = 2, Border = Rectangle.NO_BORDER, PaddingTop = 3f });
                bloques.AddCell(new PdfPCell(detalle) { Padding = 9f, BorderWidth = 1.5f });
                doc.Add(bloques);

                // ── Pie: QR simulado, CAE simulado e integridad ─────────────────────
                var pie = new PdfPTable(2) { WidthPercentage = 100f, SpacingBefore = 18f };
                pie.SetWidths(new[] { 0.55f, 3f });
                pie.AddCell(new PdfPCell(QrSimulado(writer.DirectContent, hash, 3.2f)) { Border = Rectangle.NO_BORDER, PaddingTop = 2f });

                var autorizacion = new PdfPCell { Border = Rectangle.NO_BORDER, PaddingLeft = 10f };
                autorizacion.AddElement(new Paragraph(t.GetTexto("CJ_tk_autorizado"), fNegrita));
                autorizacion.AddElement(new Paragraph(string.Format(t.GetTexto("CJ_tk_cae"), CaeSimulado(hash)), fCuerpo));
                autorizacion.AddElement(new Paragraph(string.Format(t.GetTexto("CJ_tk_vto_cae"), VencimientoCae(emision).ToString("dd/MM/yyyy")), fCuerpo));
                pie.AddCell(autorizacion);
                doc.Add(pie);

                var maqueta = new PdfPTable(1) { TotalWidth = 230f, LockedWidth = true, HorizontalAlignment = Element.ALIGN_CENTER, SpacingBefore = 14f };
                maqueta.AddCell(new PdfPCell(new Phrase(t.GetTexto("CJ_tk_maqueta"), fCodigo)) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5f, BorderColor = ReservaBLL_790MY.PdfGris });
                doc.Add(maqueta);

                doc.Close();
                return ms.ToArray();
            }
        }

        private static Paragraph Dato(string etiqueta, string valor, Font fEtiqueta, Font fValor)
        {
            var p = new Paragraph { Leading = 13f };
            p.Add(new Chunk(etiqueta.TrimEnd(':') + ": ", fEtiqueta));
            p.Add(new Chunk(valor, fValor));
            return p;
        }

        private static PdfPCell SinBorde(Paragraph contenido)
        {
            var celda = new PdfPCell { Border = Rectangle.NO_BORDER, PaddingLeft = 0f };
            celda.AddElement(contenido);
            return celda;
        }

        private static PdfPCell Encabezado(string texto, Font fuente, int alineacion)
            => new PdfPCell(new Phrase(texto, fuente))
            {
                BackgroundColor = ReservaBLL_790MY.PdfBordo,
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = alineacion,
                PaddingTop = 4f,
                PaddingBottom = 6f
            };

        private static PdfPCell Renglon(string texto, Font fuente, int alineacion)
            => new PdfPCell(new Phrase(texto, fuente))
            {
                Border = Rectangle.BOTTOM_BORDER,
                BorderColor = ReservaBLL_790MY.PdfLinea,
                HorizontalAlignment = alineacion,
                PaddingTop = 4f,
                PaddingBottom = 5f
            };

        private static void FilaImporte(PdfPTable tabla, string etiqueta, string valor, Font fuente)
        {
            tabla.AddCell(new PdfPCell(new Phrase(etiqueta, fuente)) { Border = Rectangle.NO_BORDER, PaddingTop = 2f, PaddingBottom = 3f });
            tabla.AddCell(new PdfPCell(new Phrase(valor, fuente)) { Border = Rectangle.NO_BORDER, PaddingTop = 2f, PaddingBottom = 3f, HorizontalAlignment = Element.ALIGN_RIGHT });
        }

        /// <summary>QR simulado (mismo patrón que el ticket) como imagen vectorial.</summary>
        private static Image QrSimulado(PdfContentByte cb, string hash, float lado)
        {
            bool[,] patron = PatronQrSimulado(hash);
            float tamano = ModulosQrSimulado * lado;
            PdfTemplate plantilla = cb.CreateTemplate(tamano, tamano);
            plantilla.SetColorFill(ReservaBLL_790MY.PdfTexto);
            for (int f = 0; f < ModulosQrSimulado; f++)
                for (int c = 0; c < ModulosQrSimulado; c++)
                    if (patron[f, c])
                        plantilla.Rectangle(c * lado, tamano - (f + 1) * lado, lado, lado);
            plantilla.Fill();
            return Image.GetInstance(plantilla);
        }

        private static string Numero(decimal valor)
            => valor.ToString(valor == decimal.Truncate(valor) ? "N0" : "N2", CulturaMoneda);

        #endregion
    }
}
