namespace BE_08YS
{
    /// <summary>
    /// Seccion de la carta. Se persiste como texto (nvarchar(20)), igual que
    /// PuestoEmpleado_790MY, por lo que el orden del enum no afecta a los datos.
    /// FormRegistrarPedido_790MY arma una pestana por cada valor.
    /// </summary>
    public enum CategoriaProducto_790MY
    {
        Entrada,
        PlatoPrincipal,
        Postre,
        Bebida
    }

    /// <summary>
    /// Item del menu. El stock se controla en porciones por producto:
    /// - StockPorciones: existencia fisica (se descuenta en CUN-008, al marcar Listo).
    /// - PorcionesComprometidas: porciones de pedidos Pendiente/EnPreparacion que
    ///   todavia no se descontaron. Lo calcula la consulta; no se persiste.
    /// </summary>
    public class Producto_790MY
    {
        private int _productoId;
        private string _nombre;
        private decimal _precio;
        private int _stockPorciones;
        private bool _activo;
        private CategoriaProducto_790MY _categoria = CategoriaProducto_790MY.PlatoPrincipal;

        public Producto_790MY(int productoId, string nombre, decimal precio, int stockPorciones, bool activo = true)
        {
            _productoId = productoId;
            _nombre = nombre;
            _precio = precio;
            _stockPorciones = stockPorciones;
            _activo = activo;
        }

        public Producto_790MY() { }

        public int ProductoID
        {
            get { return _productoId; }
            set { _productoId = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public decimal Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }

        public int StockPorciones
        {
            get { return _stockPorciones; }
            set { _stockPorciones = value; }
        }

        public bool Activo
        {
            get { return _activo; }
            set { _activo = value; }
        }

        public CategoriaProducto_790MY Categoria
        {
            get { return _categoria; }
            set { _categoria = value; }
        }

        /// <summary>Solo lectura: porciones reservadas por pedidos aun no despachados.</summary>
        public int PorcionesComprometidas { get; set; }

        /// <summary>Porciones que todavia pueden pedirse sin generar sobreventa.</summary>
        public int PorcionesDisponibles => _stockPorciones - PorcionesComprometidas;

        /// <summary>
        /// False para productos sin control de porciones (p. ej. preparaciones a pedido):
        /// no se rechazan por stock en CUN-005 ni se descuentan en CUN-008.
        /// </summary>
        public bool ControlaStock { get; set; } = true;

        /// <summary>True si se pueden pedir <paramref name="cantidad"/> porciones más.</summary>
        public bool Admite(int cantidad) => !ControlaStock || PorcionesDisponibles >= cantidad;

        public override string ToString() => _nombre;

        public override bool Equals(object obj)
        {
            if (!(obj is Producto_790MY other)) return false;
            return ProductoID == other.ProductoID;
        }

        public override int GetHashCode() => ProductoID.GetHashCode();
    }
}
