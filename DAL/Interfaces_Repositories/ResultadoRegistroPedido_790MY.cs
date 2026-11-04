namespace DAL_08YS.Interfaces_Repositories
{
    // Resultado de grabar un pedido. Si faltó stock, dice de qué producto y cuánto queda.
    public class ResultadoRegistroPedido_790MY
    {
        public EstadoRegistroPedido_790MY Estado { get; set; }
        public int ProductoID { get; set; }
        public string ProductoNombre { get; set; }
        public int PorcionesDisponibles { get; set; }
    }
}
