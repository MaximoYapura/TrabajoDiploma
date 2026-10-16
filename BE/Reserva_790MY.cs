using System;
using System.Collections.Generic;

namespace BE_08YS
{
    /// <summary>
    /// Ciclo de vida de una reserva.
    /// Confirmada  -> registrada, pendiente de la llegada del cliente.
    /// Asistida    -> el cliente llego y la mesa fue abierta (CUN-004, incluye Walk-in).
    /// Finalizada  -> la atencion concluyo (pago registrado) y la mesa se libero.
    /// Cancelada   -> anulada antes de la llegada del cliente.
    /// Los valores se persisten como texto (nvarchar(20)), por lo que el orden
    /// del enum no afecta a los datos existentes.
    /// </summary>
    public enum EstadoReserva_790MY
    {
        Confirmada,
        Cancelada,
        Asistida,
        Finalizada
    }

    public class Reserva_790MY
    {
        /// <summary>
        /// Estados que mantienen la mesa ocupada en el turno de la reserva.
        /// Unica fuente de verdad de la regla de ocupacion: la usan la BLL y los
        /// repositorios para no repetir literales de estado en cada consulta.
        /// Cancelada y Finalizada NO bloquean la mesa.
        /// </summary>
        public static readonly IReadOnlyList<EstadoReserva_790MY> EstadosQueOcupanMesa =
            new[] { EstadoReserva_790MY.Confirmada, EstadoReserva_790MY.Asistida };

        private int _reservaId;
        private int _clienteDni;
        private int _mesaNumero;
        private DateTime _fecha;
        private TimeSpan _hora;
        private int _cantidadComensales;
        private EstadoReserva_790MY _estado;
        private int? _camareroDni;

        public Reserva_790MY(int clienteDni, int mesaNumero, DateTime fecha, TimeSpan hora, int cantidadComensales, EstadoReserva_790MY estado)
        {
            _clienteDni = clienteDni;
            _mesaNumero = mesaNumero;
            _fecha = fecha;
            _hora = hora;
            _cantidadComensales = cantidadComensales;
            _estado = estado;
        }

        public Reserva_790MY() { }

        public int ReservaID
        {
            get { return _reservaId; }
            set { _reservaId = value; }
        }

        public int ClienteDNI
        {
            get { return _clienteDni; }
            set { _clienteDni = value; }
        }

        public int MesaNumero
        {
            get { return _mesaNumero; }
            set { _mesaNumero = value; }
        }

        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        /// <summary>Hora de inicio del turno (18:00, 20:00 o 22:00).</summary>
        public TimeSpan Hora
        {
            get { return _hora; }
            set { _hora = value; }
        }

        public int CantidadComensales
        {
            get { return _cantidadComensales; }
            set { _cantidadComensales = value; }
        }

        public EstadoReserva_790MY Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }

        /// <summary>
        /// DNI del Empleado (puesto Camarero) responsable de la atencion.
        /// Null mientras la reserva no fue abierta (estado Confirmada/Cancelada).
        /// </summary>
        public int? CamareroDNI
        {
            get { return _camareroDni; }
            set { _camareroDni = value; }
        }

        /// <summary>True si, por su estado, la reserva bloquea la mesa en su turno.</summary>
        public bool OcupaMesa
        {
            get
            {
                foreach (var e in EstadosQueOcupanMesa)
                    if (e == _estado) return true;
                return false;
            }
        }

        // Solo para lectura/visualizacion (no se persiste, no participa de Add):
        // lo completan las consultas de busqueda que hacen JOIN con Clientes,
        // para no tener que ir a buscar el cliente aparte solo para mostrar su nombre.
        public string ClienteNombreCompleto { get; set; }
    }
}
