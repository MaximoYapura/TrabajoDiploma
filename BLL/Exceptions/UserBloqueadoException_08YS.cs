using System;
using System.Runtime.Serialization;

namespace BLL_08YS
{
    public class UserBloqueadoException_08YS : Exception
    {
        public UserBloqueadoException_08YS()
        : base("La cuenta se encuentra bloqueada debido a reiterados intentos fallidos. Contacte a un administrador.")
        {
        }

        public UserBloqueadoException_08YS(string message) : base(message) { }

        public UserBloqueadoException_08YS(string message, Exception innerException) : base(message, innerException) { }
    }
}
