using BE_08YS;
using DAL_08YS;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace BLL_08YS
{
    // Reglas de las mesas: el maestro de mesas, los turnos y el plano del salón.
    public class MesaBLL_790MY
    {
        // Turnos de 2 horas entre las 18:00 y las 24:00 (horas de inicio).
        private static readonly TimeSpan[] TurnosValidos =
        {
            new TimeSpan(18, 0, 0),
            new TimeSpan(20, 0, 0),
            new TimeSpan(22, 0, 0)
        };

        public static readonly TimeSpan DuracionTurno = TimeSpan.FromHours(2);

        // Horas de inicio de los turnos de atención.
        public static IReadOnlyList<TimeSpan> Turnos => TurnosValidos;

        // Turno en curso en ese momento, o null si está fuera de horario.
        // En modo demostración, fuera de horario devuelve el turno de prueba.
        public static TimeSpan? ObtenerTurnoVigente(DateTime momento)
        {
            TimeSpan? real = ObtenerTurnoReal(momento);
            return real ?? TurnoDePrueba;
        }

        // True si el turno vigente sale del modo demostración.
        public static bool EsTurnoDePrueba(DateTime momento)
            => !ObtenerTurnoReal(momento).HasValue && TurnoDePrueba.HasValue;

        // Turno real según la hora, sin tener en cuenta el modo demostración.
        private static TimeSpan? ObtenerTurnoReal(DateTime momento)
        {
            TimeSpan hora = momento.TimeOfDay;
            foreach (var inicio in TurnosValidos)
                if (hora >= inicio && hora < inicio + DuracionTurno)
                    return inicio;
            return null;
        }

        // ─── Modo demostración ───────────────────────────────────────────────
        // Se configura en el App.config:
        //   <add key="ModoDemostracion"  value="true" />
        //   <add key="TurnoPrueba_790MY" value="22:00" />   (18:00, 20:00 o 22:00)
        // Con ModoDemostracion en false (o sin la clave) solo valen los turnos reales.
        private static readonly Lazy<TimeSpan?> _turnoDePrueba = new Lazy<TimeSpan?>(LeerTurnoDePrueba);

        private static TimeSpan? TurnoDePrueba => _turnoDePrueba.Value;

        private static TimeSpan? LeerTurnoDePrueba()
        {
            try
            {
                if (!bool.TryParse(ConfigurationManager.AppSettings["ModoDemostracion"], out bool activo) || !activo)
                    return null;

                // Si el valor falta o no es un turno, se usa el último del día.
                return TimeSpan.TryParse(ConfigurationManager.AppSettings["TurnoPrueba_790MY"], out TimeSpan turno)
                       && TurnosValidos.Contains(turno)
                    ? turno
                    : TurnosValidos[TurnosValidos.Length - 1];
            }
            catch (ConfigurationErrorsException)
            {
                return null;
            }
        }

        // Igual que ObtenerTurnoVigente(DateTime.Now), pero si no hay turno lanza el mensaje
        // de fuera de turno. Lo usa el walk-in para abrir el plano.
        public TimeSpan ObtenerTurnoVigenteObligatorio()
        {
            TimeSpan? turno = ObtenerTurnoVigente(DateTime.Now);
            if (!turno.HasValue)
                throw MensajesBLL_790MY.Regla("msg_rf2_fuera_de_turno");
            return turno.Value;
        }

        private readonly IMesaRepository_790MY    _mesaRepository;
        private readonly IReservaRepository_790MY  _reservaRepository;
        private readonly BitacoraBLL_08YS          _bitacoraBll;

        public MesaBLL_790MY(IMesaRepository_790MY mesaRepository,
                             IReservaRepository_790MY reservaRepository,
                             BitacoraBLL_08YS bitacoraBll)
        {
            _mesaRepository    = mesaRepository    ?? throw new ArgumentNullException(nameof(mesaRepository));
            _reservaRepository = reservaRepository ?? throw new ArgumentNullException(nameof(reservaRepository));
            _bitacoraBll       = bitacoraBll       ?? throw new ArgumentNullException(nameof(bitacoraBll));
        }

        // Todas las mesas (maestro de mesas).
        public List<Mesa_790MY> GetAll()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerMesas);
            return _mesaRepository.GetAll();
        }

        // Mesa con ese número, o null. No valida permiso porque lo usa ReservaBLL_790MY al
        // abrir la mesa, que ya validó AbrirMesa (Recepción no tiene VerMesas).
        internal Mesa_790MY ObtenerPorNumero(int nroMesa)
            => _mesaRepository.GetAll().FirstOrDefault(m => m.NroMesa == nroMesa);

        // Número para la próxima mesa: el más alto más uno, o 1 si no hay mesas.
        public int GetProximoNumero()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerMesas);
            var mesas = _mesaRepository.GetAll();
            int max = mesas.Count > 0 ? mesas.Max(m => m.NroMesa) : 0;
            return max + 1;
        }

        // Las mesas con su estado para el plano del salón, según el turno y los comensales.
        public List<MesaMapaDTO_790MY> ObtenerMesasMapa(DateTime fecha, TimeSpan hora, int comensales)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerMesas);
            return ObtenerMapa(fecha, hora, comensales);
        }

        // Lo mismo que ObtenerMesasMapa pero sin validar permiso. Lo usa el walk-in de
        // ReservaBLL_790MY, que ya validó AbrirMesa.
        internal List<MesaMapaDTO_790MY> ObtenerMapa(DateTime fecha, TimeSpan hora, int comensales)
        {
            if (fecha.Date < DateTime.Today)
                throw MensajesBLL_790MY.Regla("msg_rf2_fecha_anterior_hoy");

            if (comensales <= 0)
                throw MensajesBLL_790MY.Regla("msg_rf2_comensales_invalidos");

            if (!TurnosValidos.Contains(hora))
                throw MensajesBLL_790MY.Regla("msg_rf2_turno_invalido");

            // Las mesas fuera de servicio no aparecen en el plano.
            var todasLasMesas = _mesaRepository.GetAll()
                .Where(m => m.Activo && m.Estado != EstadoMesa_790MY.FueraDeServicio)
                .ToList();

            // Mesas con una reserva Confirmada o Asistida en esa fecha y hora.
            var numerosOcupados = new HashSet<int>(_mesaRepository.GetNumerosOcupadosEnTurno(fecha.Date, hora));

            // Si es el turno en curso, también cuenta la mesa ocupada ahora: puede seguir
            // ahí un grupo del turno anterior. Para turnos futuros esto no importa.
            bool esTurnoEnCurso = fecha.Date == DateTime.Today && ObtenerTurnoVigente(DateTime.Now) == hora;
            if (esTurnoEnCurso)
                foreach (var mesa in todasLasMesas.Where(m => m.Estado == EstadoMesa_790MY.Ocupada))
                    numerosOcupados.Add(mesa.NroMesa);

            // La recomendada es la mesa libre de menor capacidad que alcanza.
            int minCapacidadLibre = todasLasMesas
                .Where(m => m.Capacidad >= comensales && !numerosOcupados.Contains(m.NroMesa))
                .Select(m => m.Capacidad)
                .DefaultIfEmpty(int.MaxValue)
                .Min();

            var resultado = new List<MesaMapaDTO_790MY>(todasLasMesas.Count);

            foreach (var mesa in todasLasMesas)
            {
                EstadoMapa_790MY estadoMapa;

                if (mesa.Capacidad < comensales)
                {
                    estadoMapa = EstadoMapa_790MY.Incompatible;
                }
                else if (numerosOcupados.Contains(mesa.NroMesa))
                {
                    estadoMapa = EstadoMapa_790MY.Ocupada;
                }
                else if (mesa.Capacidad == minCapacidadLibre)
                {
                    estadoMapa = EstadoMapa_790MY.DisponibleRecomendada;
                }
                else
                {
                    estadoMapa = EstadoMapa_790MY.Disponible;
                }

                resultado.Add(new MesaMapaDTO_790MY(mesa, estadoMapa));
            }

            return resultado.OrderBy(dto => dto.Mesa.NroMesa).ToList();
        }

        // Alta de mesa. Hay un máximo de 12 mesas y el número no se puede repetir.
        public void RegistrarMesa(int numero, int capacidad)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearMesa);

            ValidarDatos(numero, capacidad);

            var todasLasMesas = _mesaRepository.GetAll();

            if (todasLasMesas.Count >= 12)
                throw MensajesBLL_790MY.Regla("msg_limite_mesas_alcanzado");

            if (_mesaRepository.Exists(numero))
                throw MensajesBLL_790MY.Regla("msg_fm_mesa_duplicada", numero);

            var mesa = new Mesa_790MY(numero, capacidad, EstadoMesa_790MY.Libre);

            _mesaRepository.Add(mesa);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.MesaRegistrada, numero.ToString());
        }

        // Modifica la capacidad y, si corresponde, la pasa a Fuera de servicio o la devuelve a Libre.
        public void ModificarMesa(Mesa_790MY mesa)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearMesa);

            ValidarDatos(mesa.NroMesa, mesa.Capacidad);

            Mesa_790MY actual = ObtenerPorNumero(mesa.NroMesa);
            if (actual == null)
                throw MensajesBLL_790MY.Regla("msg_fm_mesa_inexistente_modificar", mesa.NroMesa);

            // Desde el maestro solo se pasa a FueraDeServicio o se vuelve a Libre. Ocupada y
            // Reservada las manejan Reservas y Atención en mesa.
            EstadoMesa_790MY? nuevoEstado = null;
            if (mesa.Estado == EstadoMesa_790MY.FueraDeServicio && actual.Estado != EstadoMesa_790MY.FueraDeServicio)
                nuevoEstado = EstadoMesa_790MY.FueraDeServicio;
            else if (mesa.Estado != EstadoMesa_790MY.FueraDeServicio && actual.Estado == EstadoMesa_790MY.FueraDeServicio)
                nuevoEstado = EstadoMesa_790MY.Libre;

            // Lee IReservaRepository_790MY directo, por lo mismo que EliminarMesa.
            if (nuevoEstado == EstadoMesa_790MY.FueraDeServicio
                && (actual.Estado == EstadoMesa_790MY.Ocupada
                    || _reservaRepository.Buscar(null, null, null, EstadoReserva_790MY.Asistida)
                                         .Any(r => r.MesaNumero == mesa.NroMesa)))
                throw MensajesBLL_790MY.Regla("msg_fm_mesa_ocupada_fuera_servicio", mesa.NroMesa);

            int comensalesReservados = _reservaRepository
                .Buscar(DateTime.Today, null, null, EstadoReserva_790MY.Confirmada)
                .Where(r => r.MesaNumero == mesa.NroMesa)
                .Select(r => r.CantidadComensales)
                .DefaultIfEmpty(0)
                .Max();
            if (mesa.Capacidad < comensalesReservados)
                throw MensajesBLL_790MY.Regla("msg_fm_capacidad_bajo_reservas", mesa.NroMesa, comensalesReservados);

            if (nuevoEstado.HasValue && !_mesaRepository.UpdateEstado(mesa.NroMesa, actual.Estado, nuevoEstado.Value))
                throw MensajesBLL_790MY.Regla("msg_fm_mesa_cambio_estado", mesa.NroMesa);

            _mesaRepository.Update(mesa);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.MesaModificada, mesa.NroMesa.ToString());
        }

        // Baja de mesa. Si alguna vez tuvo reservas no se puede; si no, queda Fuera de servicio.
        public void EliminarMesa(int numero)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearMesa);

            if (!_mesaRepository.Exists(numero))
                throw MensajesBLL_790MY.Regla("msg_fm_mesa_inexistente_baja", numero);

            // Se buscan reservas de esta mesa de cualquier fecha y estado. Se lee
            // IReservaRepository_790MY directo porque pasar por ReservaBLL_790MY armaría un
            // ciclo (ReservaBLL ya depende de MesaBLL) y la fábrica no terminaría nunca.
            var todasLasReservas = _reservaRepository.Buscar(null, null, null, null);
            bool tieneHistorial  = todasLasReservas != null &&
                                   todasLasReservas.Any(r => r.MesaNumero == numero);

            if (tieneHistorial)
                throw MensajesBLL_790MY.Regla("msg_error_eliminar_mesa_con_reservas");

            // No se borra la fila: la mesa queda Fuera de servicio.
            Mesa_790MY actual = ObtenerPorNumero(numero);
            if (!_mesaRepository.UpdateEstado(numero, actual.Estado, EstadoMesa_790MY.FueraDeServicio))
                throw MensajesBLL_790MY.Regla("msg_fm_mesa_cambio_estado", numero);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.MesaEliminada, numero.ToString());
        }

        // Número mayor a cero y capacidad de 1 a 8.
        private static void ValidarDatos(int numero, int capacidad)
        {
            if (numero <= 0)
                throw MensajesBLL_790MY.Regla("msg_fm_numero_invalido");

            if (capacidad < 1 || capacidad > 8)
                throw MensajesBLL_790MY.Regla("msg_fm_capacidad_invalida", 1, 8);
        }
    }
}
