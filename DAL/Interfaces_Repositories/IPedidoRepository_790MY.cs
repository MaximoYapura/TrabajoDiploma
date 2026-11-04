using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    // Acceso a Pedidos e ItemsPedido.
    public interface IPedidoRepository_790MY
    {
        // Graba el pedido solo si hay stock, todo en una transacción. Bloquea los productos
        // (así dos pedidos a la vez no usan las mismas porciones), controla las porciones
        // disponibles, toma el precio actual de cada producto y lo inserta si la mesa sigue
        // abierta. No descuenta stock: eso lo hace cocina.
        ResultadoRegistroPedido_790MY RegistrarSiHayStock(Pedido_790MY pedido);

        Pedido_790MY GetById(int nroPedido);

        // Todos los filtros son opcionales. Ordena del pedido más viejo al más nuevo.
        List<Pedido_790MY> Buscar(IEnumerable<EstadoPedido_790MY> estados, int? camareroDni, int? reservaId);

        // Pasa el pedido de Pendiente a EnPreparacion. False si no estaba Pendiente.
        bool IniciarPreparacion(int nroPedido);

        // Marca el pedido Listo y descuenta el stock en la misma transacción. Si el pedido
        // ya se había procesado no hace nada; si no alcanza el stock revierte todo.
        // En porcionesDescontadas devuelve cuántas porciones se descontaron.
        ResultadoDespacho_790MY MarcarListoYDescontarStock(int nroPedido, DateTime momentoFin, out int porcionesDescontadas);

        // Pedidos de la reserva que todavía no salieron de cocina.
        int ContarNoDespachados(int reservaId);
    }
}
