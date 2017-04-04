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
using Encryptamajig;

namespace Devlord.Utilities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Crypt
    {
        #region Fields

        public byte[] Key { get; set; }

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

        #endregion
    }
}