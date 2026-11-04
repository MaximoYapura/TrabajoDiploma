using BE_08YS;
using BLL_08YS;
using GUI_08YS.Estilo;
using Service_08YS;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using E = GUI_08YS.RF2.EstiloRF2_790MY;

namespace GUI_08YS.RF2
{
    // Vista previa e impresión de los comprobantes de Caja y cobro:
    // - sin pago es el pre-ticket, con la leyenda "NO VÁLIDO COMO FACTURA";
    // - con pago es la Factura B PAGADA, con el detalle del pago y el QR y el CAE simulados.
    // El ticket se dibuja con GDI+ y el mismo dibujo sirve para la pantalla y la impresora.
    public partial class FormComprobanteFactura_790MY : Form, IIdiomaObserver_08YS
    {
        private const int AnchoTicket = 400;

        private static readonly CultureInfo CulturaMoneda = CultureInfo.GetCultureInfo("es-AR");
        internal static readonly Color Tinta = Color.FromArgb(30, 28, 26);
        internal static readonly Color TintaSuave = Color.FromArgb(95, 90, 85);
        private static readonly Color Papel = Color.White;
        private static readonly Color TintaLogo = Color.Black;

        private readonly Factura_790MY _factura;
        private readonly Pago_790MY _pago;
        private readonly Image _logo;
        private Bitmap _render;

        // El dibujo del horno del logo, en blanco y negro como lo imprime una impresora
        // térmica. Se prepara una sola vez. Null si no está la imagen.
        private static readonly Lazy<Bitmap> IsotipoTermico = new Lazy<Bitmap>(PrepararIsotipo);

        public FormComprobanteFactura_790MY(Factura_790MY factura, Pago_790MY pago)
        {
            InitializeComponent();
            _factura = factura ?? throw new ArgumentNullException(nameof(factura));
            _pago = pago;

            _logo = IsotipoTermico.Value;

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) =>
            {
                TraductorManager_08YS.Instance.Desuscribir(this);
                picTicket_790MY.Image = null;
                _render?.Dispose();
            };
        }

        private bool EsFactura => _pago != null;

        private void FormComprobanteFactura_790MY_Load(object sender, EventArgs e) => UpdateIdioma();

        public void UpdateIdioma()
        {
            E.TraducirTags(this);
            Text = EsFactura
                ? E.T("CJ_comprobante_factura_titulo", _factura.NumeroComprobante)
                : E.T("CJ_comprobante_preticket_titulo", _factura.MesaNumero);
            Renderizar();
        }

        #region Render en pantalla

        private void Renderizar()
        {
            int alto;
            using (var medida = new Bitmap(1, 1))
            using (Graphics g = Graphics.FromImage(medida))
                alto = Dibujar(g, 0, 0);

            var bmp = new Bitmap(AnchoTicket, alto);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Papel);
                Dibujar(g, 0, 0);
            }

            picTicket_790MY.Image = null;
            _render?.Dispose();
            _render = bmp;
            picTicket_790MY.Image = _render;
            CentrarTicket();
        }

        private void pnlScroll_790MY_Resize(object sender, EventArgs e) => CentrarTicket();

        private void CentrarTicket()
        {
            int x = Math.Max(0, (pnlScroll_790MY.ClientSize.Width - picTicket_790MY.Width) / 2);
            picTicket_790MY.Location = new Point(x, 20 + pnlScroll_790MY.AutoScrollPosition.Y);
        }

        #endregion

        #region Impresión

        // Imprime el ticket con la vista previa de Windows.
        private void btnImprimir_790MY_Click(object sender, EventArgs e)
        {
            bnrAvisos_790MY.Ocultar();
            try
            {
                using (var doc = new PrintDocument())
                using (var preview = new PrintPreviewDialog { Document = doc, Width = 900, Height = 900 })
                {
                    doc.DocumentName = EsFactura ? _factura.NumeroComprobante : E.T("CJ_preticket_doc", _factura.MesaNumero);
                    doc.PrintPage += (s, ev) =>
                    {
                        ev.Graphics.PageUnit = GraphicsUnit.Display;
                        int alto;
                        using (var medida = new Bitmap(1, 1))
                        using (Graphics gm = Graphics.FromImage(medida))
                            alto = Dibujar(gm, 0, 0);

                        float escala = Math.Min(1f, ev.MarginBounds.Height / (float)alto);
                        ev.Graphics.TranslateTransform(ev.MarginBounds.Left, ev.MarginBounds.Top);
                        ev.Graphics.ScaleTransform(escala, escala);
                        Dibujar(ev.Graphics, 0, 0);
                        ev.HasMorePages = false;
                    };
                    preview.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                // Si falla la impresión (sin impresora, por ejemplo) se avisa en el banner y se
                // puede reintentar. El comprobante ya está grabado.
                bnrAvisos_790MY.Mostrar(E.T("CJ_msg_error_impresion", ex.Message), TipoBanner_790MY.Error, cerrable: true);
            }
        }

        #endregion

        #region Dibujo del ticket

        // Dibuja el comprobante completo y devuelve su alto en píxeles.
        private int Dibujar(Graphics g, int origenX, int origenY)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            using (var t = new TicketFactura_790MY(g, origenX, origenY, AnchoTicket))
            {
                DibujarEncabezado(t);
                if (EsFactura) DibujarDatosFactura(t); else DibujarDatosPreTicket(t);
                DibujarItems(t);
                DibujarTotales(t);
                if (EsFactura) DibujarPago(t); else DibujarPiePreTicket(t);
                t.Espacio(18);
                t.Tijera();
                t.Espacio(14);
                return (int)Math.Ceiling(t.Y - origenY);
            }
        }

        private void DibujarEncabezado(TicketFactura_790MY t)
        {
            t.Espacio(22);
            if (_logo != null)
            {
                float anchoLogo = 150f;
                float altoLogo = anchoLogo * _logo.Height / _logo.Width;
                var destino = new RectangleF(t.X + (t.Ancho - anchoLogo) / 2f, t.Y, anchoLogo, altoLogo);
                var calidad = t.G.InterpolationMode;
                t.G.InterpolationMode = InterpolationMode.HighQualityBicubic;
                t.G.DrawImage(_logo, destino);
                t.G.InterpolationMode = calidad;
                t.Espacio(altoLogo + 10);
                t.Centrado("EL FOGÓN DEL SUR", t.FNegrita);
            }
            else
            {
                t.Centrado("El Fogón", t.FMarca);
                t.Centrado("D E L   S U R", t.FNegrita);
            }

            if (EsFactura)
            {
                // Datos del emisor (Factura B, modelo de ARCA): constantes de FacturaBLL_790MY.
                t.Centrado(E.T("CJ_tk_slogan"), t.FChica);
                t.Espacio(4);
                t.Centrado(FacturaBLL_790MY.EmisorRazonSocial, t.FNegrita);
                t.Centrado(E.T("CJ_tk_cuit_iibb", FacturaBLL_790MY.EmisorCuit, FacturaBLL_790MY.EmisorIngresosBrutos), t.FChica);
                t.Centrado(FacturaBLL_790MY.EmisorDomicilio, t.FChica);
                t.Centrado(E.T("CJ_tk_inicio_actividades", FacturaBLL_790MY.EmisorInicioActividades), t.FChica);
                t.Centrado(E.T("CJ_tk_iva_ri"), t.FNegritaChica);
            }
            else
            {
                t.Centrado(E.T("CJ_tk_slogan"), t.FChica);
                t.Centrado(FacturaBLL_790MY.EmisorDomicilio, t.FChica);
            }
            t.Espacio(8);
            t.Separador(grueso: true);
            t.Espacio(8);
        }

        private void DibujarDatosPreTicket(TicketFactura_790MY t)
        {
            t.Centrado(E.T("CJ_tk_preticket_titulo"), t.FTitulo);
            t.Espacio(8);
            t.Fila(E.T("CJ_tk_fecha"), DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            t.Fila(E.T("CJ_tk_mesa"), _factura.MesaNumero.ToString(), negritaDer: true);
            t.Fila(E.T("CJ_tk_reserva"), _factura.NroReserva.ToString(), negritaDer: true);
            t.Fila(E.T("CJ_tk_camarero"), _factura.CamareroNombre ?? "-");
            t.Fila(E.T("CJ_tk_comensales"), _factura.CantidadComensales.ToString());
            t.Fila(E.T("CJ_tk_cuenta_nro"), _factura.NumeroComprobante);
            t.Espacio(10);
            t.Recuadro(E.T("CJ_tk_no_valido"), t.FNegrita, 2f);
            t.Espacio(10);
        }

        private void DibujarDatosFactura(TicketFactura_790MY t)
        {
            // "FACTURA" + recuadro con la letra del comprobante.
            float altoCaja = 44f;
            t.G.DrawString(E.T("CJ_tk_factura"), t.FTitulo, t.Pincel, t.X, t.Y + 12);
            var caja = new RectangleF(t.X + t.Ancho - 56, t.Y, 56, altoCaja);   // entra "COD. 06" completo
            using (var lapiz = new Pen(Tinta, 1.5f))
                t.G.DrawRectangle(lapiz, caja.X, caja.Y, caja.Width, caja.Height);
            using (var centro = new StringFormat { Alignment = StringAlignment.Center })
            {
                t.G.DrawString("B", t.FLetra, t.Pincel, new RectangleF(caja.X, caja.Y + 1, caja.Width, 28), centro);
                t.G.DrawString("COD. 06", t.FMini, t.PincelSuave, new RectangleF(caja.X, caja.Y + 29, caja.Width, 14), centro);
            }
            t.Espacio(altoCaja + 10);

            // Punto de venta y número por separado, como el modelo oficial.
            DateTime emision = FacturaBLL_790MY.FechaEmisionComprobante(_factura, _pago);
            t.Fila(E.T("CJ_tk_punto_venta"), FacturaBLL_790MY.FormatearPuntoVenta(), negritaDer: true);
            t.Fila(E.T("CJ_tk_comp_nro"), FacturaBLL_790MY.FormatearNumeroComprobante(_factura), negritaDer: true);
            t.Fila(E.T("CJ_tk_fecha_emision"), emision.ToString("dd/MM/yyyy HH:mm"));
            t.Espacio(8);
            t.Recuadro(E.T("CJ_tk_consumidor_final_leyenda"), t.FNegrita, 1f);
            t.Espacio(6);
            bool efectivo = _pago.MetodoPago == MetodoPago_790MY.Efectivo;
            t.Fila(E.T("CJ_tk_condicion_venta"), E.T(efectivo ? "CJ_tk_condicion_efectivo" : "CJ_tk_condicion_tarjeta"));
            t.Espacio(4);
            t.Fila(E.T("CJ_tk_mesa"), E.T("CJ_tk_mesa_reserva", _factura.MesaNumero, _factura.NroReserva));
            t.Fila(E.T("CJ_tk_camarero"), _factura.CamareroNombre ?? "-");
            t.Fila(E.T("CJ_tk_comensales"), _factura.CantidadComensales.ToString());
            t.Espacio(8);
        }

        private void DibujarItems(TicketFactura_790MY t)
        {
            t.Separador(grueso: false);
            t.Espacio(8);
            t.Columnas(E.T("CJ_tk_cant"), E.T("CJ_tk_descripcion"), E.T("CJ_tk_punit"), E.T("CJ_tk_subt"), t.FNegrita);
            t.Espacio(2);
            t.Linea(Tinta, 1f);
            t.Espacio(6);
            foreach (var item in _factura.Items.OrderBy(i => i.NroItem))
                t.Columnas(item.Cantidad.ToString(), item.Descripcion, Numero(item.PrecioUnitario), Numero(item.Subtotal), t.FCuerpo);

            if (IvaContenido)
            {
                t.Espacio(4);
                t.Izquierda(E.T("CJ_tk_precios_finales"), t.FMini, suave: true);
            }
            else if (EsFactura)
            {
                t.Espacio(4);
                t.Izquierda(E.T("CJ_tk_importes_congelados"), t.FMini, suave: true);
            }
            t.Espacio(6);
            t.Separador(grueso: false);
            t.Espacio(8);
        }

        // Factura B con el IVA incluido. Las facturas viejas sumaban el IVA aparte y se
        // reimprimen como eran.
        private bool IvaContenido => FacturaBLL_790MY.TieneIvaContenido(_factura);

        private void DibujarTotales(TicketFactura_790MY t)
        {
            t.Fila(E.T("CJ_tk_subtotal"), "$ " + Numero(_factura.Subtotal));
            t.Fila(E.T("CJ_tk_descuento", _factura.PorcentajeDescuento.ToString("0.##", CulturaMoneda)),
                   _factura.MontoDescuento == 0 ? "$ 0" : "- $ " + Numero(_factura.MontoDescuento));
            if (!IvaContenido)
            {
                t.Fila(EsFactura ? E.T("CJ_tk_subtotal_neto") : E.T("CJ_tk_subtotal_desc"), "$ " + Numero(_factura.SubtotalNeto));
                t.Fila(EsFactura ? E.T("CJ_tk_iva") : E.T("CJ_tk_iva_informativo"), "$ " + Numero(_factura.IVA));
            }
            t.Espacio(6);
            t.Linea(Tinta, 1f); t.Espacio(3); t.Linea(Tinta, 1f);
            t.Espacio(8);
            string rotuloTotal = !EsFactura ? E.T("CJ_tk_total_pagar") : IvaContenido ? E.T("CJ_tk_total") : E.T("CJ_tk_total_general");
            t.FilaGrande(rotuloTotal, "$ " + Numero(_factura.MontoTotal));
            t.Espacio(6);
            t.Linea(Tinta, 1f); t.Espacio(3); t.Linea(Tinta, 1f);
            t.Espacio(12);

            if (!IvaContenido) return;
            if (EsFactura)
            {
                DibujarTransparenciaFiscal(t);
            }
            else
            {
                t.Fila(E.T("CJ_tk_iva_contenido_informativo"), "$ " + Numero(_factura.IVA));
                t.Espacio(10);
            }
        }

        // Bloque de transparencia fiscal (Ley 27.743): cuánto IVA incluye el total.
        private void DibujarTransparenciaFiscal(TicketFactura_790MY t)
        {
            float inicio = t.Y;
            t.Margen(10);
            t.Espacio(8);
            t.Izquierda(E.T("CJ_tk_transparencia_titulo"), t.FNegritaChica);
            t.Espacio(2);
            t.Fila(E.T("CJ_tk_iva_contenido"), "$ " + Numero(_factura.IVA));
            t.Fila(E.T("CJ_tk_otros_impuestos"), "$ " + Numero(0m));
            t.Espacio(6);
            t.Margen(-10);
            using (var lapiz = new Pen(Tinta, 1f))
                t.G.DrawRectangle(lapiz, t.X, inicio, t.Ancho, t.Y - inicio);
            t.Espacio(14);
        }

        private void DibujarPiePreTicket(TicketFactura_790MY t)
        {
            t.Centrado(E.T("CJ_tk_gracias_preferencia"), t.FCuerpo);
            t.Centrado(E.T("CJ_tk_solicite_factura"), t.FCuerpo);
            t.Espacio(10);
            t.Centrado(E.T("CJ_tk_preticket_pie", _factura.MesaNumero), t.FMini, suave: true);
        }

        private void DibujarPago(TicketFactura_790MY t)
        {
            // ─── Recuadro "DETALLE DE PAGO" ───
            float inicio = t.Y;
            t.Margen(14);
            t.Espacio(10);
            t.Centrado(E.T("CJ_tk_detalle_pago"), t.FNegrita);
            t.Espacio(2);
            t.Linea(Tinta, 1f);
            t.Espacio(6);

            bool efectivo = _pago.MetodoPago == MetodoPago_790MY.Efectivo;
            t.Fila(E.T("CJ_tk_metodo"), efectivo ? E.T("CJ_efectivo").Trim() : E.T("CJ_tarjeta").Trim(), negritaDer: true);
            if (efectivo)
            {
                t.Fila(E.T("CJ_tk_cuotas"), E.T("CJ_tk_no_aplica"));
                t.Fila(E.T("CJ_tk_monto_recibido"), "$ " + Numero(_pago.MontoRecibido ?? 0));
                t.Fila(E.T("CJ_tk_cambio"), "$ " + Numero(_pago.Cambio ?? 0), negritaDer: true);
            }
            else
            {
                string ultimos = string.IsNullOrEmpty(_pago.NroTarjeta) ? "" : _pago.NroTarjeta.Substring(_pago.NroTarjeta.Length - 4);
                t.Fila(E.T("CJ_tk_tarjeta"), $"{_pago.MarcaTarjeta} ···· {ultimos}");
                t.Fila(E.T("CJ_tk_cuotas"), E.T("CJ_tk_cuotas_detalle", _pago.CantidadCuotas ?? 1, Numero(_pago.ImporteCuota ?? _pago.MontoAbonado)));
                t.Fila(E.T("CJ_tk_autorizacion"), _pago.NroAutorizacion ?? "-", negritaDer: true);
            }
            t.Fila(E.T("CJ_tk_propina"), "$ " + Numero(_pago.MontoPropina));
            t.Espacio(4);
            t.Separador(grueso: false);
            t.Espacio(6);
            t.Fila(E.T("CJ_tk_total_abonado"), "$ " + Numero(_pago.MontoAbonado), negritaIzq: true, negritaDer: true);
            t.Espacio(2);
            t.Izquierda(E.T("CJ_tk_propina_nota"), t.FMini, suave: true);
            t.Espacio(8);
            t.Margen(-14);
            using (var lapiz = new Pen(Tinta, 2f))
                t.G.DrawRectangle(lapiz, t.X, inicio, t.Ancho, t.Y - inicio);
            t.Espacio(22);

            // ─── Sello PAGADA ───
            DibujarSello(t);
            t.Espacio(14);
            t.Separador(grueso: false);
            t.Espacio(12);

            // ─── QR simulado + CAE simulado (derivados del hash, que no se imprime) ───
            string hash = FacturaBLL_790MY.CalcularHashComprobante(_factura, _pago);
            DibujarAutorizacion(t, hash);
            t.Centrado(E.T("CJ_tk_qr"), t.FMini, suave: true);
            t.Espacio(8);
            t.Linea(Tinta, 1f);
            t.Espacio(12);
            t.Centrado(E.T("CJ_tk_gracias"), t.FChica);
            t.Espacio(8);
            t.Recuadro(E.T("CJ_tk_maqueta"), t.FMini, 1f, angosto: true);
        }

        private void DibujarSello(TicketFactura_790MY t)
        {
            var estado = t.G.Save();
            float cx = t.X + t.Ancho / 2f, cy = t.Y + 40;
            t.G.TranslateTransform(cx, cy);
            t.G.RotateTransform(-8f);
            var rect = new RectangleF(-110, -36, 220, 72);
            using (var lapiz = new Pen(Tinta, 1.5f))
            {
                t.G.DrawRectangle(lapiz, rect.X, rect.Y, rect.Width, rect.Height);
                t.G.DrawRectangle(lapiz, rect.X + 4, rect.Y + 4, rect.Width - 8, rect.Height - 8);
            }

            // "PAGADA" / "PAID" según el idioma activo, con espaciado uniforme entre letras.
            DibujarTextoEspaciado(t.G, E.T("CJ_tk_sello"), t.FSello, t.Pincel,
                                  new RectangleF(rect.X + 12, rect.Y + 8, rect.Width - 24, 34));
            using (var centro = new StringFormat { Alignment = StringAlignment.Center })
                t.G.DrawString(E.T("CJ_tk_liquidada", (_pago.FechaHoraPago).ToString("dd/MM/yyyy · HH:mm")),
                               t.FNegritaChica, t.Pincel, new RectangleF(rect.X, rect.Y + 44, rect.Width, 20), centro);
            t.G.Restore(estado);
            t.Espacio(84);
        }

        // Dibuja el texto letra por letra, centrado y con el mismo espacio entre letras (para
        // el sello "P A G A D A"). Si no entra, primero achica el espacio y después la letra.
        private static void DibujarTextoEspaciado(Graphics g, string texto, Font fuente, Brush pincel, RectangleF area)
        {
            string letras = new string((texto ?? string.Empty).Where(c => !char.IsWhiteSpace(c)).ToArray())
                .ToUpper(CultureInfo.CurrentCulture);
            if (letras.Length == 0) return;

            using (var tipografico = (StringFormat)StringFormat.GenericTypographic.Clone())
            {
                Font actual = fuente;
                try
                {
                    float[] anchos = null;
                    float tracking = 0f, total = 0f;
                    for (int intento = 0; intento < 12; intento++)
                    {
                        Font f = actual;
                        anchos = letras.Select(c => g.MeasureString(c.ToString(), f, PointF.Empty, tipografico).Width).ToArray();
                        float soloLetras = anchos.Sum();
                        float maxTracking = letras.Length > 1 ? (area.Width - soloLetras) / (letras.Length - 1) : 0f;
                        tracking = Math.Max(0f, Math.Min(f.Size * 0.55f, maxTracking));   // ≈ media letra
                        total = soloLetras + tracking * (letras.Length - 1);
                        if (total <= area.Width) break;

                        var menor = new Font(f.FontFamily, f.Size * 0.9f, f.Style);
                        if (!ReferenceEquals(actual, fuente)) actual.Dispose();
                        actual = menor;
                    }

                    float alto = g.MeasureString(letras, actual, PointF.Empty, tipografico).Height;
                    float x = area.X + (area.Width - total) / 2f;
                    float y = area.Y + (area.Height - alto) / 2f;
                    for (int i = 0; i < letras.Length; i++)
                    {
                        g.DrawString(letras[i].ToString(), actual, pincel, x, y, tipografico);
                        x += anchos[i] + tracking;
                    }
                }
                finally
                {
                    if (!ReferenceEquals(actual, fuente)) actual.Dispose();
                }
            }
        }

        // Pie de la factura: el QR a la izquierda y a la derecha "Comprobante autorizado",
        // el CAE y su vencimiento. Todo simulado a partir del hash.
        private void DibujarAutorizacion(TicketFactura_790MY t, string hash)
        {
            const float lado = 4f;
            float tamano = DibujarQrSimulado(t, hash, t.X + 6, t.Y, lado);

            float x = t.X + tamano + 24, y = t.Y + 6, ancho = t.X + t.Ancho - x;
            DateTime vto = FacturaBLL_790MY.VencimientoCae(FacturaBLL_790MY.FechaEmisionComprobante(_factura, _pago));
            var lineas = new[]
            {
                (E.T("CJ_tk_autorizado"), t.FNegritaChica),
                (E.T("CJ_tk_cae", FacturaBLL_790MY.CaeSimulado(hash)), t.FChica),
                (E.T("CJ_tk_vto_cae", vto.ToString("dd/MM/yyyy")), t.FChica)
            };
            foreach (var (texto, fuente) in lineas)
            {
                SizeF med = t.G.MeasureString(texto, fuente, (int)ancho);
                t.G.DrawString(texto, fuente, t.Pincel, new RectangleF(x, y, ancho, med.Height + 2));
                y += med.Height + 3;
            }
            t.Espacio(Math.Max(tamano, y - t.Y) + 14);
        }

        // QR simulado (patrón de FacturaBLL_790MY.PatronQrSimulado, el mismo del PDF). Devuelve su lado.
        private static float DibujarQrSimulado(TicketFactura_790MY t, string hash, float x0, float y0, float lado)
        {
            bool[,] patron = FacturaBLL_790MY.PatronQrSimulado(hash);
            int modulos = FacturaBLL_790MY.ModulosQrSimulado;
            float tamano = modulos * lado;

            using (var pincel = new SolidBrush(Tinta))
            {
                for (int f = 0; f < modulos; f++)
                    for (int c = 0; c < modulos; c++)
                        if (patron[f, c]) t.G.FillRectangle(pincel, x0 + c * lado, y0 + f * lado, lado, lado);
            }
            using (var lapiz = new Pen(Color.FromArgb(200, 195, 190)))
                t.G.DrawRectangle(lapiz, x0 - 6, y0 - 6, tamano + 12, tamano + 12);
            return tamano;
        }

        // Recorta el dibujo del horno y lo pasa a negro puro sobre fondo transparente.
        private static Bitmap PrepararIsotipo()
        {
            Bitmap original;
            try { original = Properties.Resources.LogoFogonBN; }
            catch (Exception) { return null; }
            if (original == null) return null;

            const int umbral = 140;   // luminancia por debajo de la cual el píxel es trazo
            const int margen = 8;

            using (original)
            using (var fuente = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(fuente))
                    g.DrawImage(original, 0, 0, original.Width, original.Height);

                int ancho = fuente.Width, alto = fuente.Height;
                var datos = fuente.LockBits(new Rectangle(0, 0, ancho, alto), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                var pixeles = new int[ancho * alto];
                Marshal.Copy(datos.Scan0, pixeles, 0, pixeles.Length);
                fuente.UnlockBits(datos);

                bool EsTrazo(int argb)
                {
                    int r = (argb >> 16) & 0xFF, g = (argb >> 8) & 0xFF, b = argb & 0xFF;
                    return (r * 299 + g * 587 + b * 114) / 1000 < umbral;
                }

                int minX = ancho, minY = alto, maxX = -1, maxY = -1;
                for (int y = 0; y < alto; y++)
                    for (int x = 0; x < ancho; x++)
                        if (EsTrazo(pixeles[y * ancho + x]))
                        {
                            if (x < minX) minX = x;
                            if (x > maxX) maxX = x;
                            if (y < minY) minY = y;
                            if (y > maxY) maxY = y;
                        }
                if (maxX < 0) return null;

                // La imagen trae el horno y abajo el texto del logo. Se corta en el primer
                // espacio en blanco debajo del horno para quedarse solo con el dibujo.
                int franjaMinima = Math.Max(6, alto / 40);
                int vacias = 0;
                for (int y = minY; y <= maxY; y++)
                {
                    bool filaVacia = true;
                    for (int x = 0; x < ancho && filaVacia; x++)
                        if (EsTrazo(pixeles[y * ancho + x])) filaVacia = false;

                    vacias = filaVacia ? vacias + 1 : 0;
                    if (vacias < franjaMinima) continue;

                    maxY = y - vacias;
                    minX = ancho; maxX = -1;
                    for (int yy = minY; yy <= maxY; yy++)
                        for (int x = 0; x < ancho; x++)
                            if (EsTrazo(pixeles[yy * ancho + x]))
                            {
                                if (x < minX) minX = x;
                                if (x > maxX) maxX = x;
                            }
                    break;
                }

                minX = Math.Max(0, minX - margen); minY = Math.Max(0, minY - margen);
                maxX = Math.Min(ancho - 1, maxX + margen); maxY = Math.Min(alto - 1, maxY + margen);
                int w = maxX - minX + 1, h = maxY - minY + 1;

                var salida = new int[w * h];
                int negro = TintaLogo.ToArgb();
                for (int y = 0; y < h; y++)
                    for (int x = 0; x < w; x++)
                        salida[y * w + x] = EsTrazo(pixeles[(y + minY) * ancho + x + minX]) ? negro : 0;

                var resultado = new Bitmap(w, h, PixelFormat.Format32bppArgb);
                var destino = resultado.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                Marshal.Copy(salida, 0, destino.Scan0, salida.Length);
                resultado.UnlockBits(destino);
                return resultado;
            }
        }

        private static string Numero(decimal valor)
            => valor.ToString(valor == decimal.Truncate(valor) ? "N0" : "N2", CulturaMoneda);

        #endregion
    }
}
