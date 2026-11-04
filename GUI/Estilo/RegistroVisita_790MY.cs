using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace GUI_08YS.Estilo
{
    // Recuerda qué usuarios ya vieron la visita guiada, en el archivo
    // %LocalAppData%\El Fogon del Sur\ayuda.json (no en la base; Program Files no se puede
    // escribir sin permisos). Se guarda por usuario, no por rol. Si el archivo falla, se
    // toma como "no vista" y el ingreso sigue normal.
    internal static class RegistroVisita_790MY
    {
        private sealed class Datos
        {
            [JsonProperty("visitasVistas")]
            public List<string> VisitasVistas { get; set; } = new List<string>();
        }

        internal static string Ruta =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                         "El Fogon del Sur", "ayuda.json");

        // True si el usuario ya vio la visita en esta PC.
        public static bool FueVista(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario)) return false;
            try
            {
                return Leer().VisitasVistas.Any(u => string.Equals(u, usuario, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex) when (EsErrorDeArchivo(ex))
            {
                return false;
            }
        }

        // Anota que el usuario ya vio la visita.
        public static void MarcarVista(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario)) return;
            try
            {
                Datos datos;
                try { datos = Leer(); }
                catch (JsonException) { datos = new Datos(); }   // archivo dañado: se reescribe

                if (datos.VisitasVistas.Any(u => string.Equals(u, usuario, StringComparison.OrdinalIgnoreCase)))
                    return;
                datos.VisitasVistas.Add(usuario);

                Directory.CreateDirectory(Path.GetDirectoryName(Ruta));
                string temporal = Ruta + ".tmp";
                File.WriteAllText(temporal, JsonConvert.SerializeObject(datos, Formatting.Indented));
                if (File.Exists(Ruta)) File.Delete(Ruta);
                File.Move(temporal, Ruta);
            }
            catch (Exception ex) when (EsErrorDeArchivo(ex))
            {
                System.Diagnostics.Trace.TraceWarning("Ayuda: no se pudo guardar la visita vista: " + ex.Message);
            }
        }

        private static Datos Leer()
        {
            if (!File.Exists(Ruta)) return new Datos();
            var datos = JsonConvert.DeserializeObject<Datos>(File.ReadAllText(Ruta));
            if (datos?.VisitasVistas == null) return new Datos();
            return datos;
        }

        private static bool EsErrorDeArchivo(Exception ex)
            => ex is IOException || ex is UnauthorizedAccessException || ex is JsonException
            || ex is NotSupportedException || ex is System.Security.SecurityException || ex is ArgumentException;
    }
}
