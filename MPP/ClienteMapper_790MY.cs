using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_08YS;
namespace MPP_08YS
{
    public static class ClienteMapper_790MY
    {
        public static List<Cliente_790MY> FromDataTable(DataTable dt)
        {
            var clientes = new List<Cliente_790MY>();
            foreach (DataRow row in dt.Rows)
                clientes.Add(FromDataRow(row));
            return clientes;
        }

        public static Cliente_790MY FromDataRow(DataRow row) => new Cliente_790MY
        {
            DNI = Convert.ToInt32(row["DNI"]),
            Nombre = row["Nombre"].ToString(),
            Apellido = row["Apellido"].ToString(),
            Email = row["Email"].ToString(),
            Telefono = row["Telefono"].ToString(),
            Activo = Convert.ToBoolean(row["Activo"])
        };
    }
}
