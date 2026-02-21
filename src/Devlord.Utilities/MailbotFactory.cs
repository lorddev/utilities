// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailbotFactory.cs" company="Lord Design">
//   © 2022 Lord Design
// </copyright>
// <license type="GPL-3.0">
//   You may use freely and commercially without modification; if you make changes, please share back to the
//   community.
// </license>
// <author>Aaron Lord</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Devlord.Utilities.Exceptions;
using Microsoft.Extensions.Options;

[assembly: InternalsVisibleTo("Devlord.Utilities.Tests")]
namespace Devlord.Utilities
{
    public class MailbotFactory : IMailbotFactory
    {
        private static readonly object DictionaryLock = new object();

        private static readonly Dictionary<string, Mailbot> Instances = new Dictionary<string, Mailbot>();

        private readonly DevlordOptions _options;
        
        public MailbotFactory(IOptionsMonitor<DevlordOptions> options)
        {
            // Note: This does not reload settings when they change because we're using a singleton.
            _options = options.CurrentValue;
        }
        
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public Mailbot GetMailbot(string name)
        {
            lock (DictionaryLock)
            {
                if (Instances.ContainsKey(name))
                {
                    return Instances[name];
                }

                var thisOptions = _options.MailSettings.FirstOrDefault(n => n.Name == name);

                if (thisOptions == null)
                {
                    throw new DevlordConfigurationException($"Missing mail options for name {name}");
                }
                
                var instance = new Mailbot
                {
                    SmtpServer = thisOptions.SmtpServer,
                    SmtpPort = thisOptions.SmtpPort,
                    SmtpLogin = thisOptions.SmtpLogin,
                    SmtpPassword = thisOptions.SmtpPassword,
                    Throttles = new Throttles(thisOptions.MaxPerMinute, thisOptions.MaxPerHour, thisOptions.MaxPerDay)
                };

                Instances.Add(name, instance);
                return instance;
            }
        }
    }
}