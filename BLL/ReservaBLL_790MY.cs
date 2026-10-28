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
    // Clase parcial: la generación del comprobante (PDF + email) vive en
    // ReservaBLL_790MY.Comprobante.cs para no mezclarla con las reglas de dominio.
    //
    // Patrón de cada operación que modifica datos (igual al resto del proyecto):
    //   1) SessionManager_08YS.ValidatePermission  -> autorización (Composite) en la BLL,
    //      de modo que invocar la BLL directamente sin permiso también falla.
    //   2) Validaciones de negocio con mensajes traducidos (MensajesBLL_790MY).
    //   3) Persistencia atómica en el repositorio (una transacción SQL por operación).
    //   4) DVManager_08YS.Recalcular()                -> DVH/DVV de todas las tablas monitoreadas.
    //   5) BitacoraBLL_08YS.RegistrarEventoPostCommit -> auditoría del hecho ya confirmado.
    public partial class ReservaBLL_790MY
    {
        private readonly IReservaRepository_790MY _reservaRepository;
        private readonly IClienteRepository_790MY _clienteRepository;
        private readonly IMesaRepository_790MY _mesaRepository;
        private readonly IEmpleadoRepository_790MY _empleadoRepository;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public ReservaBLL_790MY(IReservaRepository_790MY reservaRepository, IClienteRepository_790MY clienteRepository,
                                IMesaRepository_790MY mesaRepository, IEmpleadoRepository_790MY empleadoRepository,
                                BitacoraBLL_08YS bitacoraBll)
        {
            _reservaRepository = reservaRepository ?? throw new ArgumentNullException(nameof(reservaRepository));
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
            _mesaRepository = mesaRepository ?? throw new ArgumentNullException(nameof(mesaRepository));
            _empleadoRepository = empleadoRepository ?? throw new ArgumentNullException(nameof(empleadoRepository));
            _bitacoraBll = bitacoraBll ?? throw new ArgumentNullException(nameof(bitacoraBll));
        }

        // ═══════════════════════════════════════════════════════════════════
        //  RF1 — Registrar reserva
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Registra la reserva y la devuelve con su ReservaID ya asignado por el
        /// repositorio, para que la capa de presentación pueda emitir el comprobante.
        /// </summary>
        public Reserva_790MY RegistrarReserva(int clienteDni, Mesa_790MY mesa, DateTime fecha, TimeSpan hora, int comensales)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarReserva);

            if (mesa == null)
                throw MensajesBLL_790MY.Regla("msg_rf2_falta_mesa");

            // GetByDni filtra por Activo = 1: un cliente dado de baja logicamente
            // no deberia poder hacer reservas.
            if (_clienteRepository.GetByDni(clienteDni) == null)
                throw MensajesBLL_790MY.Regla("msg_rf2_cliente_inexistente");

            ValidarComensales(mesa, comensales);

            DateTime fechaHora = fecha.Date + hora;
            if (fechaHora <= DateTime.Now)
                throw MensajesBLL_790MY.Regla("msg_rf2_reserva_en_pasado");

            var reserva = new Reserva_790MY(clienteDni, mesa.NroMesa, fecha.Date, hora, comensales, EstadoReserva_790MY.Confirmada);

            // Alta atómica: la re-validación de disponibilidad y el INSERT ocurren en la
            // misma transacción con bloqueo de rango, por lo que dos puestos de recepción
            // no pueden confirmar la misma mesa para el mismo turno (antes la verificación
            // y el alta eran dos pasos separados y existía una ventana de carrera).
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

        public List<Reserva_790MY> Buscar(DateTime? desde, DateTime? hasta, int? clienteDni, EstadoReserva_790MY? estado)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerReservas);

            if (desde.HasValue && hasta.HasValue && desde.Value.Date > hasta.Value.Date)
                throw MensajesBLL_790MY.Regla("msg_rep_rango_fechas_invalido", desde.Value, hasta.Value);

            return _reservaRepository.Buscar(desde, hasta, clienteDni, estado);
        }

        // ═══════════════════════════════════════════════════════════════════
        //  RF2 — CUN-004 Abrir Mesa
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Nómina de reservas Confirmadas de HOY para el turno en curso, filtrable por
        /// número de reserva o DNI del cliente (pasos 2-4 del CUN-004).
        /// Devuelve lista vacía fuera del horario de atención.
        /// </summary>
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

        /// <summary>
        /// Cantidad de mesas abiertas (reservas Asistidas) que tiene a cargo cada
        /// camarero, para orientar a Recepción al elegir el responsable (CUN-004).
        /// </summary>
        public Dictionary<int, int> ContarMesasACargo(IEnumerable<int> camarerosDni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AbrirMesa);

            var resultado = new Dictionary<int, int>();
            foreach (int dni in (camarerosDni ?? Enumerable.Empty<int>()).Distinct())
                resultado[dni] = _reservaRepository.GetAsistidasPorCamarero(dni).Count;
            return resultado;
        }

        /// <summary>
        /// CUN-004 escenario principal: el cliente llega con reserva previa.
        /// Confirmada -> Asistida, asigna el camarero y marca la mesa Ocupada
        /// (todo en una transacción).
        /// </summary>
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

            Mesa_790MY mesa = _mesaRepository.GetAll().FirstOrDefault(m => m.NroMesa == reserva.MesaNumero);
            if (mesa == null || !mesa.Activo || mesa.Estado == EstadoMesa_790MY.FueraDeServicio)
                throw MensajesBLL_790MY.Regla("msg_rf2_mesa_fuera_servicio", reserva.MesaNumero);
            if (mesa.Estado == EstadoMesa_790MY.Ocupada)
                throw MensajesBLL_790MY.Regla("msg_rf2_mesa_fisicamente_ocupada", reserva.MesaNumero);

            // La transición es condicional en la base: si entre la validación y este punto
            // otro puesto abrió la reserva u ocupó la mesa, no se modifica nada.
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

        /// <summary>
        /// CUN-005 Registrar Walk-In (extiende el paso 3 de CUN-004). La mesa se elige con el mismo
        /// FormSeleccionarMesa_790MY usado en RF1, abierto con DateTime.Today y el turno
        /// en curso (MesaBLL_790MY.ObtenerTurnoVigenteObligatorio). Se crea la reserva en
        /// tiempo real directamente en estado Asistida, con camarero, y la mesa queda
        /// Ocupada, todo en una transacción.
        /// La reserva se guarda con Hora = inicio del turno en curso (no la hora exacta de
        /// llegada) para que la regla de ocupación por turno la contemple igual que al resto.
        /// </summary>
        public Reserva_790MY AbrirMesaWalkIn(int clienteDni, Mesa_790MY mesa, int comensales, int camareroDni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AbrirMesa);

            TimeSpan? turno = MesaBLL_790MY.ObtenerTurnoVigente(DateTime.Now);
            if (!turno.HasValue)
                throw MensajesBLL_790MY.Regla("msg_rf2_fuera_de_turno");

            if (mesa == null)
                throw MensajesBLL_790MY.Regla("msg_rf2_falta_mesa");

            // La FK Reservas -> Clientes exige un cliente registrado y activo; si no existe,
            // la GUI debe ofrecer el alta (FormRegistrarCliente_790MY) antes de reintentar.
            if (_clienteRepository.GetByDni(clienteDni) == null)
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

        // ─── Helpers ─────────────────────────────────────────────────────────

        private static void ValidarComensales(Mesa_790MY mesa, int comensales)
        {
            if (comensales <= 0)
                throw MensajesBLL_790MY.Regla("msg_rf2_comensales_invalidos");

            if (comensales > mesa.Capacidad)
                throw MensajesBLL_790MY.Regla("msg_rf2_capacidad_insuficiente", mesa.NroMesa, mesa.Capacidad, comensales);
        }

        /// <summary>
        /// El camarero responsable debe existir, estar activo y tener puesto Camarero.
        /// Se consulta el repositorio de empleados directamente (no EmpleadoBLL) para no
        /// exigir el permiso de ABM de empleados a quien solo tiene AbrirMesa.
        /// </summary>
        private Empleado_790MY ValidarCamarero(int camareroDni)
        {
            Empleado_790MY empleado = _empleadoRepository.GetByDni(camareroDni);
            if (empleado == null || !empleado.Activo || empleado.Puesto != PuestoEmpleado_790MY.Camarero)
                throw MensajesBLL_790MY.Regla("msg_rf2_camarero_invalido", camareroDni);
            return empleado;
        }
    }
}
