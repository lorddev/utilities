// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mailbot.cs" company="Lord Design">
//   © Lord Design. Modified GPL: You may use freely and commercially without modification; you can modify if result 
//   is also free.
// </copyright>
// <summary>
//   Multi-threaded mailer to keep your systems running by sending messages asynchronously. If you're using it in a
//   service that sends a lot of messages, be aware that you'll be limited by your .NET working threads.
// </summary>
// <author>aaron@lorddesign.net</author>
// --------------------------------------------------------------------------------------------------------------------

#if NETSTANDARD1_5
using System;
using System.Threading.Tasks;
using Devlord.Utilities.Properties;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Devlord.Utilities
{
    /// <summary>
    /// Multi-threaded mailer to keep your systems running by sending messages asynchronously. If you're using it in a
    /// service that sends a lot of messages, be aware that you'll be limited by your .NET working threads.
    /// </summary>
    public partial class Mailbot
    {
        #region Methods

        // Per minute 180, Per hour 3600, Per day 10,000
        private async Task SendMail(object message)
        {
            var botmail = message as Botmail;
            if (botmail == null)
            {
                throw new ArgumentException("Message must be of type Botmail");
            }

            _throttles.Wait();

            var mm = new MimeMessage();
            mm.From.Add(new MailboxAddress(botmail.Addresser));
            mm.Subject = botmail.Subject;
            mm.Body = new TextPart(botmail.Format.ToString().ToLower())
            {
                Text = botmail.Body
            };

            botmail.Addressees.ForEach(a => mm.To.Add(new MailboxAddress(a)));
            using (var client = new SmtpClient())
            {
                client.Connect(SmtpServer, Settings.Default.SmtpPort, SecureSocketOptions.SslOnConnect);
                client.Authenticate(Settings.Default.SmtpLogin,
                    _crypt.DecryptSecret(Settings.Default.SmtpPassword));
                await client.SendAsync(mm);
                _throttles.Increment();
                client.Disconnect(true);
            }
        }

        #endregion
    }
}

#endif