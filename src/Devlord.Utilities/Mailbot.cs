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

using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Devlord.Utilities
{
    /// <summary>
    /// Multi-threaded mailer to keep your systems running by sending messages asynchronously. If you're using it in a
    /// service that sends a lot of messages, be aware that you'll be limited by your .NET working threads.
    /// </summary>
    public class Mailbot
    {
        #region Fields

        /// <summary>
        /// The throttles.
        /// </summary>
        internal Throttles Throttles = new Throttles();

        #endregion
        
        #region Constructors and Destructors

        /// <summary>
        /// Private constructor to enforce use of singleton.
        /// </summary>
        internal Mailbot()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the SMTP server.
        /// </summary>
        public string SmtpServer { get; internal set; }

        public int SmtpPort { get; internal set; }
        public string SmtpLogin { get; internal set; }
        public string SmtpPassword { get; internal set; }

        #endregion

        #region Methods

        // Per minute 180, Per hour 3600, Per day 10,000
        public async Task QueueMail(Botmail message)
        {
            await SendMail(message);
        }


        // Per minute 180, Per hour 3600, Per day 10,000
        private async Task SendMail(Botmail botmail)
        {
            Throttles.Wait();

            var mm = new MimeMessage();
            mm.From.Add(MailboxAddress.Parse(botmail.Addresser));
            mm.Subject = botmail.Subject;
            mm.Body = new TextPart(botmail.Format.ToString().ToLower())
            {
                Text = botmail.Body
            };

            botmail.Addressees.ForEach(a => mm.To.Add(MailboxAddress.Parse(a)));
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(SmtpServer, SmtpPort, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(SmtpLogin, SmtpPassword);
                await client.SendAsync(mm);
                Throttles.Increment();
                client.Disconnect(true);
            }
        }

        #endregion

    }
}