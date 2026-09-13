using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_08YS;
using DAL_08YS.Interfaces_Repositories;
using DAL_08YS.Repositories_Interfaces;
using DAL_08YS.SQL;
using Service_08YS;

namespace BLL_08YS
{
    public static class BLLFactory_08YS
    {
        public static UserBLL_08YS CreateUserBLL()
        {
            IDbFactory_08YS factory = new SqlDbFactory_08YS();
            IUserRepository_08YS repo = new SqlUserRepository_08YS(factory);
            IRolRepository_08YS rolRepo = new SqlRolRepository_08YS(factory);
            BitacoraBLL_08YS bitacoraBll = CreateBitacoraBLL();
            return new UserBLL_08YS(repo, rolRepo, bitacoraBll);
        }

        public static BitacoraBLL_08YS CreateBitacoraBLL()
        {
            IDbFactory_08YS factory = new SqlDbFactory_08YS();
            IBitacoraRepository_08YS repo = new SqlBitacoraRepository_08YS(factory);
            return new BitacoraBLL_08YS(repo);
        }

        public static FamiliaBLL_08YS CreateFamiliaBLL()
        {
            IDbFactory_08YS factory = new SqlDbFactory_08YS();
            return new FamiliaBLL_08YS(
                       new SqlFamiliaRepository_08YS(factory),
                       new SqlRolRepository_08YS(factory),
                       new SqlPermisoRepository_08YS(factory),
                       CreateBitacoraBLL()
                   );
        }

        public static RolBLL_08YS CreateRolBLL()
        {
            IDbFactory_08YS factory = new SqlDbFactory_08YS();
            IRolRepository_08YS rolRepo = new SqlRolRepository_08YS(factory);
            IFamiliaRepository_08YS familiaRepo = new SqlFamiliaRepository_08YS(factory);
            IPermisoRepository_08YS permisoRepo = new SqlPermisoRepository_08YS(factory);
            BitacoraBLL_08YS bitacoraBll = CreateBitacoraBLL();
            return new RolBLL_08YS(rolRepo, familiaRepo, permisoRepo, bitacoraBll);
        }

        public static DvBLL_08YS CreateDvBLL()
        {
            IDbFactory_08YS factory = new SqlDbFactory_08YS();
            return new DvBLL_08YS(new SqlDvRepository_08YS(factory));
        }

        public static BackupBLL_08YS CreateBackupBLL()
        {
            IDbFactory_08YS factory = new SqlDbFactory_08YS();
            IBackupRepository_08YS backupRepo = new SqlBackupRepository_08YS(factory);
            return new BackupBLL_08YS(backupRepo);
        }
    }
}
