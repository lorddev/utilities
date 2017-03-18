// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Crypt.cs" company="">
//   
// </copyright>
// <created>10/22/2012 3:42 PM</created>
// <author>aaron@lorddesign.net</author>
// <summary>
//   TODO: Update summary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Devlord.Utilities
{
    using System;
    using System.Security.Cryptography;
    
    using Encryptamajig;

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
        /// The <see cref="byte[]"/>.
        /// </returns>
        public static byte[] MakeKey()
        {
            var random = new byte[100];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random);
            return random;
        }

        public string HideSecretPassword(string secret)
        {
            return AesEncryptamajig.Encrypt(secret, Convert.ToBase64String(this.Key));
        }
        
        public string DecryptSecret(string cipher)
        {
            return AesEncryptamajig.Decrypt(cipher, Convert.ToBase64String(this.Key));
        }

        #endregion
    }
}