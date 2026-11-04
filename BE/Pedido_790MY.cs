using System;
using System.Collections.Generic;
using System.Linq;

namespace BE_08YS
{
    // Estados del pedido:
    // Pendiente: lo cargó el camarero; el stock se controló pero no se descontó.
    // EnPreparacion: cocina empezó a prepararlo.
    // Listo: cocina lo terminó y se descontó el stock.
    public enum EstadoPedido_790MY
    {
        Pendiente,
        EnPreparacion,
        Listo
    }

    // Pedido (comanda) de una mesa abierta.
    public class Pedido_790MY
    {
        // Estados en los que las porciones están reservadas pero todavía no descontadas.
        public static readonly IReadOnlyList<EstadoPedido_790MY> EstadosConStockComprometido =
            new[] { EstadoPedido_790MY.Pendiente, EstadoPedido_790MY.EnPreparacion };

        private int _nroPedido;
        private int _reservaId;
        private DateTime _fecha;
        private TimeSpan _hora;
        private EstadoPedido_790MY _estado;
        private DateTime? _fechaFin;
        private TimeSpan? _horaFin;
        private bool _stockDescontado;

        public Pedido_790MY(int reservaId, DateTime fecha, TimeSpan hora)
        {
            _reservaId = reservaId;
            _fecha = fecha.Date;
            _hora = hora;
            _estado = EstadoPedido_790MY.Pendiente;
        }

        public Pedido_790MY() { }

        public int NroPedido
        {
            get { return _nroPedido; }
            set { _nroPedido = value; }
        }

        public int ReservaID
        {
            get { return _reservaId; }
            set { _reservaId = value; }
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

        public EstadoPedido_790MY Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }

        public DateTime? FechaFin
        {
            get { return _fechaFin; }
            set { _fechaFin = value; }
        }

        public TimeSpan? HoraFin
        {
            get { return _horaFin; }
            set { _horaFin = value; }
        }

        // Queda en true cuando cocina descontó el stock. Sirve para no descontar
        // dos veces si el pedido se marca Listo de nuevo.
        public bool StockDescontado
        {
            get { return _stockDescontado; }
            set { _stockDescontado = value; }
        }

        public List<ItemPedido_790MY> Items { get; set; } = new List<ItemPedido_790MY>();

        // Datos de la reserva para mostrar en cocina. Se leen con JOIN, no se guardan.
        public int MesaNumero { get; set; }
        public int? CamareroDNI { get; set; }

        public decimal Total => Items.Sum(i => i.Subtotal);
    }
}
