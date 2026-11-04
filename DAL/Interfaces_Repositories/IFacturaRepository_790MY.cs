using BE_08YS;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    // Cómo terminó el intento de emitir una factura.
    public enum ResultadoGenerarFactura_790MY
    {
        Ok,
        // La mesa ya no está abierta.
        ReservaNoAsistida,
        // La reserva ya tiene una factura Pagada: no se vuelve a facturar.
        YaPagada
    }

    // Acceso a Facturas e ItemsFactura.
    public interface IFacturaRepository_790MY
    {
        // Lo que se le cobra a la mesa: los ítems de los pedidos Listo, agrupados por
        // producto y por el precio que quedó guardado en el pedido.
        List<ItemFactura_790MY> ObtenerConsumosFacturables(int reservaId);

        // Emite la factura en una transacción: controla que la mesa siga abierta y no esté
        // pagada, anula la factura Abierta anterior si había una e inserta la nueva con su
        // detalle. En nroFacturaAnulada devuelve la que se anuló.
        ResultadoGenerarFactura_790MY GenerarFactura(Factura_790MY factura, out int? nroFacturaAnulada);

        // Factura con su detalle y datos de mesa / cliente / camarero.
        Factura_790MY GetById(int nroFactura);

        // Factura Abierta de la reserva, o null.
        Factura_790MY GetAbiertaPorReserva(int reservaId);

        // Para el reporte predictivo: por cada factura pagada desde esa fecha, los
        // productos distintos que tuvo.
        List<CanastaFactura_790MY> ObtenerCanastasPagadas(System.DateTime desde);
    }
}
