using DAL_08YS;
using DAL_08YS.Interfaces_Repositories;
using DAL_08YS.Repositories_Interfaces;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace BLL_08YS
{
    public class UserDniDuplicadoException_08YS     : Exception { }
    public class UserAutoModificacionException_08YS : Exception { }
    public class UserAutoEstadoException_08YS       : Exception { }
    public class PwdActualIncorrectaException_08YS  : Exception { }
    public class UserNotFoundException_08YS         : Exception { }
    public class LogoutPersistenceException_08YS    : Exception { }

    public class UserBLL_08YS
    {
        private readonly IUserRepository_08YS _userRepository;
        private readonly IRolRepository_08YS _rolRepo;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public UserBLL_08YS(IUserRepository_08YS userRepository, IRolRepository_08YS rolRepo, BitacoraBLL_08YS bitacoraBll)
        {
            this._userRepository = userRepository;
            _rolRepo = rolRepo;
            _bitacoraBll = bitacoraBll;
        }

        #region Login

        public User_08YS ValidarLogin(string username, string password)
        {
            if (SessionManager_08YS.Instance.IsLogged)
                throw new InvalidOperationException();

            var user = _userRepository.GetByUsername(username) ?? throw new UserNoRegistradoException_08YS();

            if (user.Bloqueado) throw new UserBloqueadoException_08YS();
            if (!user.Activo) throw new UserDesactivadoException_08YS();

            if (!Encriptador_08YS.Verificar(password, user.Hash, user.Salt))
                RegistrarIntentoFallido(username);

            return user;
        }

        public User_08YS Login(User_08YS user, out bool passwordDefault)
        {
            user.Rol = _rolRepo.GetById(user.Rol.RolID);
            SessionManager_08YS.Instance.SetCurrentUser(user);

            _bitacoraBll.RegistrarEvento(Evento.LoginExitoso);
            passwordDefault = Encriptador_08YS.Verificar(user.DNI.ToString() + user.Apellido, user.Hash, user.Salt);

            return user;
        }

        private void RegistrarIntentoFallido(string username)
        {
            _bitacoraBll.RegistrarEvento(Evento.LoginFallido, username: username);

            int intentos = _bitacoraBll.ContarIntentosFallidos(username, ventanaHoras: 2);

            if (intentos >= 3)
                UserLockOut(username);

            throw new AuthenticationException();
        }

        public bool ManejaInconsistencias(User_08YS user) =>
            _rolRepo.GetById(user.Rol.RolID).GetPermisos().Any(p => p.Nombre == Permisos.ManejarInconsistencias.ToString() );

        #endregion

        public void UserLockOut(string username)
        {
            var user = _userRepository.GetByUsername(username) ?? throw new UserNotFoundException_08YS();

            _userRepository.LockOut(user.Username);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.UsuarioBloqueado, username: username);

            throw new UserBloqueadoException_08YS();
        }

        #region GestionUsuario

        public void DesbloquearUsuario(string username)
        {
            var user = _userRepository.GetByUsername(username) ?? throw new UserNotFoundException_08YS();

            SessionManager_08YS.Instance.ValidatePermission(Permisos.DesbloquearUsuario);

            _userRepository.Unlock(user.Username);

            string passwordDefault = user.DNI.ToString() + user.Apellido;
            Encriptador_08YS.CrearHash(passwordDefault, out string nuevoHash, out string nuevoSalt);

            _userRepository.UpdatePassword(user.Username, nuevoHash, nuevoSalt);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.UsuarioDesbloqueado, targetUsername: username);
        }

        public void CrearUsuario(int dni, string nombre, string apellido, string email, Rol_08YS rol)
        {
            if (_userRepository.Exists(dni))
                throw new UserDniDuplicadoException_08YS();

            string username = dni.ToString() + nombre;
            string passwordDefault = dni.ToString() + apellido;
            Encriptador_08YS.CrearHash(passwordDefault, out string hash, out string salt);

            User_08YS nuevo = new User_08YS(username, dni, rol, nombre, apellido, email, hash, salt, false, true);

            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearUsuario);

            _userRepository.Create(nuevo);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.UsuarioCreado, targetUsername: username);
        }

        public void ModificarUsuario(string username, string nuevoEmail, Rol_08YS nuevoRol)
        {
            if (username == SessionManager_08YS.Instance.Current.Username)
                throw new UserAutoModificacionException_08YS();

            var user = _userRepository.GetByUsername(username) ?? throw new KeyNotFoundException();

            var cambioRol = user.Rol != nuevoRol;
            var cambioEmail = user.Email != nuevoEmail;
            user.Email = nuevoEmail;
            user.Rol = nuevoRol;

            SessionManager_08YS.Instance.ValidatePermission(Permisos.ModificarUsuario);

            _userRepository.Modify(user, user.Username);
            DVManager_08YS.Recalcular();
            if(cambioEmail) _bitacoraBll.RegistrarEvento(Evento.UsuarioEmailModificado, targetUsername: username);
            if(cambioRol) _bitacoraBll.RegistrarEvento(Evento.UsuarioRolModificado, targetUsername: username);
        }

        public void AlternarEstado(string username)
        {
            if (username == SessionManager_08YS.Instance.Current.Username)
                throw new UserAutoEstadoException_08YS();

            var user = _userRepository.GetByUsername(username) ?? throw new KeyNotFoundException();

            SessionManager_08YS.Instance.ValidatePermission(Permisos.DesActivarUsuario);
            bool nuevoEstado = !user.Activo;

            _userRepository.UpdateState(username, nuevoEstado);
            DVManager_08YS.Recalcular();

            user.Activo = nuevoEstado;
            Evento accion = user.Activo ? Evento.UsuarioHabilitado : Evento.UsuarioDeshabilitado;
            _bitacoraBll.RegistrarEvento(accion, targetUsername: username);
        }

        #endregion

        public void CambiarContraseña(string passwordActual, string passwordNueva)
        {
            if (!Encriptador_08YS.Verificar(passwordActual, SessionManager_08YS.Instance.Current.Hash, SessionManager_08YS.Instance.Current.Salt))
                throw new PwdActualIncorrectaException_08YS();

            Encriptador_08YS.CrearHash(passwordNueva, out string hashNuevo, out string saltNuevo);

            _userRepository.UpdatePassword(SessionManager_08YS.Instance.Current.Username, hashNuevo, saltNuevo);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.CambioContraseña);
        }

        public void CambiarIdiomaUsuario(string nuevoIdioma)
        {
            if (SessionManager_08YS.Instance.Current == null) return;

            // 1. Modificamos ÚNICAMENTE el objeto local en memoria de la sesión actual
            SessionManager_08YS.Instance.Current.Idioma = nuevoIdioma;
        }

        public void Logout()
        {
            // Verificamos que efectivamente haya una sesión activa
            if (SessionManager_08YS.Instance.IsLogged)
            {
                string username = SessionManager_08YS.Instance.Current.Username;
                string idiomaFinal = SessionManager_08YS.Instance.Current.Idioma;

                try
                {
                    _userRepository.UpdateLanguage(username, idiomaFinal);
                    DVManager_08YS.Recalcular();
                }
                catch (Exception)
                {
                    // Lanzamos una excepción tipada interna en lugar de texto plano
                    throw new LogoutPersistenceException_08YS();
                }
            }

            SessionManager_08YS.Instance.CerrarSesion();
        }

        public List<User_08YS> GetAll() => _userRepository.GetAll();
    }
}
