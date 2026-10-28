using BE_08YS;

namespace BLL_08YS
{
    /// <summary>Resultado de un cobro confirmado: datos para el comprobante definitivo.</summary>
    public class ResultadoCobroDTO_790MY
    {
        public Factura_790MY Factura { get; set; }
        public Pago_790MY Pago { get; set; }
    }
}
