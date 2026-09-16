using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL_08YS
{
    public class MesaBLL_790MY
    {
        // Turnos fijos de 2 horas entre las 18:00 y las 24:00 (horas de inicio válidas).
        private static readonly TimeSpan[] TurnosValidos =
        {
            new TimeSpan(18, 0, 0),
            new TimeSpan(20, 0, 0),
            new TimeSpan(22, 0, 0)
        };

        private readonly IMesaRepository_790MY _mesaRepository;

        public MesaBLL_790MY(IMesaRepository_790MY mesaRepository)
        {
            _mesaRepository = mesaRepository ?? throw new ArgumentNullException(nameof(mesaRepository));
        }

        public List<Mesa_790MY> GetAll() => _mesaRepository.GetAll();

        public List<Mesa_790MY> BuscarDisponibles(DateTime fecha, TimeSpan hora, int comensales)
        {
            if (fecha.Date < DateTime.Today)
                throw new ArgumentException("La fecha de la reserva no puede ser anterior a hoy.");

            if (comensales <= 0)
                throw new ArgumentException("La cantidad de comensales debe ser mayor a cero.");

            if (!TurnosValidos.Contains(hora))
                throw new ArgumentException("El turno seleccionado no es válido.");

            return _mesaRepository.GetDisponibles(fecha.Date, hora, comensales);
        }

        public void ActualizarEstado(int nroMesa, EstadoMesa_790MY estado)
        {
            _mesaRepository.UpdateEstado(nroMesa, estado);
        }
    }
}
