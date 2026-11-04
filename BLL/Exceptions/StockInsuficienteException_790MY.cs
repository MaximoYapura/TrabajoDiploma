using System;

namespace BLL_08YS.Exceptions
{
    // Se pidieron más porciones de las que hay. Hereda de ArgumentException para que las
    // pantallas la muestren como cualquier regla de negocio, y trae el producto y las
    // porciones que quedan.
    public class StockInsuficienteException_790MY : ArgumentException
    {
        public int ProductoID { get; }
        public string ProductoNombre { get; }
        public int PorcionesDisponibles { get; }

        public StockInsuficienteException_790MY(int productoId, string productoNombre, int porcionesDisponibles, string mensaje)
            : base(mensaje)
        {
            ProductoID = productoId;
            ProductoNombre = productoNombre;
            PorcionesDisponibles = porcionesDisponibles;
        }
    }
}
