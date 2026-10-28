using System;

namespace BLL_08YS
{
    /// <summary>
    /// Recomendación de negocio derivada de las reglas. La BLL entrega los datos y la GUI
    /// arma el texto en el idioma activo (claves RI_*).
    /// </summary>
    public class SugerenciaInteligente_790MY
    {
        public TipoSugerencia_790MY Tipo { get; set; }
        public ReglaAsociacion_790MY Regla { get; set; }

        /// <summary>Solo para PromocionDiaBajaDemanda.</summary>
        public DayOfWeek? DiaMenorDemanda { get; set; }
    }
}
