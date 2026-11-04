namespace BE_08YS
{
    // Lo que se carga en la caja para cobrar. Es la entrada de PagoBLL_790MY.RegistrarCobro.
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
