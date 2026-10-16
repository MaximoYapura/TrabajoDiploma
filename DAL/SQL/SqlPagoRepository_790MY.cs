using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static DAL_08YS.SQL.SqlFacturaRepository_790MY;

namespace DAL_08YS.SQL
{
    public class SqlPagoRepository_790MY : IPagoRepository_790MY
    {
        private readonly string _connectionString;

        public SqlPagoRepository_790MY()
            : this(SqlDbFactory_08YS.ConnectionString)
        {
        }

        public SqlPagoRepository_790MY(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // ─── CUN-007 ─────────────────────────────────────────────────────────

        public ResultadoRegistroCobro_790MY RegistrarCobro(Pago_790MY pago, out int reservaId, out int mesaNumero)
        {
            if (pago == null) throw new ArgumentNullException(nameof(pago));
            reservaId = 0;
            mesaNumero = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction tx = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        // 1) Factura Abierta (bloqueada: dos puestos no pueden cobrarla a la vez).
                        using (var cmd = new SqlCommand(@"
                                SELECT f.NroReserva, r.MesaNumero, r.Estado
                                FROM Facturas f WITH (UPDLOCK, HOLDLOCK)
                                INNER JOIN Reservas r WITH (UPDLOCK, HOLDLOCK) ON r.ReservaID = f.NroReserva
                                WHERE f.NroFactura = @NroFactura AND f.Estado = @Abierta", connection, tx))
                        {
                            cmd.Parameters.Add("@NroFactura", SqlDbType.Int).Value = pago.NroFactura;
                            cmd.Parameters.Add("@Abierta", SqlDbType.NVarChar, 20).Value = EstadoFactura_790MY.Abierta.ToString();
                            string estadoReserva;
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    reader.Close();
                                    tx.Rollback();
                                    return ResultadoRegistroCobro_790MY.FacturaNoAbierta;
                                }
                                reservaId = reader.GetInt32(0);
                                mesaNumero = reader.GetInt32(1);
                                estadoReserva = reader.GetString(2);
                            }

                            if (estadoReserva != EstadoReserva_790MY.Asistida.ToString())
                            {
                                tx.Rollback();
                                return ResultadoRegistroCobro_790MY.ReservaNoAsistida;
                            }
                        }

                        // 2) Sin pedidos sin despachar (dejarían porciones comprometidas).
                        using (var cmd = new SqlCommand { Connection = connection, Transaction = tx })
                        {
                            string comprometido = FiltroOcupacionMesa_790MY.CondicionPedidoComprometido(cmd, null);
                            cmd.CommandText = "SELECT COUNT(1) FROM Pedidos WHERE ReservaID = @ReservaID AND " + comprometido;
                            cmd.Parameters.Add("@ReservaID", SqlDbType.Int).Value = reservaId;
                            if ((int)cmd.ExecuteScalar() > 0)
                            {
                                tx.Rollback();
                                return ResultadoRegistroCobro_790MY.PedidosSinDespachar;
                            }
                        }

                        // 3) Alta del pago.
                        using (var cmd = new SqlCommand(@"
                                INSERT INTO Pagos (NroFactura, FechaHoraPago, MetodoPago, MontoAbonado, MontoRecibido, Cambio,
                                                   MontoPropina, NroTarjeta, MarcaTarjeta, CantidadCuotas, NroAutorizacion)
                                OUTPUT inserted.IdPago
                                VALUES (@NroFactura, @FechaHoraPago, @MetodoPago, @MontoAbonado, @MontoRecibido, @Cambio,
                                        @MontoPropina, @NroTarjeta, @MarcaTarjeta, @CantidadCuotas, @NroAutorizacion)", connection, tx))
                        {
                            cmd.Parameters.Add("@NroFactura", SqlDbType.Int).Value = pago.NroFactura;
                            cmd.Parameters.Add("@FechaHoraPago", SqlDbType.DateTime2).Value = pago.FechaHoraPago;
                            cmd.Parameters.Add("@MetodoPago", SqlDbType.NVarChar, 20).Value = pago.MetodoPago.ToString();
                            AgregarImporte(cmd, "@MontoAbonado", pago.MontoAbonado);
                            AgregarImporteNulable(cmd, "@MontoRecibido", pago.MontoRecibido);
                            AgregarImporteNulable(cmd, "@Cambio", pago.Cambio);
                            AgregarImporte(cmd, "@MontoPropina", pago.MontoPropina);
                            cmd.Parameters.Add("@NroTarjeta", SqlDbType.NVarChar, 25).Value = (object)pago.NroTarjeta ?? DBNull.Value;
                            cmd.Parameters.Add("@MarcaTarjeta", SqlDbType.NVarChar, 20).Value = (object)pago.MarcaTarjeta ?? DBNull.Value;
                            cmd.Parameters.Add("@CantidadCuotas", SqlDbType.Int).Value = (object)pago.CantidadCuotas ?? DBNull.Value;
                            cmd.Parameters.Add("@NroAutorizacion", SqlDbType.NVarChar, 20).Value = (object)pago.NroAutorizacion ?? DBNull.Value;
                            pago.IdPago = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // 4) Factura Pagada, Reserva Finalizada, Mesa Libre.
                        using (var cmd = new SqlCommand(@"
                                UPDATE Facturas SET Estado = @Pagada WHERE NroFactura = @NroFactura;
                                UPDATE Reservas SET Estado = @Finalizada WHERE ReservaID = @ReservaID;
                                UPDATE Mesas SET Estado = @Libre
                                WHERE NroMesa = @NroMesa
                                  AND Estado = @Ocupada
                                  AND NOT EXISTS (SELECT 1 FROM Reservas
                                                  WHERE MesaNumero = @NroMesa AND Estado = @Asistida);", connection, tx))
                        {
                            cmd.Parameters.Add("@Pagada", SqlDbType.NVarChar, 20).Value = EstadoFactura_790MY.Pagada.ToString();
                            cmd.Parameters.Add("@Finalizada", SqlDbType.NVarChar, 20).Value = EstadoReserva_790MY.Finalizada.ToString();
                            cmd.Parameters.Add("@Libre", SqlDbType.NVarChar, 20).Value = EstadoMesa_790MY.Libre.ToString();
                            cmd.Parameters.Add("@Ocupada", SqlDbType.NVarChar, 20).Value = EstadoMesa_790MY.Ocupada.ToString();
                            cmd.Parameters.Add("@Asistida", SqlDbType.NVarChar, 20).Value = EstadoReserva_790MY.Asistida.ToString();
                            cmd.Parameters.Add("@NroFactura", SqlDbType.Int).Value = pago.NroFactura;
                            cmd.Parameters.Add("@ReservaID", SqlDbType.Int).Value = reservaId;
                            cmd.Parameters.Add("@NroMesa", SqlDbType.Int).Value = mesaNumero;
                            cmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                        return ResultadoRegistroCobro_790MY.Ok;
                    }
                    catch
                    {
                        IntentarRollback(tx);
                        pago.IdPago = 0;
                        throw;
                    }
                }
            }
        }

        // ─── Consultas ───────────────────────────────────────────────────────

        public Pago_790MY GetPorFactura(int nroFactura)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                    SELECT IdPago, NroFactura, FechaHoraPago, MetodoPago, MontoAbonado, MontoRecibido, Cambio,
                           MontoPropina, NroTarjeta, MarcaTarjeta, CantidadCuotas, NroAutorizacion
                    FROM Pagos WHERE NroFactura = @NroFactura", con))
            {
                cmd.Parameters.Add("@NroFactura", SqlDbType.Int).Value = nroFactura;
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return null;
                    return new Pago_790MY
                    {
                        IdPago = r.GetInt32(0),
                        NroFactura = r.GetInt32(1),
                        FechaHoraPago = r.GetDateTime(2),
                        MetodoPago = (MetodoPago_790MY)Enum.Parse(typeof(MetodoPago_790MY), r.GetString(3)),
                        MontoAbonado = r.GetDecimal(4),
                        MontoRecibido = r.IsDBNull(5) ? (decimal?)null : r.GetDecimal(5),
                        Cambio = r.IsDBNull(6) ? (decimal?)null : r.GetDecimal(6),
                        MontoPropina = r.GetDecimal(7),
                        NroTarjeta = r.IsDBNull(8) ? null : r.GetString(8),
                        MarcaTarjeta = r.IsDBNull(9) ? null : r.GetString(9),
                        CantidadCuotas = r.IsDBNull(10) ? (int?)null : r.GetInt32(10),
                        NroAutorizacion = r.IsDBNull(11) ? null : r.GetString(11)
                    };
                }
            }
        }

        public List<CobroArqueo_790MY> ObtenerCobrosDelDia(DateTime fecha)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                    SELECT p.IdPago, p.NroFactura, p.FechaHoraPago, r.MesaNumero, p.MetodoPago,
                           f.MontoDescuento, p.MontoPropina, f.MontoTotal
                    FROM Pagos p
                    INNER JOIN Facturas f ON f.NroFactura = p.NroFactura
                    INNER JOIN Reservas r ON r.ReservaID = f.NroReserva
                    WHERE p.FechaHoraPago >= @Desde AND p.FechaHoraPago < @Hasta
                      AND f.Estado = @Pagada
                    ORDER BY p.FechaHoraPago, p.IdPago", con))
            {
                cmd.Parameters.Add("@Desde", SqlDbType.DateTime2).Value = fecha.Date;
                cmd.Parameters.Add("@Hasta", SqlDbType.DateTime2).Value = fecha.Date.AddDays(1);
                cmd.Parameters.Add("@Pagada", SqlDbType.NVarChar, 20).Value = EstadoFactura_790MY.Pagada.ToString();
                con.Open();

                var lista = new List<CobroArqueo_790MY>();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        lista.Add(new CobroArqueo_790MY
                        {
                            IdPago = r.GetInt32(0),
                            NroFactura = r.GetInt32(1),
                            FechaHoraPago = r.GetDateTime(2),
                            MesaNumero = r.GetInt32(3),
                            MetodoPago = (MetodoPago_790MY)Enum.Parse(typeof(MetodoPago_790MY), r.GetString(4)),
                            MontoDescuento = r.GetDecimal(5),
                            MontoPropina = r.GetDecimal(6),
                            ImporteCobrado = r.GetDecimal(7)
                        });
                    }
                }
                return lista;
            }
        }

        private static void AgregarImporteNulable(SqlCommand cmd, string nombre, decimal? valor)
        {
            var p = cmd.Parameters.Add(nombre, SqlDbType.Decimal);
            p.Precision = 12;
            p.Scale = 2;
            p.Value = valor.HasValue ? (object)valor.Value : DBNull.Value;
        }
    }
}
