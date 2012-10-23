// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Botmail.cs" company="Lord Design">
//   (c) Lord Design. Modified GPL: You may use freely and commercially without modification; you can modify if result 
//   is also free.
// </copyright>
// <author>aaron@lorddesign.net</author>
// --------------------------------------------------------------------------------------------------------------------
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
        public string Addressee { get; set; }

        /// <summary>
        /// Gets or sets the addresser.
        /// </summary>
        public string Addresser { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
        #endregion
    }
}