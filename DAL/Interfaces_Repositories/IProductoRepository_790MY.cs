using BE_08YS;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    // Acceso a la tabla Productos. Las consultas traen calculadas las porciones comprometidas.
    public interface IProductoRepository_790MY
    {
        // Productos activos (la carta de Registrar pedido).
        List<Producto_790MY> GetActivos();

        Producto_790MY GetById(int productoId);

        // Todos los productos, activos y dados de baja (para el maestro).
        List<Producto_790MY> GetAll();

        // True si otro producto ya usa ese nombre.
        bool ExisteNombre(string nombre, int exceptoProductoId);

        // Inserta y completa producto.ProductoID.
        void Add(Producto_790MY producto);

        // Actualiza solo si el stock sigue siendo el que leyó la pantalla (stockLeido).
        // Devuelve false si cocina lo cambió mientras se editaba.
        bool Update(Producto_790MY producto, int stockLeido);

        // Baja lógica sin tocar el stock. False si ya estaba dado de baja.
        bool DarDeBaja(int productoId);
    }
}
