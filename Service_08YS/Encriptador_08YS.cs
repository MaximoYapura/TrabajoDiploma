using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service_08YS
{
    public static class Encriptador_08YS
    {
        public static void CrearHash(string password, out string hash, out string salt)
        {
            byte[] saltBytes = new byte[16];                    // creo un array de 16 bytes (128 bits) para la salt aleatoria
            using (var rng = RandomNumberGenerator.Create())    // uso un generador criptográfico para rellenar saltBytes con valores random
                rng.GetBytes(saltBytes);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);         // se crea el hash ingresando la contraseña, el salt las iteraciones 
                hash = Convert.ToBase64String(hashBytes);       // y el algoritmo que se va a utilizar para el hash (sha256 en este caso)
                salt = Convert.ToBase64String(saltBytes);
            }
        }

        public static bool Verificar(string password, string hashGuardado, string saltGuardado) //verifica que la contraseña sea correcta
        {
            byte[] saltBytes = Convert.FromBase64String(saltGuardado); //se convierte la salt desde texto (Base64) a bytes

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100_000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);                 // genero un nuevo hash con los mismos parametros que "en teoria" se
                string hashNuevo = Convert.ToBase64String(hashBytes);   // usaron para guardar y lo convierto a texto para compararlo despues
                return hashNuevo == hashGuardado;                       // si los datos son los mismos significa que la contraseña es correcta
            }
        }

        
    }
}
