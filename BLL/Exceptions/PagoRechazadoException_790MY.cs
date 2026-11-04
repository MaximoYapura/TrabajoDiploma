using System;

namespace BLL_08YS.Exceptions
{
    // El banco simulado rechazó la tarjeta. No se grabó nada (solo el evento en la
    // bitácora) y la pantalla pide otro medio de pago.
    public class PagoRechazadoException_790MY : Exception
    {
        public string MotivoRechazo { get; }

        public PagoRechazadoException_790MY(string motivo, string mensaje) : base(mensaje)
        {
            MotivoRechazo = motivo;
        }
    }
}
