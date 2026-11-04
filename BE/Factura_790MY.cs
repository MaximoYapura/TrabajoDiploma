using System;
using System.Collections.Generic;

namespace BE_08YS
{
    // Estados de la factura (se guardan como texto):
    // Abierta: emitida y pendiente de cobro.
    // Pagada: cobrada; la reserva queda Finalizada.
    // Anulada: reemplazada por otra porque cambiaron los consumos o el descuento. Nunca se borra.
    public enum EstadoFactura_790MY
    {
        Abierta,
        Pagada,
        Anulada
    }

    // Cómo se carga el descuento en la caja: por porcentaje o por monto fijo.
    public enum TipoDescuento_790MY
    {
        Porcentaje,
        MontoFijo
    }

    // Factura B de una reserva. Los importes quedan fijos al emitirla.
    public class Factura_790MY
    {
        // Alícuota de IVA contenida en el precio final (Factura B): IVA = total × 0,21 / 1,21.
        public const decimal AlicuotaIVA = 0.21m;

        // Punto de venta fijo del comprobante simulado (Factura B).
        public const int PuntoVenta = 1;

        public int NroFactura { get; set; }
        public int NroReserva { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Subtotal { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal MontoDescuento { get; set; }
        public decimal SubtotalNeto { get; set; }
        public decimal IVA { get; set; }
        public decimal MontoTotal { get; set; }
        public EstadoFactura_790MY Estado { get; set; } = EstadoFactura_790MY.Abierta;

        // Detalle congelado al emitir: precios y descripciones no cambian aunque cambie la carta.
        public List<ItemFactura_790MY> Items { get; set; } = new List<ItemFactura_790MY>();

        // Datos para el comprobante. Se leen con JOIN y no se guardan en Facturas.
        public int MesaNumero { get; set; }
        public int CantidadComensales { get; set; }
        public string ClienteNombre { get; set; }
        public string CamareroNombre { get; set; }

        // Número completo, por ejemplo "B-0001-00000234".
        public string NumeroComprobante => $"B-{PuntoVenta:0000}-{NroFactura:00000000}";
    }
}
