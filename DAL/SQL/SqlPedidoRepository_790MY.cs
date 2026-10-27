using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL_08YS.SQL
{
    public class SqlPedidoRepository_790MY : IPedidoRepository_790MY
    {
        private const string SelectPedidos = @"
                SELECT pe.NroPedido, pe.ReservaID, pe.Fecha, pe.Hora, pe.Estado, pe.FechaFin, pe.HoraFin,
                       pe.StockDescontado, r.MesaNumero, r.CamareroDNI
                FROM Pedidos pe
                INNER JOIN Reservas r ON r.ReservaID = pe.ReservaID ";

        private readonly string _connectionString;

        public SqlPedidoRepository_790MY()
            : this(SqlDbFactory_08YS.ConnectionString)
        {
        }

        public SqlPedidoRepository_790MY(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // ─── CUN-006: alta con validacion de stock ───────────────────────────

        public ResultadoRegistroPedido_790MY RegistrarSiHayStock(Pedido_790MY pedido)
        {
            if (pedido == null) throw new ArgumentNullException(nameof(pedido));

            // Cantidades totales por producto (un producto puede repetirse en varios items
            // con distintas observaciones). Orden por ID => orden de bloqueo estable, sin deadlocks.
            var solicitadoPorProducto = pedido.Items
                .GroupBy(i => i.ProductoID)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Cantidad));

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction tx = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        var precios = new Dictionary<int, decimal>();

                        foreach (int productoId in solicitadoPorProducto.Keys.OrderBy(id => id))
                        {
                            Producto_790MY producto;
                            using (var cmd = new SqlCommand { Connection = connection, Transaction = tx })
                            {
                                cmd.CommandText = SqlProductoRepository_790MY.SelectConComprometidas(cmd, "WITH (UPDLOCK, HOLDLOCK)")
                                                  + "WHERE p.ProductoID = @ProductoID";
                                cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = productoId;
                                using (var reader = cmd.ExecuteReader())
                                    producto = reader.Read() ? SqlProductoRepository_790MY.Mapear(reader) : null;
                            }

                            if (producto == null || !producto.Activo || producto.Precio <= 0)
                            {
                                tx.Rollback();
                                return new ResultadoRegistroPedido_790MY
                                {
                                    Estado = EstadoRegistroPedido_790MY.ProductoNoVigente,
                                    ProductoID = productoId,
                                    ProductoNombre = producto?.Nombre
                                };
                            }

                            int solicitado = solicitadoPorProducto[productoId];
                            if (!producto.Admite(solicitado))
                            {
                                tx.Rollback();
                                return new ResultadoRegistroPedido_790MY
                                {
                                    Estado = EstadoRegistroPedido_790MY.StockInsuficiente,
                                    ProductoID = productoId,
                                    ProductoNombre = producto.Nombre,
                                    PorcionesDisponibles = Math.Max(0, producto.PorcionesDisponibles)
                                };
                            }

                            precios[productoId] = producto.Precio;
                        }

                        // Insercion del pedido condicionada a que la reserva siga Asistida.
                        using (var cmd = new SqlCommand(@"
                                INSERT INTO Pedidos (ReservaID, Fecha, Hora, Estado, FechaFin, HoraFin, StockDescontado)
                                OUTPUT inserted.NroPedido
                                SELECT @ReservaID, @Fecha, @Hora, @Pendiente, NULL, NULL, 0
                                WHERE EXISTS (SELECT 1 FROM Reservas WITH (UPDLOCK, HOLDLOCK)
                                              WHERE ReservaID = @ReservaID AND Estado = @Asistida);", connection, tx))
                        {
                            cmd.Parameters.Add("@ReservaID", SqlDbType.Int).Value = pedido.ReservaID;
                            cmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = pedido.Fecha.Date;
                            cmd.Parameters.Add("@Hora", SqlDbType.Time).Value = pedido.Hora;
                            cmd.Parameters.Add("@Pendiente", SqlDbType.NVarChar, 20).Value = EstadoPedido_790MY.Pendiente.ToString();
                            cmd.Parameters.Add("@Asistida", SqlDbType.NVarChar, 20).Value = EstadoReserva_790MY.Asistida.ToString();

                            object id = cmd.ExecuteScalar();
                            if (id == null || id == DBNull.Value)
                            {
                                tx.Rollback();
                                return new ResultadoRegistroPedido_790MY { Estado = EstadoRegistroPedido_790MY.ReservaNoAsistida };
                            }
                            pedido.NroPedido = Convert.ToInt32(id);
                        }

                        int nroItem = 1;
                        foreach (var item in pedido.Items)
                        {
                            item.NroPedido = pedido.NroPedido;
                            item.NroItem = nroItem++;
                            item.PrecioUnitario = precios[item.ProductoID];

                            using (var cmd = new SqlCommand(@"
                                    INSERT INTO ItemsPedido (NroPedido, NroItem, ProductoID, Cantidad, Observaciones, PrecioUnitario)
                                    VALUES (@NroPedido, @NroItem, @ProductoID, @Cantidad, @Observaciones, @PrecioUnitario)", connection, tx))
                            {
                                cmd.Parameters.Add("@NroPedido", SqlDbType.Int).Value = item.NroPedido;
                                cmd.Parameters.Add("@NroItem", SqlDbType.Int).Value = item.NroItem;
                                cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = item.ProductoID;
                                cmd.Parameters.Add("@Cantidad", SqlDbType.Int).Value = item.Cantidad;
                                cmd.Parameters.Add("@Observaciones", SqlDbType.NVarChar, 200).Value =
                                    string.IsNullOrWhiteSpace(item.Observaciones) ? (object)DBNull.Value : item.Observaciones.Trim();
                                var pPrecio = cmd.Parameters.Add("@PrecioUnitario", SqlDbType.Decimal);
                                pPrecio.Precision = 10;
                                pPrecio.Scale = 2;
                                pPrecio.Value = item.PrecioUnitario;
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        pedido.Estado = EstadoPedido_790MY.Pendiente;
                        pedido.StockDescontado = false;
                        return new ResultadoRegistroPedido_790MY { Estado = EstadoRegistroPedido_790MY.Ok };
                    }
                    catch
                    {
                        IntentarRollback(tx);
                        pedido.NroPedido = 0;
                        throw;
                    }
                }
            }
        }

        // ─── Consultas ───────────────────────────────────────────────────────

        public Pedido_790MY GetById(int nroPedido)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(SelectPedidos + "WHERE pe.NroPedido = @NroPedido", con))
            {
                cmd.Parameters.Add("@NroPedido", SqlDbType.Int).Value = nroPedido;
                con.Open();
                var lista = LeerPedidos(cmd);
                if (lista.Count == 0) return null;
                CargarItems(con, lista);
                return lista[0];
            }
        }

        public List<Pedido_790MY> Buscar(IEnumerable<EstadoPedido_790MY> estados, int? camareroDni, int? reservaId)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand { Connection = con })
            {
                var condiciones = new List<string> { "1 = 1" };

                var listaEstados = estados?.Distinct().ToList();
                if (listaEstados != null && listaEstados.Count > 0)
                {
                    var nombres = new List<string>();
                    for (int i = 0; i < listaEstados.Count; i++)
                    {
                        string p = "@Est" + i;
                        nombres.Add(p);
                        cmd.Parameters.Add(p, SqlDbType.NVarChar, 20).Value = listaEstados[i].ToString();
                    }
                    condiciones.Add("pe.Estado IN (" + string.Join(", ", nombres) + ")");
                }

                if (camareroDni.HasValue)
                {
                    condiciones.Add("r.CamareroDNI = @CamareroDNI");
                    cmd.Parameters.Add("@CamareroDNI", SqlDbType.Int).Value = camareroDni.Value;
                }

                if (reservaId.HasValue)
                {
                    condiciones.Add("pe.ReservaID = @ReservaID");
                    cmd.Parameters.Add("@ReservaID", SqlDbType.Int).Value = reservaId.Value;
                }

                cmd.CommandText = SelectPedidos + "WHERE " + string.Join(" AND ", condiciones) +
                                  " ORDER BY pe.Fecha, pe.Hora, pe.NroPedido";
                con.Open();
                var lista = LeerPedidos(cmd);
                CargarItems(con, lista);
                return lista;
            }
        }

        public int ContarNoDespachados(int reservaId)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand { Connection = con })
            {
                string comprometido = FiltroOcupacionMesa_790MY.CondicionPedidoComprometido(cmd, null);
                cmd.CommandText = "SELECT COUNT(1) FROM Pedidos WHERE ReservaID = @ReservaID AND " + comprometido;
                cmd.Parameters.Add("@ReservaID", SqlDbType.Int).Value = reservaId;
                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        // ─── CUN-008 ─────────────────────────────────────────────────────────

        public bool IniciarPreparacion(int nroPedido)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(
                "UPDATE Pedidos SET Estado = @EnPreparacion WHERE NroPedido = @NroPedido AND Estado = @Pendiente", con))
            {
                cmd.Parameters.Add("@EnPreparacion", SqlDbType.NVarChar, 20).Value = EstadoPedido_790MY.EnPreparacion.ToString();
                cmd.Parameters.Add("@Pendiente", SqlDbType.NVarChar, 20).Value = EstadoPedido_790MY.Pendiente.ToString();
                cmd.Parameters.Add("@NroPedido", SqlDbType.Int).Value = nroPedido;
                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public ResultadoDespacho_790MY MarcarListoYDescontarStock(int nroPedido, DateTime momentoFin, out int porcionesDescontadas)
        {
            porcionesDescontadas = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction tx = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        // 0) Bloqueo de las filas de Productos ANTES de tocar Pedidos, en el mismo
                        //    orden que usa RegistrarSiHayStock (Productos -> Pedidos), para no
                        //    generar interbloqueos entre el registro de pedidos y el despacho.
                        using (var cmd = new SqlCommand(@"
                                SELECT COUNT(1) FROM Productos WITH (UPDLOCK, HOLDLOCK)
                                WHERE ProductoID IN (SELECT ProductoID FROM ItemsPedido WHERE NroPedido = @NroPedido)",
                                connection, tx))
                        {
                            cmd.Parameters.Add("@NroPedido", SqlDbType.Int).Value = nroPedido;
                            cmd.ExecuteScalar();
                        }

                        // 1) Transicion idempotente: solo una ejecucion puede pasar de
                        //    StockDescontado = 0 a 1. Un reproceso (doble clic, reintento,
                        //    segundo puesto de cocina) afecta 0 filas y no descuenta nada.
                        int filas;
                        using (var cmd = new SqlCommand { Connection = connection, Transaction = tx })
                        {
                            string comprometido = FiltroOcupacionMesa_790MY.CondicionPedidoComprometido(cmd, null);
                            cmd.CommandText = @"
                                UPDATE Pedidos
                                SET Estado = @Listo, FechaFin = @FechaFin, HoraFin = @HoraFin, StockDescontado = 1
                                WHERE NroPedido = @NroPedido AND " + comprometido;
                            cmd.Parameters.Add("@Listo", SqlDbType.NVarChar, 20).Value = EstadoPedido_790MY.Listo.ToString();
                            cmd.Parameters.Add("@FechaFin", SqlDbType.Date).Value = momentoFin.Date;
                            cmd.Parameters.Add("@HoraFin", SqlDbType.Time).Value = new TimeSpan(momentoFin.Hour, momentoFin.Minute, momentoFin.Second);
                            cmd.Parameters.Add("@NroPedido", SqlDbType.Int).Value = nroPedido;
                            filas = cmd.ExecuteNonQuery();
                        }

                        if (filas == 0)
                        {
                            bool existe;
                            using (var cmd = new SqlCommand("SELECT COUNT(1) FROM Pedidos WHERE NroPedido = @NroPedido", connection, tx))
                            {
                                cmd.Parameters.Add("@NroPedido", SqlDbType.Int).Value = nroPedido;
                                existe = (int)cmd.ExecuteScalar() > 0;
                            }
                            tx.Rollback();
                            return existe ? ResultadoDespacho_790MY.YaProcesado : ResultadoDespacho_790MY.NoEncontrado;
                        }

                        // 2) Descuento. El WHERE StockPorciones >= Total impide stock negativo:
                        //    si algun producto no alcanza, se actualizan menos filas que productos
                        //    del pedido y se revierte TODO (estado del pedido incluido).
                        int productosDelPedido, productosDescontados, totalPorciones;
                        using (var cmd = new SqlCommand(@"
                                SELECT COUNT(DISTINCT i.ProductoID), ISNULL(SUM(i.Cantidad), 0)
                                FROM ItemsPedido i
                                INNER JOIN Productos p ON p.ProductoID = i.ProductoID
                                WHERE i.NroPedido = @NroPedido AND p.ControlaStock = 1", connection, tx))
                        {
                            cmd.Parameters.Add("@NroPedido", SqlDbType.Int).Value = nroPedido;
                            using (var reader = cmd.ExecuteReader())
                            {
                                reader.Read();
                                productosDelPedido = reader.GetInt32(0);
                                totalPorciones = reader.GetInt32(1);
                            }
                        }

                        using (var cmd = new SqlCommand(@"
                                UPDATE p
                                SET p.StockPorciones = p.StockPorciones - x.Total
                                FROM Productos p WITH (UPDLOCK)
                                INNER JOIN (SELECT ProductoID, SUM(Cantidad) AS Total
                                            FROM ItemsPedido WHERE NroPedido = @NroPedido
                                            GROUP BY ProductoID) x ON x.ProductoID = p.ProductoID
                                WHERE p.ControlaStock = 1 AND p.StockPorciones >= x.Total", connection, tx))
                        {
                            cmd.Parameters.Add("@NroPedido", SqlDbType.Int).Value = nroPedido;
                            productosDescontados = cmd.ExecuteNonQuery();
                        }

                        if (productosDescontados != productosDelPedido)
                        {
                            tx.Rollback();
                            return ResultadoDespacho_790MY.StockFisicoInsuficiente;
                        }

                        tx.Commit();
                        porcionesDescontadas = totalPorciones;
                        return ResultadoDespacho_790MY.Ok;
                    }
                    catch
                    {
                        IntentarRollback(tx);
                        porcionesDescontadas = 0;
                        throw;
                    }
                }
            }
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        private static void IntentarRollback(SqlTransaction tx)
        {
            try { tx.Rollback(); }
            catch (InvalidOperationException) { /* la transaccion ya fue cerrada por el servidor */ }
        }

        private static List<Pedido_790MY> LeerPedidos(SqlCommand cmd)
        {
            var lista = new List<Pedido_790MY>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int ordFechaFin = reader.GetOrdinal("FechaFin");
                    int ordHoraFin = reader.GetOrdinal("HoraFin");
                    int ordCamarero = reader.GetOrdinal("CamareroDNI");

                    lista.Add(new Pedido_790MY
                    {
                        NroPedido = reader.GetInt32(reader.GetOrdinal("NroPedido")),
                        ReservaID = reader.GetInt32(reader.GetOrdinal("ReservaID")),
                        Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                        Hora = reader.GetTimeSpan(reader.GetOrdinal("Hora")),
                        Estado = (EstadoPedido_790MY)Enum.Parse(typeof(EstadoPedido_790MY), reader.GetString(reader.GetOrdinal("Estado"))),
                        FechaFin = reader.IsDBNull(ordFechaFin) ? (DateTime?)null : reader.GetDateTime(ordFechaFin),
                        HoraFin = reader.IsDBNull(ordHoraFin) ? (TimeSpan?)null : reader.GetTimeSpan(ordHoraFin),
                        StockDescontado = reader.GetBoolean(reader.GetOrdinal("StockDescontado")),
                        MesaNumero = reader.GetInt32(reader.GetOrdinal("MesaNumero")),
                        CamareroDNI = reader.IsDBNull(ordCamarero) ? (int?)null : reader.GetInt32(ordCamarero)
                    });
                }
            }
            return lista;
        }

        private static void CargarItems(SqlConnection con, List<Pedido_790MY> pedidos)
        {
            if (pedidos.Count == 0) return;

            var porNro = pedidos.ToDictionary(p => p.NroPedido);
            using (var cmd = new SqlCommand { Connection = con })
            {
                var nombres = new List<string>();
                int i = 0;
                foreach (int nro in porNro.Keys)
                {
                    string p = "@Nro" + i++;
                    nombres.Add(p);
                    cmd.Parameters.Add(p, SqlDbType.Int).Value = nro;
                }

                cmd.CommandText = @"
                    SELECT i.NroPedido, i.NroItem, i.ProductoID, i.Cantidad, i.Observaciones, i.PrecioUnitario, pr.Nombre
                    FROM ItemsPedido i
                    INNER JOIN Productos pr ON pr.ProductoID = i.ProductoID
                    WHERE i.NroPedido IN (" + string.Join(", ", nombres) + @")
                    ORDER BY i.NroPedido, i.NroItem";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int ordObs = reader.GetOrdinal("Observaciones");
                        var item = new ItemPedido_790MY
                        {
                            NroPedido = reader.GetInt32(reader.GetOrdinal("NroPedido")),
                            NroItem = reader.GetInt32(reader.GetOrdinal("NroItem")),
                            ProductoID = reader.GetInt32(reader.GetOrdinal("ProductoID")),
                            Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                            Observaciones = reader.IsDBNull(ordObs) ? null : reader.GetString(ordObs),
                            PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                            ProductoNombre = reader.GetString(reader.GetOrdinal("Nombre"))
                        };
                        porNro[item.NroPedido].Items.Add(item);
                    }
                }
            }
        }
    }
}
