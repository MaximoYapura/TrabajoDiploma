namespace BE_08YS
{
    // Sección de la carta. Se guarda como texto, así que el orden del enum no importa.
    // Registrar pedido muestra una pestaña por cada valor.
    public enum CategoriaProducto_790MY
    {
        Entrada,
        PlatoPrincipal,
        Postre,
        Bebida
    }

    // Producto de la carta. El stock se maneja en porciones:
    // StockPorciones es lo que hay físicamente (cocina lo descuenta al marcar Listo).
    // PorcionesComprometidas son las de pedidos que todavía no salieron de cocina;
    // la calcula la consulta y no se guarda.
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

        // Porciones de pedidos que todavía no se despacharon.
        public int PorcionesComprometidas { get; set; }

        // Porciones que todavía se pueden pedir.
        public int PorcionesDisponibles => _stockPorciones - PorcionesComprometidas;

        // En false el producto no tiene límite de porciones: no se rechaza por stock
        // al pedir ni se descuenta en cocina.
        public bool ControlaStock { get; set; } = true;

        // True si se pueden pedir esa cantidad de porciones.
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
