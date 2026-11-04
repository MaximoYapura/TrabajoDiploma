using System;

namespace BLL_08YS
{
    // Una mesa abierta en la lista de mesas por cobrar.
    public class MesaCobroDTO_790MY
    {
        public int ReservaID { get; set; }
        public int MesaNumero { get; set; }
        public int CantidadComensales { get; set; }
        public string CamareroNombre { get; set; }
        public string ClienteNombre { get; set; }

        // Hora de inicio del turno.
        public TimeSpan Desde { get; set; }

        // Suma de lo consumido, sin descuento.
        public decimal SubtotalConsumido { get; set; }

        public bool TieneFacturaAbierta { get; set; }
    }
}
