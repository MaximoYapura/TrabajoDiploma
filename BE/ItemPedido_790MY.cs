namespace BE_08YS
{
    // Renglón de un pedido: producto, cantidad y observaciones para cocina.
    public class ItemPedido_790MY
    {
        public int NroPedido { get; set; }
        public int NroItem { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public string Observaciones { get; set; }

        // Precio del producto en el momento del pedido. No cambia si después cambia la carta.
        public decimal PrecioUnitario { get; set; }

        // Se lee con JOIN a Productos, no se guarda.
        public string ProductoNombre { get; set; }

        public decimal Subtotal => PrecioUnitario * Cantidad;

        public ItemPedido_790MY() { }

        public ItemPedido_790MY(int productoId, int cantidad, string observaciones = null)
        {
            ProductoID = productoId;
            Cantidad = cantidad;
            Observaciones = observaciones;
        }
    }
}
