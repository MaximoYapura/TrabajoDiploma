using BE_08YS;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL_08YS.SQL
{
    /// <summary>
    /// Arma la condicion SQL "la reserva ocupa la mesa" a partir de
    /// Reserva_790MY.EstadosQueOcupanMesa, para que todas las consultas de
    /// disponibilidad (mapa, re-validacion, alta atomica) usen la misma regla
    /// sin repetir literales de estado.
    /// </summary>
    internal static class FiltroOcupacionMesa_790MY
    {
        /// <summary>
        /// Devuelve "alias.Estado IN (@EstOcupa0, @EstOcupa1, ...)" y agrega los
        /// parametros correspondientes al comando.
        /// </summary>
        public static string CondicionEstado(SqlCommand cmd, string alias)
        {
            var nombres = new List<string>();
            int i = 0;
            foreach (EstadoReserva_790MY estado in Reserva_790MY.EstadosQueOcupanMesa)
            {
                string p = "@EstOcupa" + i++;
                nombres.Add(p);
                if (!cmd.Parameters.Contains(p))
                    cmd.Parameters.Add(p, SqlDbType.NVarChar, 20).Value = estado.ToString();
            }

            string columna = string.IsNullOrEmpty(alias) ? "Estado" : alias + ".Estado";
            return columna + " IN (" + string.Join(", ", nombres) + ")";
        }

        /// <summary>
        /// Igual que CondicionEstado pero para los estados de pedido cuyo stock
        /// esta comprometido y aun no descontado.
        /// </summary>
        public static string CondicionPedidoComprometido(SqlCommand cmd, string alias)
        {
            var nombres = Pedido_790MY.EstadosConStockComprometido
                .Select((e, idx) =>
                {
                    string p = "@EstComp" + idx;
                    if (!cmd.Parameters.Contains(p))
                        cmd.Parameters.Add(p, SqlDbType.NVarChar, 20).Value = e.ToString();
                    return p;
                })
                .ToList();

            string pref = string.IsNullOrEmpty(alias) ? "" : alias + ".";
            return "(" + pref + "StockDescontado = 0 AND " + pref + "Estado IN (" + string.Join(", ", nombres) + "))";
        }
    }
}
