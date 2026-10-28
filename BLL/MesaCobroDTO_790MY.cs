using System;

namespace BLL_08YS
{
    /// <summary>Mesa ocupada (reserva Asistida) en la lista "Mesas para cobrar".</summary>
    public class MesaCobroDTO_790MY
    {
        public int ReservaID { get; set; }
        public int MesaNumero { get; set; }
        public int CantidadComensales { get; set; }
        public string CamareroNombre { get; set; }
        public string ClienteNombre { get; set; }

        /// <summary>Hora de inicio del turno / apertura de la mesa.</summary>
        public TimeSpan Desde { get; set; }

        /// <summary>Subtotal de consumos entregados (sin descuento ni IVA).</summary>
        public decimal SubtotalConsumido { get; set; }

        public bool TieneFacturaAbierta { get; set; }
    }
}
