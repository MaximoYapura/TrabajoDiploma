using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_08YS;
using Service_08YS.Entities.Bitacora;
using MPP_08YS;
using DAL_08YS.Repositories_Interfaces;

namespace DAL_08YS
{
    public class SqlBitacoraRepository_08YS : Connection_08YS, IBitacoraRepository_08YS
    {
        public SqlBitacoraRepository_08YS(IDbFactory_08YS factory) : base(factory) { }

        private IDbDataParameter[] ToParameters(BitacoraEvento_08YS b)
        {
            return new[]
            {
                Param("@username",       b.Username),
                Param("@targetUsername", (object)b.TargetUsername ?? DBNull.Value),
                Param("@fecha_hora",     b.FechaHora),
                Param("@modulo",         (int)b.Modulo),
                Param("@evento",         (int)b.Evento),
                Param("@criticidad",     (int)b.Criticidad)
            };
        }

        public void RegistrarEvento(BitacoraEvento_08YS evento)
        {
            ExecuteNonQuery(
                "INSERT INTO Bitacora (Username, TargetUsername, FechaHora, Modulo, Evento, Criticidad) " +
                "VALUES (@username, @targetUsername, @fecha_hora, @modulo, @evento, @criticidad)",
                ToParameters(evento));
        }

        public List<BitacoraEvento_08YS> GetAll()
        {
            DataTable dt = GetDataTable("SELECT * FROM Bitacora ORDER BY FechaHora DESC");
            return BitacoraMapper_08YS.FromDataTable(dt);
        }

        public List<BitacoraEvento_08YS> Filtrar(BitacoraFiltro_08YS filtro)
        {
            StringBuilder query = new StringBuilder("SELECT * FROM Bitacora WHERE 1=1");
            List<IDbDataParameter> parametros = new List<IDbDataParameter>();

            if (!string.IsNullOrWhiteSpace(filtro.Username))
            {
                query.Append(" AND Username LIKE @username");
                parametros.Add(Param("@username", $"%{filtro.Username}%"));
            }

            if (filtro.FechaDesde.HasValue && filtro.FechaHasta.HasValue)
            {
                query.Append(" AND FechaHora >= @desde AND FechaHora <= @hasta");
                parametros.Add(Param("@desde", filtro.FechaDesde.Value));
                parametros.Add(Param("@hasta", filtro.FechaHasta.Value));
            }

            if (filtro.Modulo.HasValue)
            {
                query.Append(" AND Modulo = @modulo");
                parametros.Add(Param("@modulo", (int)filtro.Modulo.Value));
            }

            if (filtro.Evento.HasValue)
            {
                query.Append(" AND Evento = @evento");
                parametros.Add(Param("@evento", (int)filtro.Evento.Value));
            }

            if (filtro.Criticidad.HasValue)
            {
                query.Append(" AND Criticidad = @criticidad");
                parametros.Add(Param("@criticidad", (int)filtro.Criticidad.Value));
            }

            if (!string.IsNullOrWhiteSpace(filtro.TargetUsername))
            {
                query.Append(" AND TargetUsername LIKE @targetUsername");
                parametros.Add(Param("@targetUsername", $"%{filtro.TargetUsername}%"));
            }

            query.Append(" ORDER BY FechaHora DESC");

            return BitacoraMapper_08YS.FromDataTable(GetDataTable(query.ToString(), parametros.ToArray()));
        }

        public int ContarIntentosFallidos(string username, int ventanaHoras)
        {
            return ExecuteScalar<int>(
                @"SELECT COUNT(*) FROM Bitacora
                  WHERE Username  = @username
                  AND Evento    = @evento
                  AND FechaHora >= DATEADD(HOUR, -@horas, GETDATE())
                  AND FechaHora >  ISNULL(
                      (SELECT MAX(FechaHora) FROM Bitacora
                          WHERE Evento IN (@desbloqueo, @loginExitoso)
                          AND (
                              Username       = @username    -- LoginExitoso: el actor es el propio usuario
                              OR TargetUsername = @username -- UsuarioDesbloqueado: el usuario es el sujeto
                          )),
                      CAST('19000101' AS DATETIME))",
                new[]
                {
                    Param("@username",     username),
                    Param("@evento",       (int)Evento.LoginFallido),
                    Param("@horas",        ventanaHoras),
                    Param("@desbloqueo",   (int)Evento.UsuarioDesbloqueado),
                    Param("@loginExitoso", (int)Evento.LoginExitoso)
                }
            );
        }
    }
}
