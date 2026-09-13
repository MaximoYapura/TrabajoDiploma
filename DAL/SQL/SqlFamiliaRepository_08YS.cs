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
    public class SqlFamiliaRepository_08YS : Connection_08YS , IFamiliaRepository_08YS
    {
        public SqlFamiliaRepository_08YS(IDbFactory_08YS factory) : base(factory) { }

        public List<Familia_08YS> GetAllRoots()
        {
            DataSet ds = GetDataSet("sp_GetAllFamilias", storedProcedure: true);

            var familias = AccessMapper_08YS.EnsamblarFamilias(ds.Tables[0], ds.Tables[1]);
            return AccessMapper_08YS.FamiliasRaiz(familias, ds.Tables[0]);
        }

        public Dictionary<int, Familia_08YS> GetAllDictionary()
        {
            DataSet ds = GetDataSet("sp_GetAllFamilias", storedProcedure: true);
            return AccessMapper_08YS.EnsamblarFamilias(ds.Tables[0], ds.Tables[1]);
        }
        public (List<int> FamiliaIds, List<int> RolIds) GetAncestors(int familiaId)
        {
            DataSet ds = GetDataSet("sp_GetAncestrosFamilia",
                new[] { Param("@FamiliaID", familiaId) },
                storedProcedure: true);

            var familiaIds = ds.Tables[0].AsEnumerable()
                .Select(r => Convert.ToInt32(r["FamiliaID"])).ToList();

            var rolIds = ds.Tables[1].AsEnumerable()
                .Select(r => Convert.ToInt32(r["RolID"])).ToList();

            return (familiaIds, rolIds);
        }

        public bool IsInUse(int familiaId)
            => ExecuteScalar<int>(
                "SELECT COUNT(1) FROM RolFamilia WHERE FamiliaID = @id",
                new[] { Param("@id", familiaId) }) > 0
            || ExecuteScalar<int>(
                "SELECT COUNT(1) FROM FamiliaIntegrada WHERE FamiliaID = @id",
                new[] { Param("@id", familiaId) }) > 0;

        public void Create(string nombre, List<AccessComponent_08YS> componentes)
        {
            var permisosIds = componentes.OfType<Permiso_08YS>().Select(p => p.PermisoID).ToList();
            var subFamiliasIds = componentes.OfType<Familia_08YS>().Select(f => f.FamiliaID).ToList();

            var outputParam = ParamOutput("@NuevoFamiliaID");

            ExecuteNonQuery("sp_CreateFamilia",
                new IDbDataParameter[]
                {
                    Param("@Nombre",      nombre),
                    ParamTVP("@Permisos",    AccessMapper_08YS.ToIdTable(permisosIds)),
                    ParamTVP("@SubFamilias", AccessMapper_08YS.ToIdTable(subFamiliasIds)),
                    outputParam
                },
                storedProcedure: true);
            // outputParam.Value contiene el ID generado por SCOPE_IDENTITY()
        }

        public void Modify(int familiaId, string nombre, List<AccessComponent_08YS> componentes)
        {
            var permisosIds = componentes.OfType<Permiso_08YS>().Select(p => p.PermisoID).ToList();
            var subFamiliasIds = componentes.OfType<Familia_08YS>().Select(f => f.FamiliaID).ToList();

            ExecuteNonQuery("sp_ModifyFamilia",
                new IDbDataParameter[]
                {
                    Param("@FamiliaID",   familiaId),
                    Param("@Nombre",      nombre),
                    ParamTVP("@Permisos",    AccessMapper_08YS.ToIdTable(permisosIds)),
                    ParamTVP("@SubFamilias", AccessMapper_08YS.ToIdTable(subFamiliasIds))
                },
                storedProcedure: true);
        }

        public void Delete(int familiaId)
            => ExecuteNonQuery("sp_DeleteFamilia",
                new[] { Param("@FamiliaID", familiaId) },
                storedProcedure: true);
    }
}
