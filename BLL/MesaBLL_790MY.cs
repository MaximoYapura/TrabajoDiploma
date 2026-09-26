using BE_08YS;
using DAL_08YS;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
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
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public MesaBLL_790MY(IMesaRepository_790MY mesaRepository, BitacoraBLL_08YS bitacoraBll)
        {
            _mesaRepository = mesaRepository ?? throw new ArgumentNullException(nameof(mesaRepository));
            _bitacoraBll = bitacoraBll ?? throw new ArgumentNullException(nameof(bitacoraBll));
        }

        public List<Mesa_790MY> GetAll()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerMesas);
            return _mesaRepository.GetAll();
        }

        public List<Mesa_790MY> BuscarDisponibles(DateTime fecha, TimeSpan hora, int comensales)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerMesas);

            if (fecha.Date < DateTime.Today)
                throw new ArgumentException("La fecha de la reserva no puede ser anterior a hoy.");

            if (comensales <= 0)
                throw new ArgumentException("La cantidad de comensales debe ser mayor a cero.");

            if (!TurnosValidos.Contains(hora))
                throw new ArgumentException("El turno seleccionado no es válido.");

            return _mesaRepository.GetDisponibles(fecha.Date, hora, comensales);
        }

        public void RegistrarMesa(int numero, int capacidad)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearMesa);

            ValidarDatos(numero, capacidad);

            if (_mesaRepository.Exists(numero))
                throw new ArgumentException($"Ya existe una mesa con el número {numero}.");

            var mesa = new Mesa_790MY(numero, capacidad, EstadoMesa_790MY.Libre);

            _mesaRepository.Add(mesa);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.MesaRegistrada, targetUsername: numero.ToString());
        }

        public void ModificarMesa(Mesa_790MY mesa)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearMesa);

            ValidarDatos(mesa.NroMesa, mesa.Capacidad);

            if (!_mesaRepository.Exists(mesa.NroMesa))
                throw new ArgumentException("La mesa que intenta modificar no existe.");

            _mesaRepository.Update(mesa);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.MesaModificada, targetUsername: mesa.NroMesa.ToString());
        }

        public void EliminarMesa(int numero)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearMesa);

            if (!_mesaRepository.Exists(numero))
                throw new ArgumentException("La mesa que intenta eliminar no existe.");

            _mesaRepository.DeleteLogico(numero);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.MesaEliminada, targetUsername: numero.ToString());
        }

        public void ActualizarEstado(int nroMesa, EstadoMesa_790MY estado)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearMesa);
            _mesaRepository.UpdateEstado(nroMesa, estado);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.MesaEstadoActualizado, targetUsername: $"Mesa {nroMesa} - {estado}");
        }

        private static void ValidarDatos(int numero, int capacidad)
        {
            if (numero <= 0)
                throw new ArgumentException("El número de mesa debe ser mayor a cero.");

            if (capacidad <= 0)
                throw new ArgumentException("La capacidad debe ser mayor a cero.");
        }
    }
}
