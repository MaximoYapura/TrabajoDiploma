using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service_08YS.Entities.Bitacora;

namespace DAL_08YS.Repositories_Interfaces
{
    public interface IBitacoraRepository_08YS
    {
        void RegistrarEvento(BitacoraEvento_08YS evento);

        List<BitacoraEvento_08YS> GetAll();

        List<BitacoraEvento_08YS> Filtrar(BitacoraFiltro_08YS filtro);

        int ContarIntentosFallidos(string username, int ventanaHoras);
    }
}
