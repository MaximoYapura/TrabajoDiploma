using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IPedidoRepository_790MY
    {
        /// <summary>
        /// CUN-006. En una unica transaccion SERIALIZABLE:
        ///  1) bloquea (UPDLOCK/HOLDLOCK) las filas de Productos involucradas, en orden de ID;
        ///  2) verifica producto vigente y StockPorciones - comprometidas >= cantidad pedida;
        ///  3) toma el precio vigente como PrecioUnitario de cada item;
        ///  4) verifica que la reserva siga Asistida;
        ///  5) inserta Pedido (Pendiente, StockDescontado = 0) e items.
        /// NO descuenta inventario. Asigna NroPedido al pedido si Estado = Ok.
        /// Los bloqueos serializan dos registros concurrentes sobre los mismos
        /// productos, evitando que ambos "vean" las mismas porciones libres.
        /// </summary>
        ResultadoRegistroPedido_790MY RegistrarSiHayStock(Pedido_790MY pedido);

        Pedido_790MY GetById(int nroPedido);

        /// <summary>Filtros opcionales; ordena por Fecha, Hora, NroPedido (orden de cocina).</summary>
        List<Pedido_790MY> Buscar(IEnumerable<EstadoPedido_790MY> estados, int? camareroDni, int? reservaId);

        /// <summary>Pendiente -> EnPreparacion. False si no estaba Pendiente.</summary>
        bool IniciarPreparacion(int nroPedido);

        /// <summary>
        /// CUN-008. En una unica transaccion: marca el pedido Listo con FechaFin/HoraFin
        /// y StockDescontado = 1 (solo si StockDescontado = 0 y estado Pendiente/EnPreparacion)
        /// y descuenta las porciones de cada producto. Cualquier falla revierte ambos cambios.
        /// porcionesDescontadas devuelve la suma descontada (0 si no hubo descuento).
        /// </summary>
        ResultadoDespacho_790MY MarcarListoYDescontarStock(int nroPedido, DateTime momentoFin, out int porcionesDescontadas);

        /// <summary>Cantidad de pedidos de la reserva aun no despachados (Pendiente/EnPreparacion).</summary>
        int ContarNoDespachados(int reservaId);
    }
}
