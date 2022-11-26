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
using System.Runtime.CompilerServices;
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
        public Mailbot GetMailbot(string smtpServer)
        {
            smtpServer = smtpServer.ToLower();
            lock (DictionaryLock)
            {
                if (Instances.ContainsKey(smtpServer))
                {
                    return Instances[smtpServer];
                }

                var instance = new Mailbot
                {
                    SmtpServer = smtpServer,
                    SmtpPort = _options.SmtpPort,
                    SmtpLogin = _options.SmtpLogin,
                    EncryptedPassword = _options.SmtpPassword
                };
                if (smtpServer == "smtp.gmail.com")
                {
                    instance.Throttles = new GmailThrottles();
                }

                instance.SmtpServer = smtpServer;
                Instances.Add(smtpServer, instance);
                return instance;
            }
        }
    }
}