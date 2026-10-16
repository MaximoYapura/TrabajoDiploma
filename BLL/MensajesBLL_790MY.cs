using Service_08YS;
using System;

namespace BLL_08YS
{
    /// <summary>
    /// Atajo para obtener mensajes de negocio traducidos desde el servicio de
    /// idiomas existente (TraductorManager_08YS + idiomas.json). No es un servicio
    /// nuevo: solo centraliza el string.Format de las claves con parametros.
    /// </summary>
    internal static class MensajesBLL_790MY
    {
        public static string Texto(string clave, params object[] args)
        {
            string plantilla = TraductorManager_08YS.Instance.GetTexto(clave);
            if (args == null || args.Length == 0)
                return plantilla;

            try { return string.Format(plantilla, args); }
            catch (FormatException) { return plantilla; }
        }

        /// <summary>ArgumentException con mensaje traducido (regla de negocio violada).</summary>
        public static ArgumentException Regla(string clave, params object[] args)
            => new ArgumentException(Texto(clave, args));
    }
}
