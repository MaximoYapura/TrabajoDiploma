using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_08YS;
namespace DAL_08YS.Interfaces_Repositories
{
    public interface IClienteRepository_790MY
    {
        bool Exists(int dni);
        void Create(Cliente_790MY cliente);
        List<Cliente_790MY> GetAll();
    }
}
