using System;

namespace BE_08YS
{
    // Puesto del empleado en el restaurante.
    public enum PuestoEmpleado_790MY
    {
        Camarero,
        Chef,
        Recepcionista
    }

    // Empleado del restaurante. No depende de la seguridad: lo único que lo une a un
    // usuario del sistema es UsuarioDNI, que puede quedar vacío si el empleado no usa
    // el sistema. Los permisos los resuelve siempre SessionManager_08YS.
    public class Empleado_790MY
    {
        private int _dni;
        private string _nombre;
        private string _apellido;
        private PuestoEmpleado_790MY _puesto;
        private int? _usuarioDni;
        private bool _activo;

        public Empleado_790MY(int dni, string nombre, string apellido, PuestoEmpleado_790MY puesto, int? usuarioDni = null, bool activo = true)
        {
            _dni = dni;
            _nombre = nombre;
            _apellido = apellido;
            _puesto = puesto;
            _usuarioDni = usuarioDni;
            _activo = activo;
        }

        public Empleado_790MY() { }

        public int DNI
        {
            get { return _dni; }
            set { _dni = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Apellido
        {
            get { return _apellido; }
            set { _apellido = value; }
        }

        public PuestoEmpleado_790MY Puesto
        {
            get { return _puesto; }
            set { _puesto = value; }
        }

        // DNI del usuario del sistema asociado; null si no tiene usuario.
        public int? UsuarioDNI
        {
            get { return _usuarioDni; }
            set { _usuarioDni = value; }
        }

        public bool Activo
        {
            get { return _activo; }
            set { _activo = value; }
        }

        public string NombreCompleto => $"{_nombre} {_apellido}";

        public override string ToString() => NombreCompleto;

        public override bool Equals(object obj)
        {
            if (!(obj is Empleado_790MY other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DNI == other.DNI;
        }

        public override int GetHashCode() => DNI.GetHashCode();
    }
}
