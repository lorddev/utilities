// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Crypt.cs" company="Lord Design">
//   © 2017 Lord Design
// </copyright>
// <license type="GPL-3.0">
//   You may use freely and commercially without modification; if you make changes, please share back to the
//   community.
// </license>
// <author>Aaron Lord</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Text;
using Encryptamajig;
using BC = BCrypt.Net.BCrypt;

namespace Devlord.Utilities.Cryptography
{
    /// <summary>
    /// A basic wrapper for crucial encryption algorithms.
    /// </summary>
    public class Crypt
    {
        #region Fields

        public byte[] Key { get; set; }

        public static UTF8Encoding SafeUtf8 = new UTF8Encoding(false, true);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Run this method in a Test project to quickly get a new key of strong random bytes. Iterate through the byte
        /// with a StringBuilder to construct your private field instantiator.
        /// </summary>
        /// <returns>
        /// The <see cref="byte" />.
        /// </returns>
        public static byte[] MakeKey()
        {
            var random = new byte[100];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(random);
            return random;
        }

        public string HideSecretPassword(string secret)
        {
            return AesEncryptamajig.Encrypt(secret, Convert.ToBase64String(Key));
        }

        public string DecryptSecret(string cipher)
        {
            return AesEncryptamajig.Decrypt(cipher, Convert.ToBase64String(Key));
        }

        public string OneWay(string cleartext)
        {
            var bcrypt = BC.HashPassword(cleartext);
            return bcrypt;
        }

        /// <summary>
        /// Todo: BCrypt expects string for cleartext, but that's not secure due to GC issues with strings in memory.
        /// Need to "override" BCrypt's behavior here, or somehow access their private static method.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="storedCipherText"></param>
        /// <returns></returns>
        public bool Verify(byte[] input, string storedCipherText)
        {
            return BC.Verify(SafeUtf8.GetString(input), storedCipherText);
        }
        #endregion
    }
}