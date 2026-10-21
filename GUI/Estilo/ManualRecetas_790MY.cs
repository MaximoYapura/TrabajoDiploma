using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Service_08YS;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Manual de Recetas (Etapa 5, Fase 8a, A8): un PDF por receta en la carpeta Manual junto
    /// al ejecutable (el instalador copia docs\Manual\*.pdf a {app}\Manual; en desarrollo los
    /// copia el proyecto a bin). Lo abre el visor de PDF predeterminado del equipo.
    /// </summary>
    internal static class ManualRecetas_790MY
    {
        public static string Carpeta => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Manual");

        /// <summary>
        /// Abre <paramref name="receta"/> (p. ej. «Receta_AbrirMesa.pdf»); sin receta abre la
        /// carpeta del manual. Devuelve null si se abrió, o el mensaje para el banner si no.
        /// </summary>
        public static string AbrirManual(string receta)
        {
            string ruta = string.IsNullOrEmpty(receta) ? Carpeta : Path.Combine(Carpeta, receta);
            bool existe = string.IsNullOrEmpty(receta) ? Directory.Exists(ruta) : File.Exists(ruta);
            if (!existe)
                return string.Format(TraductorManager_08YS.Instance.GetTexto("AY_manual_no_encontrado"), ruta);
            try
            {
                Process.Start(new ProcessStartInfo(ruta) { UseShellExecute = true });
                return null;
            }
            catch (Exception ex) when (ex is Win32Exception || ex is ExternalException || ex is InvalidOperationException)
            {
                // Sin visor de PDF asociado o acceso denegado.
                return string.Format(TraductorManager_08YS.Instance.GetTexto("AY_manual_no_abre"), ruta, ex.Message);
            }
        }
    }
}
