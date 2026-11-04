namespace BE_08YS
{
    // Renglón de la factura. La descripción y el precio se copian al emitir.
    public class ItemFactura_790MY
    {
        public int NroFactura { get; set; }
        public int NroItem { get; set; }
        public int ProductoID { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}
