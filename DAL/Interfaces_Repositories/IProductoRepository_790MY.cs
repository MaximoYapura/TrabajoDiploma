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
    }
}
