using BE_08YS;
using BLL_08YS.Exceptions;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL_08YS
{
    // Reglas de Registrar pedido (CUN-006, el camarero) y de Cocina (CUN-008, el chef).
    // El stock se maneja en dos momentos:
    // - al pedir solo se controla que alcancen las porciones disponibles (el stock menos
    //   lo que ya pidieron otros pedidos que siguen en cocina), sin descontar nada;
    // - al marcar Listo se descuenta de verdad, en la misma transacción que el cambio de
    //   estado. StockDescontado evita descontar dos veces el mismo pedido.
    public class PedidoBLL_790MY
    {
        private const int LargoMaximoObservaciones = 200;

        // Máximo de unidades por pedido de un producto que no controla stock (los que lo
        // controlan ya están limitados por sus porciones).
        public const int MaximoPorItemSinControl = 99;

        private readonly IPedidoRepository_790MY _pedidoRepository;
        private readonly ProductoBLL_790MY _productoBll;
        private readonly ReservaBLL_790MY _reservaBll;
        private readonly EmpleadoBLL_790MY _empleadoBll;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public PedidoBLL_790MY(IPedidoRepository_790MY pedidoRepository, ProductoBLL_790MY productoBll,
                               ReservaBLL_790MY reservaBll, EmpleadoBLL_790MY empleadoBll,
                               BitacoraBLL_08YS bitacoraBll)
        {
            _pedidoRepository = pedidoRepository ?? throw new ArgumentNullException(nameof(pedidoRepository));
            _productoBll = productoBll ?? throw new ArgumentNullException(nameof(productoBll));
            _reservaBll = reservaBll ?? throw new ArgumentNullException(nameof(reservaBll));
            _empleadoBll = empleadoBll ?? throw new ArgumentNullException(nameof(empleadoBll));
            _bitacoraBll = bitacoraBll ?? throw new ArgumentNullException(nameof(bitacoraBll));
        }

        // ═══════════════════════════════════════════════════════════════════
        //  Registrar pedido (camarero)
        // ═══════════════════════════════════════════════════════════════════

        // Mesas abiertas a cargo del camarero logueado. Si está vacía, la pantalla lo avisa.
        public List<Reserva_790MY> ObtenerMesasAsignadas()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarPedido);
            Empleado_790MY camarero = ObtenerCamareroDeSesion();
            return _reservaBll.ObtenerAsistidasPorCamarero(camarero.DNI);
        }

        // Productos activos con sus porciones disponibles.
        public List<Producto_790MY> ObtenerCatalogo()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarPedido);
            return _productoBll.ObtenerActivos();
        }

        // Graba el pedido de una mesa si hay stock. El precio lo pone el repositorio.
        public Pedido_790MY RegistrarPedido(int reservaId, IList<ItemPedido_790MY> items)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarPedido);
            Empleado_790MY camarero = ObtenerCamareroDeSesion();

            // Al menos un ítem, cantidades mayores a cero y observaciones cortas.
            if (items == null || items.Count == 0)
                throw MensajesBLL_790MY.Regla("msg_rf2_pedido_sin_items");

            foreach (var item in items)
            {
                if (item == null || item.Cantidad <= 0)
                    throw MensajesBLL_790MY.Regla("msg_rf2_pedido_cantidad_invalida");
                if (item.Observaciones != null && item.Observaciones.Trim().Length > LargoMaximoObservaciones)
                    throw MensajesBLL_790MY.Regla("msg_rf2_pedido_observaciones_largas", LargoMaximoObservaciones);
            }

            // La mesa tiene que estar abierta y a cargo de este camarero.
            Reserva_790MY reserva = _reservaBll.ObtenerPorId(reservaId)
                ?? throw MensajesBLL_790MY.Regla("msg_rf2_reserva_inexistente", reservaId);

            if (reserva.Estado != EstadoReserva_790MY.Asistida)
                throw MensajesBLL_790MY.Regla("msg_rf2_reserva_no_asistida", reservaId);

            if (reserva.CamareroDNI != camarero.DNI)
                throw MensajesBLL_790MY.Regla("msg_rf2_mesa_no_asignada", reserva.MesaNumero);

            foreach (var renglones in items.GroupBy(i => i.ProductoID))
            {
                Producto_790MY producto = _productoBll.ObtenerPorId(renglones.Key);
                if (producto != null && !producto.ControlaStock && renglones.Sum(i => i.Cantidad) > MaximoPorItemSinControl)
                    throw MensajesBLL_790MY.Regla("msg_rf2_pedido_cantidad_maxima", producto.Nombre, MaximoPorItemSinControl);
            }

            DateTime ahora = DateTime.Now;
            var pedido = new Pedido_790MY(reservaId, ahora.Date, new TimeSpan(ahora.Hour, ahora.Minute, ahora.Second))
            {
                Items = items.Select(i => new ItemPedido_790MY(i.ProductoID, i.Cantidad,
                            string.IsNullOrWhiteSpace(i.Observaciones) ? null : i.Observaciones.Trim()))
                             .ToList()
            };

            ResultadoRegistroPedido_790MY resultado = _pedidoRepository.RegistrarSiHayStock(pedido);

            switch (resultado.Estado)
            {
                case EstadoRegistroPedido_790MY.Ok:
                    break;

                case EstadoRegistroPedido_790MY.StockInsuficiente:
                    // Falta stock: no se grabó nada.
                    _bitacoraBll.RegistrarEventoPostCommit(Evento.PedidoRechazadoStock,
                        $"Reserva {reservaId} - Prod {resultado.ProductoID}");
                    throw new StockInsuficienteException_790MY(
                        resultado.ProductoID, resultado.ProductoNombre, resultado.PorcionesDisponibles,
                        MensajesBLL_790MY.Texto("msg_rf2_stock_insuficiente", resultado.ProductoNombre, resultado.PorcionesDisponibles));

                case EstadoRegistroPedido_790MY.ProductoNoVigente:
                    throw MensajesBLL_790MY.Regla("msg_rf2_producto_no_vigente",
                        resultado.ProductoNombre ?? resultado.ProductoID.ToString());

                case EstadoRegistroPedido_790MY.ReservaNoAsistida:
                    throw MensajesBLL_790MY.Regla("msg_rf2_reserva_no_asistida", reservaId);

                default:
                    throw new InvalidOperationException(resultado.Estado.ToString());
            }

            pedido.MesaNumero = reserva.MesaNumero;
            pedido.CamareroDNI = reserva.CamareroDNI;

            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.PedidoRegistrado,
                $"Pedido {pedido.NroPedido} - Mesa {reserva.MesaNumero}");

            return pedido;
        }

        // Pedidos de las mesas abiertas del camarero, en cualquier estado. La pantalla los
        // usa para mostrar el estado de cada mesa y avisar cuando uno está Listo.
        public List<Pedido_790MY> ObtenerPedidosDeMisMesas()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarPedido);
            Empleado_790MY camarero = ObtenerCamareroDeSesion();

            var mesasAbiertas = new HashSet<int>(_reservaBll.ObtenerAsistidasPorCamarero(camarero.DNI).Select(r => r.ReservaID));
            return _pedidoRepository
                .Buscar(null, camarero.DNI, null)
                .Where(p => mesasAbiertas.Contains(p.ReservaID))
                .ToList();
        }

        // ═══════════════════════════════════════════════════════════════════
        //  Cocina (chef)
        // ═══════════════════════════════════════════════════════════════════

        // Pedidos pendientes y en preparación, del más viejo al más nuevo.
        public List<Pedido_790MY> ObtenerComandasCocina()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.DespacharPedido);
            return _pedidoRepository.Buscar(Pedido_790MY.EstadosConStockComprometido, null, null);
        }

        // Últimos pedidos listos de hoy, del más nuevo al más viejo (columna "Listas").
        public List<Pedido_790MY> ObtenerListosDelDia(int maximo = 10)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.DespacharPedido);
            return _pedidoRepository
                .Buscar(new[] { EstadoPedido_790MY.Listo }, null, null)
                .Where(p => (p.FechaFin ?? p.Fecha).Date == DateTime.Today)
                .OrderByDescending(p => p.HoraFin ?? p.Hora)
                .Take(maximo)
                .ToList();
        }

        // Camareros activos por DNI, para mostrar en cada comanda quién la pidió. Usa la
        // consulta internal de EmpleadoBLL para no pedirle al chef el permiso de empleados.
        public Dictionary<int, Empleado_790MY> ObtenerCamarerosPorDni()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.DespacharPedido);
            return _empleadoBll
                .ObtenerCamarerosActivos()
                .ToDictionary(e => e.DNI);
        }

        // Pasa el pedido de Pendiente a EnPreparacion.
        public void IniciarPreparacion(int nroPedido)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.DespacharPedido);

            if (!_pedidoRepository.IniciarPreparacion(nroPedido))
            {
                Pedido_790MY actual = _pedidoRepository.GetById(nroPedido);
                if (actual == null)
                    throw MensajesBLL_790MY.Regla("msg_rf2_pedido_inexistente", nroPedido);
                throw MensajesBLL_790MY.Regla("msg_rf2_pedido_no_iniciable", nroPedido,
                    MensajesBLL_790MY.Texto("estado_pedido_" + actual.Estado));
            }

            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.PedidoEnPreparacion, $"Pedido {nroPedido}");
        }

        // Marca el pedido Listo y descuenta el stock en la misma transacción. Si se marca dos
        // veces no descuenta de nuevo. Devuelve el pedido actualizado.
        public Pedido_790MY MarcarPedidoListo(int nroPedido)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.DespacharPedido);

            ResultadoDespacho_790MY resultado =
                _pedidoRepository.MarcarListoYDescontarStock(nroPedido, DateTime.Now, out int porciones);

            switch (resultado)
            {
                case ResultadoDespacho_790MY.Ok:
                    break;

                case ResultadoDespacho_790MY.NoEncontrado:
                    throw MensajesBLL_790MY.Regla("msg_rf2_pedido_inexistente", nroPedido);

                case ResultadoDespacho_790MY.YaProcesado:
                    throw MensajesBLL_790MY.Regla("msg_rf2_pedido_ya_despachado", nroPedido);

                case ResultadoDespacho_790MY.StockFisicoInsuficiente:
                    // No debería pasar porque las porciones se reservan al pedir, salvo que se
                    // haya cambiado el stock a mano. Se revirtió todo y el pedido sigue en cocina.
                    _bitacoraBll.RegistrarEventoPostCommit(Evento.StockDescuentoFallido, $"Pedido {nroPedido}");
                    throw MensajesBLL_790MY.Regla("msg_rf2_stock_fisico_insuficiente", nroPedido);

                default:
                    throw new InvalidOperationException(resultado.ToString());
            }

            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.PedidoListo, $"Pedido {nroPedido}");
            _bitacoraBll.RegistrarEventoPostCommit(Evento.StockDescontado, $"Pedido {nroPedido} - {porciones} porc.");

            return _pedidoRepository.GetById(nroPedido);
        }

        // ─── Consultas para otras BLL ────────────────────────────────────────

        // Pedidos de la reserva que siguen en cocina. No valida permiso porque lo usa
        // FacturaBLL_790MY, que ya validó el de caja.
        internal int ContarNoDespachados(int reservaId)
            => _pedidoRepository.ContarNoDespachados(reservaId);

        // ─── Helpers ─────────────────────────────────────────────────────────

        // Camarero vinculado al usuario logueado (Empleados.UsuarioDNI). El permiso ya se
        // validó; esto es para saber de qué camarero son las mesas.
        private Empleado_790MY ObtenerCamareroDeSesion()
        {
            Empleado_790MY empleado = _empleadoBll.ObtenerEmpleadoDeSesion();
            if (empleado == null || !empleado.Activo || empleado.Puesto != PuestoEmpleado_790MY.Camarero)
                throw MensajesBLL_790MY.Regla("msg_rf2_usuario_no_camarero");

            return empleado;
        }
    }
}
