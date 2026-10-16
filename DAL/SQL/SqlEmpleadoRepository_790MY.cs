using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_08YS.SQL
{
    public class SqlEmpleadoRepository_790MY : IEmpleadoRepository_790MY
    {
        private const string Columnas = "SELECT DNI, Nombre, Apellido, Puesto, UsuarioDNI, Activo FROM Empleados ";

        private readonly string _connectionString;

        public SqlEmpleadoRepository_790MY()
            : this(SqlDbFactory_08YS.ConnectionString)
        {
        }

        public SqlEmpleadoRepository_790MY(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public bool Exists(int dni)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT COUNT(1) FROM Empleados WHERE DNI = @DNI", con))
            {
                cmd.Parameters.Add("@DNI", SqlDbType.Int).Value = dni;
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public Empleado_790MY GetByDni(int dni)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(Columnas + "WHERE DNI = @DNI", con))
            {
                cmd.Parameters.Add("@DNI", SqlDbType.Int).Value = dni;
                con.Open();
                using (var reader = cmd.ExecuteReader())
                    return reader.Read() ? Mapear(reader) : null;
            }
        }

        public Empleado_790MY GetByUsuarioDni(int usuarioDni)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(Columnas + "WHERE UsuarioDNI = @UsuarioDNI", con))
            {
                cmd.Parameters.Add("@UsuarioDNI", SqlDbType.Int).Value = usuarioDni;
                con.Open();
                using (var reader = cmd.ExecuteReader())
                    return reader.Read() ? Mapear(reader) : null;
            }
        }

        public List<Empleado_790MY> GetAll()
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(Columnas + "ORDER BY Apellido, Nombre", con))
            {
                con.Open();
                return LeerLista(cmd);
            }
        }

        public List<Empleado_790MY> GetActivosPorPuesto(PuestoEmpleado_790MY puesto)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(Columnas + "WHERE Activo = 1 AND Puesto = @Puesto ORDER BY Apellido, Nombre", con))
            {
                cmd.Parameters.Add("@Puesto", SqlDbType.NVarChar, 20).Value = puesto.ToString();
                con.Open();
                return LeerLista(cmd);
            }
        }

        public bool UsuarioVinculadoAOtroEmpleado(int usuarioDni, int exceptoDni)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Empleados WHERE UsuarioDNI = @UsuarioDNI AND DNI <> @DNI", con))
            {
                cmd.Parameters.Add("@UsuarioDNI", SqlDbType.Int).Value = usuarioDni;
                cmd.Parameters.Add("@DNI", SqlDbType.Int).Value = exceptoDni;
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public void Add(Empleado_790MY empleado)
        {
            const string sql =
                "INSERT INTO Empleados (DNI, Nombre, Apellido, Puesto, UsuarioDNI, Activo) " +
                "VALUES (@DNI, @Nombre, @Apellido, @Puesto, @UsuarioDNI, @Activo)";

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                CargarParametros(cmd, empleado);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Empleado_790MY empleado)
        {
            const string sql =
                "UPDATE Empleados SET Nombre = @Nombre, Apellido = @Apellido, Puesto = @Puesto, " +
                "UsuarioDNI = @UsuarioDNI, Activo = @Activo WHERE DNI = @DNI";

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                CargarParametros(cmd, empleado);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        private static void CargarParametros(SqlCommand cmd, Empleado_790MY e)
        {
            cmd.Parameters.Add("@DNI", SqlDbType.Int).Value = e.DNI;
            cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = e.Nombre;
            cmd.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = e.Apellido;
            cmd.Parameters.Add("@Puesto", SqlDbType.NVarChar, 20).Value = e.Puesto.ToString();
            cmd.Parameters.Add("@UsuarioDNI", SqlDbType.Int).Value = e.UsuarioDNI.HasValue ? (object)e.UsuarioDNI.Value : DBNull.Value;
            cmd.Parameters.Add("@Activo", SqlDbType.Bit).Value = e.Activo;
        }

        private static List<Empleado_790MY> LeerLista(SqlCommand cmd)
        {
            var lista = new List<Empleado_790MY>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    lista.Add(Mapear(reader));
            }
            return lista;
        }

        private static Empleado_790MY Mapear(SqlDataReader reader)
        {
            int ordUsuario = reader.GetOrdinal("UsuarioDNI");
            return new Empleado_790MY
            {
                DNI = reader.GetInt32(reader.GetOrdinal("DNI")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                Puesto = (PuestoEmpleado_790MY)Enum.Parse(typeof(PuestoEmpleado_790MY), reader.GetString(reader.GetOrdinal("Puesto"))),
                UsuarioDNI = reader.IsDBNull(ordUsuario) ? (int?)null : reader.GetInt32(ordUsuario),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
            };
        }
    }
}
