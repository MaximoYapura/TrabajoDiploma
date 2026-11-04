using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    // Cómo terminó el intento de registrar un cobro.
    public enum ResultadoRegistroCobro_790MY
    {
        Ok,
        // La factura no existe o ya no está Abierta (la cobró o anuló otro puesto).
        FacturaNoAbierta,
        // La mesa de la factura ya no está abierta.
        ReservaNoAsistida,
        // Quedan pedidos sin despachar en la mesa.
        PedidosSinDespachar,
        // Los consumos cambiaron desde que se emitió la factura; hay que volver a emitirla.
        FacturaDesactualizada,
        // La caja de ese día ya está cerrada.
        CajaCerrada
    }

    // Acceso a la tabla Pagos.
    public interface IPagoRepository_790MY
    {
        // Registra el cobro en una sola transacción. Primero vuelve a controlar la factura,
        // la caja del día, los consumos y los pedidos pendientes; si está todo bien,
        // inserta el pago y deja la factura Pagada, la reserva Finalizada y la mesa Libre.
        // Si algo falla no queda nada a medias. Devuelve la reserva y la mesa liberadas.
        ResultadoRegistroCobro_790MY RegistrarCobro(Pago_790MY pago, out int reservaId, out int mesaNumero);

        // Pago de una factura, o null.
        Pago_790MY GetPorFactura(int nroFactura);

        // Cobros de ese día, ordenados por hora (para el arqueo).
        List<CobroArqueo_790MY> ObtenerCobrosDelDia(DateTime fecha);
    }
}
