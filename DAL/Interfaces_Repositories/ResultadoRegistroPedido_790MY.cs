namespace DAL_08YS.Interfaces_Repositories
{
    /// <summary>Resultado del alta transaccional de un pedido (CUN-006).</summary>
    public class ResultadoRegistroPedido_790MY
    {
        public EstadoRegistroPedido_790MY Estado { get; set; }
        public int ProductoID { get; set; }
        public string ProductoNombre { get; set; }
        public int PorcionesDisponibles { get; set; }
    }
}
