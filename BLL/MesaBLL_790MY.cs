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
    public class MesaBLL_790MY
    {
        // Turnos fijos de 2 horas entre las 18:00 y las 24:00 (horas de inicio válidas).
        private static readonly TimeSpan[] TurnosValidos =
        {
            new TimeSpan(18, 0, 0),
            new TimeSpan(20, 0, 0),
            new TimeSpan(22, 0, 0)
        };

        public static readonly TimeSpan DuracionTurno = TimeSpan.FromHours(2);

        /// <summary>Horas de inicio de los turnos de atención.</summary>
        public static IReadOnlyList<TimeSpan> Turnos => TurnosValidos;

        /// <summary>
        /// Turno en curso para el momento indicado (inicio &lt;= hora &lt; inicio + 2h),
        /// o null si el restaurante está fuera del horario de atención.
        /// Lo usan la apertura de mesas (CUN-004) y el Walk-in (CUN-005).
        /// Fuera de horario, si el modo prueba está activo (App.config), devuelve el
        /// turno de prueba configurado para poder testear sin depender de la hora.
        /// </summary>
        public static TimeSpan? ObtenerTurnoVigente(DateTime momento)
        {
            TimeSpan? real = ObtenerTurnoReal(momento);
            return real ?? TurnoDePrueba;
        }

        /// <summary>True si el turno vigente del momento indicado proviene del modo prueba.</summary>
        public static bool EsTurnoDePrueba(DateTime momento)
            => !ObtenerTurnoReal(momento).HasValue && TurnoDePrueba.HasValue;

        private static TimeSpan? ObtenerTurnoReal(DateTime momento)
        {
            TimeSpan hora = momento.TimeOfDay;
            foreach (var inicio in TurnosValidos)
                if (hora >= inicio && hora < inicio + DuracionTurno)
                    return inicio;
            return null;
        }

        // ─── Modo demostración ───────────────────────────────────────────────
        // App.config de la GUI, <appSettings>:
        //   <add key="ModoDemostracion"  value="true" />
        //   <add key="TurnoPrueba_790MY" value="22:00" />   (18:00, 20:00 o 22:00)
        // Con ModoDemostracion en false o ausente se aplica el control de turnos estricto.
        private static readonly Lazy<TimeSpan?> _turnoDePrueba = new Lazy<TimeSpan?>(LeerTurnoDePrueba);

        private static TimeSpan? TurnoDePrueba => _turnoDePrueba.Value;

        private static TimeSpan? LeerTurnoDePrueba()
        {
            try
            {
                if (!bool.TryParse(ConfigurationManager.AppSettings["ModoDemostracion"], out bool activo) || !activo)
                    return null;

                // Un valor ausente o que no es un inicio de turno válido cae en el último
                // turno del día, que es el que admite registrar reservas "para hoy" más tarde.
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

        /// <summary>
        /// Igual que ObtenerTurnoVigente(DateTime.Now) pero lanza una regla de negocio
        /// traducida si no hay turno en curso. La GUI lo usa para abrir
        /// FormSeleccionarMesa_790MY en modo Walk-in: new FormSeleccionarMesa_790MY(
        /// DateTime.Today, mesaBll.ObtenerTurnoVigenteObligatorio(), comensales).
        /// </summary>
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

        public List<Mesa_790MY> GetAll()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerMesas);
            return _mesaRepository.GetAll();
        }

        /// <summary>
        /// Mesa con ese número, o null. Sin ValidatePermission: lo usa ReservaBLL_790MY
        /// en CUN-004, que ya validó AbrirMesa (Recepción no tiene VerMesas).
        /// </summary>
        internal Mesa_790MY ObtenerPorNumero(int nroMesa)
            => _mesaRepository.GetAll().FirstOrDefault(m => m.NroMesa == nroMesa);

        /// <summary>
        /// Devuelve el próximo número de mesa disponible: Max(NroMesa) + 1, o 1 si no hay mesas.
        /// </summary>
        public int GetProximoNumero()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerMesas);
            var mesas = _mesaRepository.GetAll();
            int max = mesas.Count > 0 ? mesas.Max(m => m.NroMesa) : 0;
            return max + 1;
        }

         /// <summary>
        /// Devuelve todas las mesas clasificadas para el mapa del salón según
        /// el turno y la cantidad de comensales solicitados.
        /// </summary>
        public List<MesaMapaDTO_790MY> ObtenerMesasMapa(DateTime fecha, TimeSpan hora, int comensales)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerMesas);

            if (fecha.Date < DateTime.Today)
                throw MensajesBLL_790MY.Regla("msg_rf2_fecha_anterior_hoy");

            if (comensales <= 0)
                throw MensajesBLL_790MY.Regla("msg_rf2_comensales_invalidos");

            if (!TurnosValidos.Contains(hora))
                throw MensajesBLL_790MY.Regla("msg_rf2_turno_invalido");

            // Las mesas dadas de baja (FueraDeServicio / Activo = 0) no se dibujan en el
            // plano: no pueden asignarse ni en reservas ni en Walk-in.
            var todasLasMesas = _mesaRepository.GetAll()
                .Where(m => m.Activo && m.Estado != EstadoMesa_790MY.FueraDeServicio)
                .ToList();

            // Ocupación por turno: reservas Confirmadas y Asistidas del mismo (fecha, hora).
            // Canceladas y Finalizadas no bloquean (Reserva_790MY.EstadosQueOcupanMesa).
            var numerosOcupados = new HashSet<int>(_mesaRepository.GetNumerosOcupadosEnTurno(fecha.Date, hora));

            // Si se consulta el turno EN CURSO, además se respeta el estado físico de la
            // mesa: un grupo de un turno anterior que todavía no se retiró (mesa Ocupada,
            // reserva aún Asistida) no puede recibir a otro grupo. Para turnos futuros
            // el estado físico actual no aplica.
            bool esTurnoEnCurso = fecha.Date == DateTime.Today && ObtenerTurnoVigente(DateTime.Now) == hora;
            if (esTurnoEnCurso)
                foreach (var mesa in todasLasMesas.Where(m => m.Estado == EstadoMesa_790MY.Ocupada))
                    numerosOcupados.Add(mesa.NroMesa);

            // Mejor ajuste: mínima capacidad entre las mesas libres que pueden alojar a los comensales.
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

        public void RegistrarMesa(int numero, int capacidad)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearMesa);

            ValidarDatos(numero, capacidad);

            var todasLasMesas = _mesaRepository.GetAll();

            if (todasLasMesas.Count >= 12)
                throw new ArgumentException(TraductorManager_08YS.Instance.GetTexto("msg_limite_mesas_alcanzado") ?? "Se ha alcanzado el límite máximo de 12 mesas para el salón.");

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

            // ── Integridad referencial: historial completo (activas + pasadas) ──────────
            // Excepción: se lee IReservaRepository_790MY directo (solo lectura). Pasar por
            // ReservaBLL_790MY formaría un ciclo ReservaBLL -> MesaBLL -> ReservaBLL y
            // BLLFactory_790MY entraría en recursión infinita.
            // Se consulta el repositorio de reservas para verificar si existen reservas
            // asociadas a esta mesa, sin importar su estado ni fecha.
            // Si las hay, se prohíbe la eliminación y se informa al usuario.
            var todasLasReservas = _reservaRepository.Buscar(null, null, null, null);
            bool tieneHistorial  = todasLasReservas != null &&
                                   todasLasReservas.Any(r => r.MesaNumero == numero);

            if (tieneHistorial)
                throw new ArgumentException(
                    TraductorManager_08YS.Instance.GetTexto("msg_error_eliminar_mesa_con_reservas")
                    ?? "No se puede eliminar la mesa porque posee reservas asociadas en el historial.");

            // ── Baja por estado: FueraDeServicio (no DELETE físico) ──────────────────────
            // La mesa no tiene reservas en absoluto: se marca como FueraDeServicio
            // en lugar de eliminarse físicamente, para preservar la integridad del modelo.
            _mesaRepository.UpdateEstado(numero, EstadoMesa_790MY.FueraDeServicio);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.MesaEliminada, targetUsername: numero.ToString());
        }

        private static void ValidarDatos(int numero, int capacidad)
        {
            if (numero <= 0)
                throw new ArgumentException("El número de mesa debe ser mayor a cero.");

            if (capacidad < 1 || capacidad > 8)
                throw new ArgumentException("La capacidad de la mesa debe estar entre 1 y 8 personas.");
        }
    }
}
