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
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Devlord.Utilities
{
    public class DevlordSettings
    {
        public string GoogleMapsApiKey { get; set; }
        private static readonly IConfiguration _configuration = GetConfig();
        
        public static DevlordSettings Default { get; } = new DevlordSettings();

        public int SmtpPort => int.Parse(GetValue("SmtpPort"));
        public string SmtpLogin => GetValue("SmtpLogin");
        public string SmtpPassword => GetValue("SmtpPassword");
        
        private static IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json",
                    true,
                    true);
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