using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL_08YS.SQL
{
    public class SqlProductoRepository_790MY : IProductoRepository_790MY
    {
        private readonly string _connectionString;

        public SqlProductoRepository_790MY()
            : this(SqlDbFactory_08YS.ConnectionString)
        {
        }

        public SqlProductoRepository_790MY(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Arma el SELECT de productos con la subconsulta de porciones comprometidas.
        /// Se reutiliza desde SqlPedidoRepository_790MY dentro de su transaccion.
        /// </summary>
        internal static string SelectConComprometidas(SqlCommand cmd, string hintsProductos)
        {
            string comprometido = FiltroOcupacionMesa_790MY.CondicionPedidoComprometido(cmd, "pe");
            return
                "SELECT p.ProductoID, p.Nombre, p.Precio, p.StockPorciones, p.Activo, p.Categoria, " +
                "       ISNULL((SELECT SUM(i.Cantidad) FROM ItemsPedido i " +
                "               INNER JOIN Pedidos pe ON pe.NroPedido = i.NroPedido " +
                "               WHERE i.ProductoID = p.ProductoID AND " + comprometido + "), 0) AS Comprometidas " +
                "FROM Productos p " + (hintsProductos ?? string.Empty) + " ";
        }

        internal static Producto_790MY Mapear(SqlDataReader reader)
        {
            return new Producto_790MY
            {
                ProductoID = reader.GetInt32(reader.GetOrdinal("ProductoID")),
                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                StockPorciones = reader.GetInt32(reader.GetOrdinal("StockPorciones")),
                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                Categoria = Enum.TryParse(reader.GetString(reader.GetOrdinal("Categoria")), out CategoriaProducto_790MY categoria)
                    ? categoria
                    : CategoriaProducto_790MY.PlatoPrincipal,
                PorcionesComprometidas = Convert.ToInt32(reader["Comprometidas"])
            };
        }

        public List<Producto_790MY> GetActivos()
        {
            var lista = new List<Producto_790MY>();
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand { Connection = con })
            {
                cmd.CommandText = SelectConComprometidas(cmd, null) + "WHERE p.Activo = 1 ORDER BY p.Categoria, p.Nombre";
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        lista.Add(Mapear(reader));
                }
            }
            return lista;
        }

        public Producto_790MY GetById(int productoId)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand { Connection = con })
            {
                cmd.CommandText = SelectConComprometidas(cmd, null) + "WHERE p.ProductoID = @ProductoID";
                cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoId;
                con.Open();
                using (var reader = cmd.ExecuteReader())
                    return reader.Read() ? Mapear(reader) : null;
            }
        }
    }
}
