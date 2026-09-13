using DAL_08YS.Instalador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_08YS
{
    /// <summary>
    /// Fachada de la BLL para el proceso de instalación silenciosa de la base de datos.
    /// PUNTO CRÍTICO DE ARQUITECTURA: Program.cs (UI/Config) llama solo a esta clase.
    /// La UI nunca debe importar DAL_08YS directamente.
    /// </summary>
    public static class BLLInstalador_08YS
    {
        public static void AsegurarBaseDatos() => InstaladorBD_08YS.AsegurarBaseDatos();
    }
}
