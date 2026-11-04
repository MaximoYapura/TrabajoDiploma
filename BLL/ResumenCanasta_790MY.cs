using System;

namespace BLL_08YS
{
    // Datos generales del reporte predictivo (cuentas analizadas y período).
    public class ResumenCanasta_790MY
    {
        public int CuentasAnalizadas { get; set; }
        public int DiasAnalizados { get; set; }
        public DateTime Desde { get; set; }

        // False si todavía no hay suficientes cuentas cobradas; en ese caso no hay reglas.
        public bool DatosSuficientes { get; set; }
    }
}
