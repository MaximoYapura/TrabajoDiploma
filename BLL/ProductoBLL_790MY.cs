using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;

namespace BLL_08YS
{
    // Reglas del maestro de productos (la carta). La baja es lógica: un producto con
    // pedidos viejos no se puede borrar.
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

        // Todos los productos (maestro).
        public List<Producto_790MY> GetAll()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionProductos);
            return _productoRepository.GetAll();
        }

        // Alta de producto.
        public void RegistrarProducto(Producto_790MY producto)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionProductos);

            ValidarDatos(producto, 0);

            _productoRepository.Add(producto);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.ProductoRegistrado, $"Prod {producto.ProductoID} {producto.Nombre}");
        }

        // Modificación. stockLeido es el stock que vio la pantalla al abrir el producto.
        public void ModificarProducto(Producto_790MY producto, int stockLeido)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionProductos);

            Producto_790MY actual = ObtenerExistente(producto?.ProductoID ?? 0);
            ValidarDatos(producto, producto.ProductoID);

            // El stock no puede quedar por debajo de lo que ya pidieron pedidos que siguen en
            // cocina, porque después no se podría descontar.
            if (producto.ControlaStock && producto.StockPorciones < actual.PorcionesComprometidas)
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_stock_bajo_comprometido", actual.PorcionesComprometidas);

            // Si cocina descontó stock mientras se editaba, no se pisa.
            if (!_productoRepository.Update(producto, stockLeido))
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_stock_cambio", producto.Nombre,
                    ObtenerExistente(producto.ProductoID).StockPorciones);

            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.ProductoModificado, $"Prod {producto.ProductoID} {producto.Nombre}");
        }

        // Baja lógica: deja de aparecer en la carta de Registrar pedido.
        public void DarDeBaja(int productoId)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionProductos);

            Producto_790MY producto = ObtenerExistente(productoId);
            if (!producto.Activo)
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_ya_inactivo", producto.Nombre);

            // Solo cambia Activo, así no pisa el stock que pudo descontar cocina.
            if (!_productoRepository.DarDeBaja(productoId))
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_ya_inactivo", producto.Nombre);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.ProductoDadoDeBaja, $"Prod {producto.ProductoID} {producto.Nombre}");
        }

        // ─── Consultas para otras BLL ────────────────────────────────────────
        // No validan permiso: las usan PedidoBLL (el camarero) y ProductoCBLL (la auditoría),
        // que no tienen GestionProductos y ya validaron el suyo.

        // Productos activos con sus porciones disponibles (carta de Registrar pedido).
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

        // Nombre, categoría, precio y stock válidos, y nombre sin repetir.
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
