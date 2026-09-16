using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL_08YS.SQL
{
    public class SqlClienteRepository_790MY: IClienteRepository_790MY
    {
        private readonly string _connectionString;

        public SqlClienteRepository_790MY()
            : this(ConfigurationManager.ConnectionStrings["DAL.Properties.Settings.TP_INGENIERIA_SOFTWAREConnectionString"].ConnectionString)
        {
        }

        public SqlClienteRepository_790MY(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public bool Exists(int dni)
        {
            const string sql = "SELECT COUNT(1) FROM Clientes WHERE DNI = @Dni";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Dni", SqlDbType.Int).Value = dni;
                connection.Open();
                return (int)command.ExecuteScalar() > 0;
            }
        }

        public void Create(Cliente_790MY cliente)
        {
            const string sql = @"INSERT INTO Clientes (DNI, Nombre, Apellido, Email, Telefono, Activo)
                                  VALUES (@Dni, @Nombre, @Apellido, @Email, @Telefono, @Activo)";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Dni", SqlDbType.Int).Value = cliente.DNI;
                command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = cliente.Nombre;
                command.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = cliente.Apellido;
                command.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = cliente.Email;
                command.Parameters.Add("@Telefono", SqlDbType.NVarChar, 30).Value = cliente.Telefono;
                command.Parameters.Add("@Activo", SqlDbType.Bit).Value = cliente.Activo;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Cliente_790MY> GetAll()
        {
            const string sql = "SELECT DNI, Nombre, Apellido, Email, Telefono, Activo FROM Clientes";
            var resultado = new List<Cliente_790MY>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resultado.Add(new Cliente_790MY
                        {
                            DNI = reader.GetInt32(reader.GetOrdinal("DNI")),
                            Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                            Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                            Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                        });
                    }
                }
            }

            return resultado;
        }

    }
}
