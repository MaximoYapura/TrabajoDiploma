using Service_08YS;
using System;

namespace BLL_08YS
{
    // Atajo para sacar los mensajes de negocio traducidos de idiomas.json
    // (TraductorManager_08YS), con sus parámetros ya reemplazados.
    internal static class MensajesBLL_790MY
    {
        // Texto traducido de la clave, con los args aplicados.
        public static string Texto(string clave, params object[] args)
        {
            string plantilla = TraductorManager_08YS.Instance.GetTexto(clave);
            if (args == null || args.Length == 0)
                return plantilla;

            try { return string.Format(plantilla, args); }
            catch (FormatException) { return plantilla; }
        }

        // ArgumentException con el texto traducido. Se usa con throw cuando no se cumple una regla.
        public static ArgumentException Regla(string clave, params object[] args)
            => new ArgumentException(Texto(clave, args));
    }
}
