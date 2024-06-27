using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Psytest.Instruments
{
    public static class Crypter
    {
        public static int Key = 143;

        // Шифрование данных
        public static string Encrypt(string InputString)
        {
            byte[] DataEncrypt = Encoding.UTF8.GetBytes(InputString);
            for (int i = 0; i < DataEncrypt.Length; i++)
            {
                DataEncrypt[i] = (byte)(DataEncrypt[i] ^ Key);
            }
            return Convert.ToBase64String(DataEncrypt);
        }

        // Расшифрование данных
        public static string Decrypt(string DataEncrypt)
        {
            byte[] DataDecrypt = Convert.FromBase64String(DataEncrypt);
            for (int i = 0; i < DataDecrypt.Length; i++)
            {
                DataDecrypt[i] = (byte)(DataDecrypt[i] ^ Key);
            }
            return Encoding.UTF8.GetString(DataDecrypt);
        }

        public static string CalculateHash(string data)
        {
            var array = Encoding.UTF8.GetBytes(data);
            var hashArray = new SHA256CryptoServiceProvider().ComputeHash(array);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashArray) 
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }
    }
}
