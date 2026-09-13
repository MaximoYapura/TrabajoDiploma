using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_08YS
{
    public class ComponenteEnUsoException_08YS : Exception { }
    public class NombreDuplicadoException_08YS : Exception { }
    public class PermisosDuplicadosException_08YS : Exception { }
    public class RolBLL_08YS : AccesoBLL_08YS
    {
        private readonly BitacoraBLL_08YS _bitacoraBll;
        private readonly IRolRepository_08YS _rolRepo;
        private readonly IFamiliaRepository_08YS _familiaRepo;

        public RolBLL_08YS(IRolRepository_08YS rolRepo, IFamiliaRepository_08YS familiaRepo, IPermisoRepository_08YS permisoRepo, BitacoraBLL_08YS bitacoraBll) : base(permisoRepo)
        {
            _rolRepo = rolRepo;
            _familiaRepo = familiaRepo;
            _bitacoraBll = bitacoraBll;
        }

        public List<Rol_08YS> GetAll() => _rolRepo.GetAll();

        public List<Rol_08YS> GetAllPlano() => _rolRepo.GetAllPlano();

        public List<AccessComponent_08YS> GetComponentesDisponibles()
        {
            var permisos = _permisoRepo.GetAll().Cast<AccessComponent_08YS>();
            var familias = _familiaRepo.GetAllRoots().Cast<AccessComponent_08YS>();
            return permisos.Concat(familias).ToList();
        }

        public void Crear(string nombre, HashSet<AccessComponent_08YS> componentes)
        {
            ValidarDatosEntrada(nombre, componentes);
            ValidarRolNoExistente(nombre, componentes, null);
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearRoles);

            _rolRepo.Create(nombre, componentes.ToList());
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.RolCreado);
        }

        public void Modificar(int rolId, string nombre, HashSet<AccessComponent_08YS> componentes)
        {
            ValidarDatosEntrada(nombre, componentes);
            ValidarRolNoExistente(nombre, componentes, rolId);
            SessionManager_08YS.Instance.ValidatePermission(Permisos.ModificarRoles);

            _rolRepo.Modify(rolId, nombre, componentes.ToList());
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.RolModificado);

            if(rolId == SessionManager_08YS.Instance.Current.Rol.RolID)
                SessionManager_08YS.Instance.InvalidarSesion();
        }

        public void Eliminar(int rolId)
        {
            if (_rolRepo.IsInUse(rolId))
                throw new ComponenteEnUsoException_08YS();

            SessionManager_08YS.Instance.ValidatePermission(Permisos.EliminarRoles);

            _rolRepo.Delete(rolId);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.RolEliminado);
        }

        public void ValidarRolNoExistente(string nombre, HashSet<AccessComponent_08YS> componentes, int? excluirId)
        {
            var permsNuevos = componentes.SelectMany(c => c.GetPermisos()).Select(p => p.PermisoID).ToHashSet();
            var roles = _rolRepo.GetAll().Where(r => !excluirId.HasValue || r.RolID != excluirId.Value).ToList();

            if (roles.Any(r => r.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
                throw new NombreDuplicadoException_08YS();

            if (roles.Any(r => r.GetPermisos().Select(p => p.PermisoID).ToHashSet().SetEquals(permsNuevos)))
                throw new PermisosDuplicadosException_08YS();
        }
    }
}
