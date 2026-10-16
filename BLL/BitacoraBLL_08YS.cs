using DAL_08YS;
using DAL_08YS.Repositories_Interfaces;
using Service_08YS;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_08YS
{
    public class BitacoraBLL_08YS
    {
        private readonly IBitacoraRepository_08YS _repo;

        public BitacoraBLL_08YS(IBitacoraRepository_08YS repo) => _repo = repo;

        // username = null → toma el usuario de sesión (flujo normal post-username)
        // username = "username" → se pasa explicitamente (flujo de intento fallido, sin sesion aun)
        public void RegistrarEvento(Evento evento, string username = null, string targetUsername = null)
        {
            username = string.IsNullOrWhiteSpace(username)
                ? SessionManager_08YS.Instance.Current?.Username ?? "Desconocido"
                : username;

            var metadata = EventCatalog_08YS.GetMetadata(evento);

            // Bitacora.TargetUsername es nvarchar(50): se trunca para que un detalle
            // largo no haga fallar el INSERT (y deje la operacion sin auditar).
            if (targetUsername != null && targetUsername.Length > LargoMaximoTarget)
                targetUsername = targetUsername.Substring(0, LargoMaximoTarget);

            _repo.RegistrarEvento(new BitacoraEvento_08YS(
                username,
                DateTime.Now,
                metadata.Modulo,
                evento,
                metadata.Criticidad,
                targetUsername));   // null para eventos sin sujeto
        }

        private const int LargoMaximoTarget = 50;

        /// <summary>
        /// Registro de eventos de negocio que se invoca DESPUES de que la operacion ya
        /// fue confirmada en la base (COMMIT). Mismo criterio que DVManager_08YS.Recalcular:
        /// si la bitacora falla no se relanza, porque la GUI informaria un error sobre un
        /// dato que si se guardo y el usuario podria reintentar y duplicarlo. Se reintenta
        /// una vez y, si persiste, se deja traza de diagnostico.
        /// </summary>
        public void RegistrarEventoPostCommit(Evento evento, string targetUsername = null)
        {
            for (int intento = 1; intento <= 2; intento++)
            {
                try
                {
                    RegistrarEvento(evento, targetUsername: targetUsername);
                    return;
                }
                catch (Exception ex)
                {
                    if (intento == 2)
                        System.Diagnostics.Trace.TraceError(
                            $"No se pudo registrar en bitacora el evento {evento} ({targetUsername}): {ex.Message}");
                }
            }
        }

        public int ContarIntentosFallidos(string username, int ventanaHoras = 2)
            => _repo.ContarIntentosFallidos(username, ventanaHoras);

        public List<BitacoraEvento_08YS> GetAll()
            => _repo.GetAll();

        public List<BitacoraEvento_08YS> Filtrar(BitacoraFiltro_08YS filtro)
            => _repo.Filtrar(filtro);
    }
}
