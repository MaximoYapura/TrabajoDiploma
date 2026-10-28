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
        private const string ColumnasSelect = @"
                SELECT r.ReservaID, r.ClienteDNI, r.MesaNumero, r.Fecha, r.Hora, r.CantidadComensales, r.Estado,
                       r.CamareroDNI, c.Nombre, c.Apellido
                FROM Reservas r
                INNER JOIN Clientes c ON c.DNI = r.ClienteDNI ";

        private readonly string _connectionString;

        public SqlReservaRepository_790MY()
            : this(SqlDbFactory_08YS.ConnectionString)
        {
        }

        public SqlReservaRepository_790MY(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // ─── Alta atomica ────────────────────────────────────────────────────

        public bool AddSiMesaDisponible(Reserva_790MY reserva, bool ocuparMesaFisica)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction tx = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        // 1) Re-validacion bajo bloqueo. UPDLOCK + HOLDLOCK toma un bloqueo
                        //    de rango sobre (MesaNumero, Fecha, Hora): un segundo puesto que
                        //    intente lo mismo queda esperando hasta el COMMIT/ROLLBACK y
                        //    luego ve la reserva insertada, evitando la doble asignacion.
                        using (var check = new SqlCommand { Connection = connection, Transaction = tx })
                        {
                            string ocupa = FiltroOcupacionMesa_790MY.CondicionEstado(check, "r");
                            check.CommandText = @"
                                SELECT COUNT(1)
                                FROM Mesas m WITH (UPDLOCK, HOLDLOCK)
                                WHERE m.NroMesa = @MesaNumero
                                  AND m.Activo = 1
                                  AND m.Estado <> @FueraDeServicio
                                  AND m.Capacidad >= @Comensales
                                  AND (@ExigirLibre = 0 OR m.Estado <> @Ocupada)
                                  AND NOT EXISTS (
                                      SELECT 1 FROM Reservas r WITH (UPDLOCK, HOLDLOCK)
                                      WHERE r.MesaNumero = m.NroMesa
                                        AND r.Fecha = @Fecha AND r.Hora = @Hora
                                        AND " + ocupa + @")";

                            check.Parameters.Add("@MesaNumero", SqlDbType.Int).Value = reserva.MesaNumero;
                            check.Parameters.Add("@Comensales", SqlDbType.Int).Value = reserva.CantidadComensales;
                            check.Parameters.Add("@Fecha", SqlDbType.Date).Value = reserva.Fecha.Date;
                            check.Parameters.Add("@Hora", SqlDbType.Time).Value = reserva.Hora;
                            check.Parameters.Add("@ExigirLibre", SqlDbType.Bit).Value = ocuparMesaFisica;
                            check.Parameters.Add("@Ocupada", SqlDbType.NVarChar, 20).Value = EstadoMesa_790MY.Ocupada.ToString();
                            check.Parameters.Add("@FueraDeServicio", SqlDbType.NVarChar, 20).Value = EstadoMesa_790MY.FueraDeServicio.ToString();

                            if ((int)check.ExecuteScalar() == 0)
                            {
                                tx.Rollback();
                                return false;
                            }
                        }

                        // 2) Insercion.
                        using (var insert = new SqlCommand(@"
                                INSERT INTO Reservas (ClienteDNI, MesaNumero, Fecha, Hora, CantidadComensales, Estado, CamareroDNI)
                                VALUES (@ClienteDNI, @MesaNumero, @Fecha, @Hora, @CantidadComensales, @Estado, @CamareroDNI);
                                SELECT CAST(SCOPE_IDENTITY() AS int);", connection, tx))
                        {
                            insert.Parameters.Add("@ClienteDNI", SqlDbType.Int).Value = reserva.ClienteDNI;
                            insert.Parameters.Add("@MesaNumero", SqlDbType.Int).Value = reserva.MesaNumero;
                            insert.Parameters.Add("@Fecha", SqlDbType.Date).Value = reserva.Fecha.Date;
                            insert.Parameters.Add("@Hora", SqlDbType.Time).Value = reserva.Hora;
                            insert.Parameters.Add("@CantidadComensales", SqlDbType.Int).Value = reserva.CantidadComensales;
                            insert.Parameters.Add("@Estado", SqlDbType.NVarChar, 20).Value = reserva.Estado.ToString();
                            insert.Parameters.Add("@CamareroDNI", SqlDbType.Int).Value =
                                reserva.CamareroDNI.HasValue ? (object)reserva.CamareroDNI.Value : DBNull.Value;

                            reserva.ReservaID = Convert.ToInt32(insert.ExecuteScalar());
                        }

                        // 3) Walk-in: la mesa queda fisicamente ocupada en la misma transaccion.
                        if (ocuparMesaFisica)
                            ActualizarEstadoMesa(connection, tx, reserva.MesaNumero, EstadoMesa_790MY.Ocupada);

                        tx.Commit();
                        return true;
                    }
                    catch
                    {
                        IntentarRollback(tx);
                        reserva.ReservaID = 0;
                        throw;
                    }
                }
            }
        }

        // ─── Consultas ───────────────────────────────────────────────────────

        public Reserva_790MY GetById(int reservaId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(ColumnasSelect + "WHERE r.ReservaID = @ReservaID", connection))
            {
                command.Parameters.Add("@ReservaID", SqlDbType.Int).Value = reservaId;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                    return reader.Read() ? Mapear(reader) : null;
            }
        }

        public List<Reserva_790MY> Buscar(DateTime? desde, DateTime? hasta, int? clienteDni, EstadoReserva_790MY? estado)
        {
            const string filtro = @"
                WHERE (@Desde IS NULL OR r.Fecha >= @Desde)
                  AND (@Hasta IS NULL OR r.Fecha <= @Hasta)
                  AND (@ClienteDNI IS NULL OR r.ClienteDNI = @ClienteDNI)
                  AND (@Estado IS NULL OR r.Estado = @Estado)
                ORDER BY r.Fecha, r.Hora";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(ColumnasSelect + filtro, connection))
            {
                command.Parameters.Add("@Desde", SqlDbType.Date).Value = desde.HasValue ? (object)desde.Value.Date : DBNull.Value;
                command.Parameters.Add("@Hasta", SqlDbType.Date).Value = hasta.HasValue ? (object)hasta.Value.Date : DBNull.Value;
                command.Parameters.Add("@ClienteDNI", SqlDbType.Int).Value = clienteDni.HasValue ? (object)clienteDni.Value : DBNull.Value;
                command.Parameters.Add("@Estado", SqlDbType.NVarChar, 20).Value = estado.HasValue ? (object)estado.Value.ToString() : DBNull.Value;

                connection.Open();
                return LeerLista(command);
            }
        }

        public List<Reserva_790MY> GetAsistidasPorCamarero(int camareroDni)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(ColumnasSelect +
                "WHERE r.CamareroDNI = @CamareroDNI AND r.Estado = @Asistida ORDER BY r.MesaNumero", connection))
            {
                command.Parameters.Add("@CamareroDNI", SqlDbType.Int).Value = camareroDni;
                command.Parameters.Add("@Asistida", SqlDbType.NVarChar, 20).Value = EstadoReserva_790MY.Asistida.ToString();
                connection.Open();
                return LeerLista(command);
            }
        }

        // ─── Transiciones de estado ──────────────────────────────────────────

        public bool AbrirReserva(int reservaId, int camareroDni)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction tx = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        int mesaNumero;

                        // Transicion condicional: solo se abre si sigue Confirmada y la mesa
                        // no esta ocupada fisicamente ni fuera de servicio. El WHERE actua
                        // como control optimista: si otro puesto ya la abrio, afecta 0 filas.
                        using (var cmd = new SqlCommand(@"
                                UPDATE r
                                SET r.Estado = @Asistida, r.CamareroDNI = @CamareroDNI
                                OUTPUT inserted.MesaNumero
                                FROM Reservas r
                                INNER JOIN Mesas m WITH (UPDLOCK, HOLDLOCK) ON m.NroMesa = r.MesaNumero
                                WHERE r.ReservaID = @ReservaID
                                  AND r.Estado = @Confirmada
                                  AND m.Activo = 1
                                  AND m.Estado NOT IN (@Ocupada, @FueraDeServicio)", connection, tx))
                        {
                            cmd.Parameters.Add("@Asistida", SqlDbType.NVarChar, 20).Value = EstadoReserva_790MY.Asistida.ToString();
                            cmd.Parameters.Add("@Confirmada", SqlDbType.NVarChar, 20).Value = EstadoReserva_790MY.Confirmada.ToString();
                            cmd.Parameters.Add("@Ocupada", SqlDbType.NVarChar, 20).Value = EstadoMesa_790MY.Ocupada.ToString();
                            cmd.Parameters.Add("@FueraDeServicio", SqlDbType.NVarChar, 20).Value = EstadoMesa_790MY.FueraDeServicio.ToString();
                            cmd.Parameters.Add("@CamareroDNI", SqlDbType.Int).Value = camareroDni;
                            cmd.Parameters.Add("@ReservaID", SqlDbType.Int).Value = reservaId;

                            object resultado = cmd.ExecuteScalar();
                            if (resultado == null || resultado == DBNull.Value)
                            {
                                tx.Rollback();
                                return false;
                            }
                            mesaNumero = Convert.ToInt32(resultado);
                        }

                        ActualizarEstadoMesa(connection, tx, mesaNumero, EstadoMesa_790MY.Ocupada);

                        tx.Commit();
                        return true;
                    }
                    catch
                    {
                        IntentarRollback(tx);
                        throw;
                    }
                }
            }
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        private static void ActualizarEstadoMesa(SqlConnection connection, SqlTransaction tx, int nroMesa, EstadoMesa_790MY estado)
        {
            using (var cmd = new SqlCommand("UPDATE Mesas SET Estado = @Estado WHERE NroMesa = @NroMesa", connection, tx))
            {
                cmd.Parameters.Add("@Estado", SqlDbType.NVarChar, 20).Value = estado.ToString();
                cmd.Parameters.Add("@NroMesa", SqlDbType.Int).Value = nroMesa;
                cmd.ExecuteNonQuery();
            }
        }

        private static void IntentarRollback(SqlTransaction tx)
        {
            try { tx.Rollback(); }
            catch (InvalidOperationException) { /* la transaccion ya fue cerrada por el servidor */ }
        }

        private static List<Reserva_790MY> LeerLista(SqlCommand command)
        {
            var resultado = new List<Reserva_790MY>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                    resultado.Add(Mapear(reader));
            }
            return resultado;
        }

        private static Reserva_790MY Mapear(SqlDataReader reader)
        {
            int ordCamarero = reader.GetOrdinal("CamareroDNI");
            return new Reserva_790MY
            {
                ReservaID = reader.GetInt32(reader.GetOrdinal("ReservaID")),
                ClienteDNI = reader.GetInt32(reader.GetOrdinal("ClienteDNI")),
                MesaNumero = reader.GetInt32(reader.GetOrdinal("MesaNumero")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                Hora = reader.GetTimeSpan(reader.GetOrdinal("Hora")),
                CantidadComensales = reader.GetInt32(reader.GetOrdinal("CantidadComensales")),
                Estado = (EstadoReserva_790MY)Enum.Parse(typeof(EstadoReserva_790MY), reader.GetString(reader.GetOrdinal("Estado"))),
                CamareroDNI = reader.IsDBNull(ordCamarero) ? (int?)null : reader.GetInt32(ordCamarero),
                ClienteNombreCompleto = $"{reader.GetString(reader.GetOrdinal("Nombre"))} {reader.GetString(reader.GetOrdinal("Apellido"))}"
            };
        }
    }
}
