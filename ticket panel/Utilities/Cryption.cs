using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Sanay.NetFlow.Library.Class
{
    public static class Cryption
    {
        private static string EncryptionKey
        {
            get
            {
                return "{^_^}_";
            }
        }
        public static string Encrypt(string value, string key)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (string.IsNullOrEmpty(key))
            {
                key = String.Empty;
            }
            // Get the bytes of the string
            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(value);
            //var keyBytes = Encoding.UTF8.GetBytes(key);
            var bytesEncrypted = EncryptStringToBytes_Aes(value, EncryptionKey + key);
            return Convert.ToBase64String(bytesEncrypted);
        }
        public static string Decrypt(string value, string key)
        {
            //DECRYPT FROM CRYPTOJS
            var encrypted = Convert.FromBase64String(value);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, EncryptionKey + key);
            return decriptedFromJavascript;
        }

        public static string DecryptHex(string value, string key)
        {
            byte[] raw = new byte[value.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
            }

            var encrypted = raw;
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, key);
            return decriptedFromJavascript;
        }
        private static byte[] EncryptStringToBytes_Aes(string plainText, string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var IV = Encoding.UTF8.GetBytes(key.Substring(0, 16));
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (keyBytes == null || keyBytes.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Aes object

            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                //aesAlg.KeySize = keyBytes.Length * 8;
                aesAlg.Key = keyBytes;
                aesAlg.IV = IV;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
        private static string DecryptStringFromBytes(byte[] cipherText, string key)
        {
            var keybytes = Encoding.UTF8.GetBytes(key);
            var iv = Encoding.UTF8.GetBytes(key.Substring(0, 16));
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (keybytes == null || keybytes.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;
                rijAlg.Key = keybytes;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                try
                {
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch (Exception e)
                {

                    return "null";
                }
            }
            return plaintext;
        }

    }
}
