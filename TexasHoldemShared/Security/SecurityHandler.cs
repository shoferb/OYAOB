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
        private readonly byte[] _iv;
        private const string KeyStr = "25B4895DF77AFtkv";
        private readonly byte[] _key;
        private readonly AesManaged _aes;
        private readonly ICryptoTransform _encryptor;
        private readonly ICryptoTransform _decryptor;

        public SecurityHandler()
        {
            //generate 128 bit key and IV from the strings
            _iv = Encoding.UTF8.GetBytes(IvStr);
            _key = Encoding.UTF8.GetBytes(KeyStr);

            _aes = new AesManaged
            {
                IV = _iv,
                Key = _key
            };
            _encryptor = _aes.CreateEncryptor();
            _decryptor = _aes.CreateDecryptor();
        }

        public long GenerateNewSessionId()
        {
            return DateTime.Now.ToFileTime();
        }

        public byte[] Encrypt(byte[] data)
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
            return encrypted;
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
                        plainText = srDecrypt.ReadToEnd();
                    }
                }
            }
            return plainText;
        }
    }
}
