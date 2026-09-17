using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using System;

namespace BLL_08YS
{
    public class ReservaBLL_790MY
    {
        private readonly IReservaRepository_790MY _reservaRepository;
        private readonly IClienteRepository_790MY _clienteRepository;

        public ReservaBLL_790MY(IReservaRepository_790MY reservaRepository, IClienteRepository_790MY clienteRepository)
        {
            _reservaRepository = reservaRepository ?? throw new ArgumentNullException(nameof(reservaRepository));
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
        }

        public void RegistrarReserva(int clienteDni, Mesa_790MY mesa, DateTime fecha, TimeSpan hora, int comensales)
        {
            if (mesa == null)
                throw new ArgumentException("Debe seleccionar una mesa.");

            if (!_clienteRepository.Exists(clienteDni))
                throw new ArgumentException("El cliente indicado no está registrado.");

            if (comensales <= 0)
                throw new ArgumentException("La cantidad de comensales debe ser mayor a cero.");

            if (comensales > mesa.Capacidad)
                throw new ArgumentException($"La mesa {mesa.NroMesa} tiene capacidad para {mesa.Capacidad} comensales; no alcanza para {comensales}.");

            DateTime fechaHora = fecha.Date + hora;
            if (fechaHora <= DateTime.Now)
                throw new ArgumentException("La fecha y hora de la reserva deben ser posteriores al momento actual.");

            var reserva = new Reserva_790MY(clienteDni, mesa.NroMesa, fecha.Date, hora, comensales, EstadoReserva_790MY.Confirmada);

            _reservaRepository.Add(reserva);
        }
    }
}
