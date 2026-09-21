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
    public class SqlClienteRepository_790MY : IClienteRepository_790MY
    {
        private readonly string _connectionString;

        public SqlClienteRepository_790MY()
            : this(SqlDbFactory_08YS.ConnectionString)
        {
        }

        public SqlClienteRepository_790MY(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public bool Exists(int dni)
        {
            // Sin filtro de Activo a proposito: ver comentario en la interfaz.
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
            const string sql = @"INSERT INTO Clientes (DNI, Nombre, Apellido, Email, Telefono, Direccion, Activo)
                                  VALUES (@Dni, @Nombre, @Apellido, @Email, @Telefono, @Direccion, @Activo)";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(sql, connection, transaction))
                    {
                        command.Parameters.Add("@Dni", SqlDbType.Int).Value = cliente.DNI;
                        command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = cliente.Nombre;
                        command.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = cliente.Apellido;
                        command.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = cliente.Email;
                        command.Parameters.Add("@Telefono", SqlDbType.NVarChar, 30).Value = cliente.Telefono;
                        command.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = (object)cliente.Direccion ?? DBNull.Value;
                        command.Parameters.Add("@Activo", SqlDbType.Bit).Value = cliente.Activo;
                        command.ExecuteNonQuery();
                    }

                    ReemplazarAdicionales(connection, transaction, cliente);

                    transaction.Commit();
                }
            }
        }

        public void Update(Cliente_790MY cliente)
        {
            // No se actualiza DNI (clave) ni Activo (eso lo maneja DeleteLogico).
            const string sql = @"UPDATE Clientes
                                  SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email,
                                      Telefono = @Telefono, Direccion = @Direccion
                                  WHERE DNI = @Dni";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(sql, connection, transaction))
                    {
                        command.Parameters.Add("@Dni", SqlDbType.Int).Value = cliente.DNI;
                        command.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = cliente.Nombre;
                        command.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = cliente.Apellido;
                        command.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = cliente.Email;
                        command.Parameters.Add("@Telefono", SqlDbType.NVarChar, 30).Value = cliente.Telefono;
                        command.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = (object)cliente.Direccion ?? DBNull.Value;
                        command.ExecuteNonQuery();
                    }

                    ReemplazarAdicionales(connection, transaction, cliente);

                    transaction.Commit();
                }
            }
        }

        public void DeleteLogico(int dni)
        {
            const string sql = "UPDATE Clientes SET Activo = 0 WHERE DNI = @Dni";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Dni", SqlDbType.Int).Value = dni;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Cliente_790MY> GetAll()
        {
            const string sql = "SELECT DNI, Nombre, Apellido, Email, Telefono, Direccion, Activo FROM Clientes WHERE Activo = 1";
            var resultado = new List<Cliente_790MY>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resultado.Add(MapearCliente(reader));
                    }
                }
            }

            // N+1 deliberado: el volumen esperado de clientes en este proyecto es chico;
            // se prioriza código simple por sobre optimizar una consulta que no lo justifica hoy.
            foreach (var cliente in resultado)
                CargarAdicionales(cliente);

            return resultado;
        }

        public Cliente_790MY GetByDni(int dni)
        {
            const string sql = "SELECT DNI, Nombre, Apellido, Email, Telefono, Direccion, Activo FROM Clientes WHERE DNI = @Dni AND Activo = 1";

            Cliente_790MY cliente = null;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.Add("@Dni", SqlDbType.Int).Value = dni;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                        cliente = MapearCliente(reader);
                }
            }

            if (cliente != null)
                CargarAdicionales(cliente);

            return cliente;
        }

        private void CargarAdicionales(Cliente_790MY cliente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("SELECT Email FROM EmailsAdicionalesCliente WHERE ClienteDNI = @Dni", connection))
                {
                    command.Parameters.Add("@Dni", SqlDbType.Int).Value = cliente.DNI;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            cliente.EmailsAdicionales.Add(reader.GetString(0));
                    }
                }

                using (var command = new SqlCommand("SELECT Telefono FROM TelefonosAdicionalesCliente WHERE ClienteDNI = @Dni", connection))
                {
                    command.Parameters.Add("@Dni", SqlDbType.Int).Value = cliente.DNI;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            cliente.TelefonosAdicionales.Add(reader.GetString(0));
                    }
                }
            }
        }

        // Reemplaza por completo los adicionales de un cliente (borra e inserta de nuevo).
        // Es la forma más simple y correcta de sincronizar una colección chica sin tener
        // que diffear altas/bajas/modificaciones una por una.
        private void ReemplazarAdicionales(SqlConnection connection, SqlTransaction transaction, Cliente_790MY cliente)
        {
            using (var command = new SqlCommand("DELETE FROM EmailsAdicionalesCliente WHERE ClienteDNI = @Dni", connection, transaction))
            {
                command.Parameters.Add("@Dni", SqlDbType.Int).Value = cliente.DNI;
                command.ExecuteNonQuery();
            }

            foreach (var email in cliente.EmailsAdicionales ?? new List<string>())
            {
                using (var command = new SqlCommand("INSERT INTO EmailsAdicionalesCliente (ClienteDNI, Email) VALUES (@Dni, @Email)", connection, transaction))
                {
                    command.Parameters.Add("@Dni", SqlDbType.Int).Value = cliente.DNI;
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = email;
                    command.ExecuteNonQuery();
                }
            }

            using (var command = new SqlCommand("DELETE FROM TelefonosAdicionalesCliente WHERE ClienteDNI = @Dni", connection, transaction))
            {
                command.Parameters.Add("@Dni", SqlDbType.Int).Value = cliente.DNI;
                command.ExecuteNonQuery();
            }

            foreach (var telefono in cliente.TelefonosAdicionales ?? new List<string>())
            {
                using (var command = new SqlCommand("INSERT INTO TelefonosAdicionalesCliente (ClienteDNI, Telefono) VALUES (@Dni, @Telefono)", connection, transaction))
                {
                    command.Parameters.Add("@Dni", SqlDbType.Int).Value = cliente.DNI;
                    command.Parameters.Add("@Telefono", SqlDbType.NVarChar, 30).Value = telefono;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static Cliente_790MY MapearCliente(SqlDataReader reader)
        {
            return new Cliente_790MY
            {
                DNI = reader.GetInt32(reader.GetOrdinal("DNI")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                Direccion = reader.IsDBNull(reader.GetOrdinal("Direccion")) ? null : reader.GetString(reader.GetOrdinal("Direccion")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
            };
        }
    }
}
