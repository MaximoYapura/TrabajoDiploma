using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_08YS
{
    public enum UserRole
    {
        Admin,
        Basico
    }
    public class User_08YS
    {
        private string _username;
        private int _dni;
        private Rol_08YS _rol;
        private string _nombre;
        private string _apellido;
        private string _hash;
        private string _salt;
        private string _email;
        private bool _bloqueado;
        private bool _activo;
        private string _idioma;
      
        public User_08YS(string username, int dni, Rol_08YS rol, string nombre, string ape, string email, string hash, string salt, bool bloqueado = false, bool activo = true,string idioma="es")
        {
            this._username = username;
            this._dni = dni;
            this._rol = rol;
            this._nombre = nombre;
            this._apellido = ape;
            this._hash = hash;
            this._salt = salt;
            this._email = email;
            this._bloqueado = bloqueado;
            this._activo = activo;
            this.Idioma = idioma;
        }

        public User_08YS() { }

        public override bool Equals(object obj)
        {
            if (!(obj is User_08YS other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DNI == other.DNI;
        }

        public override int GetHashCode() => DNI.GetHashCode();

        public static bool operator ==(User_08YS a, User_08YS b)
        {
            if (a is null && b is null) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(User_08YS a, User_08YS b) => !(a == b);

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public int DNI
        {
            get { return _dni; }
            set { _dni = value; }
        }

        public Rol_08YS Rol
        {
            get { return _rol; }
            set { _rol = value; }
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

        [Browsable(false)]
        public string Hash
        {
            get { return _hash; }
            set { _hash = value; }
        }

        [Browsable(false)]
        public string Salt
        {
            get { return _salt; }
            set { _salt = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }


        public bool Bloqueado
        {
            get { return _bloqueado; }
            set { _bloqueado = value; }
        }

        public bool Activo
        {
            get { return _activo; }
            set { _activo = value; }
        }
        [Browsable(false)]
        public string Idioma
        {
            get { return _idioma; }
            set { _idioma = value; }
        }
    }
}
