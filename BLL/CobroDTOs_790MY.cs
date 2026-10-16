using BE_08YS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL_08YS
{
    /// <summary>Mesa ocupada (reserva Asistida) en la lista "Mesas para cobrar".</summary>
    public class MesaCobroDTO_790MY
    {
        public int ReservaID { get; set; }
        public int MesaNumero { get; set; }
        public int CantidadComensales { get; set; }
        public string CamareroNombre { get; set; }
        public string ClienteNombre { get; set; }

        /// <summary>Hora de inicio del turno / apertura de la mesa.</summary>
        public TimeSpan Desde { get; set; }

        /// <summary>Subtotal de consumos entregados (sin descuento ni IVA).</summary>
        public decimal SubtotalConsumido { get; set; }

        public bool TieneFacturaAbierta { get; set; }
    }

    /// <summary>Importes calculados de una cuenta (CUN-006, paso 4).</summary>
    public class ImportesCuenta_790MY
    {
        public decimal Subtotal { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal MontoDescuento { get; set; }
        public decimal SubtotalNeto { get; set; }
        public decimal IVA { get; set; }
        public decimal MontoTotal { get; set; }
    }

    /// <summary>Detalle de consumo de una mesa listo para revisar / facturar.</summary>
    public class DetalleCuentaDTO_790MY
    {
        public int ReservaID { get; set; }
        public int MesaNumero { get; set; }
        public int CantidadComensales { get; set; }
        public string CamareroNombre { get; set; }
        public string ClienteNombre { get; set; }
        public List<ItemFactura_790MY> Items { get; set; } = new List<ItemFactura_790MY>();
        public ImportesCuenta_790MY Importes { get; set; }

        /// <summary>Factura Abierta vigente (null si todavía no se generó la cuenta).</summary>
        public Factura_790MY FacturaAbierta { get; set; }

        /// <summary>Pedidos Pendiente/EnPreparacion: impiden el cobro hasta que cocina los despache.</summary>
        public int PedidosSinDespachar { get; set; }
    }

    /// <summary>Resultado de un cobro confirmado: datos para el comprobante definitivo.</summary>
    public class ResultadoCobroDTO_790MY
    {
        public Factura_790MY Factura { get; set; }
        public Pago_790MY Pago { get; set; }
    }

    /// <summary>Arqueo de caja de una jornada.</summary>
    public class ReporteArqueoCaja_790MY
    {
        public DateTime Fecha { get; set; }
        public List<CobroArqueo_790MY> Cobros { get; set; } = new List<CobroArqueo_790MY>();

        public decimal TotalEfectivo => Cobros.Where(c => c.MetodoPago == MetodoPago_790MY.Efectivo).Sum(c => c.ImporteCobrado);
        public decimal TotalTarjetas => Cobros.Where(c => c.MetodoPago == MetodoPago_790MY.Tarjeta).Sum(c => c.ImporteCobrado);
        public decimal TotalPropinas => Cobros.Sum(c => c.MontoPropina);
        public decimal TotalDescuentos => Cobros.Sum(c => c.MontoDescuento);

        /// <summary>Efectivo + Tarjetas (descuentos deducidos, IVA incluido). Las propinas se informan aparte.</summary>
        public decimal RecaudacionNeta => TotalEfectivo + TotalTarjetas;

        public int CantidadCuentas => Cobros.Count;
        public decimal TicketPromedio => Cobros.Count == 0 ? 0 : Math.Round(RecaudacionNeta / Cobros.Count, 2, MidpointRounding.AwayFromZero);
    }
}
