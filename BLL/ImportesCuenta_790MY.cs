namespace BLL_08YS
{
    /// <summary>Importes calculados de una cuenta (CUN-007, pasos 2-3).</summary>
    public class ImportesCuenta_790MY
    {
        public decimal Subtotal { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public decimal MontoDescuento { get; set; }
        public decimal SubtotalNeto { get; set; }
        public decimal IVA { get; set; }
        public decimal MontoTotal { get; set; }
    }
}
