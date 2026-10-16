using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    public enum ResultadoRegistroCobro_790MY
    {
        Ok,
        /// <summary>La factura no existe o ya no está Abierta (cobrada o anulada por otro puesto).</summary>
        FacturaNoAbierta,
        /// <summary>La reserva de la factura ya no está Asistida.</summary>
        ReservaNoAsistida,
        /// <summary>Quedan pedidos Pendiente/EnPreparacion: se comprometería stock para siempre.</summary>
        PedidosSinDespachar
    }

    public interface IPagoRepository_790MY
    {
        /// <summary>
        /// CUN-007. En una única transacción SERIALIZABLE:
        ///  1) bloquea la factura y verifica que siga Abierta;
        ///  2) verifica que la reserva siga Asistida y sin pedidos sin despachar;
        ///  3) inserta el Pago;
        ///  4) Factura -> Pagada, Reserva -> Finalizada, Mesa -> Libre.
        /// Cualquier falla revierte todo. Asigna pago.IdPago si Ok.
        /// mesaNumero / reservaId devuelven los datos liberados (para la bitácora).
        /// </summary>
        ResultadoRegistroCobro_790MY RegistrarCobro(Pago_790MY pago, out int reservaId, out int mesaNumero);

        Pago_790MY GetPorFactura(int nroFactura);

        /// <summary>Cobros confirmados en el día calendario indicado, ordenados por hora.</summary>
        List<CobroArqueo_790MY> ObtenerCobrosDelDia(DateTime fecha);
    }
}
