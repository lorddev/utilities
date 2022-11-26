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
using Devlord.Utilities.Cryptography;
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

        private readonly Crypt _crypt = new Crypt();

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
            _crypt.Key = new byte[]
            {
                80, 20, 245, 0, 124, 61, 192, 137, 232, 79, 249, 228, 1, 246, 187, 3, 228, 215, 250, 11,
                131, 33, 180, 143, 41, 217, 4, 16, 219, 34, 50, 115, 96, 140, 146, 24, 5, 69, 58, 183, 66, 88, 58, 44,
                213, 81, 26, 187, 247, 101, 163, 248, 103, 3, 179, 60, 14, 152, 90, 230, 92, 69, 100, 246, 32, 27, 201,
                123, 99, 229, 66, 118, 185, 241, 136, 38, 174, 104, 203, 207, 4, 175, 223, 104, 140, 234, 20, 228, 209,
                175, 94, 212, 105, 165, 47, 61, 100, 219, 18,
                224
            };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the SMTP server.
        /// </summary>
        public string SmtpServer { get; internal set; }

        public int SmtpPort { get; internal set; }
        public string SmtpLogin { get; internal set; }
        public string EncryptedPassword { get; internal set; }

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
            mm.From.Add(new MailboxAddress(botmail.Addresser));
            mm.Subject = botmail.Subject;
            mm.Body = new TextPart(botmail.Format.ToString().ToLower())
            {
                Text = botmail.Body
            };

            botmail.Addressees.ForEach(a => mm.To.Add(new MailboxAddress(a)));
            using (var client = new SmtpClient())
            {
                client.Connect(SmtpServer, SmtpPort, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(SmtpLogin,
                    _crypt.DecryptSecret(EncryptedPassword));
                await client.SendAsync(mm);
                Throttles.Increment();
                client.Disconnect(true);
            }
        }

        #endregion

    }
}