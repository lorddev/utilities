// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Botmail.cs" company="Lord Design">
//   (c) Lord Design. Modified GPL: You may use freely and commercially without modification; you can modify if result 
//   is also free.
// </copyright>
// <author>aaron@lorddesign.net</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace LordDesign.Utilities
{
    /// <summary>
    /// The botmail.
    /// </summary>
    public class Botmail
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the addressee.
        /// </summary>
        public IEnumerable<string> Addressees { get; set; }

        /// <summary>
        /// Gets or sets the addresser.
        /// </summary>
        public string Addresser { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool Ssl { get; set; }

        #endregion
    }
}