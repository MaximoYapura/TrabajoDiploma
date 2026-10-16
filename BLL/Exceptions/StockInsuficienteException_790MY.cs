using System;

namespace BLL_08YS.Exceptions
{
    /// <summary>
    /// CUN-005 escenario alternativo 4.1: la cantidad pedida supera las porciones
    /// disponibles. Hereda de ArgumentException para que los formularios que ya
    /// capturan reglas de negocio la muestren sin cambios; expone los datos para
    /// que la GUI pueda resaltar el item y sugerir un producto alternativo.
    /// </summary>
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
