using DAL_08YS.Interfaces_Repositories;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL_08YS.SQL
{
    public class SqlCierreCajaRepository_790MY : ICierreCajaRepository_790MY
    {
        // Violación de PRIMARY KEY / UNIQUE constraint.
        private const int ErrorClaveDuplicada = 2627;

        private readonly string _connectionString;

        public SqlCierreCajaRepository_790MY()
            : this(SqlDbFactory_08YS.ConnectionString)
        {
        }

        public SqlCierreCajaRepository_790MY(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public bool Registrar(DateTime fecha, decimal neta, int cantidadCuentas, string usuario)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                    INSERT INTO CierresCaja (Fecha, Neta, CantidadCuentas, Usuario)
                    VALUES (@Fecha, @Neta, @CantidadCuentas, @Usuario)", con))
            {
                cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = fecha.Date;
                var pNeta = cmd.Parameters.Add("@Neta", SqlDbType.Decimal);
                pNeta.Precision = 12;
                pNeta.Scale = 2;
                pNeta.Value = neta;
                cmd.Parameters.Add("@CantidadCuentas", SqlDbType.Int).Value = cantidadCuentas;
                cmd.Parameters.Add("@Usuario", SqlDbType.NVarChar, 50).Value = usuario;
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (SqlException ex) when (ex.Number == ErrorClaveDuplicada)
                {
                    // Otra terminal cerró la misma jornada entre la consulta y el INSERT.
                    return false;
                }
            }
        }

        public bool Existe(DateTime fecha)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT COUNT(1) FROM CierresCaja WHERE Fecha = @Fecha", con))
            {
                cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = fecha.Date;
                con.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
    }
}
