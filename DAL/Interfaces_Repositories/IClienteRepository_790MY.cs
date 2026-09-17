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
        // Todos los metodos de consulta (Exists, GetByDni, GetAll) operan solo
        // sobre clientes con Activo = 1 (borrado logico).
        bool Exists(int dni);
        void Create(Cliente_790MY cliente);
        void Update(Cliente_790MY cliente);
        void DeleteLogico(int dni);
        List<Cliente_790MY> GetAll();
        Cliente_790MY GetByDni(int dni);
    }
}
