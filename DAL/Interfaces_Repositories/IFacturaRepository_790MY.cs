using BE_08YS;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    public enum ResultadoGenerarFactura_790MY
    {
        Ok,
        /// <summary>La reserva dejó de estar Asistida antes del commit.</summary>
        ReservaNoAsistida,
        /// <summary>La reserva ya tiene una factura Pagada: no se vuelve a facturar.</summary>
        YaPagada
    }

    public interface IFacturaRepository_790MY
    {
        /// <summary>
        /// Consumos facturables de la reserva: ítems de pedidos Listo (entregados),
        /// agrupados por producto y precio unitario congelado en el pedido.
        /// </summary>
        List<ItemFactura_790MY> ObtenerConsumosFacturables(int reservaId);

        /// <summary>
        /// CUN-006. En una transacción: verifica que la reserva siga Asistida y no tenga
        /// factura Pagada, anula la factura Abierta anterior (si la hay) e inserta la
        /// nueva factura (Abierta) con su detalle. Asigna factura.NroFactura si Ok.
        /// nroFacturaAnulada devuelve la factura reemplazada (o null).
        /// </summary>
        ResultadoGenerarFactura_790MY GenerarFactura(Factura_790MY factura, out int? nroFacturaAnulada);

        /// <summary>Factura con su detalle y datos de mesa / cliente / camarero.</summary>
        Factura_790MY GetById(int nroFactura);

        /// <summary>Factura Abierta vigente de la reserva (null si no hay).</summary>
        Factura_790MY GetAbiertaPorReserva(int reservaId);
    }
}
