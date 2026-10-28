using System;
using System.Collections.Generic;

namespace BE_08YS
{
    /// <summary>
    /// "Canasta" de una cuenta cobrada para el análisis predictivo (reglas de asociación):
    /// qué productos distintos se pidieron juntos en la misma factura, sin importar cantidades.
    /// </summary>
    public class CanastaFactura_790MY
    {
        public int NroFactura { get; set; }
        public DateTime FechaEmision { get; set; }

        /// <summary>ProductoID -> descripción congelada en la factura.</summary>
        public Dictionary<int, string> Productos { get; } = new Dictionary<int, string>();
    }
}
