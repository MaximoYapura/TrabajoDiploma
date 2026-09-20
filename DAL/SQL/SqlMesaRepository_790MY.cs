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
            : this(SqlDbFactory_08YS.ConnectionString)
        {
        }

        public SqlMesaRepository_790MY(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public bool Exists(int nroMesa)
        {
            const string sql = "SELECT COUNT(1) FROM Mesas WHERE NroMesa = @NroMesa";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@NroMesa", SqlDbType.Int).Value = nroMesa;
                connection.Open();
                return (int)command.ExecuteScalar() > 0;
            }
        }

        public List<Mesa_790MY> GetAll()
        {
            const string sql = "SELECT NroMesa, Capacidad, Estado, Activo FROM Mesas WHERE Activo = 1";
            var resultado = new List<Mesa_790MY>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resultado.Add(MapearMesa(reader));
                    }
                }
            }

            return resultado;
        }

        public List<Mesa_790MY> GetDisponibles(DateTime fecha, TimeSpan hora, int comensales)
        {
            const string sql = @"
                SELECT m.NroMesa, m.Capacidad, m.Estado, m.Activo
                FROM Mesas m
                WHERE m.Activo = 1
                  AND m.Capacidad >= @Comensales
                  AND NOT EXISTS (
                        SELECT 1
                        FROM Reservas r
                        WHERE r.MesaNumero = m.NroMesa
                          AND r.Fecha = @Fecha
                          AND r.Hora = @Hora
                          AND r.Estado = @EstadoConfirmada
                  )
                ORDER BY m.Capacidad, m.NroMesa";

            var resultado = new List<Mesa_790MY>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Comensales", SqlDbType.Int).Value = comensales;
                command.Parameters.Add("@Fecha", SqlDbType.Date).Value = fecha.Date;
                command.Parameters.Add("@Hora", SqlDbType.Time).Value = hora;
                command.Parameters.Add("@EstadoConfirmada", SqlDbType.NVarChar, 20).Value = EstadoReserva_790MY.Confirmada.ToString();

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resultado.Add(MapearMesa(reader));
                    }
                }
            }

            return resultado;
        }

        public void Add(Mesa_790MY mesa)
        {
            const string sql = @"INSERT INTO Mesas (NroMesa, Capacidad, Estado, Activo)
                                  VALUES (@NroMesa, @Capacidad, @Estado, @Activo)";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@NroMesa", SqlDbType.Int).Value = mesa.NroMesa;
                command.Parameters.Add("@Capacidad", SqlDbType.Int).Value = mesa.Capacidad;
                command.Parameters.Add("@Estado", SqlDbType.NVarChar, 20).Value = mesa.Estado.ToString();
                command.Parameters.Add("@Activo", SqlDbType.Bit).Value = mesa.Activo;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Mesa_790MY mesa)
        {
            // No actualiza NroMesa (clave) ni Activo (eso lo maneja DeleteLogico).
            const string sql = @"UPDATE Mesas SET Capacidad = @Capacidad, Estado = @Estado WHERE NroMesa = @NroMesa";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@NroMesa", SqlDbType.Int).Value = mesa.NroMesa;
                command.Parameters.Add("@Capacidad", SqlDbType.Int).Value = mesa.Capacidad;
                command.Parameters.Add("@Estado", SqlDbType.NVarChar, 20).Value = mesa.Estado.ToString();

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteLogico(int nroMesa)
        {
            const string sql = "UPDATE Mesas SET Activo = 0 WHERE NroMesa = @NroMesa";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@NroMesa", SqlDbType.Int).Value = nroMesa;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateEstado(int nroMesa, EstadoMesa_790MY estado)
        {
            const string sql = "UPDATE Mesas SET Estado = @Estado WHERE NroMesa = @NroMesa";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Estado", SqlDbType.NVarChar, 20).Value = estado.ToString();
                command.Parameters.Add("@NroMesa", SqlDbType.Int).Value = nroMesa;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static Mesa_790MY MapearMesa(SqlDataReader reader)
        {
            return new Mesa_790MY
            {
                NroMesa = reader.GetInt32(reader.GetOrdinal("NroMesa")),
                Capacidad = reader.GetInt32(reader.GetOrdinal("Capacidad")),
                Estado = (EstadoMesa_790MY)Enum.Parse(typeof(EstadoMesa_790MY), reader.GetString(reader.GetOrdinal("Estado"))),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
            };
        }
    }
}
