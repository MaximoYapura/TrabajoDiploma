using System;

namespace BLL_08YS
{
    public class ResumenCanasta_790MY
    {
        public int CuentasAnalizadas { get; set; }
        public int DiasAnalizados { get; set; }
        public DateTime Desde { get; set; }

        /// <summary>
        /// false si la BD todavía no tiene suficientes cuentas cobradas para calcular
        /// asociaciones: no se generan reglas ni sugerencias y la GUI lo informa.
        /// </summary>
        public bool DatosSuficientes { get; set; }
    }
}
