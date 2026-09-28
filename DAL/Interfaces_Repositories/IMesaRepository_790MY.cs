using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IMesaRepository_790MY
    {
        bool Exists(int nroMesa);
        List<Mesa_790MY> GetAll();
        List<Mesa_790MY> GetDisponibles(DateTime fecha, TimeSpan hora, int comensales);
        void Add(Mesa_790MY mesa);
        void Update(Mesa_790MY mesa);
        void Delete(int nroMesa);
        bool TieneReservasActivas(int nroMesa);
        void UpdateEstado(int nroMesa, EstadoMesa_790MY estado);
        ISet<int> GetNumerosOcupadosEnTurno(DateTime fecha, TimeSpan hora);
    }
}
