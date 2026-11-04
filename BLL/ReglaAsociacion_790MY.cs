namespace BLL_08YS
{
    // Regla "si pide A, también pide B" del reporte predictivo. Los porcentajes van de 0 a 100.
    public class ReglaAsociacion_790MY
    {
        public int Numero { get; set; }
        public int ProductoAId { get; set; }
        public string ProductoA { get; set; }
        public int ProductoBId { get; set; }
        public string ProductoB { get; set; }

        // Cuentas que incluyen A y B.
        public int CuentasConAyB { get; set; }

        // Cuentas que incluyen A.
        public int CuentasConA { get; set; }

        // Soporte % = cuentas con A y B / total de cuentas.
        public decimal Soporte { get; set; }

        // Confianza % = cuentas con A y B / cuentas con A.
        public decimal Confianza { get; set; }

        // Lift: mayor a 1 quiere decir que A y B aparecen juntos más de lo que darían por azar.
        public decimal Lift { get; set; }
    }
}
