using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_08YS.Entities
{
    public class ResultadoValidacion_08YS
    {
        public enum TipoResultado { Ok, Advertencia, Error }

        public TipoResultado Tipo { get; private set; }
        public string Mensaje { get; private set; }
        public bool EsValido => Tipo != TipoResultado.Error;

        private ResultadoValidacion_08YS() { }

        public static ResultadoValidacion_08YS Ok()
            => new ResultadoValidacion_08YS { Tipo = TipoResultado.Ok };

        public static ResultadoValidacion_08YS Advertencia(string mensaje)
            => new ResultadoValidacion_08YS { Tipo = TipoResultado.Advertencia, Mensaje = mensaje };

        public static ResultadoValidacion_08YS Error(string mensaje)
            => new ResultadoValidacion_08YS { Tipo = TipoResultado.Error, Mensaje = mensaje };
    }
}
