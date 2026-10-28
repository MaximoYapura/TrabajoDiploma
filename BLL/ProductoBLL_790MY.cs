using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;

namespace BLL_08YS
{
    /// <summary>
    /// ABM de la carta (FormProductos_790MY). Mismo patrón que el resto de las BLL:
    /// patente -> validaciones traducidas -> persistencia -> DV -> bitácora post-commit.
    /// La baja es lógica (Activo = 0): un producto con pedidos históricos no puede borrarse.
    /// </summary>
    public class ProductoBLL_790MY
    {
        private const int LargoMaximoNombre = 100;
        private const decimal PrecioMaximo = 99999999.99m;   // decimal(10,2)

        private readonly IProductoRepository_790MY _productoRepository;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public ProductoBLL_790MY(IProductoRepository_790MY productoRepository, BitacoraBLL_08YS bitacoraBll)
        {
            _productoRepository = productoRepository ?? throw new ArgumentNullException(nameof(productoRepository));
            _bitacoraBll = bitacoraBll ?? throw new ArgumentNullException(nameof(bitacoraBll));
        }

        public List<Producto_790MY> GetAll()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionProductos);
            return _productoRepository.GetAll();
        }

        public void RegistrarProducto(Producto_790MY producto)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionProductos);

            ValidarDatos(producto, 0);

            _productoRepository.Add(producto);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.ProductoRegistrado, $"Prod {producto.ProductoID} {producto.Nombre}");
        }

        public void ModificarProducto(Producto_790MY producto)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionProductos);

            Producto_790MY actual = ObtenerExistente(producto?.ProductoID ?? 0);
            ValidarDatos(producto, producto.ProductoID);

            // No se puede bajar el stock físico por debajo de lo ya comprometido por
            // pedidos Pendiente/EnPreparacion: al despacharlos fallaría el descuento.
            if (producto.ControlaStock && producto.StockPorciones < actual.PorcionesComprometidas)
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_stock_bajo_comprometido", actual.PorcionesComprometidas);

            _productoRepository.Update(producto);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.ProductoModificado, $"Prod {producto.ProductoID} {producto.Nombre}");
        }

        /// <summary>Baja lógica: deja de aparecer en la carta de Registrar Pedido.</summary>
        public void DarDeBaja(int productoId)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionProductos);

            Producto_790MY producto = ObtenerExistente(productoId);
            if (!producto.Activo)
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_ya_inactivo", producto.Nombre);

            producto.Activo = false;
            _productoRepository.Update(producto);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.ProductoDadoDeBaja, $"Prod {producto.ProductoID} {producto.Nombre}");
        }

        // ─── Consultas para otras BLL ────────────────────────────────────────
        // Sin ValidatePermission: el Camarero (PedidoBLL, RegistrarPedido) y la auditoría
        // (ProductoCBLL, AuditarProductos) no tienen GestionProductos; cada llamador ya
        // validó su propia patente.

        /// <summary>Productos activos con sus porciones disponibles (carta de Registrar Pedido).</summary>
        internal List<Producto_790MY> ObtenerActivos()
            => _productoRepository.GetActivos();

        internal Producto_790MY ObtenerPorId(int productoId)
            => _productoRepository.GetById(productoId);

        internal bool ExisteNombre(string nombre, int exceptoId)
            => _productoRepository.ExisteNombre(nombre, exceptoId);

        // ─── Validaciones ────────────────────────────────────────────────────

        private Producto_790MY ObtenerExistente(int productoId)
            => _productoRepository.GetById(productoId)
               ?? throw MensajesBLL_790MY.Regla("msg_rf2_producto_inexistente", productoId);

        private void ValidarDatos(Producto_790MY producto, int exceptoId)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));

            producto.Nombre = producto.Nombre?.Trim();
            if (string.IsNullOrWhiteSpace(producto.Nombre) || producto.Nombre.Length > LargoMaximoNombre)
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_nombre_invalido", LargoMaximoNombre);

            if (!Enum.IsDefined(typeof(CategoriaProducto_790MY), producto.Categoria))
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_categoria_invalida");

            if (producto.Precio <= 0 || producto.Precio > PrecioMaximo)
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_precio_invalido");

            if (producto.StockPorciones < 0)
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_stock_invalido");

            if (_productoRepository.ExisteNombre(producto.Nombre, exceptoId))
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_duplicado", producto.Nombre);
        }
    }
}
