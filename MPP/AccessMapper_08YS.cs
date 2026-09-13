using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP_08YS
{
    public static class AccessMapper_08YS
    {
        #region ReArmado de composite 

        /// <summary>
        /// Construye el diccionario de familias con su arbol ensamblado.
        /// familiasDt: columnas FamiliaID, Nombre, FamiliaPadreID (nullable)
        /// permisosDt: columnas FamiliaID, PermisoID, PermisoNombre, Descripcion
        /// </summary>
        public static Dictionary<int, Familia_08YS> EnsamblarFamilias(
            DataTable familiasDt,
            DataTable permisosDt)
        {
            // Crear un objeto Familia por cada FamiliaID unico
            // GroupBy porque el LEFT JOIN puede dar filas duplicadas si una familia
            // tiene varios padres (aunque no deberia pasar con
            // un arbol bien hecho, es por precaucion)
            var familias = familiasDt.AsEnumerable()
                .GroupBy(r => Convert.ToInt32(r["FamiliaID"]))
                .ToDictionary(
                    g => g.Key,
                    g => new Familia_08YS
                    {
                        FamiliaID = g.Key,
                        Nombre = g.First()["Nombre"].ToString()
                    });

            // Asignar permisos directos a cada familia
            foreach (DataRow row in permisosDt.Rows)
            {
                int fid = Convert.ToInt32(row["FamiliaID"]);
                if (familias.TryGetValue(fid, out var f))
                    f.Agregar(PermisoFromRow(row));
            }

            // Ensamblar arbol: los hijos se agregan a sus padres
            foreach (DataRow row in familiasDt.Rows)
            {
                if (row["FamiliaPadreID"] == DBNull.Value) continue;

                int hijoID = Convert.ToInt32(row["FamiliaID"]);
                int padreID = Convert.ToInt32(row["FamiliaPadreID"]);

                if (familias.TryGetValue(padreID, out var padre) &&
                    familias.TryGetValue(hijoID, out var hijo))
                    padre.Agregar(hijo);
            }

            return familias;
        }

        /// <summary>
        /// Filtra del diccionario solo las familias raiz
        /// </summary>
        public static List<Familia_08YS> FamiliasRaiz(
            Dictionary<int, Familia_08YS> familias,
            DataTable familiasDt)
        {
            var hijosIds = familiasDt.AsEnumerable()
                .Where(r => r["FamiliaPadreID"] != DBNull.Value)
                .Select(r => Convert.ToInt32(r["FamiliaID"]))
                .ToHashSet();

            return familias.Values
                .Where(f => !hijosIds.Contains(f.FamiliaID))
                .ToList();
        }

        #endregion

        #region Mappers individuales

        public static Permiso_08YS PermisoFromRow(DataRow row) => new Permiso_08YS
        {
            PermisoID = Convert.ToInt32(row["PermisoID"]),
            Nombre = row["PermisoNombre"].ToString(),
            Descripcion = row["Descripcion"] == DBNull.Value
                ? null : row["Descripcion"].ToString()
        };

        public static List<Permiso_08YS> PermisosFromTable(DataTable dt)
            => dt.AsEnumerable().Select(PermisoFromRow).ToList();

        public static Familia_08YS FamiliaFromRow(DataRow row) => new Familia_08YS
        {
            FamiliaID = Convert.ToInt32(row["FamiliaID"]),
            Nombre = row["Nombre"].ToString()
        };

        public static DataTable ToIdTable(IEnumerable<int> ids)
        {
            var dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            foreach (var id in ids) dt.Rows.Add(id);
            return dt;
        }

        public static List<Rol_08YS> RolesPlanosFromTable(DataTable dt)
        {
            var listaRoles = new List<Rol_08YS>();

            if (dt == null) return listaRoles;

            foreach (DataRow row in dt.Rows)
            {
                listaRoles.Add(new Rol_08YS
                {
                    RolID = Convert.ToInt32(row["RolID"]),
                    Nombre = row["Nombre"].ToString()
                   
                });
            }

            return listaRoles;
        }
        #endregion

    }
}

