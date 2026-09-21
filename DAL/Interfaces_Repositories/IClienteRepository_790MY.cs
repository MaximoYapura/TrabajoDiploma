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
        // No filtra por Activo: verifica existencia fisica de la fila para prevenir
        // una violacion de clave primaria al registrar un DNI que ya existe (aunque
        // este dado de baja logicamente). Para saber si un cliente existe Y esta
        // activo (por ejemplo, para permitirle hacer una reserva), usar GetByDni
        // (que si filtra por Activo = 1) y chequear que no devuelva null.
        bool Exists(int dni);
        void Create(Cliente_790MY cliente);
        void Update(Cliente_790MY cliente);
        void DeleteLogico(int dni);
        List<Cliente_790MY> GetAll();
        Cliente_790MY GetByDni(int dni);
    }
}
