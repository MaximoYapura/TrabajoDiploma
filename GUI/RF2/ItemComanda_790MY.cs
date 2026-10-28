using BE_08YS;

namespace GUI_08YS.RF2
{
    /// <summary>Renglón de la comanda todavía no enviada (FormRegistrarPedido_790MY).</summary>
    internal sealed class ItemComanda_790MY
    {
        public Producto_790MY Producto { get; set; }
        public int Cantidad { get; set; }
        public string Observaciones { get; set; }
        public decimal Subtotal => Producto.Precio * Cantidad;
    }
}
