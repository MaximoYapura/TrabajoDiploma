using MPP_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IPermisoRepository_08YS
    {
        List<Permiso_08YS> GetAll();
    }
}
