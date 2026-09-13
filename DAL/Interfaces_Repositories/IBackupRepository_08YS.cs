using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IBackupRepository_08YS
    {
        void RealizarBackup(string rutaArchivo, string nombreBaseDatos);
        void RestoreBackup(string rutaBak, string nombreBaseDatos);
        void ValidarPermisoEscritura(string carpeta, string nombreBaseDatos);
    }
}
