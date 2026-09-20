namespace BE_08YS
{
    public enum EstadoMesa_790MY
    {
        Libre,
        Ocupada,
        Reservada
    }

    public class Mesa_790MY
    {
        private int _nroMesa;
        private int _capacidad;
        private EstadoMesa_790MY _estado;
        private bool _activo;

        public Mesa_790MY(int nroMesa, int capacidad, EstadoMesa_790MY estado, bool activo = true)
        {
            _nroMesa = nroMesa;
            _capacidad = capacidad;
            _estado = estado;
            _activo = activo;
        }

        public Mesa_790MY() { }

        public int NroMesa
        {
            get { return _nroMesa; }
            set { _nroMesa = value; }
        }

        public int Capacidad
        {
            get { return _capacidad; }
            set { _capacidad = value; }
        }

        public EstadoMesa_790MY Estado
        {
            get { return _estado; }
            set { _estado = value; }
        }

        public bool Activo
        {
            get { return _activo; }
            set { _activo = value; }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Mesa_790MY other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return NroMesa == other.NroMesa;
        }

        public override int GetHashCode() => NroMesa.GetHashCode();
    }
}
