using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IMesaRepository_790MY
    {
        List<Mesa_790MY> GetAll();

        // Devuelve las mesas con capacidad suficiente para "comensales" y sin una
        // reserva Confirmada que se solape con el turno (fecha, hora) solicitado.
        // No filtra por Mesa.Estado: ese campo refleja el estado físico en tiempo real
        // (util para una pantalla de piso "ahora"), pero no predice la disponibilidad
        // de la mesa en una fecha/turno futuro, que depende exclusivamente de Reservas.
        List<Mesa_790MY> GetDisponibles(DateTime fecha, TimeSpan hora, int comensales);

        void UpdateEstado(int nroMesa, EstadoMesa_790MY estado);
    }
}
