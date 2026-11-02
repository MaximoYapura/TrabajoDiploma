using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IMesaRepository_790MY
    {
        bool Exists(int nroMesa);
        List<Mesa_790MY> GetAll();
        void Add(Mesa_790MY mesa);
        /// <summary>Datos del maestro (Capacidad y Activo). El estado operativo no se escribe acá.</summary>
        void Update(Mesa_790MY mesa);

        /// <summary>
        /// Cambia el estado solo si la mesa sigue en <paramref name="estadoEsperado"/>.
        /// False (sin cambios) si otro módulo la cambió en el medio.
        /// </summary>
        bool UpdateEstado(int nroMesa, EstadoMesa_790MY estadoEsperado, EstadoMesa_790MY estado);
        ISet<int> GetNumerosOcupadosEnTurno(DateTime fecha, TimeSpan hora);
    }
}
