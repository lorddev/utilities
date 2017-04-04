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
        public static Settings Default { get; } = new Settings();

        private readonly IConfiguration _configuration;

        private Settings()
        {
            _configuration = GetConfig();
        }

        public int SmtpPort => GetValue<int>("Devlord.Utilities:SmtpPort");
        public string SmtpLogin => GetValue<string>("Devlord.Utilities:SmtpLogin");
        public string SmtpPassword => GetValue<string>("Devlord.Utilities:SmtpPassword");

        private static IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder()
#if NET451
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
#else
                .SetBasePath(AppContext.BaseDirectory)
#endif
                .AddJsonFile("devlord.utilities.json",
                    true,
                    true);

            return builder.Build();
        }

        public T GetValue<T>(string propertyName)
        {
            var value = _configuration[propertyName];
            if (value != null)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            throw new SettingNotFoundException(propertyName);
        }
    }
}