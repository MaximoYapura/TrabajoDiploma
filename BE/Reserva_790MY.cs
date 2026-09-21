using System;

namespace BE_08YS
{
    public enum EstadoReserva_790MY
    {
        Confirmada,
        Cancelada
    }

    public class Reserva_790MY
    {
        private int _reservaId;
        private int _clienteDni;
        private int _mesaNumero;
        private DateTime _fecha;
        private TimeSpan _hora;
        private int _cantidadComensales;
        private EstadoReserva_790MY _estado;

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

        // Solo para lectura/visualizacion (no se persiste, no participa de Add):
        // lo completan las consultas de busqueda que hacen JOIN con Clientes,
        // para no tener que ir a buscar el cliente aparte solo para mostrar su nombre.
        public string ClienteNombreCompleto { get; set; }
    }
}
