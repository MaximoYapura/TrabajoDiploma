using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service_08YS
{
    public class DvEncriptador_08YS
    {
        private static readonly byte[] _clave =
        Encoding.UTF8.GetBytes("Horizon2024_DV_SecretKey_32bytes");

        /// <summary>
        /// Cifra el valor usando AES-256-CBC.
        /// El IV se deriva del nombre de la tabla para que sea determinístico:
        /// misma tabla + mismos datos → mismo resultado cifrado → comparación posible.
        /// </summary>
        public static string Cifrar(string valor, string nombreTabla)
        {
            byte[] iv = DerivarIV(nombreTabla);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _clave;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encriptor = aes.CreateEncryptor())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(valor);
                    byte[] bytesCifrados = encriptor.TransformFinalBlock(bytes, 0, bytes.Length);
                    return Convert.ToBase64String(bytesCifrados);
                }
            }
        }

        /// <summary>
        /// Deriva un IV de 16 bytes a partir del nombre de la tabla usando SHA-256.
        /// Mismo nombre de tabla → mismo IV siempre.
        /// </summary>
        private static byte[] DerivarIV(string nombreTabla)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(nombreTabla));
                byte[] iv = new byte[16];
                Array.Copy(hash, iv, 16);
                return iv;
            }
        }
    }
}
