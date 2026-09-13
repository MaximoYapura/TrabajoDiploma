using MPP_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IRolRepository_08YS
    {
        List<Rol_08YS> GetAllPlano();

        List<Rol_08YS> GetAll(Dictionary<int, Familia_08YS> familiasCompartidas = null);

        void Create(string nombre, List<AccessComponent_08YS> componentes);

        bool IsInUse(int rolId);

        void Delete(int rolId);

        void Modify(int rolId, string nombre, List<AccessComponent_08YS> componentes);

        Rol_08YS GetById(int rolID);
    }
}
