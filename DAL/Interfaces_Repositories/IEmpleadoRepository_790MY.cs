using BE_08YS;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IEmpleadoRepository_790MY
    {
        bool Exists(int dni);

        /// <summary>Devuelve el empleado (activo o no) o null si no existe.</summary>
        Empleado_790MY GetByDni(int dni);

        /// <summary>Empleado vinculado al usuario del sistema indicado, o null.</summary>
        Empleado_790MY GetByUsuarioDni(int usuarioDni);

        List<Empleado_790MY> GetAll();

        List<Empleado_790MY> GetActivosPorPuesto(PuestoEmpleado_790MY puesto);

        /// <summary>True si el usuario ya esta vinculado a un empleado distinto de exceptoDni.</summary>
        bool UsuarioVinculadoAOtroEmpleado(int usuarioDni, int exceptoDni);

        void Add(Empleado_790MY empleado);

        void Update(Empleado_790MY empleado);
    }
}
