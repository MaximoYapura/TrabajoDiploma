using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Service_08YS;

namespace GUI_08YS.Estilo
{
    // Manual de recetas: un PDF por caso de uso en la carpeta Manual, al lado del ejecutable
    // (el instalador copia ahí docs\Manual). Se abre con el visor de PDF del equipo.
    internal static class ManualRecetas_790MY
    {
        public static string Carpeta => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Manual");

        // Abre la receta (por ejemplo "Receta_AbrirMesa.pdf"); sin receta abre la carpeta.
        // Devuelve null si se abrió, o el mensaje de error para el banner.
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
