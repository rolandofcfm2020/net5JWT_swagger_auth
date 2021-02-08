using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApiNet5.Backend
{
    public class CryptographySC
    {
        //https://www.codeproject.com/Tips/839656/How-to-encrypt-and-decrypt-string-using-AES-algori
        //must be private if is in controller because Swagger get nuts!
        private static string secretKey = "0406b130-bd65-11ea-b3de-0242ac130004-ab0660bb-a138-45e9-bdf1-9ff1ac62ae5e-0242ac130004-ab0660bb-a138-45e9-b";

        public static string Encrypt(string password, string saltValue)
        {

            string secretKey = CryptographySC.secretKey;
            var saltBuffer = Encoding.UTF8.GetBytes(saltValue);
            byte[] clearBytes = Encoding.Unicode.GetBytes(password);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(secretKey, saltBuffer, 1000, HashAlgorithmName.SHA256);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    password = Convert.ToBase64String(ms.ToArray());
                }
            }

            return password;
        }

        public static string Decrypt(string cipherText, string saltValue)
        {
            string secretKey = CryptographySC.secretKey;
            var saltBuffer = Encoding.UTF8.GetBytes(saltValue);
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(secretKey, saltBuffer, 1000, HashAlgorithmName.SHA256);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
