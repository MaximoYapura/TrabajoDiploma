using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DAL_08YS.SQL
{
    /// <summary>
    /// ProductoCDAL: lectura del historial de Productos_C y activación de versiones.
    /// Nunca inserta ni borra: las versiones las generan los triggers de Productos.
    /// </summary>
    public class SqlProductoCRepository_790MY : IProductoCRepository_790MY
    {
        private const string SelectBase =
            "SELECT c.ID, c.ProductoID, c.Nombre, c.Categoria, c.Precio, c.StockPorciones, " +
            "       c.ControlaStock, c.Activo, c.Fecha, c.Act " +
            "FROM Productos_C c ";

        private readonly string _connectionString;

        public SqlProductoCRepository_790MY()
            : this(SqlDbFactory_08YS.ConnectionString)
        {
        }

        public SqlProductoCRepository_790MY(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public List<Producto_C_790MY> ObtenerHistorial(int? productoID, string nombre, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand { Connection = con })
            {
                var where = new StringBuilder("WHERE 1 = 1 ");

                if (productoID.HasValue)
                {
                    where.Append("AND c.ProductoID = @ProductoID ");
                    cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoID.Value;
                }
                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    where.Append("AND c.Nombre LIKE @Nombre ");
                    cmd.Parameters.Add("@Nombre", SqlDbType.NVarChar, 102).Value = "%" + EscaparLike(nombre.Trim()) + "%";
                }
                if (fechaDesde.HasValue)
                {
                    where.Append("AND c.Fecha >= @Desde ");
                    cmd.Parameters.Add("@Desde", SqlDbType.DateTime).Value = fechaDesde.Value.Date;
                }
                if (fechaHasta.HasValue)
                {
                    // Rango semiabierto: incluye todo el día "hasta".
                    where.Append("AND c.Fecha < @Hasta ");
                    cmd.Parameters.Add("@Hasta", SqlDbType.DateTime).Value = fechaHasta.Value.Date.AddDays(1);
                }

                cmd.CommandText = SelectBase + where + "ORDER BY c.Fecha DESC, c.ID DESC";
                con.Open();
                return Leer(cmd);
            }
        }

        public Producto_C_790MY ObtenerVigente(int productoID)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(SelectBase + "WHERE c.ProductoID = @ProductoID AND c.Act = 1 ORDER BY c.ID DESC", con))
            {
                cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoID;
                con.Open();
                List<Producto_C_790MY> lista = Leer(cmd);
                return lista.Count > 0 ? lista[0] : null;
            }
        }

        public Producto_C_790MY ObtenerPorId(int idHistorico)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(SelectBase + "WHERE c.ID = @ID", con))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = idHistorico;
                con.Open();
                List<Producto_C_790MY> lista = Leer(cmd);
                return lista.Count > 0 ? lista[0] : null;
            }
        }

        public void ActivarVersion(int idHistorico)
        {
            // Los triggers (producto_c_actualizado -> Productos -> producto_actualizado)
            // corren dentro de esta misma sentencia: si alguno falla, se revierte todo.
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("UPDATE Productos_C SET Act = 1 WHERE ID = @ID AND Act = 0", con))
            {
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = idHistorico;
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private static List<Producto_C_790MY> Leer(SqlCommand cmd)
        {
            var lista = new List<Producto_C_790MY>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    lista.Add(Mapear(reader));
            }
            return lista;
        }

        private static Producto_C_790MY Mapear(SqlDataReader reader)
        {
            return new Producto_C_790MY
            {
                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                ProductoID = reader.GetInt32(reader.GetOrdinal("ProductoID")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Categoria = Enum.TryParse(reader.GetString(reader.GetOrdinal("Categoria")), out CategoriaProducto_790MY categoria)
                    ? categoria
                    : CategoriaProducto_790MY.PlatoPrincipal,
                Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                StockPorciones = reader.GetInt32(reader.GetOrdinal("StockPorciones")),
                ControlaStock = reader.GetBoolean(reader.GetOrdinal("ControlaStock")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                Act = reader.GetBoolean(reader.GetOrdinal("Act"))
            };
        }

        /// <summary>Evita que %, _ o [ escritos por el usuario actúen como comodines del LIKE.</summary>
        private static string EscaparLike(string texto)
            => texto.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
    }
}
