using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using System;
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
    }
}
