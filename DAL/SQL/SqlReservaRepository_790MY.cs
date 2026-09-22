using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_08YS.SQL
{
    public class SqlReservaRepository_790MY : IReservaRepository_790MY
    {
        private readonly string _connectionString;

        public SqlReservaRepository_790MY()
            : this(SqlDbFactory_08YS.ConnectionString)
        {
        }

        public SqlReservaRepository_790MY(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void Add(Reserva_790MY reserva)
        {
            const string sql = @"
                INSERT INTO Reservas (ClienteDNI, MesaNumero, Fecha, Hora, CantidadComensales, Estado)
                VALUES (@ClienteDNI, @MesaNumero, @Fecha, @Hora, @CantidadComensales, @Estado);
                SELECT CAST(SCOPE_IDENTITY() AS int);";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@ClienteDNI", SqlDbType.Int).Value = reserva.ClienteDNI;
                command.Parameters.Add("@MesaNumero", SqlDbType.Int).Value = reserva.MesaNumero;
                command.Parameters.Add("@Fecha", SqlDbType.Date).Value = reserva.Fecha.Date;
                command.Parameters.Add("@Hora", SqlDbType.Time).Value = reserva.Hora;
                command.Parameters.Add("@CantidadComensales", SqlDbType.Int).Value = reserva.CantidadComensales;
                command.Parameters.Add("@Estado", SqlDbType.NVarChar, 20).Value = reserva.Estado.ToString();

                connection.Open();
                object idGenerado = command.ExecuteScalar();
                reserva.ReservaID = Convert.ToInt32(idGenerado);
            }
        }

        public List<Reserva_790MY> Buscar(DateTime? desde, DateTime? hasta, int? clienteDni, EstadoReserva_790MY? estado)
        {
            const string sql = @"
                SELECT r.ReservaID, r.ClienteDNI, r.MesaNumero, r.Fecha, r.Hora, r.CantidadComensales, r.Estado,
                       c.Nombre, c.Apellido
                FROM Reservas r
                INNER JOIN Clientes c ON c.DNI = r.ClienteDNI
                WHERE (@Desde IS NULL OR r.Fecha >= @Desde)
                  AND (@Hasta IS NULL OR r.Fecha <= @Hasta)
                  AND (@ClienteDNI IS NULL OR r.ClienteDNI = @ClienteDNI)
                  AND (@Estado IS NULL OR r.Estado = @Estado)
                ORDER BY r.Fecha, r.Hora";

            var resultado = new List<Reserva_790MY>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Desde", SqlDbType.Date).Value = desde.HasValue ? (object)desde.Value.Date : DBNull.Value;
                command.Parameters.Add("@Hasta", SqlDbType.Date).Value = hasta.HasValue ? (object)hasta.Value.Date : DBNull.Value;
                command.Parameters.Add("@ClienteDNI", SqlDbType.Int).Value = clienteDni.HasValue ? (object)clienteDni.Value : DBNull.Value;
                command.Parameters.Add("@Estado", SqlDbType.NVarChar, 20).Value = estado.HasValue ? (object)estado.Value.ToString() : DBNull.Value;

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resultado.Add(new Reserva_790MY
                        {
                            ReservaID = reader.GetInt32(reader.GetOrdinal("ReservaID")),
                            ClienteDNI = reader.GetInt32(reader.GetOrdinal("ClienteDNI")),
                            MesaNumero = reader.GetInt32(reader.GetOrdinal("MesaNumero")),
                            Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                            Hora = reader.GetTimeSpan(reader.GetOrdinal("Hora")),
                            CantidadComensales = reader.GetInt32(reader.GetOrdinal("CantidadComensales")),
                            Estado = (EstadoReserva_790MY)Enum.Parse(typeof(EstadoReserva_790MY), reader.GetString(reader.GetOrdinal("Estado"))),
                            ClienteNombreCompleto = $"{reader.GetString(reader.GetOrdinal("Nombre"))} {reader.GetString(reader.GetOrdinal("Apellido"))}"
                        });
                    }
                }
            }

            return resultado;
        }

        public void CancelarReserva(int reservaId)
        {
            const string sql = "UPDATE Reservas SET Estado = @Estado WHERE ReservaID = @ReservaID";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Estado", SqlDbType.NVarChar, 20).Value = EstadoReserva_790MY.Cancelada.ToString();
                command.Parameters.Add("@ReservaID", SqlDbType.Int).Value = reservaId;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
