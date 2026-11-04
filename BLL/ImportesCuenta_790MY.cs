namespace BLL_08YS
{
    // Importes de una cuenta: subtotal, descuento, IVA y total. Los calcula FacturaBLL_790MY.CalcularImportes.
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
