using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_08YS
{
    public class BackupBLL_08YS
    {
        private readonly IBackupRepository_08YS _repo;
        private const string NombreBaseDatos = "TP_Ing_Soft";

        // C:\ por defecto: el servicio de SQL Server (NT SERVICE\MSSQL$SQLEXPRESS)
        // tiene permisos de escritura en rutas del disco del sistema.
        // Documents o rutas de usuario pueden no tener esos permisos.
        public static string CarpetaDefault
            => @"C:\HorizonHotel\Backups";

        public BackupBLL_08YS(IBackupRepository_08YS repo) => _repo = repo;

        // ── Backup ────────────────────────────────────────────────────────────────

        public string RealizarBackup(string carpeta)
        {
            AsegurarCarpetaExiste(carpeta);

            string nombreArchivo = $"{NombreBaseDatos}_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

            try
            {
                _repo.RealizarBackup(rutaCompleta, NombreBaseDatos);
                return rutaCompleta;
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(TradducirErrorBackup(ex), ex);
            }
        }

        /// <summary>
        /// Valida que SQL Server pueda escribir en la carpeta seleccionada.
        /// Se llama al momento de seleccionar la carpeta en el form,
        /// antes de intentar el backup real.
        /// </summary>
        public ResultadoValidacion_08YS ValidarCarpeta(string carpeta)
        {
            // 1. Validar que la ruta no tenga caracteres inválidos
            try { Path.GetFullPath(carpeta); }
            catch
            {
                return ResultadoValidacion_08YS.Error(
                    TraductorManager_08YS.Instance.GetTexto("backup_error_ruta_invalida"));
            }

            // 2. Ruta demasiado larga (deja margen para el nombre de archivo)
            if (carpeta.Length > 200)
                return ResultadoValidacion_08YS.Error(
                    TraductorManager_08YS.Instance.GetTexto("backup_error_ruta_larga"));

            // 3. Ruta de red: SQL Server puede no alcanzarla si no está configurado
            if (carpeta.StartsWith(@"\\"))
                return ResultadoValidacion_08YS.Advertencia(
                    TraductorManager_08YS.Instance.GetTexto("backup_advertencia_red"));

            // 4. Crear la carpeta si no existe
            try { AsegurarCarpetaExiste(carpeta); }
            catch (Exception ex)
            {
                return ResultadoValidacion_08YS.Error(
                    string.Format(
                        TraductorManager_08YS.Instance.GetTexto("backup_error_crear_carpeta"),
                        ex.Message));
            }

            // 5. Verificar espacio libre (mínimo 500 MB como margen razonable)
            try
            {
                string drive = Path.GetPathRoot(carpeta);
                long espacioLibre = new DriveInfo(drive).AvailableFreeSpace;
                long minimo = 500L * 1024 * 1024;
                if (espacioLibre < minimo)
                    return ResultadoValidacion_08YS.Error(
                        string.Format(
                            TraductorManager_08YS.Instance.GetTexto("backup_error_espacio"),
                            (espacioLibre / (1024.0 * 1024)).ToString("F0")));
            }
            catch { /* Si no puede leer el drive, continúa — el backup dirá si hay problema */ }

            // 6. Validar que SQL Server (el servicio, no la app) pueda escribir
            try
            {
                _repo.ValidarPermisoEscritura(carpeta, NombreBaseDatos);
            }
            catch (SqlException ex)
            {
                return ResultadoValidacion_08YS.Error(TradducirErrorPermiso(ex));
            }
            catch (Exception ex)
            {
                return ResultadoValidacion_08YS.Error(ex.Message);
            }

            return ResultadoValidacion_08YS.Ok();
        }

        // ── Restore ───────────────────────────────────────────────────────────────

        public void RestaurarBackup(string rutaBak)
        {
            if (!File.Exists(rutaBak))
                throw new InvalidOperationException(
                    TraductorManager_08YS.Instance.GetTexto("restore_archivo_no_existe"));

            if (!rutaBak.EndsWith(".bak", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException(
                    TraductorManager_08YS.Instance.GetTexto("restore_formato_invalido"));

            // Verificar que el archivo sea legible (no esté en uso o corrupto a nivel filesystem)
            try { using (File.OpenRead(rutaBak)) { } }
            catch (IOException)
            {
                throw new InvalidOperationException(
                    TraductorManager_08YS.Instance.GetTexto("restore_archivo_en_uso"));
            }

            try
            {
                _repo.RestoreBackup(rutaBak, NombreBaseDatos);
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(TradducirErrorRestore(ex), ex);
            }
        }

        public string GenerarNombreArchivo()
            => $"{NombreBaseDatos}_{DateTime.Now:yyyyMMdd_HHmmss}.bak";

        // ── Helpers ───────────────────────────────────────────────────────────────

        public static void AsegurarCarpetaExiste(string carpeta)
        {
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        /// <summary>
        /// Traduce SqlExceptions de backup a mensajes entendibles para el usuario.
        /// Los números de error son estándar de SQL Server.
        /// </summary>
        private string TradducirErrorBackup(SqlException ex)
        {
            switch (ex.Number)
            {
                case 3201: // Cannot open backup device
                case 4861: // Cannot bulk load — insufficient permissions
                    return TradducirErrorPermiso(ex);

                case 112:  // Disk full
                    return TraductorManager_08YS.Instance.GetTexto("backup_error_disco_lleno_sql");

                case 945:  // DB cannot be opened — damaged or in recovery
                case 926:  // DB suspect
                    return TraductorManager_08YS.Instance.GetTexto("backup_error_bd_no_disponible");

                default:
                    return string.Format(
                        TraductorManager_08YS.Instance.GetTexto("backup_error_sql"),
                        ex.Number, ex.Message);
            }
        }

        private string TradducirErrorRestore(SqlException ex)
        {
            switch (ex.Number)
            {
                case 3234: // Logical file is not part of this database
                case 3013: // Backup or restore operation is terminating abnormally
                case 4305: // Too recent to apply to database
                    return TraductorManager_08YS.Instance.GetTexto("restore_error_archivo_invalido");

                case 112:
                    return TraductorManager_08YS.Instance.GetTexto("backup_error_disco_lleno_sql");

                default:
                    return string.Format(
                        TraductorManager_08YS.Instance.GetTexto("restore_error_sql"),
                        ex.Number, ex.Message);
            }
        }

        private string TradducirErrorPermiso(SqlException ex)
            => string.Format(
                TraductorManager_08YS.Instance.GetTexto("backup_error_permisos"),
                @"NT SERVICE\MSSQL$SQLEXPRESS");
    }
}
