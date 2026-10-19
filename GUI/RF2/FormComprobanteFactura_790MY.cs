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
    /// <summary>
    /// Vista previa e impresión de los comprobantes de Caja y cobro:
    ///  - Sin pago: cuenta / pre-ticket de la factura Abierta (CUN-006), con la leyenda
    ///    "NO VÁLIDO COMO FACTURA".
    ///  - Con pago: Factura B definitiva en estado PAGADA (CUN-007), con detalle del
    ///    pago, QR simulado, código de verificación y hash SHA-256 calculados por la BLL.
    /// El ticket se dibuja con GDI+ una sola vez y se reutiliza para pantalla e impresora.
    /// </summary>
    public partial class FormComprobanteFactura_790MY : Form, IIdiomaObserver_08YS
    {
        private const int AnchoTicket = 400;

        private static readonly CultureInfo CulturaMoneda = CultureInfo.GetCultureInfo("es-AR");
        private static readonly Color Tinta = Color.FromArgb(30, 28, 26);
        private static readonly Color TintaSuave = Color.FromArgb(95, 90, 85);
        private static readonly Color Papel = Color.White;
        private static readonly Color TintaLogo = Color.Black;

        private readonly Factura_790MY _factura;
        private readonly Pago_790MY _pago;
        private readonly Image _logo;
        private Bitmap _render;

        /// <summary>
        /// Isotipo monocromático (horno con fuego) preparado una sola vez por ejecución:
        /// recortado al horno (sin el logotipo de texto ni bloque de color de fondo) y
        /// binarizado a trazo negro sólido sobre papel blanco, como lo imprime una
        /// comandera térmica de 80 mm. Null si el recurso no está disponible.
        /// </summary>
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
                // Etapa 4: la falla de impresión (sin impresora, cola detenida) no es un error
                // fatal: el comprobante ya está registrado. Se informa en el banner y se puede
                // reintentar sin cerrar la vista previa.
                bnrAvisos_790MY.Mostrar(E.T("CJ_msg_error_impresion", ex.Message), TipoBanner_790MY.Error, cerrable: true);
            }
        }

        #endregion

        #region Dibujo del ticket

        /// <summary>Dibuja el comprobante completo y devuelve su alto en píxeles.</summary>
        private int Dibujar(Graphics g, int origenX, int origenY)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            using (var t = new Ticket(g, origenX, origenY, AnchoTicket))
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

        private void DibujarEncabezado(Ticket t)
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
                t.Centrado("El Fogón del Sur S.R.L.", t.FNegrita);
                t.Centrado("CUIT: 30-12345678-9", t.FChica);
                t.Centrado("Av. del Fogón 1234 · Buenos Aires", t.FChica);
                t.Centrado(E.T("CJ_tk_iva_ri"), t.FChica);
            }
            else
            {
                t.Centrado(E.T("CJ_tk_slogan"), t.FChica);
                t.Centrado("Av. del Fogón 1234 · Buenos Aires", t.FChica);
            }
            t.Espacio(8);
            t.Separador(grueso: true);
            t.Espacio(8);
        }

        private void DibujarDatosPreTicket(Ticket t)
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

        private void DibujarDatosFactura(Ticket t)
        {
            // "FACTURA" + recuadro con la letra del comprobante.
            float altoCaja = 44f;
            t.G.DrawString(E.T("CJ_tk_factura"), t.FTitulo, t.Pincel, t.X, t.Y + 12);
            var caja = new RectangleF(t.X + t.Ancho - 46, t.Y, 46, altoCaja);
            using (var lapiz = new Pen(Tinta, 1.5f))
                t.G.DrawRectangle(lapiz, caja.X, caja.Y, caja.Width, caja.Height);
            using (var centro = new StringFormat { Alignment = StringAlignment.Center })
            {
                t.G.DrawString("B", t.FLetra, t.Pincel, new RectangleF(caja.X, caja.Y + 1, caja.Width, 28), centro);
                t.G.DrawString("COD. 06", t.FMini, t.PincelSuave, new RectangleF(caja.X, caja.Y + 29, caja.Width, 14), centro);
            }
            t.Espacio(altoCaja + 10);

            DateTime momento = _pago?.FechaHoraPago ?? _factura.FechaEmision;
            t.Fila(E.T("CJ_tk_nro"), _factura.NumeroComprobante, negritaDer: true);
            t.Fila(E.T("CJ_tk_fecha"), momento.ToString("dd/MM/yyyy"));
            t.Fila(E.T("CJ_tk_hora"), momento.ToString("HH:mm:ss"));
            t.Fila(E.T("CJ_tk_cliente"), E.T("CJ_tk_consumidor_final"));
            t.Fila(E.T("CJ_tk_mesa"), E.T("CJ_tk_mesa_reserva", _factura.MesaNumero, _factura.NroReserva));
            t.Fila(E.T("CJ_tk_camarero"), _factura.CamareroNombre ?? "-");
            t.Espacio(8);
        }

        private void DibujarItems(Ticket t)
        {
            t.Separador(grueso: false);
            t.Espacio(8);
            t.Columnas(E.T("CJ_tk_cant"), E.T("CJ_tk_descripcion"), E.T("CJ_tk_punit"), E.T("CJ_tk_subt"), t.FNegrita);
            t.Espacio(2);
            t.Linea(Tinta, 1f);
            t.Espacio(6);
            foreach (var item in _factura.Items.OrderBy(i => i.NroItem))
                t.Columnas(item.Cantidad.ToString(), item.Descripcion, Numero(item.PrecioUnitario), Numero(item.Subtotal), t.FCuerpo);

            if (EsFactura)
            {
                t.Espacio(4);
                t.Izquierda(E.T("CJ_tk_importes_congelados"), t.FMini, suave: true);
            }
            t.Espacio(6);
            t.Separador(grueso: false);
            t.Espacio(8);
        }

        private void DibujarTotales(Ticket t)
        {
            t.Fila(E.T("CJ_tk_subtotal"), "$ " + Numero(_factura.Subtotal));
            t.Fila(E.T("CJ_tk_descuento", _factura.PorcentajeDescuento.ToString("0.##", CulturaMoneda)),
                   _factura.MontoDescuento == 0 ? "$ 0" : "- $ " + Numero(_factura.MontoDescuento));
            t.Fila(EsFactura ? E.T("CJ_tk_subtotal_neto") : E.T("CJ_tk_subtotal_desc"), "$ " + Numero(_factura.SubtotalNeto));
            t.Fila(EsFactura ? E.T("CJ_tk_iva") : E.T("CJ_tk_iva_informativo"), "$ " + Numero(_factura.IVA));
            t.Espacio(6);
            t.Linea(Tinta, 1f); t.Espacio(3); t.Linea(Tinta, 1f);
            t.Espacio(8);
            t.FilaGrande(EsFactura ? E.T("CJ_tk_total_general") : E.T("CJ_tk_total_pagar"), "$ " + Numero(_factura.MontoTotal));
            t.Espacio(6);
            t.Linea(Tinta, 1f); t.Espacio(3); t.Linea(Tinta, 1f);
            t.Espacio(12);
        }

        private void DibujarPiePreTicket(Ticket t)
        {
            t.Centrado(E.T("CJ_tk_gracias_preferencia"), t.FCuerpo);
            t.Centrado(E.T("CJ_tk_solicite_factura"), t.FCuerpo);
            t.Espacio(10);
            t.Centrado(E.T("CJ_tk_preticket_pie", _factura.MesaNumero), t.FMini, suave: true);
        }

        private void DibujarPago(Ticket t)
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
            t.Espacio(10);
            t.Margen(-14);
            using (var lapiz = new Pen(Tinta, 2f))
                t.G.DrawRectangle(lapiz, t.X, inicio, t.Ancho, t.Y - inicio);
            t.Espacio(22);

            // ─── Sello PAGADA ───
            DibujarSello(t);
            t.Espacio(14);
            t.Separador(grueso: false);
            t.Espacio(12);

            // ─── QR simulado + integridad ───
            string hash = FacturaBLL_790MY.CalcularHashComprobante(_factura, _pago);
            DibujarQrSimulado(t, hash);
            t.Centrado(E.T("CJ_tk_qr"), t.FMini, suave: true);
            t.Espacio(8);
            t.Linea(Tinta, 1f);
            t.Espacio(8);
            t.Centrado(E.T("CJ_tk_dv", FacturaBLL_790MY.CodigoVerificacion(hash)), t.FChica);
            t.Centrado(E.T("CJ_tk_hash"), t.FNegritaChica);
            t.Centrado(hash.Substring(0, 8) + " " + hash.Substring(8, 8), t.FChica);
            t.Centrado(hash.Substring(16, 8) + " " + hash.Substring(24, 8), t.FChica);
            t.Espacio(12);
            t.Centrado(E.T("CJ_tk_gracias"), t.FChica);
            t.Espacio(8);
            t.Recuadro(E.T("CJ_tk_maqueta"), t.FMini, 1f, angosto: true);
        }

        private void DibujarSello(Ticket t)
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

        /// <summary>
        /// Dibuja el texto letra por letra, centrado en el área y con el mismo espacio entre
        /// cada par de letras. Cada glifo se mide con StringFormat tipográfico (sin el relleno
        /// que GDI+ agrega por defecto), así "P A G A D A" y "P A I D" no quedan pegadas ni
        /// desparejas. Si el texto no entra, se reduce primero el espaciado y luego la fuente.
        /// </summary>
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

        /// <summary>Patrón tipo QR derivado del hash (simulación visual, no decodificable).</summary>
        private static void DibujarQrSimulado(Ticket t, string hash)
        {
            const int modulos = 25;
            const float lado = 5f;
            float tamano = modulos * lado;
            float x0 = t.X + (t.Ancho - tamano) / 2f, y0 = t.Y;

            var semilla = new Random(Convert.ToInt32(hash.Substring(0, 7), 16));
            using (var pincel = new SolidBrush(Tinta))
            {
                for (int f = 0; f < modulos; f++)
                    for (int c = 0; c < modulos; c++)
                    {
                        bool enPatron = (f < 8 && c < 8) || (f < 8 && c >= modulos - 8) || (f >= modulos - 8 && c < 8);
                        bool lleno = enPatron ? ModuloPatron(f, c, modulos) : semilla.Next(100) < 48;
                        if (lleno) t.G.FillRectangle(pincel, x0 + c * lado, y0 + f * lado, lado, lado);
                    }
            }
            using (var lapiz = new Pen(Color.FromArgb(200, 195, 190)))
                t.G.DrawRectangle(lapiz, x0 - 6, y0 - 6, tamano + 12, tamano + 12);
            t.Espacio(tamano + 14);
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

        /// <summary>
        /// Recorta el isotipo B/N a la caja de sus trazos (con un margen) y lo binariza:
        /// trazo negro sólido y fondo transparente, sin grises de la compresión JPEG.
        /// </summary>
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

                // El recurso trae el isotipo (horno con fuego) y debajo el logotipo de texto.
                // En el ticket va solo el isotipo: se corta en la primera franja horizontal
                // vacía suficientemente alta que aparece después del dibujo.
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

        /// <summary>Cursor de escritura del ticket: posición vertical, márgenes y fuentes.</summary>
        private sealed class Ticket : IDisposable
        {
            private const float MargenLateral = 22f;

            public Graphics G { get; }
            public float X { get; private set; }
            public float Y { get; private set; }
            public float Ancho { get; private set; }

            public readonly Font FCuerpo = new Font("Courier New", 9.5f);
            public readonly Font FChica = new Font("Courier New", 8.5f);
            public readonly Font FMini = new Font("Courier New", 7.5f);
            public readonly Font FNegrita = new Font("Courier New", 9.5f, FontStyle.Bold);
            public readonly Font FNegritaChica = new Font("Courier New", 8.5f, FontStyle.Bold);
            public readonly Font FTitulo = new Font("Courier New", 12f, FontStyle.Bold);
            public readonly Font FTotal = new Font("Courier New", 15f, FontStyle.Bold);
            public readonly Font FLetra = new Font("Courier New", 18f, FontStyle.Bold);
            public readonly Font FSello = new Font("Courier New", 24f, FontStyle.Bold);
            public readonly Font FMarca = new Font("Georgia", 20f, FontStyle.Bold);
            public readonly SolidBrush Pincel = new SolidBrush(Tinta);
            public readonly SolidBrush PincelSuave = new SolidBrush(TintaSuave);

            private readonly StringFormat _centro = new StringFormat { Alignment = StringAlignment.Center };
            private readonly StringFormat _derecha = new StringFormat { Alignment = StringAlignment.Far };

            public Ticket(Graphics g, float x, float y, float anchoTotal)
            {
                G = g;
                X = x + MargenLateral;
                Y = y;
                Ancho = anchoTotal - 2 * MargenLateral;
            }

            public void Espacio(float alto) => Y += alto;

            /// <summary>Achica (positivo) o restaura (negativo) el área de escritura.</summary>
            public void Margen(float delta)
            {
                X += delta;
                Ancho -= 2 * delta;
            }

            public void Centrado(string texto, Font fuente, bool suave = false)
            {
                var area = new RectangleF(X, Y, Ancho, 1000);
                SizeF med = G.MeasureString(texto, fuente, (int)Ancho, _centro);
                G.DrawString(texto, fuente, suave ? PincelSuave : Pincel, area, _centro);
                Y += med.Height + 2;
            }

            public void Izquierda(string texto, Font fuente, bool suave = false)
            {
                SizeF med = G.MeasureString(texto, fuente, (int)Ancho);
                G.DrawString(texto, fuente, suave ? PincelSuave : Pincel, new RectangleF(X, Y, Ancho, med.Height + 2));
                Y += med.Height + 2;
            }

            public void Fila(string izquierda, string derecha, bool negritaIzq = false, bool negritaDer = false)
            {
                Font fi = negritaIzq ? FNegrita : FCuerpo;
                Font fd = negritaDer ? FNegrita : FCuerpo;
                float alto = Math.Max(G.MeasureString(izquierda, fi).Height, G.MeasureString(derecha, fd).Height);
                G.DrawString(izquierda, fi, Pincel, X, Y);
                G.DrawString(derecha, fd, Pincel, new RectangleF(X, Y, Ancho, alto + 2), _derecha);
                Y += alto + 3;
            }

            public void FilaGrande(string izquierda, string derecha)
            {
                SizeF med = G.MeasureString(derecha, FTotal);
                G.DrawString(izquierda, FNegrita, Pincel, X, Y + (med.Height - G.MeasureString(izquierda, FNegrita).Height) / 2f + 2);
                G.DrawString(derecha, FTotal, Pincel, new RectangleF(X, Y, Ancho, med.Height + 2), _derecha);
                Y += med.Height + 2;
            }

            /// <summary>Cant | Descripción (con salto de línea) | P.Unit | Subt.</summary>
            public void Columnas(string cant, string descripcion, string precio, string subtotal, Font fuente)
            {
                float wCant = 36, wPrecio = 78, wSub = 78;
                float wDesc = Ancho - wCant - wPrecio - wSub - 8;
                SizeF medDesc = G.MeasureString(descripcion, fuente, (int)wDesc);
                float alto = Math.Max(medDesc.Height, G.MeasureString("0", fuente).Height);

                G.DrawString(cant, fuente, Pincel, X, Y);
                G.DrawString(descripcion, fuente, Pincel, new RectangleF(X + wCant, Y, wDesc, alto + 2));
                G.DrawString(precio, fuente, Pincel, new RectangleF(X + wCant + wDesc, Y, wPrecio, alto), _derecha);
                G.DrawString(subtotal, fuente, Pincel, new RectangleF(X + Ancho - wSub, Y, wSub, alto), _derecha);
                Y += alto + 4;
            }

            public void Linea(Color color, float grosor)
            {
                using (var lapiz = new Pen(color, grosor))
                    G.DrawLine(lapiz, X, Y, X + Ancho, Y);
            }

            public void Separador(bool grueso)
            {
                using (var lapiz = new Pen(Tinta, grueso ? 2f : 1f) { DashStyle = DashStyle.Dash })
                    G.DrawLine(lapiz, X, Y, X + Ancho, Y);
                Y += grueso ? 2 : 1;
            }

            public void Recuadro(string texto, Font fuente, float grosor, bool angosto = false)
            {
                float ancho = angosto ? Math.Min(Ancho, G.MeasureString(texto, fuente).Width + 24) : Ancho;
                float x = X + (Ancho - ancho) / 2f;
                SizeF med = G.MeasureString(texto, fuente, (int)(ancho - 20), _centro);
                var rect = new RectangleF(x, Y, ancho, med.Height + 16);
                using (var lapiz = new Pen(Tinta, grosor))
                    G.DrawRectangle(lapiz, rect.X, rect.Y, rect.Width, rect.Height);
                G.DrawString(texto, fuente, Pincel, new RectangleF(rect.X + 10, rect.Y + 8, rect.Width - 20, med.Height + 2), _centro);
                Y += rect.Height;
            }

            public void Tijera()
            {
                using (var simbolo = new Font("Segoe UI Symbol", 11f))
                    G.DrawString("✂", simbolo, PincelSuave, X, Y - 11);
                using (var lapiz = new Pen(TintaSuave, 1f) { DashStyle = DashStyle.Dash })
                    G.DrawLine(lapiz, X + 22, Y, X + Ancho, Y);
            }

            public void Dispose()
            {
                foreach (var f in new[] { FCuerpo, FChica, FMini, FNegrita, FNegritaChica, FTitulo, FTotal, FLetra, FSello, FMarca })
                    f.Dispose();
                Pincel.Dispose();
                PincelSuave.Dispose();
                _centro.Dispose();
                _derecha.Dispose();
            }
        }
    }
}
