using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_08YS
{
    public class Cliente_790MY
    {
        private int _dni;
        private string _nombre;
        private string _apellido;
        private string _email;
        private string _telefono;
        private string _direccion;
        private bool _activo;

        public Cliente_790MY(int dni, string nombre, string apellido, string email, string telefono, string direccion = null, bool activo = true)
        {
            _dni = dni;
            _nombre = nombre;
            _apellido = apellido;
            _email = email;
            _telefono = telefono;
            _direccion = direccion;
            _activo = activo;
        }

        public Cliente_790MY() { }

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

        // Email/Telefono "principales": obligatorios, son los del formulario ppal.
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Telefono
        {
            get { return _telefono; }
            set { _telefono = value; }
        }

        public string Direccion
        {
            get { return _direccion; }
            set { _direccion = value; }
        }

        public bool Activo
        {
            get { return _activo; }
            set { _activo = value; }
        }

        // Emails/telefonos adicionales (relacion 1:n). No incluyen al principal.
        public List<string> EmailsAdicionales { get; set; } = new List<string>();
        public List<string> TelefonosAdicionales { get; set; } = new List<string>();

        public override bool Equals(object obj)
        {
            if (!(obj is Cliente_790MY other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DNI == other.DNI;
        }

        public override int GetHashCode() => DNI.GetHashCode();

        public static bool operator ==(Cliente_790MY a, Cliente_790MY b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Cliente_790MY a, Cliente_790MY b) => !(a == b);
    }
}
