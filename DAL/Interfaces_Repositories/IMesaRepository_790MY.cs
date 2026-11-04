using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    // Acceso a la tabla Mesas.
    public interface IMesaRepository_790MY
    {
        bool Exists(int nroMesa);
        List<Mesa_790MY> GetAll();
        void Add(Mesa_790MY mesa);
        // Guarda capacidad y Activo. El estado se cambia con UpdateEstado.
        void Update(Mesa_790MY mesa);

        // Cambia el estado solo si la mesa sigue en estadoEsperado. Devuelve false si
        // otra pantalla la cambió en el medio.
        bool UpdateEstado(int nroMesa, EstadoMesa_790MY estadoEsperado, EstadoMesa_790MY estado);

        // Números de las mesas que tienen una reserva que las ocupa en ese turno.
        ISet<int> GetNumerosOcupadosEnTurno(DateTime fecha, TimeSpan hora);
    }
}
