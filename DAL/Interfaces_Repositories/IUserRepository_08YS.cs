using Service_08YS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_08YS.Repositories_Interfaces
{
    public interface IUserRepository_08YS
    {
        List<User_08YS> GetAll();

        User_08YS GetByUsername(string username);

        void LockOut(string username);

        void Unlock(string username);

        void Create(User_08YS user);

        bool Exists(int DNI);

        void Modify(User_08YS user, string login);

        void UpdateState(string username, bool nuevoEstado);

        void UpdatePassword(string username, string hashNuevo, string saltNuevo);

        void UpdateLanguage(string username, string language);
    }
}
