using BE_08YS;
using System;
using System.Globalization;

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

        /// <summary>Punto de venta con 5 dígitos, como el modelo oficial ("00001").</summary>
        public static string FormatearPuntoVenta()
            => Factura_790MY.PuntoVenta.ToString("00000", CultureInfo.InvariantCulture);

        /// <summary>
        /// Número del comprobante con 8 dígitos ("00000234"). Factura_790MY.NumeroComprobante
        /// no cambia: entra en el hash de las facturas ya emitidas.
        /// </summary>
        public static string FormatearNumeroComprobante(Factura_790MY factura)
            => factura.NroFactura.ToString("00000000", CultureInfo.InvariantCulture);

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

        #endregion
    }
}
