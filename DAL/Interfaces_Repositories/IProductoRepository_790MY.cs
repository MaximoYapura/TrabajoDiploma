using BE_08YS;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IProductoRepository_790MY
    {
        /// <summary>
        /// Productos activos con PorcionesComprometidas calculadas a partir de los
        /// pedidos Pendiente/EnPreparacion todavia no descontados.
        /// </summary>
        List<Producto_790MY> GetActivos();

        Producto_790MY GetById(int productoId);

        /// <summary>Todos los productos (activos e inactivos), con comprometidas calculadas.</summary>
        List<Producto_790MY> GetAll();

        bool ExisteNombre(string nombre, int exceptoProductoId);

        /// <summary>Asigna producto.ProductoID con el valor generado.</summary>
        void Add(Producto_790MY producto);

        /// <summary>
        /// Actualiza el producto solo si su stock físico sigue siendo <paramref name="stockLeido"/>
        /// (el que vio la pantalla). False si cocina lo cambió en el medio: no se pisa.
        /// </summary>
        bool Update(Producto_790MY producto, int stockLeido);

        /// <summary>Baja lógica (Activo = 0) sin tocar el stock. False si ya estaba inactivo.</summary>
        bool DarDeBaja(int productoId);
    }
}
