using DAL_08YS.Interfaces_Repositories;
using MPP_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_08YS.SQL
{
    public class SqlPermisoRepository_08YS : Connection_08YS , IPermisoRepository_08YS
    {
        public SqlPermisoRepository_08YS(IDbFactory_08YS factory) : base(factory) { }

        public List<Permiso_08YS> GetAll()
                => AccessMapper_08YS.PermisosFromTable(
                        GetDataTable("sp_GetAllPermisos", storedProcedure: true));

    }
}
