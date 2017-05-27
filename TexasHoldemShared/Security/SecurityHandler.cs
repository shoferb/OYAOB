using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldemShared.Security
{
    public class SecurityHandler : ISecurity
    {
        private const string IvStr = "4DC9F8AFA6D2Cahd";
        private const string KeyStr = "25B4895DF77AFtkv";
        private readonly ICryptoTransform _encryptor;
        private readonly ICryptoTransform _decryptor;

        public SecurityHandler()
        {
            var iv = Encoding.UTF8.GetBytes(IvStr);
            var key = Encoding.UTF8.GetBytes(KeyStr);

            var aes = new AesManaged
            {
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                Key = key,
                IV = iv,
            };
            _encryptor = aes.CreateEncryptor();
            _decryptor = aes.CreateDecryptor();
        }

        public long GenerateNewSessionId()
        {
            return DateTime.Now.ToFileTime();
        }

        public byte[] Encrypt(string data)
        {
            byte[] encrypted;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, _encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(data);
                    }
                }
                encrypted = msEncrypt.ToArray();
            } 
            return encrypted;
        }

        public string EncryptToString(string data)
        {
            byte[] encrypted;
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, _encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(data);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            } 
            return Encoding.UTF8.GetString(encrypted);
        }

        public string Decrypt(byte[] data)
        {
            string plainText;

            using (MemoryStream msDecrypt = new MemoryStream(data))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, _decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream 
                        // and place them in a string.
                        plainText = srDecrypt.ReadToEnd();
                    }
                }
            }
            return plainText;
        }
    }
}
