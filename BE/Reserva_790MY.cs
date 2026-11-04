using System;
using System.Collections.Generic;

namespace BE_08YS
{
    // Estados de la reserva (se guardan como texto):
    // Confirmada: registrada, falta que llegue el cliente.
    // Asistida: el cliente llegó y la mesa está abierta (también los walk-in).
    // Finalizada: se cobró y la mesa quedó libre.
    // Cancelada: se anuló antes de que llegara el cliente.
    public enum EstadoReserva_790MY
    {
        Confirmada,
        Cancelada,
        Asistida,
        Finalizada
    }

    // Reserva de una mesa para un turno. Los walk-in también se guardan como reserva.
    public class Reserva_790MY
    {
        // Estados que ocupan la mesa en su turno. La BLL y los repositorios usan esta
        // lista para no repetir la regla. Cancelada y Finalizada no ocupan.
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

        // Hora de inicio del turno (18:00, 20:00 o 22:00).
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

        // Camarero a cargo de la mesa. Queda vacío hasta que se abre la mesa.
        public int? CamareroDNI
        {
            get { return _camareroDni; }
            set { _camareroDni = value; }
        }

        // Nombre del cliente para mostrar. Lo completan las consultas con JOIN; no se guarda.
        public string ClienteNombreCompleto { get; set; }
    }
}
