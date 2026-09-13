using DAL_08YS.Interfaces_Repositories;
using MPP_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_08YS.SQL
{
    public class SqlRolRepository_08YS : Connection_08YS , IRolRepository_08YS
    {
        public SqlRolRepository_08YS(IDbFactory_08YS factory) : base(factory) { }

        public List<Rol_08YS> GetAllPlano()
        {

            string query = "SELECT RolID, Nombre FROM Roles";
            DataTable dt = GetDataTable(query);

         
            return AccessMapper_08YS.RolesPlanosFromTable(dt);
        }

        public List<Rol_08YS> GetAll(Dictionary<int, Familia_08YS> familiasCompartidas = null)
        {
            DataSet ds = GetDataSet("sp_GetAllRolesConEstructura", storedProcedure: true);

            // Si se pasa un diccionario compartido, lo reutiliza en lugar de crear
            // instancias nuevas — necesario para que las mutaciones de validación
            // se propaguen correctamente entre Familia y Rol.
            var familias = familiasCompartidas
                ?? AccessMapper_08YS.EnsamblarFamilias(ds.Tables[3], ds.Tables[4]);

            var roles = ds.Tables[0].AsEnumerable()
                .ToDictionary(
                    r => Convert.ToInt32(r["RolID"]),
                    r => new Rol_08YS { RolID = Convert.ToInt32(r["RolID"]), Nombre = r["Nombre"].ToString() });

            foreach (DataRow row in ds.Tables[1].Rows)
            {
                int rolId = Convert.ToInt32(row["RolID"]);
                int familiaId = Convert.ToInt32(row["FamiliaID"]);
                if (roles.TryGetValue(rolId, out var rol) && familias.TryGetValue(familiaId, out var f))
                    rol.Agregar(f);
            }

            foreach (DataRow row in ds.Tables[2].Rows)
            {
                int rolId = Convert.ToInt32(row["RolID"]);
                if (roles.TryGetValue(rolId, out var rol))
                    rol.Agregar(AccessMapper_08YS.PermisoFromRow(row));
            }

            return roles.Values.ToList();
        }

        public void Create(string nombre, List<AccessComponent_08YS> componentes)
        {
            var familiasIds = componentes.OfType<Familia_08YS>().Select(f => f.FamiliaID).ToList();
            var permisosIds = componentes.OfType<Permiso_08YS>().Select(p => p.PermisoID).ToList();

            var outputParam = ParamOutput("@NuevoRolID");

            ExecuteNonQuery("sp_CreateRol",
                new IDbDataParameter[]
                {
                Param("@Nombre",   nombre),
                ParamTVP("@Familias", AccessMapper_08YS.ToIdTable(familiasIds)),
                ParamTVP("@Permisos", AccessMapper_08YS.ToIdTable(permisosIds)),
                outputParam
                },
                storedProcedure: true);
        }

        public void Modify(int rolId, string nombre, List<AccessComponent_08YS> componentes)
        {
            var familiasIds = componentes.OfType<Familia_08YS>().Select(f => f.FamiliaID).ToList();
            var permisosIds = componentes.OfType<Permiso_08YS>().Select(p => p.PermisoID).ToList();

            ExecuteNonQuery("sp_ModifyRol",
                new IDbDataParameter[]
                {
                    Param("@RolID",   rolId),
                    Param("@Nombre",  nombre),
                    ParamTVP("@Familias", AccessMapper_08YS.ToIdTable(familiasIds)),
                    ParamTVP("@Permisos", AccessMapper_08YS.ToIdTable(permisosIds))
                },
                storedProcedure: true);
        }

        public bool IsInUse(int rolId)
            => ExecuteScalar<int>(
                "SELECT COUNT(1) FROM Users WHERE RolID = @id",
                new[] { Param("@id", rolId) }) > 0;

        public void Delete(int rolId)
            => ExecuteNonQuery("sp_DeleteRol",
                new[] { Param("@RolID", rolId) },
                storedProcedure: true);

        public Rol_08YS GetById(int rolId)
        {
            DataSet ds = GetDataSet("sp_GetRolByID",
        new[] { Param("@RolID", rolId) },
        storedProcedure: true);

            if (ds.Tables[0].Rows.Count == 0) return null;

            // RS2 y RS3 → árbol de familias usando el mapper compartido
            var familias = AccessMapper_08YS.EnsamblarFamilias(ds.Tables[2], ds.Tables[3]);

            var rolRow = ds.Tables[0].Rows[0];
            var rol = new Rol_08YS
            {
                RolID = Convert.ToInt32(rolRow["RolID"]),
                Nombre = rolRow["Nombre"].ToString()
            };

            // RS1 → permisos directos del rol
            foreach (DataRow row in ds.Tables[1].Rows)
                rol.Agregar(AccessMapper_08YS.PermisoFromRow(row));

            // RS2 → solo las familias raíz van al rol directamente
            foreach (DataRow row in ds.Tables[2].Rows)
            {
                if (row["FamiliaPadreID"] != DBNull.Value) continue;

                int familiaId = Convert.ToInt32(row["FamiliaID"]);
                if (familias.TryGetValue(familiaId, out var familia))
                    rol.Agregar(familia);
            }

            return rol;
        }
    }
}
