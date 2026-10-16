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
    /// <summary>
    /// CUN-005 Registrar Pedido y CUN-008 Despachar Pedido.
    ///
    /// Stock en dos momentos:
    ///  - CUN-005 valida (sin descontar) contra las porciones DISPONIBLES =
    ///    StockPorciones - porciones comprometidas por pedidos Pendiente/EnPreparacion.
    ///    Contar lo comprometido evita la sobreventa entre pedidos que todavía no se
    ///    despacharon, sin adelantar el descuento definitivo.
    ///  - CUN-008 descuenta efectivamente al marcar Listo, en la misma transacción que
    ///    el cambio de estado, con una marca de idempotencia (StockDescontado).
    /// </summary>
    public class PedidoBLL_790MY
    {
        private const int LargoMaximoObservaciones = 200;

        private readonly IPedidoRepository_790MY _pedidoRepository;
        private readonly IProductoRepository_790MY _productoRepository;
        private readonly IReservaRepository_790MY _reservaRepository;
        private readonly IEmpleadoRepository_790MY _empleadoRepository;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public PedidoBLL_790MY(IPedidoRepository_790MY pedidoRepository, IProductoRepository_790MY productoRepository,
                               IReservaRepository_790MY reservaRepository, IEmpleadoRepository_790MY empleadoRepository,
                               BitacoraBLL_08YS bitacoraBll)
        {
            _pedidoRepository = pedidoRepository ?? throw new ArgumentNullException(nameof(pedidoRepository));
            _productoRepository = productoRepository ?? throw new ArgumentNullException(nameof(productoRepository));
            _reservaRepository = reservaRepository ?? throw new ArgumentNullException(nameof(reservaRepository));
            _empleadoRepository = empleadoRepository ?? throw new ArgumentNullException(nameof(empleadoRepository));
            _bitacoraBll = bitacoraBll ?? throw new ArgumentNullException(nameof(bitacoraBll));
        }

        // ═══════════════════════════════════════════════════════════════════
        //  CUN-005 — Camarero
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Mesas abiertas (reservas Asistidas) a cargo del camarero logueado.
        /// Lista vacía => la GUI muestra msg_rf2_sin_mesas_asignadas (escenario alt. 1.1).
        /// </summary>
        public List<Reserva_790MY> ObtenerMesasAsignadas()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarPedido);
            Empleado_790MY camarero = ObtenerCamareroDeSesion();
            return _reservaRepository.GetAsistidasPorCamarero(camarero.DNI);
        }

        /// <summary>Catálogo con porciones disponibles (stock - comprometido).</summary>
        public List<Producto_790MY> ObtenerCatalogo()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarPedido);
            return _productoRepository.GetActivos();
        }

        public Pedido_790MY RegistrarPedido(int reservaId, IList<ItemPedido_790MY> items)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarPedido);
            Empleado_790MY camarero = ObtenerCamareroDeSesion();

            // Paso 6: cantidades > 0 y datos válidos. El precio vigente lo toma el
            // repositorio desde Productos dentro de la transacción (no la GUI).
            if (items == null || items.Count == 0)
                throw MensajesBLL_790MY.Regla("msg_rf2_pedido_sin_items");

            foreach (var item in items)
            {
                if (item == null || item.Cantidad <= 0)
                    throw MensajesBLL_790MY.Regla("msg_rf2_pedido_cantidad_invalida");
                if (item.Observaciones != null && item.Observaciones.Trim().Length > LargoMaximoObservaciones)
                    throw MensajesBLL_790MY.Regla("msg_rf2_pedido_observaciones_largas", LargoMaximoObservaciones);
            }

            // Precondición: mesa ocupada con reserva Asistida asignada a este camarero.
            Reserva_790MY reserva = _reservaRepository.GetById(reservaId)
                ?? throw MensajesBLL_790MY.Regla("msg_rf2_reserva_inexistente", reservaId);

            if (reserva.Estado != EstadoReserva_790MY.Asistida)
                throw MensajesBLL_790MY.Regla("msg_rf2_reserva_no_asistida", reservaId);

            if (reserva.CamareroDNI != camarero.DNI)
                throw MensajesBLL_790MY.Regla("msg_rf2_mesa_no_asignada", reserva.MesaNumero);

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
                    // Escenario alternativo 4.1: no se genera ningún pedido (rollback completo).
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

        /// <summary>
        /// Pedidos Listos de las mesas del camarero logueado: es el mecanismo de
        /// "notificación mediante el sistema" del CUN-008 paso 8 (la GUI lo consulta
        /// periódicamente; no existe aún infraestructura de mensajería push).
        /// </summary>
        public List<Pedido_790MY> ObtenerPedidosListosDelCamarero()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarPedido);
            Empleado_790MY camarero = ObtenerCamareroDeSesion();

            var mesasAbiertas = new HashSet<int>(_reservaRepository.GetAsistidasPorCamarero(camarero.DNI).Select(r => r.ReservaID));
            return _pedidoRepository
                .Buscar(new[] { EstadoPedido_790MY.Listo }, camarero.DNI, null)
                .Where(p => mesasAbiertas.Contains(p.ReservaID))
                .ToList();
        }

        /// <summary>
        /// Todos los pedidos (cualquier estado) de las mesas abiertas del camarero
        /// logueado. FormRegistrarPedido_790MY los usa para las etiquetas de estado
        /// de cada mesa ("# 229 · Listo").
        /// </summary>
        public List<Pedido_790MY> ObtenerPedidosDeMisMesas()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarPedido);
            Empleado_790MY camarero = ObtenerCamareroDeSesion();

            var mesasAbiertas = new HashSet<int>(_reservaRepository.GetAsistidasPorCamarero(camarero.DNI).Select(r => r.ReservaID));
            return _pedidoRepository
                .Buscar(null, camarero.DNI, null)
                .Where(p => mesasAbiertas.Contains(p.ReservaID))
                .ToList();
        }

        // ═══════════════════════════════════════════════════════════════════
        //  CUN-008 — Chef
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>Comandas Pendiente / EnPreparacion por hora de solicitud (KDS).</summary>
        public List<Pedido_790MY> ObtenerComandasCocina()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.DespacharPedido);
            return _pedidoRepository.Buscar(Pedido_790MY.EstadosConStockComprometido, null, null);
        }

        /// <summary>
        /// Últimos pedidos finalizados hoy, para la columna "Listas para retirar" del KDS.
        /// Ordenados del más reciente al más antiguo.
        /// </summary>
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

        /// <summary>
        /// Nombre de los camareros activos por DNI, para mostrar en cada comanda quién
        /// la pidió. Se consulta el repositorio (no EmpleadoBLL) para no exigir al Chef
        /// el permiso de ABM de empleados.
        /// </summary>
        public Dictionary<int, Empleado_790MY> ObtenerCamarerosPorDni()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.DespacharPedido);
            return _empleadoRepository
                .GetActivosPorPuesto(PuestoEmpleado_790MY.Camarero)
                .ToDictionary(e => e.DNI);
        }

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

        /// <summary>
        /// Marca el pedido como Listo y descuenta el stock en la misma transacción.
        /// Reprocesar el mismo pedido no vuelve a descontar (StockDescontado).
        /// Devuelve el pedido actualizado (incluye MesaNumero y CamareroDNI para notificar).
        /// </summary>
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
                    // No debería ocurrir (CUN-005 reserva las porciones), salvo un ajuste
                    // manual de inventario. Se revirtió todo: el pedido sigue sin despachar.
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

        // ─── Helpers ─────────────────────────────────────────────────────────

        /// <summary>
        /// Resuelve el Empleado camarero vinculado al usuario logueado (Users.DNI ->
        /// Empleados.UsuarioDNI). La autorización ya la resolvió el Composite; esto
        /// solo identifica a qué camarero corresponden las mesas.
        /// </summary>
        private Empleado_790MY ObtenerCamareroDeSesion()
        {
            User_08YS usuario = SessionManager_08YS.Instance.Current
                ?? throw new UnauthorizedAccessException(MensajesBLL_790MY.Texto("msg_rf2_sin_sesion"));

            Empleado_790MY empleado = _empleadoRepository.GetByUsuarioDni(usuario.DNI);
            if (empleado == null || !empleado.Activo || empleado.Puesto != PuestoEmpleado_790MY.Camarero)
                throw MensajesBLL_790MY.Regla("msg_rf2_usuario_no_camarero");

            return empleado;
        }
    }
}
