// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Lord Design">
//   © Lord Design
// </copyright>
// <license type="GPL">
//   You may use freely and commercially without modification; you can modify if result 
//   is also free.
// </license>
// <summary>
// </summary>
// <author>Aaron Lord</author>
// --------------------------------------------------------------------------------------------------------------------

namespace Devlord.Utilities
{
    public class DevlordOptions
    {
        public string GoogleMapsApiKey { get; set; }
        
        public DevlordMailSettings[] MailSettings { get; set; }
    }
}