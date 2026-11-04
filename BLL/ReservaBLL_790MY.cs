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
    // Reglas de las reservas (RF1) y de la apertura de mesas (RF2: con reserva y walk-in).
    // El comprobante en PDF y el mail están en ReservaBLL_790MY.Comprobante.cs.
    //
    // Cada operación que modifica datos sigue el mismo orden que el resto de las BLL:
    //   1) valida el permiso (aunque la pantalla esconda el botón);
    //   2) valida las reglas, con mensajes traducidos (MensajesBLL_790MY);
    //   3) graba en el repositorio, en una sola transacción;
    //   4) recalcula los dígitos verificadores;
    //   5) registra el evento en la bitácora, ya con los datos guardados.
    public partial class ReservaBLL_790MY
    {
        private readonly IReservaRepository_790MY _reservaRepository;
        private readonly ClienteBLL_790MY _clienteBll;
        private readonly MesaBLL_790MY _mesaBll;
        private readonly EmpleadoBLL_790MY _empleadoBll;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public ReservaBLL_790MY(IReservaRepository_790MY reservaRepository, ClienteBLL_790MY clienteBll,
                                MesaBLL_790MY mesaBll, EmpleadoBLL_790MY empleadoBll,
                                BitacoraBLL_08YS bitacoraBll)
        {
            _reservaRepository = reservaRepository ?? throw new ArgumentNullException(nameof(reservaRepository));
            _clienteBll = clienteBll ?? throw new ArgumentNullException(nameof(clienteBll));
            _mesaBll = mesaBll ?? throw new ArgumentNullException(nameof(mesaBll));
            _empleadoBll = empleadoBll ?? throw new ArgumentNullException(nameof(empleadoBll));
            _bitacoraBll = bitacoraBll ?? throw new ArgumentNullException(nameof(bitacoraBll));
        }

        // ═══════════════════════════════════════════════════════════════════
        //  RF1: registrar reserva
        // ═══════════════════════════════════════════════════════════════════

        // Registra la reserva y la devuelve con su número, para que la pantalla muestre el comprobante.
        public Reserva_790MY RegistrarReserva(int clienteDni, Mesa_790MY mesa, DateTime fecha, TimeSpan hora, int comensales)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarReserva);

            if (mesa == null)
                throw MensajesBLL_790MY.Regla("msg_rf2_falta_mesa");

            // Un cliente dado de baja no puede reservar.
            if (!_clienteBll.ExisteActivo(clienteDni))
                throw MensajesBLL_790MY.Regla("msg_rf2_cliente_inexistente");

            ValidarComensales(mesa, comensales);

            // Solo se reserva al inicio de un turno de atención (18, 20 o 22 h).
            if (!MesaBLL_790MY.Turnos.Contains(hora))
                throw MensajesBLL_790MY.Regla("msg_rf2_turno_invalido");

            DateTime fechaHora = fecha.Date + hora;
            if (fechaHora <= DateTime.Now)
                throw MensajesBLL_790MY.Regla("msg_rf2_reserva_en_pasado");

            var reserva = new Reserva_790MY(clienteDni, mesa.NroMesa, fecha.Date, hora, comensales, EstadoReserva_790MY.Confirmada);

            // El repositorio vuelve a controlar que la mesa esté libre y graba en la misma
            // transacción, así dos recepciones no pueden reservar la misma mesa y turno.
            if (!_reservaRepository.AddSiMesaDisponible(reserva, ocuparMesaFisica: false))
            {
                _bitacoraBll.RegistrarEventoPostCommit(Evento.ReservaRechazadaDisponibilidad,
                    $"Mesa {mesa.NroMesa} {fecha:dd/MM} {hora:hh\\:mm}");
                throw MensajesBLL_790MY.Regla("msg_rf2_mesa_no_disponible", mesa.NroMesa);
            }

            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(
                Evento.ReservaRegistrada,
                $"Reserva {reserva.ReservaID} - Cliente DNI {clienteDni}");

            return reserva;
        }

        // Búsqueda del reporte de reservas. Todos los filtros son opcionales.
        public List<Reserva_790MY> Buscar(DateTime? desde, DateTime? hasta, int? clienteDni, EstadoReserva_790MY? estado)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerReservas);

            if (desde.HasValue && hasta.HasValue && desde.Value.Date > hasta.Value.Date)
                throw MensajesBLL_790MY.Regla("msg_rep_rango_fechas_invalido", desde.Value, hasta.Value);

            return _reservaRepository.Buscar(desde, hasta, clienteDni, estado);
        }

        // ═══════════════════════════════════════════════════════════════════
        //  RF2: abrir mesa (CUN-004) y walk-in (CUN-005)
        // ═══════════════════════════════════════════════════════════════════

        // Reservas Confirmadas de hoy para el turno en curso, filtrables por número o DNI.
        // Fuera de horario devuelve la lista vacía.
        public List<Reserva_790MY> ObtenerReservasParaApertura(int? nroReserva = null, int? clienteDni = null)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AbrirMesa);

            TimeSpan? turno = MesaBLL_790MY.ObtenerTurnoVigente(DateTime.Now);
            if (!turno.HasValue)
                return new List<Reserva_790MY>();

            return _reservaRepository
                .Buscar(DateTime.Today, DateTime.Today, clienteDni, EstadoReserva_790MY.Confirmada)
                .Where(r => r.Hora == turno.Value)
                .Where(r => !nroReserva.HasValue || r.ReservaID == nroReserva.Value)
                .OrderBy(r => r.MesaNumero)
                .ToList();
        }

        // Cuántas mesas abiertas tiene cada camarero, para mostrarlo al elegir el responsable.
        public Dictionary<int, int> ContarMesasACargo(IEnumerable<int> camarerosDni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AbrirMesa);

            var resultado = new Dictionary<int, int>();
            foreach (int dni in (camarerosDni ?? Enumerable.Empty<int>()).Distinct())
                resultado[dni] = _reservaRepository.GetAsistidasPorCamarero(dni).Count;
            return resultado;
        }

        // El cliente llega con reserva: la reserva pasa a Asistida con el camarero y la
        // mesa queda Ocupada, todo en una transacción.
        public Reserva_790MY AbrirMesa(int reservaId, int camareroDni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AbrirMesa);

            Reserva_790MY reserva = _reservaRepository.GetById(reservaId)
                ?? throw MensajesBLL_790MY.Regla("msg_rf2_reserva_inexistente", reservaId);

            switch (reserva.Estado)
            {
                case EstadoReserva_790MY.Confirmada:
                    break;
                case EstadoReserva_790MY.Asistida:
                    throw MensajesBLL_790MY.Regla("msg_rf2_reserva_ya_asistida", reservaId);
                default:
                    throw MensajesBLL_790MY.Regla("msg_rf2_reserva_no_abrible", reservaId,
                        MensajesBLL_790MY.Texto("estado_reserva_" + reserva.Estado));
            }

            TimeSpan? turno = MesaBLL_790MY.ObtenerTurnoVigente(DateTime.Now);
            if (reserva.Fecha.Date != DateTime.Today || !turno.HasValue || reserva.Hora != turno.Value)
                throw MensajesBLL_790MY.Regla("msg_rf2_reserva_fuera_de_turno", reservaId);

            Empleado_790MY camarero = ValidarCamarero(camareroDni);

            Mesa_790MY mesa = _mesaBll.ObtenerPorNumero(reserva.MesaNumero);
            if (mesa == null || !mesa.Activo || mesa.Estado == EstadoMesa_790MY.FueraDeServicio)
                throw MensajesBLL_790MY.Regla("msg_rf2_mesa_fuera_servicio", reserva.MesaNumero);
            if (mesa.Estado == EstadoMesa_790MY.Ocupada)
                throw MensajesBLL_790MY.Regla("msg_rf2_mesa_fisicamente_ocupada", reserva.MesaNumero);

            // Si otro puesto abrió la reserva u ocupó la mesa mientras tanto, la base no cambia nada.
            if (!_reservaRepository.AbrirReserva(reservaId, camarero.DNI))
                throw MensajesBLL_790MY.Regla("msg_rf2_apertura_concurrente", reserva.MesaNumero);

            reserva.Estado = EstadoReserva_790MY.Asistida;
            reserva.CamareroDNI = camarero.DNI;

            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.ReservaAsistida, $"Reserva {reserva.ReservaID}");
            _bitacoraBll.RegistrarEventoPostCommit(Evento.MesaAbierta, $"Mesa {reserva.MesaNumero} - Reserva {reserva.ReservaID}");
            _bitacoraBll.RegistrarEventoPostCommit(Evento.CamareroAsignado, $"Cam {camarero.DNI} - Mesa {reserva.MesaNumero}");

            return reserva;
        }

        // ─── Consultas del walk-in ───────────────────────────────────────────
        // Recepción no tiene VerClientes ni VerMesas: estas consultas piden AbrirMesa y
        // usan los métodos internal de ClienteBLL y MesaBLL.

        // Cliente activo con ese DNI, o null.
        public Cliente_790MY ObtenerClienteWalkIn(int dni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AbrirMesa);
            return _clienteBll.ObtenerActivo(dni);
        }

        // True si el DNI está registrado pero dado de baja (no se puede reservar).
        public bool ExisteClienteInactivo(int dni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AbrirMesa);
            return _clienteBll.ExisteInactivo(dni);
        }

        // Turno en curso. Si no hay, lanza el mensaje de fuera de turno.
        public TimeSpan ObtenerTurnoVigenteObligatorio()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AbrirMesa);
            return _mesaBll.ObtenerTurnoVigenteObligatorio();
        }

        // Plano del salón para el turno, igual que MesaBLL_790MY.ObtenerMesasMapa.
        public List<MesaMapaDTO_790MY> ObtenerMesasMapa(DateTime fecha, TimeSpan hora, int comensales)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AbrirMesa);
            return _mesaBll.ObtenerMapa(fecha, hora, comensales);
        }

        // Cliente sin reserva (walk-in). Crea una reserva que ya nace Asistida, con el
        // camarero, y deja la mesa Ocupada, todo en una transacción. La hora que se guarda
        // es la del inicio del turno (no la de llegada) para que cuente como las demás reservas.
        public Reserva_790MY AbrirMesaWalkIn(int clienteDni, Mesa_790MY mesa, int comensales, int camareroDni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AbrirMesa);

            TimeSpan? turno = MesaBLL_790MY.ObtenerTurnoVigente(DateTime.Now);
            if (!turno.HasValue)
                throw MensajesBLL_790MY.Regla("msg_rf2_fuera_de_turno");

            if (mesa == null)
                throw MensajesBLL_790MY.Regla("msg_rf2_falta_mesa");

            // El cliente tiene que existir; si no, la pantalla ofrece darlo de alta antes.
            if (!_clienteBll.ExisteActivo(clienteDni))
                throw MensajesBLL_790MY.Regla("msg_rf2_cliente_inexistente");

            ValidarComensales(mesa, comensales);
            Empleado_790MY camarero = ValidarCamarero(camareroDni);

            var reserva = new Reserva_790MY(clienteDni, mesa.NroMesa, DateTime.Today, turno.Value, comensales, EstadoReserva_790MY.Asistida)
            {
                CamareroDNI = camarero.DNI
            };

            if (!_reservaRepository.AddSiMesaDisponible(reserva, ocuparMesaFisica: true))
            {
                _bitacoraBll.RegistrarEventoPostCommit(Evento.ReservaRechazadaDisponibilidad,
                    $"WalkIn Mesa {mesa.NroMesa} {turno.Value:hh\\:mm}");
                throw MensajesBLL_790MY.Regla("msg_rf2_mesa_no_disponible", mesa.NroMesa);
            }

            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.WalkInRegistrado, $"Reserva {reserva.ReservaID} - Cliente DNI {clienteDni}");
            _bitacoraBll.RegistrarEventoPostCommit(Evento.MesaAbierta, $"Mesa {reserva.MesaNumero} - Reserva {reserva.ReservaID}");
            _bitacoraBll.RegistrarEventoPostCommit(Evento.CamareroAsignado, $"Cam {camarero.DNI} - Mesa {reserva.MesaNumero}");

            return reserva;
        }

        // ─── Consultas para otras BLL ────────────────────────────────────────
        // No validan permiso: las usan PedidoBLL y FacturaBLL, que ya validaron el suyo.
        // Si pidieran VerReservas, el camarero no podría usarlas.

        internal Reserva_790MY ObtenerPorId(int reservaId)
            => _reservaRepository.GetById(reservaId);

        internal List<Reserva_790MY> ObtenerAsistidasPorCamarero(int camareroDni)
            => _reservaRepository.GetAsistidasPorCamarero(camareroDni);

        // Todas las mesas abiertas, de cualquier camarero.
        internal List<Reserva_790MY> ObtenerAsistidas()
            => _reservaRepository.Buscar(null, null, null, EstadoReserva_790MY.Asistida);

        // ─── Helpers ─────────────────────────────────────────────────────────

        // Más de cero comensales y que entren en la mesa.
        private static void ValidarComensales(Mesa_790MY mesa, int comensales)
        {
            if (comensales <= 0)
                throw MensajesBLL_790MY.Regla("msg_rf2_comensales_invalidos");

            if (comensales > mesa.Capacidad)
                throw MensajesBLL_790MY.Regla("msg_rf2_capacidad_insuficiente", mesa.NroMesa, mesa.Capacidad, comensales);
        }

        // El camarero tiene que existir, estar activo y tener puesto Camarero. Usa la consulta
        // internal de EmpleadoBLL para no pedirle a Recepción el permiso del maestro de empleados.
        private Empleado_790MY ValidarCamarero(int camareroDni)
        {
            Empleado_790MY empleado = _empleadoBll.ObtenerPorDni(camareroDni);
            if (empleado == null || !empleado.Activo || empleado.Puesto != PuestoEmpleado_790MY.Camarero)
                throw MensajesBLL_790MY.Regla("msg_rf2_camarero_invalido", camareroDni);
            return empleado;
        }
    }
}
