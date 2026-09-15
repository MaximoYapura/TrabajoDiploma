using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_08YS.Exceptions
{
    public class ClienteDniDuplicadoException_790MY : Exception
    {
        public ClienteDniDuplicadoException_790MY(int dni)
            : base($"Ya existe un cliente registrado con el DNI {dni}.")
        {
        }

        public ClienteDniDuplicadoException_790MY(string message) : base(message) { }

        public ClienteDniDuplicadoException_790MY(string message, Exception innerException) : base(message, innerException) { }
    }
}
