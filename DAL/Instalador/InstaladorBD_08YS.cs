using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL_08YS.Instalador
{
    /// <summary>
    /// Crea la base de datos en silencio en el primer arranque, a partir del
    /// script.sql que queda al lado del .exe. No depende de ningún formulario:
    /// se invoca una sola vez desde BLLInstalador_08YS.AsegurarBaseDatos().
    /// </summary>
    public static class InstaladorBD_08YS
    {
        public const string NombreBaseDatos = "TP_Ing_Soft";
        private const string NombreArchivoScript = "script.sql";

        public static bool ExisteBaseDatos(string instancia)
        {
            using (var conn = new SqlConnection(ArmarConnectionStringMaster(instancia)))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT COUNT(1) FROM sys.databases WHERE name = @n", conn))
                {
                    cmd.Parameters.AddWithValue("@n", NombreBaseDatos);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public static void InstalarBaseDatos(string instancia)
        {
            string rutaScript = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NombreArchivoScript);

            if (!File.Exists(rutaScript))
                throw new FileNotFoundException(
                    $"No se encontró '{NombreArchivoScript}' junto al ejecutable. " +
                    "Verificá que el Setup Project lo incluya en Application Folder.",
                    rutaScript);

            string script = File.ReadAllText(rutaScript);

            // SqlCommand no entiende el separador "GO": hay que partir el script
            // en lotes y ejecutar cada uno por separado, sobre la misma conexión
            // (el propio script hace "USE [TP_Ing_Soft]" a mitad de camino, y esa
            // conexión mantiene ese contexto para los lotes siguientes).
            string[] lotes = Regex.Split(script, @"^\s*GO\s*$",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using (var conn = new SqlConnection(ArmarConnectionStringMaster(instancia)))
            {
                conn.Open();
                foreach (string lote in lotes)
                {
                    if (string.IsNullOrWhiteSpace(lote)) continue;

                    try
                    {
                        using (var cmd = new SqlCommand(lote, conn) { CommandTimeout = 120 })
                            cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // Lotes de configuración (ALTER DATABASE ... QUERY_STORE, ACCELERATED_DATABASE_RECOVERY,
                        // etc.) pueden no ser soportados según la edición/versión de SQL Server instalada
                        // (por ejemplo LocalDB 2019 vs. SQL Server 2022). No son críticos para el
                        // funcionamiento de la app: si fallan, se ignoran y se sigue con el resto del script
                        // para no perder la creación de tablas y datos. Se deja rastro en un log para poder
                        // diagnosticar si algo importante (una tabla) llegara a fallar.
                        try
                        {
                            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "instalador_db.log");
                            File.AppendAllText(logPath,
                                $"[{DateTime.Now}] Lote ignorado por error: {ex.Message}{Environment.NewLine}");
                        }
                        catch { /* si ni siquiera se puede loguear, no bloqueamos la instalación por esto */ }

                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Punto de entrada único: si la base no existe en la instancia configurada,
        /// la crea. Se llama desde BLLInstalador_08YS al arrancar la app.
        /// </summary>
        public static void AsegurarBaseDatos()
        {
            string instancia = new SqlConnectionStringBuilder(SqlDbFactory_08YS.ConnectionString).DataSource;

            if (!ExisteBaseDatos(instancia))
                InstalarBaseDatos(instancia);
        }

        private static string ArmarConnectionStringMaster(string instancia)
            => new SqlConnectionStringBuilder(SqlDbFactory_08YS.ConnectionString)
            {
                DataSource = instancia,
                InitialCatalog = "master"
            }.ConnectionString;
    }
}

