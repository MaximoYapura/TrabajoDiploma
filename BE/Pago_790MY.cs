using System;

namespace BE_08YS
{
    /// <summary>Se persiste como texto (nvarchar(20)).</summary>
    public enum MetodoPago_790MY
    {
        Efectivo,
        Tarjeta
    }

    /// <summary>
    /// Pago confirmado de una factura (CUN-007). Solo se persisten pagos aprobados:
    /// un rechazo de la entidad bancaria queda únicamente en la bitácora.
    /// </summary>
    public class Pago_790MY
    {
        /// <summary>Planes de cuotas habilitados para tarjeta (simulación sin recargo).</summary>
        public static readonly int[] CuotasPermitidas = { 1, 3, 6, 12 };

        public int IdPago { get; set; }
        public int NroFactura { get; set; }
        public DateTime FechaHoraPago { get; set; }
        public MetodoPago_790MY MetodoPago { get; set; }

        /// <summary>Total de la factura + propina: lo que efectivamente se cobra.</summary>
        public decimal MontoAbonado { get; set; }

        /// <summary>Efectivo entregado por el cliente (null para tarjeta).</summary>
        public decimal? MontoRecibido { get; set; }

        /// <summary>Vuelto entregado (null para tarjeta).</summary>
        public decimal? Cambio { get; set; }

        public decimal MontoPropina { get; set; }

        /// <summary>Número enmascarado ("**** **** **** 1111"); nunca el número completo.</summary>
        public string NroTarjeta { get; set; }

        /// <summary>VISA / MASTERCARD / AMEX / OTRA, deducida del prefijo del número.</summary>
        public string MarcaTarjeta { get; set; }

        public int? CantidadCuotas { get; set; }
        public string NroAutorizacion { get; set; }

        /// <summary>Importe de cada cuota (solo visualización).</summary>
        public decimal? ImporteCuota => CantidadCuotas.HasValue && CantidadCuotas.Value > 0
            ? Math.Round(MontoAbonado / CantidadCuotas.Value, 2, MidpointRounding.AwayFromZero)
            : (decimal?)null;
    }
}
