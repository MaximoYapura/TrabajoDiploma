using BE_08YS;

namespace BLL_08YS
{
    /// <summary>
    /// Comparación de una versión histórica contra la vigente del mismo producto
    /// (inspector de diferencias de FormBitacoraCambios_790MY). Los deltas se expresan
    /// como "lo que cambia al restaurar": histórico - vigente.
    /// </summary>
    public class DiferenciaVersion_790MY
    {
        public Producto_C_790MY Seleccion { get; set; }

        /// <summary>Versión vigente del producto; null si el producto ya no tiene vigente.</summary>
        public Producto_C_790MY Vigente { get; set; }

        /// <summary>True si la selección ya es la versión vigente (no hay nada que restaurar).</summary>
        public bool EsVigente => Seleccion != null && Seleccion.Act;

        public decimal DeltaPrecio { get; set; }
        public int DeltaStock { get; set; }

        public bool CambiaPrecio { get; set; }
        public bool CambiaStock { get; set; }
        public bool CambiaEstado { get; set; }
        public bool CambiaCategoria { get; set; }
        public bool CambiaNombre { get; set; }
        public bool CambiaControlaStock { get; set; }

        public int CantidadCambios =>
            (CambiaPrecio ? 1 : 0) + (CambiaStock ? 1 : 0) + (CambiaEstado ? 1 : 0) +
            (CambiaCategoria ? 1 : 0) + (CambiaNombre ? 1 : 0) + (CambiaControlaStock ? 1 : 0);
    }
}
