using System;
using System.Collections.Generic;

namespace BE_08YS
{
    /// <summary>
    /// Abierta  -> generada (CUN-007) con importes congelados, pendiente de cobro.
    /// Pagada   -> cobrada por el total (CUN-007); la reserva queda Finalizada.
    /// Anulada  -> reemplazada por una factura nueva (p. ej. se agregaron consumos
    ///             o se cambió el descuento antes del cobro). Nunca se borra.
    /// Se persiste como texto (nvarchar(20)).
    /// </summary>
    public enum EstadoFactura_790MY
    {
        Abierta,
        Pagada,
        Anulada
    }

    /// <summary>Forma en que el camarero expresa el descuento comercial (CUN-007, paso 3).</summary>
    public enum TipoDescuento_790MY
    {
        Porcentaje,
        MontoFijo
    }

    public class Factura_790MY
    {
        /// <summary>Alícuota de IVA aplicada sobre el subtotal con descuento.</summary>
        public const decimal AlicuotaIVA = 0.21m;

        /// <summary>Punto de venta fijo del comprobante simulado (Factura B).</summary>
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

        /// <summary>Detalle congelado al emitir: precios y descripciones no cambian aunque cambie la carta.</summary>
        public List<ItemFactura_790MY> Items { get; set; } = new List<ItemFactura_790MY>();

        // Solo lectura (JOIN con Reservas / Clientes / Empleados): datos del comprobante.
        public int MesaNumero { get; set; }
        public int CantidadComensales { get; set; }
        public string ClienteNombre { get; set; }
        public string CamareroNombre { get; set; }

        /// <summary>"B-0001-00000234"</summary>
        public string NumeroComprobante => $"B-{PuntoVenta:0000}-{NroFactura:00000000}";
    }
}
