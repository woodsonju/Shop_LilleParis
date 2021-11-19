using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Tools
{
    /// <summary>
    /// Calcule le SHA256, valeur de hashage d'une chaine et retourne le hachage sous la forme d'une chaine de 64 caractères au format 
    /// hexadecimal.
    /// </summary>
    public class HashTools
    {
        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sHA256Hash = SHA256.Create())  //Crée une instance de l'implémentation par defaut de SHA256
            {
                byte[] bytes = sHA256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
