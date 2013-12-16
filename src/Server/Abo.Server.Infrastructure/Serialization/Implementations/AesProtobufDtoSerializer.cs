using System;
using System.IO;
using System.Security.Cryptography;

namespace Abo.Server.Infrastructure.Serialization.Implementations
{
    public class AesProtobufDtoSerializer : ProtobufDtoSerializer
    {
        public override byte[] Serialize<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                ProtobufModel.Serialize(ms, obj);
                var bytes = ms.ToArray();
                return EncryptBytes(bytes, null, null);
            }
        }

        public override T Deserialize<T>(byte[] bytes)
        {
            var decryptedBytes = DecryptBytes(bytes, null, null);
            using (var ms = new MemoryStream(decryptedBytes))
            {
                return (T)ProtobufModel.Deserialize(ms, null, typeof(T));
            }
        }

        private static byte[] EncryptBytes(byte[] data, byte[] key, byte[] iv)
        {
            // Check arguments. 
            if (data == null || data.Length <= 0)
                throw new ArgumentNullException("data");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");
            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption. 
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(data);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return encrypted;
        }

        private static byte[] DecryptBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        var length = csDecrypt.Length;
                        var result = new byte[length];
                        csDecrypt.Read(result, 0, (int)length);
                        return result;
                    }
                }

            }
        }
    }
}
