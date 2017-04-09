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

using System;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Devlord.Utilities
{
    public class Settings
    {
        private static IConfiguration _configuration = GetConfig();
        
        public static Settings Default { get; } = new Settings();

        public int SmtpPort => int.Parse(GetValue("SmtpPort"));
        public string SmtpLogin => GetValue("SmtpLogin");
        public string SmtpPassword => GetValue("SmtpPassword");
        
        private static IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder()
#if NET451
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
#else
                .SetBasePath(AppContext.BaseDirectory)
#endif
                .AddJsonFile("appsettings.json",
                    true,
                    true);
            //builder.AddUserSecrets("22b9d517-6954-4beb-b7be-ba24eb9ac441");
            return builder.Build();
        }

        private static string GetValue(string propertyName)
        {
            var value = _configuration["Devlord.Utilities:" + propertyName];
            if (value == null)
            {
                throw new SettingNotFoundException(propertyName);
            }
            return value;
        }
    }
}