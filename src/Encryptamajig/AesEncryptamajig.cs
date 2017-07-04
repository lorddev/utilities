namespace Encryptamajig
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;

    /// <summary>
    /// A simple wrapper to the AesManaged class and the AES algorithm.
    /// Requires a securely stored key which should be a random string of characters that an attacker could never guess.
    /// Make sure to save the Key if you want to decrypt your data later!
    /// If you're using this with a Web app, put the key in the web.config and encrypt the web.config.
    /// </summary>
    public static class AesEncryptamajig
    {
        private const int SaltSize = 32;

        /// <summary>
        /// Encrypts the plainText input using the given Key.
        /// A 256-bit random salt will be generated and prepended to the cipherText before it is base64 encoded.
        /// </summary>
        /// <param name="plainText">The plain text to encrypt.</param>
        /// <param name="key">The plain text encryption key.</param>
        /// <returns>The salt and the cipherText, Base64 encoded for convenience.</returns>
        public static string Encrypt(string plainText, string key)
        {
            CheckInput(plainText, "plainText");
            CheckInput(key, "key");

            // Derive a new Salt and IV from the Key
            var bytes = DeriveBytes(key);

            // Return the encrypted keys from the memory stream, in Base64 form so we can send it right to a database (if we want).
            var cipherTextBytes = EncryptStream(plainText, bytes);
            var buffer = bytes.Salt;
            Array.Resize(ref buffer, buffer.Length + cipherTextBytes.Length);
            Array.Copy(cipherTextBytes, 0, buffer, SaltSize, cipherTextBytes.Length);

            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Decrypts the cipherText using the Key.
        /// </summary>
        /// <param name="cipherText">The cipherText to decrypt.</param>
        /// <param name="key">The plain text encryption key.</param>
        /// <returns>The decrypted text.</returns>
        public static string Decrypt(string cipherText, string key)
        {
            CheckInput(cipherText, "cipherText");
            CheckInput(key, "key");

            // Extract the salt from our cipherText
            var allTheBytes = Convert.FromBase64String(cipherText);
            var saltBytes = allTheBytes.Take(SaltSize).ToArray();
            var ciphertextBytes = allTheBytes.Skip(SaltSize).Take(allTheBytes.Length - SaltSize).ToArray();

            var bytes = DeriveBytes(key, saltBytes);
            return DecryptStream(ciphertextBytes, bytes);
        }

        private static bool CheckInput(string field, string fieldName)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentNullException(fieldName);
            }

            return field.Trim().Length > 0;
        }

        // Supress warnings due to .NET 3.5 compatibility
        // ReSharper disable SuspiciousTypeConversion.Global
        #pragma warning disable S1944 // Inappropriate casts should not be made
        private static DeriveResult DeriveBytes(string key)
        {
            var deriver = new Rfc2898DeriveBytes(key, SaltSize);
            var result = new DeriveResult
                {
                    Salt = deriver.Salt,
                    Key = deriver.GetBytes(32),
                    InitializationVector = deriver.GetBytes(16)
                };
            ((IDisposable)deriver).Dispose();
            return result;
        }

        private static DeriveResult DeriveBytes(string key, byte[] saltBytes)
        {
            var deriver = new Rfc2898DeriveBytes(key, saltBytes);
         
            // Derive the previous IV from the Key and Salt
            var result = new DeriveResult
                {
                    Key = deriver.GetBytes(32),
                    InitializationVector = deriver.GetBytes(16)
                };

            ((IDisposable)deriver).Dispose();
            return result;
        }

        private static byte[] EncryptStream(string clearText, DeriveResult bytes)
        {
             // Create an encryptor to perform the stream transform.
            // Create the streams used for encryption.
            using (var aesManaged = new AesManaged())
            using (var encryptor = aesManaged.CreateEncryptor(bytes.Key, bytes.InitializationVector))
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (var streamWriter = new StreamWriter(cryptoStream))
                {
                    // Send the data through the StreamWriter, through the CryptoStream, to the underlying MemoryStream
                    streamWriter.Write(clearText);

                    // vv Have to dispose of the SW here in order to maintain the integrity of the memoryStream.
                }
               
                // Return the encrypted keys from the memory stream, in Base64 form so we can send it right to a database (if we want).
                var cipherTextBytes = memoryStream.ToArray();
                return cipherTextBytes;
            }
        }

        private static string DecryptStream(byte[] cipher, DeriveResult keys)
        {
            // Create a decrytor to perform the stream transform.
            // Create the streams used for decryption.
            // The default Cipher Mode is CBC and the Padding is PKCS7 which are both good
            using (var aesManaged = new AesManaged())
            using (var decryptor = aesManaged.CreateDecryptor(keys.Key, keys.InitializationVector))
            using (var memoryStream = new MemoryStream(cipher))
            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            using (var streamReader = new StreamReader(cryptoStream))
            {
                // Return the decrypted keys from the decrypting stream.
                return streamReader.ReadToEnd();
            }
        }
    }
}
