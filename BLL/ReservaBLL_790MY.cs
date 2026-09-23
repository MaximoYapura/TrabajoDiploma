using BE_08YS;
using DAL_08YS;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;

namespace BLL_08YS
{
    public class ReservaBLL_790MY
    {
        private readonly IReservaRepository_790MY _reservaRepository;
        private readonly IClienteRepository_790MY _clienteRepository;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public ReservaBLL_790MY(IReservaRepository_790MY reservaRepository, IClienteRepository_790MY clienteRepository, BitacoraBLL_08YS bitacoraBll)
        {
            _reservaRepository = reservaRepository ?? throw new ArgumentNullException(nameof(reservaRepository));
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
            _bitacoraBll = bitacoraBll ?? throw new ArgumentNullException(nameof(bitacoraBll));
        }

        public void RegistrarReserva(int clienteDni, Mesa_790MY mesa, DateTime fecha, TimeSpan hora, int comensales)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarReserva);

            if (mesa == null)
                throw new ArgumentException("Debe seleccionar una mesa.");

            // GetByDni filtra por Activo = 1: un cliente dado de baja logicamente
            // no deberia poder hacer reservas.
            if (_clienteRepository.GetByDni(clienteDni) == null)
                throw new ArgumentException("El cliente indicado no está registrado o fue dado de baja.");

            if (comensales <= 0)
                throw new ArgumentException("La cantidad de comensales debe ser mayor a cero.");

            if (comensales > mesa.Capacidad)
                throw new ArgumentException($"La mesa {mesa.NroMesa} tiene capacidad para {mesa.Capacidad} comensales; no alcanza para {comensales}.");

            DateTime fechaHora = fecha.Date + hora;
            if (fechaHora <= DateTime.Now)
                throw new ArgumentException("La fecha y hora de la reserva deben ser posteriores al momento actual.");

            var reserva = new Reserva_790MY(clienteDni, mesa.NroMesa, fecha.Date, hora, comensales, EstadoReserva_790MY.Confirmada);

            _reservaRepository.Add(reserva);

            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(
                Evento.ReservaRegistrada,
                targetUsername: $"Reserva {reserva.ReservaID} - Cliente DNI {clienteDni}");
        }

        public List<Reserva_790MY> Buscar(DateTime? desde, DateTime? hasta, int? clienteDni, EstadoReserva_790MY? estado)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerReservas);
            return _reservaRepository.Buscar(desde, hasta, clienteDni, estado);
        }

        public void CancelarReserva(int reservaId)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CancelarReserva);

            _reservaRepository.CancelarReserva(reservaId);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.ReservaCancelada, targetUsername: reservaId.ToString());
        }
    }
}
