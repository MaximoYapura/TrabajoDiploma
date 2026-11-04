using BE_08YS;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    // Acceso a la tabla Empleados.
    public interface IEmpleadoRepository_790MY
    {
        bool Exists(int dni);

        // Devuelve el empleado (activo o no) o null si no existe.
        Empleado_790MY GetByDni(int dni);

        // Empleado vinculado al usuario del sistema indicado, o null.
        Empleado_790MY GetByUsuarioDni(int usuarioDni);

        List<Empleado_790MY> GetAll();

        // Empleados activos de un puesto (por ejemplo, los camareros para Abrir mesa).
        List<Empleado_790MY> GetActivosPorPuesto(PuestoEmpleado_790MY puesto);

        // True si el usuario ya está vinculado a otro empleado (distinto de exceptoDni).
        bool UsuarioVinculadoAOtroEmpleado(int usuarioDni, int exceptoDni);

        void Add(Empleado_790MY empleado);

        void Update(Empleado_790MY empleado);
    }
}
