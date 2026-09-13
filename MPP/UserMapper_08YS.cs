using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP_08YS
{
    public static class UserMapper_08YS
    {
        public static List<User_08YS> FromDataTable(DataTable dt)
        {
            var users = new List<User_08YS>();
            foreach (DataRow row in dt.Rows)
                users.Add(FromDataRow(row));
            return users;
        }

        public static User_08YS FromDataRow(DataRow row)
        {
            var usuario = new User_08YS
            {
                Username = row["Username"].ToString() ,
                DNI = Convert.ToInt32(row["DNI"]),
                Nombre =row["Nombre"].ToString(),
                Apellido = row["Apellido"].ToString(),
                Hash = row["Hash"].ToString(),
                Salt = row["Salt"].ToString(),
                Email = row["Email"].ToString(),
                Bloqueado = Convert.ToBoolean(row["Bloqueado"]),
                Activo = Convert.ToBoolean(row["Activo"]),
                Idioma = row["Idioma"].ToString()
            };

           
            usuario.Rol = new Rol_08YS();

           
            if (row.Table.Columns.Contains("RolID") && row["RolID"] != DBNull.Value)
                usuario.Rol.RolID = Convert.ToInt32(row["RolID"]);
                  
            if (row.Table.Columns.Contains("NombreRol") && row["NombreRol"] != DBNull.Value)
            {
                usuario.Rol.Nombre = row["NombreRol"].ToString();
            }

            return usuario;
        }
    }
}
