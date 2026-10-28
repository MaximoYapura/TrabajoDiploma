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
        private readonly IReservaRepository_790MY _reservaRepository;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public EmpleadoBLL_790MY(IEmpleadoRepository_790MY empleadoRepository, IUserRepository_08YS userRepository,
                                 IReservaRepository_790MY reservaRepository, BitacoraBLL_08YS bitacoraBll)
        {
            _empleadoRepository = empleadoRepository ?? throw new ArgumentNullException(nameof(empleadoRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _reservaRepository = reservaRepository ?? throw new ArgumentNullException(nameof(reservaRepository));
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
            return ObtenerCamarerosActivos();
        }

        // ─── Consultas para otras BLL ────────────────────────────────────────
        // Sin ValidatePermission: las usan ReservaBLL, PedidoBLL y FacturaBLL, que ya
        // validaron su propia patente (AbrirMesa, RegistrarPedido, DespacharPedido,
        // Caja). Exigir CrearEmpleado acá le negaría el acceso a Recepción, Camarero y Chef.

        /// <summary>
        /// Empleado vinculado al usuario logueado (Users.DNI -> Empleados.UsuarioDNI), o
        /// null si no tiene. Cada llamador aplica su regla (activo, puesto Camarero).
        /// </summary>
        internal Empleado_790MY ObtenerEmpleadoDeSesion()
        {
            User_08YS usuario = SessionManager_08YS.Instance.Current
                ?? throw new UnauthorizedAccessException(MensajesBLL_790MY.Texto("msg_rf2_sin_sesion"));

            return _empleadoRepository.GetByUsuarioDni(usuario.DNI);
        }

        internal List<Empleado_790MY> ObtenerCamarerosActivos()
            => _empleadoRepository.GetActivosPorPuesto(PuestoEmpleado_790MY.Camarero);

        internal Empleado_790MY ObtenerPorDni(int dni)
            => _empleadoRepository.GetByDni(dni);

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

            Empleado_790MY actual = _empleadoRepository.GetByDni(empleado.DNI)
                ?? throw MensajesBLL_790MY.Regla("msg_rf2_empleado_inexistente", empleado.DNI);

            // Un camarero con mesas abiertas no puede cambiar de puesto ni quedar inactivo:
            // esas mesas quedarían sin responsable para Registrar Pedido.
            bool dejaDeSerCamareroActivo = actual.Puesto == PuestoEmpleado_790MY.Camarero && actual.Activo
                && (empleado.Puesto != PuestoEmpleado_790MY.Camarero || !empleado.Activo);
            if (dejaDeSerCamareroActivo)
                ValidarSinMesasAbiertas(empleado.DNI);

            ValidarUsuarioAsociado(empleado);

            _empleadoRepository.Update(empleado);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.EmpleadoModificado, $"Empleado {empleado.DNI} ({empleado.Puesto})");
        }

        /// <summary>Baja lógica (Activo = 0). Se conserva el registro por las reservas históricas.</summary>
        public void DarDeBaja(int dni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearEmpleado);

            Empleado_790MY empleado = _empleadoRepository.GetByDni(dni)
                ?? throw MensajesBLL_790MY.Regla("msg_rf2_empleado_inexistente", dni);
            if (!empleado.Activo)
                throw MensajesBLL_790MY.Regla("msg_rf2_empleado_ya_inactivo", dni);

            ValidarSinMesasAbiertas(dni);

            empleado.Activo = false;
            _empleadoRepository.Update(empleado);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.EmpleadoDadoDeBaja, $"Empleado {dni}");
        }

        // Excepción: se lee IReservaRepository_790MY directo (solo lectura). Pasar por
        // ReservaBLL_790MY formaría un ciclo ReservaBLL -> EmpleadoBLL -> ReservaBLL y
        // BLLFactory_790MY entraría en recursión infinita.
        /// <summary>Mesas abiertas (reservas Asistidas) a cargo del empleado.</summary>
        public int ContarMesasAbiertas(int dni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearEmpleado);
            return _reservaRepository.GetAsistidasPorCamarero(dni).Count;
        }

        // ─── Validaciones ────────────────────────────────────────────────────

        // Excepción: chequeo de integridad de solo lectura antes de la baja; usa
        // IReservaRepository_790MY directo por el mismo motivo que ContarMesasAbiertas.
        private void ValidarSinMesasAbiertas(int dni)
        {
            int abiertas = _reservaRepository.GetAsistidasPorCamarero(dni).Count;
            if (abiertas > 0)
                throw MensajesBLL_790MY.Regla("msg_rf2_empleado_con_mesas_abiertas", abiertas);
        }

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
