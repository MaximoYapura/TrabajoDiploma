using System;

namespace BE_08YS
{
    // Una cuenta cobrada, para el detalle del arqueo de caja.
    public class CobroArqueo_790MY
    {
        public int IdPago { get; set; }
        public int NroFactura { get; set; }
        public DateTime FechaHoraPago { get; set; }
        public int MesaNumero { get; set; }
        public MetodoPago_790MY MetodoPago { get; set; }
        public decimal MontoDescuento { get; set; }
        public decimal MontoPropina { get; set; }

        // Total de la factura (descuento deducido, IVA incluido, sin propina).
        public decimal ImporteCobrado { get; set; }
    }
}
