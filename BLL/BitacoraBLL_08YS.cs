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

            _repo.RegistrarEvento(new BitacoraEvento_08YS(
                username,
                DateTime.Now,
                metadata.Modulo,
                evento,
                metadata.Criticidad,
                targetUsername));   // null para eventos sin sujeto
        }

        public int ContarIntentosFallidos(string username, int ventanaHoras = 2)
            => _repo.ContarIntentosFallidos(username, ventanaHoras);

        public List<BitacoraEvento_08YS> GetAll()
            => _repo.GetAll();

        public List<BitacoraEvento_08YS> Filtrar(BitacoraFiltro_08YS filtro)
            => _repo.Filtrar(filtro);
    }
}
