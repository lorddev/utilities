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


#if NET451
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

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

            using (var mm = new MailMessage
                            {
                                From = new MailAddress(botmail.Addresser),
                                Subject = botmail.Subject,
                                Body = botmail.Body,
                                IsBodyHtml = botmail.Format == MailFormat.Html
                            })
            {
                botmail.Addressees.ForEach(a => mm.To.Add(new MailAddress(a)));

                using (var client = new SmtpClient
                                        {
                                            Host = SmtpServer,
                                            Credentials =
                                                new NetworkCredential(
                                                    Settings.Default.SmtpLogin,
                                                    _crypt.DecryptSecret(Settings.Default.SmtpPassword)),
                                            EnableSsl = botmail.Ssl,
                                            Port = Settings.Default.SmtpPort
                                        })
                {
                    await client.SendMailAsync(mm);
                    _throttles.Increment();
                }
            }
        }

        #endregion
    }
}


#endif