using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL_08YS.SQL
{
    public class SqlFacturaRepository_790MY : IFacturaRepository_790MY
    {
        private const string SelectFacturas = @"
                SELECT f.NroFactura, f.NroReserva, f.FechaEmision, f.Subtotal, f.PorcentajeDescuento, f.MontoDescuento,
                       f.SubtotalNeto, f.IVA, f.MontoTotal, f.Estado,
                       r.MesaNumero, r.CantidadComensales,
                       c.Nombre + N' ' + c.Apellido AS ClienteNombre,
                       e.Nombre + N' ' + e.Apellido AS CamareroNombre
                FROM Facturas f
                INNER JOIN Reservas r ON r.ReservaID = f.NroReserva
                LEFT JOIN Clientes c ON c.DNI = r.ClienteDNI
                LEFT JOIN Empleados e ON e.DNI = r.CamareroDNI ";

        private readonly string _connectionString;

        public SqlFacturaRepository_790MY()
            : this(SqlDbFactory_08YS.ConnectionString)
        {
        }

        public SqlFacturaRepository_790MY(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // ─── Consumos ────────────────────────────────────────────────────────

        public List<ItemFactura_790MY> ObtenerConsumosFacturables(int reservaId)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                    SELECT i.ProductoID, pr.Nombre, i.PrecioUnitario, SUM(i.Cantidad) AS Cantidad,
                           MIN(pe.NroPedido) AS PrimerPedido
                    FROM Pedidos pe
                    INNER JOIN ItemsPedido i ON i.NroPedido = pe.NroPedido
                    INNER JOIN Productos pr ON pr.ProductoID = i.ProductoID
                    WHERE pe.ReservaID = @ReservaID AND pe.Estado = @Listo
                    GROUP BY i.ProductoID, pr.Nombre, i.PrecioUnitario
                    ORDER BY PrimerPedido, i.ProductoID", con))
            {
                cmd.Parameters.Add("@ReservaID", SqlDbType.Int).Value = reservaId;
                cmd.Parameters.Add("@Listo", SqlDbType.NVarChar, 20).Value = EstadoPedido_790MY.Listo.ToString();
                con.Open();

                var items = new List<ItemFactura_790MY>();
                using (var reader = cmd.ExecuteReader())
                {
                    int nro = 1;
                    while (reader.Read())
                    {
                        items.Add(new ItemFactura_790MY
                        {
                            NroItem = nro++,
                            ProductoID = reader.GetInt32(reader.GetOrdinal("ProductoID")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Nombre")),
                            PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                            Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad"))
                        });
                    }
                }
                return items;
            }
        }

        // ─── CUN-006 ─────────────────────────────────────────────────────────

        public ResultadoGenerarFactura_790MY GenerarFactura(Factura_790MY factura, out int? nroFacturaAnulada)
        {
            if (factura == null) throw new ArgumentNullException(nameof(factura));
            nroFacturaAnulada = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction tx = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        // 1) La reserva debe seguir Asistida (bloqueo para que no se cobre en paralelo).
                        using (var cmd = new SqlCommand(@"
                                SELECT COUNT(1) FROM Reservas WITH (UPDLOCK, HOLDLOCK)
                                WHERE ReservaID = @ReservaID AND Estado = @Asistida", connection, tx))
                        {
                            cmd.Parameters.Add("@ReservaID", SqlDbType.Int).Value = factura.NroReserva;
                            cmd.Parameters.Add("@Asistida", SqlDbType.NVarChar, 20).Value = EstadoReserva_790MY.Asistida.ToString();
                            if ((int)cmd.ExecuteScalar() == 0)
                            {
                                tx.Rollback();
                                return ResultadoGenerarFactura_790MY.ReservaNoAsistida;
                            }
                        }

                        // 2) Una reserva se cobra una sola vez.
                        using (var cmd = new SqlCommand(@"
                                SELECT COUNT(1) FROM Facturas WITH (UPDLOCK, HOLDLOCK)
                                WHERE NroReserva = @ReservaID AND Estado = @Pagada", connection, tx))
                        {
                            cmd.Parameters.Add("@ReservaID", SqlDbType.Int).Value = factura.NroReserva;
                            cmd.Parameters.Add("@Pagada", SqlDbType.NVarChar, 20).Value = EstadoFactura_790MY.Pagada.ToString();
                            if ((int)cmd.ExecuteScalar() > 0)
                            {
                                tx.Rollback();
                                return ResultadoGenerarFactura_790MY.YaPagada;
                            }
                        }

                        // 3) La factura Abierta anterior (si la hay) queda Anulada: nunca se borra.
                        using (var cmd = new SqlCommand(@"
                                UPDATE Facturas SET Estado = @Anulada
                                OUTPUT inserted.NroFactura
                                WHERE NroReserva = @ReservaID AND Estado = @Abierta", connection, tx))
                        {
                            cmd.Parameters.Add("@Anulada", SqlDbType.NVarChar, 20).Value = EstadoFactura_790MY.Anulada.ToString();
                            cmd.Parameters.Add("@Abierta", SqlDbType.NVarChar, 20).Value = EstadoFactura_790MY.Abierta.ToString();
                            cmd.Parameters.Add("@ReservaID", SqlDbType.Int).Value = factura.NroReserva;
                            object anulada = cmd.ExecuteScalar();
                            if (anulada != null && anulada != DBNull.Value)
                                nroFacturaAnulada = Convert.ToInt32(anulada);
                        }

                        // 4) Alta de la factura con importes congelados.
                        using (var cmd = new SqlCommand(@"
                                INSERT INTO Facturas (NroReserva, FechaEmision, Subtotal, PorcentajeDescuento, MontoDescuento,
                                                      SubtotalNeto, IVA, MontoTotal, Estado)
                                OUTPUT inserted.NroFactura
                                VALUES (@NroReserva, @FechaEmision, @Subtotal, @PorcentajeDescuento, @MontoDescuento,
                                        @SubtotalNeto, @IVA, @MontoTotal, @Abierta)", connection, tx))
                        {
                            cmd.Parameters.Add("@NroReserva", SqlDbType.Int).Value = factura.NroReserva;
                            cmd.Parameters.Add("@FechaEmision", SqlDbType.DateTime2).Value = factura.FechaEmision;
                            AgregarImporte(cmd, "@Subtotal", factura.Subtotal);
                            AgregarImporte(cmd, "@PorcentajeDescuento", factura.PorcentajeDescuento, 5);
                            AgregarImporte(cmd, "@MontoDescuento", factura.MontoDescuento);
                            AgregarImporte(cmd, "@SubtotalNeto", factura.SubtotalNeto);
                            AgregarImporte(cmd, "@IVA", factura.IVA);
                            AgregarImporte(cmd, "@MontoTotal", factura.MontoTotal);
                            cmd.Parameters.Add("@Abierta", SqlDbType.NVarChar, 20).Value = EstadoFactura_790MY.Abierta.ToString();
                            factura.NroFactura = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        int nroItem = 1;
                        foreach (var item in factura.Items)
                        {
                            item.NroFactura = factura.NroFactura;
                            item.NroItem = nroItem++;

                            using (var cmd = new SqlCommand(@"
                                    INSERT INTO ItemsFactura (NroFactura, NroItem, ProductoID, Descripcion, Cantidad, PrecioUnitario)
                                    VALUES (@NroFactura, @NroItem, @ProductoID, @Descripcion, @Cantidad, @PrecioUnitario)", connection, tx))
                            {
                                cmd.Parameters.Add("@NroFactura", SqlDbType.Int).Value = item.NroFactura;
                                cmd.Parameters.Add("@NroItem", SqlDbType.Int).Value = item.NroItem;
                                cmd.Parameters.Add("@ProductoID", SqlDbType.Int).Value = item.ProductoID;
                                cmd.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 100).Value = item.Descripcion;
                                cmd.Parameters.Add("@Cantidad", SqlDbType.Int).Value = item.Cantidad;
                                AgregarImporte(cmd, "@PrecioUnitario", item.PrecioUnitario, 10);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        factura.Estado = EstadoFactura_790MY.Abierta;
                        return ResultadoGenerarFactura_790MY.Ok;
                    }
                    catch
                    {
                        IntentarRollback(tx);
                        factura.NroFactura = 0;
                        nroFacturaAnulada = null;
                        throw;
                    }
                }
            }
        }

        // ─── Consultas ───────────────────────────────────────────────────────

        public Factura_790MY GetById(int nroFactura)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(SelectFacturas + "WHERE f.NroFactura = @NroFactura", con))
            {
                cmd.Parameters.Add("@NroFactura", SqlDbType.Int).Value = nroFactura;
                con.Open();
                return LeerConItems(con, cmd);
            }
        }

        public Factura_790MY GetAbiertaPorReserva(int reservaId)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(SelectFacturas +
                "WHERE f.NroReserva = @ReservaID AND f.Estado = @Abierta ORDER BY f.NroFactura DESC", con))
            {
                cmd.Parameters.Add("@ReservaID", SqlDbType.Int).Value = reservaId;
                cmd.Parameters.Add("@Abierta", SqlDbType.NVarChar, 20).Value = EstadoFactura_790MY.Abierta.ToString();
                con.Open();
                return LeerConItems(con, cmd);
            }
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        internal static void AgregarImporte(SqlCommand cmd, string nombre, decimal valor, byte precision = 12)
        {
            var p = cmd.Parameters.Add(nombre, SqlDbType.Decimal);
            p.Precision = precision;
            p.Scale = 2;
            p.Value = valor;
        }

        internal static void IntentarRollback(SqlTransaction tx)
        {
            try { tx.Rollback(); }
            catch (InvalidOperationException) { /* la transaccion ya fue cerrada por el servidor */ }
        }

        private static Factura_790MY LeerConItems(SqlConnection con, SqlCommand cmd)
        {
            Factura_790MY factura;
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read()) return null;
                factura = Mapear(reader);
            }

            using (var cmdItems = new SqlCommand(@"
                    SELECT NroFactura, NroItem, ProductoID, Descripcion, Cantidad, PrecioUnitario
                    FROM ItemsFactura WHERE NroFactura = @NroFactura ORDER BY NroItem", con))
            {
                cmdItems.Parameters.Add("@NroFactura", SqlDbType.Int).Value = factura.NroFactura;
                using (var reader = cmdItems.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        factura.Items.Add(new ItemFactura_790MY
                        {
                            NroFactura = reader.GetInt32(0),
                            NroItem = reader.GetInt32(1),
                            ProductoID = reader.GetInt32(2),
                            Descripcion = reader.GetString(3),
                            Cantidad = reader.GetInt32(4),
                            PrecioUnitario = reader.GetDecimal(5)
                        });
                    }
                }
            }
            return factura;
        }

        private static Factura_790MY Mapear(SqlDataReader reader)
        {
            int ordCliente = reader.GetOrdinal("ClienteNombre");
            int ordCamarero = reader.GetOrdinal("CamareroNombre");

            return new Factura_790MY
            {
                NroFactura = reader.GetInt32(reader.GetOrdinal("NroFactura")),
                NroReserva = reader.GetInt32(reader.GetOrdinal("NroReserva")),
                FechaEmision = reader.GetDateTime(reader.GetOrdinal("FechaEmision")),
                Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal")),
                PorcentajeDescuento = reader.GetDecimal(reader.GetOrdinal("PorcentajeDescuento")),
                MontoDescuento = reader.GetDecimal(reader.GetOrdinal("MontoDescuento")),
                SubtotalNeto = reader.GetDecimal(reader.GetOrdinal("SubtotalNeto")),
                IVA = reader.GetDecimal(reader.GetOrdinal("IVA")),
                MontoTotal = reader.GetDecimal(reader.GetOrdinal("MontoTotal")),
                Estado = (EstadoFactura_790MY)Enum.Parse(typeof(EstadoFactura_790MY), reader.GetString(reader.GetOrdinal("Estado"))),
                MesaNumero = reader.GetInt32(reader.GetOrdinal("MesaNumero")),
                CantidadComensales = reader.GetInt32(reader.GetOrdinal("CantidadComensales")),
                ClienteNombre = reader.IsDBNull(ordCliente) ? null : reader.GetString(ordCliente),
                CamareroNombre = reader.IsDBNull(ordCamarero) ? null : reader.GetString(ordCamarero)
            };
        }
    }
}
