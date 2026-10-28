namespace BE_08YS
{
    public class ItemPedido_790MY
    {
        public int NroPedido { get; set; }
        public int NroItem { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public string Observaciones { get; set; }

        /// <summary>Precio vigente al momento de registrar el pedido (snapshot).</summary>
        public decimal PrecioUnitario { get; set; }

        // Solo lectura (JOIN con Productos).
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
