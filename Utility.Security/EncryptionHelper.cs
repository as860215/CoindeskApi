using Jose;
using System.Security.Cryptography;
using System.Text;

namespace Utility.Security
{
    /// <summary>加解密</summary>
    public static class EncryptionHelper
    {
        /// <summary>加密用金鑰</summary>
        private const string key = "Capoo1234567890KeyBy----as860215";

        /// <summary>加密</summary>
        /// <param name="plainText">加密文字</param>
        public static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cryptoStream))
            {
                writer.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>解密</summary>
        /// <param name="cipherText">密文</param>
        public static string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
            using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
    }
}
