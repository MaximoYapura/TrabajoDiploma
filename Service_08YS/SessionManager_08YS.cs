using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Service_08YS
{
    public class SessionManager_08YS
    {
        private static readonly Lazy<SessionManager_08YS> lazy =
            new Lazy<SessionManager_08YS>(() => new SessionManager_08YS());

        public static SessionManager_08YS Instance { get { return lazy.Value; } }

        private SessionManager_08YS() { }

        public User_08YS Current { get; private set; }

        public bool IsLogged => !(Current is null);

        public void SetCurrentUser(User_08YS user)
        {
            if (IsLogged)
                throw new InvalidOperationException("Ya hay un usuario logueado. Cierre la sesión antes de iniciar otra.");

            Current = user ?? throw new ArgumentNullException(nameof(user));
        }

        public void CerrarSesion() => Current = null;

        public bool HasPermission(Permisos permiso)
        {
            if (Current?.Rol == null) return false;

            return Current.Rol.GetPermisos()
                .Any(p => p.Nombre == permiso.ToString());
        }

        public void ValidatePermission(Permisos permiso)
        {
            if (!HasPermission(permiso))
            {
                // Mensaje traducido con el servicio de idiomas existente. Si la clave
                // no estuviera cargada, GetTexto devuelve "[clave]"; en ese caso se
                // conserva el texto original para no mostrar un mensaje vacío.
                string plantilla = TraductorManager_08YS.Instance.GetTexto("msg_permiso_denegado");
                string mensaje = plantilla.StartsWith("[")
                    ? $"El usuario actual no tiene permiso para: {permiso}"
                    : string.Format(plantilla, permiso);
                throw new UnauthorizedAccessException(mensaje);
            }
        }

        public event Action SesionInvalidada;

        public void InvalidarSesion()
            => SesionInvalidada?.Invoke();
    }
}
