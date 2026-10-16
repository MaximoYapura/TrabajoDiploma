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
        RealizarRestore,
        CrearCliente,   
        VerClientes,
        VerMesas,
        CrearMesa,
        VerReservas,
        RegistrarReserva,
        VerMaestros,   // visibilidad del botón lateral Maestros
        VerReportes,   // visibilidad del botón lateral Reportes

        // ── RF2 Atención en mesa (790MY) ─────────────────────────────────
        // El nombre de cada valor debe coincidir con Permisos.Nombre en la BD
        // (ver Migraciones_790MY/CompletarRF2_AtencionMesa_790MY.sql).
        AbrirMesa,         // CUN-004: abrir mesa con reserva o Walk-in + asignar camarero
        CerrarMesa,        // finalizar la atención (reserva Finalizada, mesa Libre)
        RegistrarPedido,   // CUN-005
        DespacharPedido,   // CUN-008 (Chef): comandas, iniciar preparación, marcar Listo
        CrearEmpleado      // alta/modificación de empleados
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
