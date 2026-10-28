namespace BE_08YS
{
    /// <summary>Datos que el camarero ingresa para cobrar (entrada de PagoBLL.RegistrarCobro).</summary>
    public class SolicitudCobro_790MY
    {
        public int NroFactura { get; set; }
        public MetodoPago_790MY MetodoPago { get; set; }
        public decimal MontoPropina { get; set; }
        public decimal? MontoRecibido { get; set; }
        public string NroTarjeta { get; set; }
        public int CantidadCuotas { get; set; } = 1;
    }
}
