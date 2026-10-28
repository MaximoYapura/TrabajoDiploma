using System;
using System.Collections.Generic;
using System.Linq;

namespace BE_08YS
{
    /// <summary>
    /// Pendiente      -> registrado por el Camarero (CUN-006), stock validado pero no descontado.
    /// EnPreparacion  -> el Chef inicio la elaboracion (CUN-008, paso 3).
    /// Listo          -> el Chef lo finalizo; el stock ya fue descontado (CUN-008, paso 6/7).
    /// </summary>
    public enum EstadoPedido_790MY
    {
        Pendiente,
        EnPreparacion,
        Listo
    }

    public class Pedido_790MY
    {
        /// <summary>Estados cuyas porciones estan comprometidas pero no descontadas.</summary>
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

        /// <summary>
        /// Marca de idempotencia del descuento de inventario: una vez en true,
        /// el pedido nunca vuelve a descontar stock aunque se reprocese.
        /// </summary>
        public bool StockDescontado
        {
            get { return _stockDescontado; }
            set { _stockDescontado = value; }
        }

        public List<ItemPedido_790MY> Items { get; set; } = new List<ItemPedido_790MY>();

        // Solo lectura (JOIN con Reservas): datos para la comanda de cocina.
        public int MesaNumero { get; set; }
        public int? CamareroDNI { get; set; }

        public decimal Total => Items.Sum(i => i.Subtotal);
    }
}
