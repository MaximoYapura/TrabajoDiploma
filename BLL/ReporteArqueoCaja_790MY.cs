using BE_08YS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL_08YS
{
    // Arqueo de caja de un día. Todos los totales se calculan a partir de la lista de cobros.
    public class ReporteArqueoCaja_790MY
    {
        public DateTime Fecha { get; set; }
        public List<CobroArqueo_790MY> Cobros { get; set; } = new List<CobroArqueo_790MY>();

        public decimal TotalEfectivo => Cobros.Where(c => c.MetodoPago == MetodoPago_790MY.Efectivo).Sum(c => c.ImporteCobrado);
        public decimal TotalTarjetas => Cobros.Where(c => c.MetodoPago == MetodoPago_790MY.Tarjeta).Sum(c => c.ImporteCobrado);
        public decimal TotalPropinas => Cobros.Sum(c => c.MontoPropina);
        public decimal TotalDescuentos => Cobros.Sum(c => c.MontoDescuento);

        // Efectivo más tarjetas, con el descuento aplicado y el IVA incluido. Las propinas van aparte.
        public decimal RecaudacionNeta => TotalEfectivo + TotalTarjetas;

        public int CantidadCuentas => Cobros.Count;
        public decimal TicketPromedio => Cobros.Count == 0 ? 0 : Math.Round(RecaudacionNeta / Cobros.Count, 2, MidpointRounding.AwayFromZero);
    }
}
