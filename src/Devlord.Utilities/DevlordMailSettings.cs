// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DevlordMailSettings.cs" company="Lord Design">
//   © 2022 Lord Design
// </copyright>
// <license type="GPL-3.0">
//   You may use freely and commercially without modification; if you make changes, please share back to the
//   community.
// </license>
// <author>Aaron Lord</author>
// --------------------------------------------------------------------------------------------------------------------

namespace Devlord.Utilities
{
    public class DevlordMailSettings
    {
        public string Name { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpLogin { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpServer { get; set; }
        public int MaxPerMinute { get; set; }
        public int MaxPerHour { get; set; }
        public int MaxPerDay { get; set; }
    }
}