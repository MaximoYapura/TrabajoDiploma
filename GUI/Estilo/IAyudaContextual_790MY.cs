using System.Collections.Generic;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Pantalla con Sugerencias del Chef (Etapa 5, Fase 6, C7). Solo devuelve datos: el
    /// componente compartido AyudaSugerencias_790MY resuelve los controles y dibuja el marco
    /// y el globo, así ningún formulario crea controles visuales para la ayuda.
    /// </summary>
    public interface IAyudaContextual_790MY
    {
        IReadOnlyList<PistaAyuda_790MY> ObtenerPistas();
    }

    /// <summary>Una sugerencia: control a resaltar (por Name) y claves de idiomas.json.</summary>
    public sealed class PistaAyuda_790MY
    {
        public PistaAyuda_790MY(string nombreControl, string claveTitulo, string claveTexto, string claveProbaCon = null)
        {
            NombreControl = nombreControl;
            ClaveTitulo = claveTitulo;
            ClaveTexto = claveTexto;
            ClaveProbaCon = claveProbaCon;
        }

        public string NombreControl { get; }
        public string ClaveTitulo { get; }
        public string ClaveTexto { get; }

        /// <summary>Opcional: un dato de prueba sugerido.</summary>
        public string ClaveProbaCon { get; }
    }
}
