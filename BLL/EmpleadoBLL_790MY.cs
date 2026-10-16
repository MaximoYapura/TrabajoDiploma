using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using DAL_08YS.Repositories_Interfaces;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;

namespace BLL_08YS
{
    /// <summary>
    /// Reglas de negocio de Empleado. El empleado es independiente de la seguridad:
    /// la única relación con Users es UsuarioDNI (opcional), validada aquí contra
    /// IUserRepository_08YS.Exists sin cargar roles ni permisos del usuario.
    /// </summary>
    public class EmpleadoBLL_790MY
    {
        private const int LargoMaximoNombre = 100;

        private readonly IEmpleadoRepository_790MY _empleadoRepository;
        private readonly IUserRepository_08YS _userRepository;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public EmpleadoBLL_790MY(IEmpleadoRepository_790MY empleadoRepository, IUserRepository_08YS userRepository,
                                 BitacoraBLL_08YS bitacoraBll)
        {
            _empleadoRepository = empleadoRepository ?? throw new ArgumentNullException(nameof(empleadoRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _bitacoraBll = bitacoraBll ?? throw new ArgumentNullException(nameof(bitacoraBll));
        }

        public List<Empleado_790MY> GetAll()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearEmpleado);
            return _empleadoRepository.GetAll();
        }

        /// <summary>
        /// Camareros activos para el combo de "Camarero responsable" de CUN-004.
        /// Exige AbrirMesa (no el permiso de ABM) porque lo usa la Recepción.
        /// </summary>
        public List<Empleado_790MY> GetCamarerosActivos()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AbrirMesa);
            return _empleadoRepository.GetActivosPorPuesto(PuestoEmpleado_790MY.Camarero);
        }

        public void RegistrarEmpleado(Empleado_790MY empleado)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearEmpleado);

            ValidarDatos(empleado);

            if (_empleadoRepository.Exists(empleado.DNI))
                throw MensajesBLL_790MY.Regla("msg_rf2_empleado_duplicado", empleado.DNI);

            ValidarUsuarioAsociado(empleado);

            _empleadoRepository.Add(empleado);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.EmpleadoRegistrado, $"Empleado {empleado.DNI} ({empleado.Puesto})");
        }

        public void ModificarEmpleado(Empleado_790MY empleado)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearEmpleado);

            ValidarDatos(empleado);

            if (!_empleadoRepository.Exists(empleado.DNI))
                throw MensajesBLL_790MY.Regla("msg_rf2_empleado_inexistente", empleado.DNI);

            ValidarUsuarioAsociado(empleado);

            _empleadoRepository.Update(empleado);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.EmpleadoModificado, $"Empleado {empleado.DNI} ({empleado.Puesto})");
        }

        // ─── Validaciones ────────────────────────────────────────────────────

        private static void ValidarDatos(Empleado_790MY empleado)
        {
            if (empleado == null)
                throw new ArgumentNullException(nameof(empleado));

            if (empleado.DNI <= 0)
                throw MensajesBLL_790MY.Regla("msg_rf2_empleado_dni_invalido");

            if (string.IsNullOrWhiteSpace(empleado.Nombre) || string.IsNullOrWhiteSpace(empleado.Apellido)
                || empleado.Nombre.Trim().Length > LargoMaximoNombre || empleado.Apellido.Trim().Length > LargoMaximoNombre)
                throw MensajesBLL_790MY.Regla("msg_rf2_empleado_nombre_invalido", LargoMaximoNombre);

            if (!Enum.IsDefined(typeof(PuestoEmpleado_790MY), empleado.Puesto))
                throw MensajesBLL_790MY.Regla("msg_rf2_empleado_puesto_invalido");

            empleado.Nombre = empleado.Nombre.Trim();
            empleado.Apellido = empleado.Apellido.Trim();
        }

        /// <summary>
        /// Empleado sin usuario (UsuarioDNI = null): válido.
        /// Empleado con usuario: el usuario debe existir y no estar vinculado a otro empleado.
        /// </summary>
        private void ValidarUsuarioAsociado(Empleado_790MY empleado)
        {
            if (!empleado.UsuarioDNI.HasValue)
                return;

            int usuarioDni = empleado.UsuarioDNI.Value;

            if (!_userRepository.Exists(usuarioDni))
                throw MensajesBLL_790MY.Regla("msg_rf2_usuario_inexistente", usuarioDni);

            if (_empleadoRepository.UsuarioVinculadoAOtroEmpleado(usuarioDni, empleado.DNI))
                throw MensajesBLL_790MY.Regla("msg_rf2_usuario_ya_vinculado", usuarioDni);
        }
    }
}
