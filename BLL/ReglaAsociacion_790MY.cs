namespace BLL_08YS
{
    /// <summary>"Si pide A -> también pide B", con sus métricas expresadas en porcentaje (0-100).</summary>
    public class ReglaAsociacion_790MY
    {
        public int Numero { get; set; }
        public int ProductoAId { get; set; }
        public string ProductoA { get; set; }
        public int ProductoBId { get; set; }
        public string ProductoB { get; set; }

        /// <summary>Cuentas que incluyen A y B.</summary>
        public int CuentasConAyB { get; set; }

        /// <summary>Cuentas que incluyen A.</summary>
        public int CuentasConA { get; set; }

        /// <summary>Soporte % = cuentas con A y B / total de cuentas.</summary>
        public decimal Soporte { get; set; }

        /// <summary>Confianza % = cuentas con A y B / cuentas con A.</summary>
        public decimal Confianza { get; set; }

        /// <summary>Lift = Confianza / P(B). Mayor a 1 indica asociación real (no azar).</summary>
        public decimal Lift { get; set; }
    }
}
