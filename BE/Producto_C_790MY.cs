using System;

namespace BE_08YS
{
    /// <summary>
    /// Versión de un producto en la tabla espejo Productos_C (bitácora de cambios).
    /// La escriben los triggers de Productos: cada alta o modificación genera una fila.
    /// - Activo: estado de negocio del producto (baja lógica) al momento de la versión.
    /// - Act: auditoría; true solo para la versión vigente de cada ProductoID.
    /// </summary>
    public class Producto_C_790MY
    {
        public int ID { get; set; }

        public int ProductoID { get; set; }

        public string Nombre { get; set; }

        public CategoriaProducto_790MY Categoria { get; set; } = CategoriaProducto_790MY.PlatoPrincipal;

        public decimal Precio { get; set; }

        public int StockPorciones { get; set; }

        public bool ControlaStock { get; set; } = true;

        /// <summary>Estado de negocio: false = baja lógica.</summary>
        public bool Activo { get; set; }

        public DateTime Fecha { get; set; }

        /// <summary>Auditoría: true si es la versión vigente del producto.</summary>
        public bool Act { get; set; }

        public override string ToString() => $"{ID} - {Nombre}";
    }
}
