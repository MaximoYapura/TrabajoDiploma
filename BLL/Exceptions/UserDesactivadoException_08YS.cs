using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_08YS
{
    public class UserDesactivadoException_08YS:Exception
    {
        public UserDesactivadoException_08YS()
        : base("Su usuario se encuentra deshabilitado. Por favor, comuníquese con un administrador.")
        {
        }

        public UserDesactivadoException_08YS(string message) : base(message) { }

        public UserDesactivadoException_08YS(string message, Exception innerException) : base(message, innerException) { }
    }
}
