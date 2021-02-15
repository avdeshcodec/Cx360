using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagement.Entities.Common
{
    class AesEncryptionDecryption
    {
        /// <Summary>
        ///<Descripton>Encrypts data using AES SHA 256 algorithm</Descripton>
        /// </Summary>
        private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;


            byte[] saltBytes = new byte[] { 4, 9, 79, 69, 42, 73, 7, 82 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        /// <Summary>
        ///<Descripton>decrypts data using AES SHA 256 algorithm</Descripton>
        /// </Summary>
        private static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;


            byte[] saltBytes = new byte[] { 4, 9, 79, 69, 42, 73, 7, 82 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        /// <Summary>
        ///<Descripton>Encrypts string using AES SHA 256 algorithm</Descripton>
        /// </Summary>
        public static string Encrypt(string text)
        {
            if (!(string.IsNullOrEmpty(text)))
            {
                string _Password = @"5m@r7Data@123!@#5m@r7Data@123!@#"; //  Key 
                byte[] _OriginalBytes = Encoding.UTF8.GetBytes(text);
                byte[] _EncryptedBytes = null;
                byte[] _PasswordBytes = Encoding.UTF8.GetBytes(_Password);

                // Hash the password with SHA256
                _PasswordBytes = SHA256.Create().ComputeHash(_PasswordBytes);

                // Generating salt bytes
                byte[] saltBytes = GetRandomBytes();

                // Appending salt bytes to original bytes
                byte[] bytesToBeEncrypted = new byte[saltBytes.Length + _OriginalBytes.Length];
                for (int i = 0; i < saltBytes.Length; i++)
                {
                    bytesToBeEncrypted[i] = saltBytes[i];
                }
                for (int i = 0; i < _OriginalBytes.Length; i++)
                {
                    bytesToBeEncrypted[i + saltBytes.Length] = _OriginalBytes[i];
                }

                _EncryptedBytes = AES_Encrypt(bytesToBeEncrypted, _PasswordBytes);

                return Convert.ToBase64String(_EncryptedBytes);
            }
            else
            {
                return text;
            }


        }

        /// <Summary>
        ///<Author> Decrypts string using AES SHA 256 algorithm</Descripton>
        /// </Summary>
        public static string Decrypt(string decryptedText)
        {
            if (!(string.IsNullOrEmpty(decryptedText)))
            {
                string _Password = @"5m@r7Data@123!@#5m@r7Data@123!@#"; //  Key 
                byte[] _BytesToBeDecrypted = Convert.FromBase64String(decryptedText);
                byte[] _PasswordBytes = Encoding.UTF8.GetBytes(_Password);

                // Hash the password with SHA256
                _PasswordBytes = SHA256.Create().ComputeHash(_PasswordBytes);

                byte[] decryptedBytes = AES_Decrypt(_BytesToBeDecrypted, _PasswordBytes);

                // Getting the size of salt
                int _saltSize = 4;

                // Removing salt bytes, retrieving original bytes
                byte[] originalBytes = new byte[decryptedBytes.Length - _saltSize];
                for (int i = _saltSize; i < decryptedBytes.Length; i++)
                {
                    originalBytes[i - _saltSize] = decryptedBytes[i];
                }

                return Encoding.UTF8.GetString(originalBytes);
            }
            else
            {
                return decryptedText;
            }
        }

        /// <Summary>
        ///<Descripton>generates random salt bytes</Descripton>
        /// </Summary>
        private static byte[] GetRandomBytes()
        {
            int _SaltSize = 4;
            byte[] ba = new byte[_SaltSize];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }
    }
}
