using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_08YS;
namespace DAL_08YS.Interfaces_Repositories
{
    // Acceso a la tabla Clientes y a sus emails y teléfonos adicionales.
    public interface IClienteRepository_790MY
    {
        // True si el DNI está en la tabla, aunque el cliente esté dado de baja.
        // Para saber si está activo hay que usar GetByDni.
        bool Exists(int dni);

        // Alta y modificación. Graban también los emails y teléfonos adicionales.
        void Create(Cliente_790MY cliente);
        void Update(Cliente_790MY cliente);

        // Baja lógica (Activo = 0).
        void DeleteLogico(int dni);

        // Solo clientes activos.
        List<Cliente_790MY> GetAll();
        Cliente_790MY GetByDni(int dni);
    }
}
