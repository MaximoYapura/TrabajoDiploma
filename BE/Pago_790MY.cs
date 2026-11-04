using System;

namespace BE_08YS
{
    // Medio de pago. Se guarda como texto.
    public enum MetodoPago_790MY
    {
        Efectivo,
        Tarjeta
    }

    // Pago de una factura. Solo se guardan pagos aprobados; un rechazo del banco
    // queda únicamente en la bitácora.
    public class Pago_790MY
    {
        // Cuotas que se aceptan con tarjeta (sin recargo).
        public static readonly int[] CuotasPermitidas = { 1, 3, 6, 12 };

        public int IdPago { get; set; }
        public int NroFactura { get; set; }
        public DateTime FechaHoraPago { get; set; }
        public MetodoPago_790MY MetodoPago { get; set; }

        // Total de la factura + propina: lo que efectivamente se cobra.
        public decimal MontoAbonado { get; set; }

        // Efectivo entregado por el cliente (null para tarjeta).
        public decimal? MontoRecibido { get; set; }

        // Vuelto entregado (null para tarjeta).
        public decimal? Cambio { get; set; }

        public decimal MontoPropina { get; set; }

        // Número enmascarado ("**** **** **** 1111"); nunca el número completo.
        public string NroTarjeta { get; set; }

        // VISA / MASTERCARD / AMEX / OTRA, deducida del prefijo del número.
        public string MarcaTarjeta { get; set; }

        public int? CantidadCuotas { get; set; }
        public string NroAutorizacion { get; set; }

        // Importe de cada cuota (solo visualización).
        public decimal? ImporteCuota => CantidadCuotas.HasValue && CantidadCuotas.Value > 0
            ? Math.Round(MontoAbonado / CantidadCuotas.Value, 2, MidpointRounding.AwayFromZero)
            : (decimal?)null;
    }
}
