using DAL_08YS.Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_08YS.SQL
{
    public class SqlBackupRepository_08YS : Connection_08YS, IBackupRepository_08YS
    {
        private readonly IDbFactory_08YS _factory;

        public SqlBackupRepository_08YS(IDbFactory_08YS factory) : base(factory)
        {
            _factory = factory;
        }

        // Usa la infraestructura normal de Connection_08YS
        public void RealizarBackup(string rutaArchivo, string nombreBaseDatos)
            => ExecuteNonQuery("sp_EjecutarBackup",
                new[]
                {
                Param("@RutaArchivo",     rutaArchivo),
                Param("@NombreBaseDatos", nombreBaseDatos)
                },
                storedProcedure: true);

        /// <summary>
        /// Valida que SQL Server pueda escribir en la carpeta ejecutando un backup de prueba.
        /// Es la única forma confiable de verificar permisos del servicio de SQL Server,
        /// ya que el proceso que escribe es el servicio (NT SERVICE\MSSQL$SQLEXPRESS),
        /// no la app C#. El archivo de prueba se elimina inmediatamente.
        /// </summary>
        public void ValidarPermisoEscritura(string carpeta, string nombreBaseDatos)
        {
            string archivoTest = Path.Combine(carpeta, "_horizonhotel_permtest.bak");
            try
            {
                ExecuteNonQuery(
                    $"BACKUP DATABASE [{nombreBaseDatos}] TO DISK = @f " +
                    $"WITH FORMAT, INIT, NAME = 'test', SKIP, NOREWIND, NOUNLOAD, STATS = 100",
                    new[] { Param("@f", archivoTest) });
            }
            finally
            {
                // Eliminar siempre el archivo de prueba, haya fallado o no
                try { if (File.Exists(archivoTest)) File.Delete(archivoTest); }
                catch { }
            }
        }

        public void RestoreBackup(string rutaBak, string nombreBaseDatos)
        {
            string masterCs = BuildMasterConnectionString();

            using (var conn = new SqlConnection(masterCs))
            {
                conn.Open();

                string clausulasMove = ArmarClausulasMove(conn, nombreBaseDatos);

                EjecutarEnMaster(conn,
                    $"ALTER DATABASE [{nombreBaseDatos}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                EjecutarEnMaster(conn,
                    $"RESTORE DATABASE [{nombreBaseDatos}] FROM DISK = N'{rutaBak}' WITH REPLACE{clausulasMove}");
                EjecutarEnMaster(conn,
                    $"ALTER DATABASE [{nombreBaseDatos}] SET MULTI_USER WITH ROLLBACK IMMEDIATE");
            }

            SqlConnection.ClearAllPools();
        }

        /// <summary>
        /// Arma las cláusulas MOVE reubicando los archivos lógicos del backup a las
        /// rutas físicas ACTUALES de la base ya instalada en esta PC. Sin esto, restaurar
        /// un .bak hecho en otra instancia tira "Operating system error 5" porque SQL
        /// intenta escribir en la ruta de origen (de la otra PC), que acá no existe.
        /// Asume que la base ya fue creada por el instalador (AsegurarBaseDatos corre
        /// antes que cualquier form de restore esté disponible), así que sys.master_files
        /// ya tiene los nombres lógicos y las rutas físicas reales de esta instancia.
        /// </summary>
        private string ArmarClausulasMove(SqlConnection conn, string nombreBaseDatos)
        {
            var sb = new StringBuilder();

            using (var cmd = new SqlCommand(
                "SELECT name, physical_name FROM sys.master_files WHERE database_id = DB_ID(@bd)", conn))
            {
                cmd.Parameters.AddWithValue("@bd", nombreBaseDatos);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string logico = reader.GetString(0);
                        string fisico = reader.GetString(1);
                        sb.Append($", MOVE N'{logico}' TO N'{fisico}'");
                    }
                }
            }

            return sb.ToString();
        }

        private string BuildMasterConnectionString()
            => new SqlConnectionStringBuilder(_factory.GetConnectionString())
            { InitialCatalog = "master" }.ConnectionString;

        private static void EjecutarEnMaster(SqlConnection conn, string query)
        {
            using (var cmd = new SqlCommand(query, conn) { CommandTimeout = 600 })
                cmd.ExecuteNonQuery();
        }
    }
}
