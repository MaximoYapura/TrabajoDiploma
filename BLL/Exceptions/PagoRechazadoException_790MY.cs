using System;

namespace BLL_08YS.Exceptions
{
    /// <summary>
    /// CUN-007, escenario 8.1: la entidad bancaria (simulada) rechazó la operación.
    /// No se registró ningún pago ni cambió el estado de factura, reserva o mesa;
    /// el intento quedó en la bitácora. La GUI pide otro medio de pago.
    /// </summary>
    public class PagoRechazadoException_790MY : Exception
    {
        public string MotivoRechazo { get; }

        public PagoRechazadoException_790MY(string motivo, string mensaje) : base(mensaje)
        {
            MotivoRechazo = motivo;
        }
    }
}
