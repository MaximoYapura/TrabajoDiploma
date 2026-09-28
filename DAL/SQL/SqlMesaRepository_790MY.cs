using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_08YS.SQL
{
    public class SqlMesaRepository_790MY : IMesaRepository_790MY
    {
        private readonly string _connectionString;

        public SqlMesaRepository_790MY()
        {
            _connectionString = SqlDbFactory_08YS.ConnectionString;
        }

        public SqlMesaRepository_790MY(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        private Mesa_790MY MapearMesa(SqlDataReader reader)
        {
            var mesa = new Mesa_790MY();
            mesa.NroMesa   = (int)reader["NroMesa"];
            mesa.Capacidad = (int)reader["Capacidad"];
            mesa.Estado    = (EstadoMesa_790MY)Enum.Parse(typeof(EstadoMesa_790MY), reader["Estado"].ToString());
            mesa.Activo    = (bool)reader["Activo"];
            return mesa;
        }

        // ─── IMesaRepository_790MY ───────────────────────────────────────────

        public bool Exists(int nroMesa)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Mesas WHERE NroMesa = @NroMesa", con))
            {
                cmd.Parameters.AddWithValue("@NroMesa", nroMesa);
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public List<Mesa_790MY> GetAll()
        {
            var lista = new List<Mesa_790MY>();
            const string sql =
                "SELECT NroMesa, Capacidad, Estado, Activo " +
                "FROM Mesas " +
                "ORDER BY NroMesa";

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        lista.Add(MapearMesa(reader));
                }
            }
            return lista;
        }

        public List<Mesa_790MY> GetDisponibles(DateTime fecha, TimeSpan hora, int comensales)
        {
            var lista = new List<Mesa_790MY>();
            const string sql =
                "SELECT m.NroMesa, m.Capacidad, m.Estado, m.Activo " +
                "FROM Mesas m " +
                "WHERE m.Capacidad >= @Comensales " +
                "  AND m.NroMesa NOT IN ( " +
                "      SELECT r.MesaNumero FROM Reservas r " +
                "      WHERE r.Fecha = @Fecha AND r.Hora = @Hora " +
                "        AND r.Estado = @EstadoConfirmada " +
                "  ) " +
                "ORDER BY m.Capacidad, m.NroMesa";

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Fecha",            fecha.Date);
                cmd.Parameters.AddWithValue("@Hora",             hora);
                cmd.Parameters.AddWithValue("@Comensales",       comensales);
                cmd.Parameters.AddWithValue("@EstadoConfirmada", EstadoReserva_790MY.Confirmada.ToString());
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        lista.Add(MapearMesa(reader));
                }
            }
            return lista;
        }

        public void Add(Mesa_790MY mesa)
        {
            const string sql =
                "INSERT INTO Mesas (NroMesa, Capacidad, Estado, Activo) " +
                "VALUES (@NroMesa, @Capacidad, @Estado, @Activo)";

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@NroMesa",   mesa.NroMesa);
                cmd.Parameters.AddWithValue("@Capacidad", mesa.Capacidad);
                cmd.Parameters.AddWithValue("@Estado",    mesa.Estado.ToString());
                cmd.Parameters.AddWithValue("@Activo",    mesa.Activo);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Mesa_790MY mesa)
        {
            const string sql =
                "UPDATE Mesas " +
                "SET Capacidad = @Capacidad, Estado = @Estado, Activo = @Activo " +
                "WHERE NroMesa = @NroMesa";

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@NroMesa",   mesa.NroMesa);
                cmd.Parameters.AddWithValue("@Capacidad", mesa.Capacidad);
                cmd.Parameters.AddWithValue("@Estado",    mesa.Estado.ToString());
                cmd.Parameters.AddWithValue("@Activo",    mesa.Activo);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int nroMesa)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "DELETE FROM Mesas WHERE NroMesa = @NroMesa", con))
            {
                cmd.Parameters.AddWithValue("@NroMesa", nroMesa);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool TieneReservasActivas(int nroMesa)
        {
            const string sql =
                "SELECT COUNT(1) FROM Reservas " +
                "WHERE MesaNumero = @NroMesa " +
                "  AND Fecha >= @Hoy " +
                "  AND Estado != @EstadoCancelada";

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@NroMesa",         nroMesa);
                cmd.Parameters.AddWithValue("@Hoy",             DateTime.Today);
                cmd.Parameters.AddWithValue("@EstadoCancelada", EstadoReserva_790MY.Cancelada.ToString());
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public void UpdateEstado(int nroMesa, EstadoMesa_790MY estado)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "UPDATE Mesas SET Estado = @Estado WHERE NroMesa = @NroMesa", con))
            {
                cmd.Parameters.AddWithValue("@NroMesa", nroMesa);
                cmd.Parameters.AddWithValue("@Estado",  estado.ToString());
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public ISet<int> GetNumerosOcupadosEnTurno(DateTime fecha, TimeSpan hora)
        {
            var ocupadas = new HashSet<int>();
            const string sql =
                "SELECT DISTINCT MesaNumero FROM Reservas " +
                "WHERE Fecha = @Fecha AND Hora = @Hora AND Estado = @EstadoConfirmada";

            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Fecha",            fecha.Date);
                cmd.Parameters.AddWithValue("@Hora",             hora);
                cmd.Parameters.AddWithValue("@EstadoConfirmada", EstadoReserva_790MY.Confirmada.ToString());
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        ocupadas.Add((int)reader["MesaNumero"]);
                }
            }
            return ocupadas;
        }
    }
}
