using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// «Visita ya vista» (Etapa 5, A7 / C6 / G4): archivo local, sin tocar la base, en
    /// %LocalAppData%\El Fogon del Sur\ayuda.json (Program Files no es escribible sin
    /// privilegios). La clave es el usuario de la aplicación, no el rol: dos camareros
    /// comparten rol. Toda E/S va protegida: si falla, se asume «no vista» y el ingreso sigue.
    /// </summary>
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
