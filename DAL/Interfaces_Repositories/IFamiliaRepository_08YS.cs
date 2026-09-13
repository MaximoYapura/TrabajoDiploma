using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IFamiliaRepository_08YS
    {
        List<Familia_08YS> GetAllRoots();
        Dictionary<int, Familia_08YS> GetAllDictionary();

        (List<int> FamiliaIds, List<int> RolIds) GetAncestors(int familiaId);

        bool IsInUse(int familiaId);

        void Create(string nombre, List<AccessComponent_08YS> componentes);

        void Modify(int familiaId, string nombre, List<AccessComponent_08YS> componentes);

        void Delete(int familiaId);
    }
}
