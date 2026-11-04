using BE_08YS;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL_08YS.SQL
{
    // Arma los pedazos de SQL de "la reserva ocupa la mesa" y "el pedido tiene stock
    // comprometido" a partir de las listas de estados de BE. Así todas las consultas usan
    // la misma regla sin escribir los estados a mano.
    internal static class FiltroOcupacionMesa_790MY
    {
        // Devuelve "alias.Estado IN (@EstOcupa0, ...)" y agrega los parámetros al comando.
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

        // Lo mismo para los pedidos que todavía no descontaron stock.
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
