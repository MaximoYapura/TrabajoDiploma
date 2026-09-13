using Service_08YS.Entities.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_08YS.Entities.Acceso
{
    public enum Permisos
    {
        VerBitacora,
        VerUsuarios,
        CrearUsuario,
        ModificarUsuario,
        DesActivarUsuario,
        DesbloquearUsuario,
        VerRoles,
        CrearRoles,
        ModificarRoles,
        EliminarRoles,
        VerFamilias,
        CrearFamilias,
        ModificarFamilias,
        EliminarFamilias,
        ManejarInconsistencias,
        VerRespaldos,
        RealizarBackup,
        RealizarRestore
    }

    public class Permiso_08YS : AccessComponent_08YS
    {
        public int PermisoID { get => ID; set => ID = value; }
        public string Descripcion { get; set; }

        // El leaf devuelve un HashSet con sí mismo — ya usa el comparer correcto
        public override HashSet<Permiso_08YS> GetPermisos()
            => new HashSet<Permiso_08YS>(new[] { this }, PermisoComparer.Instance);
    }
}
