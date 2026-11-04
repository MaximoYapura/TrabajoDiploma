using System.Collections.Generic;

namespace GUI_08YS.Estilo
{
    // La implementan las pantallas que tienen ayuda con F1. Solo devuelven la lista de
    // pistas; el marco y el globo los dibuja AyudaSugerencias_790MY.
    public interface IAyudaContextual_790MY
    {
        IReadOnlyList<PistaAyuda_790MY> ObtenerPistas();
    }

    // Una pista: el nombre del control a marcar y las claves de idiomas.json del texto.
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

        // Opcional: un dato de prueba sugerido.
        public string ClaveProbaCon { get; }
    }
}
