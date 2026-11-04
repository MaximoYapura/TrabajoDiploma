using System;

namespace BLL_08YS
{
    // Sugerencia comercial del reporte predictivo. La BLL da los datos y la pantalla arma
    // el texto en el idioma activo.
    public class SugerenciaInteligente_790MY
    {
        public TipoSugerencia_790MY Tipo { get; set; }
        public ReglaAsociacion_790MY Regla { get; set; }

        // Solo para PromocionDiaBajaDemanda.
        public DayOfWeek? DiaMenorDemanda { get; set; }
    }
}
