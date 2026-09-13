using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_08YS.Entities.Bitacora;

namespace MPP_08YS
{
    public static class BitacoraMapper_08YS
    {
        public static BitacoraEvento_08YS FromDataRow(DataRow row)
        {
            return new BitacoraEvento_08YS
            {
                Username = row["Username"].ToString(),
                TargetUsername = row["TargetUsername"] == DBNull.Value ? null : row["TargetUsername"].ToString(),
                FechaHora = Convert.ToDateTime(row["FechaHora"]),
                Modulo = (Modulo)Convert.ToInt32(row["Modulo"]),
                Evento = (Evento)Convert.ToInt32(row["Evento"]),
                Criticidad = (Criticidad)Convert.ToInt32(row["Criticidad"])
            };
        }

        public static List<BitacoraEvento_08YS> FromDataTable(DataTable dt)
        {
            var eventos = new List<BitacoraEvento_08YS>();
            foreach (DataRow row in dt.Rows)
                eventos.Add(FromDataRow(row));
            return eventos;
        }
    }
}
