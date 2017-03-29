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
using System.Net;
using MailKit;
using System.Threading.Tasks;
using Devlord.Utilities.Properties;

namespace Devlord.Utilities
{
    /// <summary>
    ///     Multi-threaded mailer to keep your systems running by sending messages asynchronously. If you're using it in a
    ///     service that sends a lot of messages, be aware that you'll be limited by your .NET working threads.
    /// </summary>
    public class Mailbot
    {
#region Fields

        private readonly Crypt _crypt = new Crypt();

        /// <summary>
        ///     The throttles.
        /// </summary>
        private readonly Throttles _throttles = new Throttles();

#endregion

#region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="Mailbot" /> class.
        /// </summary>
        static Mailbot()
        {
            Instance = new Mailbot();
        }

        /// <summary>
        ///     Prevents a default instance of the <see cref="Mailbot" /> class from being created.
        /// </summary>
        private Mailbot()
        {
            // Doesn't do anything except prevent instantiation outside of the singleton.
            SmtpServer = Settings.Default.SmtpServer;
            _crypt.Key = new byte[]
                             {
                                 80, 20, 245, 0, 124, 61, 192, 137, 232, 79, 249, 228, 1, 246, 187, 3, 228, 215, 250, 11,
                                 131, 33, 180, 143, 41, 217, 4, 16, 219, 34, 50, 115, 96, 140, 146, 24, 5, 69, 58, 183, 66
                                 , 88, 58, 44, 213, 81, 26, 187, 247, 101, 163, 248, 103, 3, 179, 60, 14, 152, 90, 230, 92
                                 , 69, 100, 246, 32, 27, 201, 123, 99, 229, 66, 118, 185, 241, 136, 38, 174, 104, 203, 207
                                 , 4, 175, 223, 104, 140, 234, 20, 228, 209, 175, 94, 212, 105, 165, 47, 61, 100, 219, 18,
                                 224
                             };
        }

#endregion

#region Public Properties

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        public static Mailbot Instance { get; private set; }

        /// <summary>
        ///     Gets the SMTP server.
        /// </summary>
        public string SmtpServer { get; private set; }

#endregion

        // Per minute 180, Per hour 3600, Per day 10,000

#region Public Methods and Operators

        public async Task QueueMail(Botmail message)
        {
            await SendMail(message);
        }

#endregion

#region Methods

        private async Task SendMail(object message)
        {
            var botmail = message as Botmail;
            _throttles.Wait();

            var client = new SmtpClient
                             {
                                 Host = SmtpServer,
                                 Credentials =
                                     new NetworkCredential(
                                     Settings.Default.SmtpLogin,
                                     _crypt.DecryptSecret(Settings.Default.SmtpPassword)),
                                 EnableSsl = botmail.Ssl,
                                 Port = Settings.Default.SmtpPort
                             };

            var mm = new MailMessage
                         {
                             From = new MailAddress(botmail.Addresser),
                             Subject = botmail.Subject,
                             Body = botmail.Body
                         };

            botmail.Addressees.ForEach(a => mm.To.Add(new MailAddress(a)));

            await Task.Run(() => 
            {
                client.Send(mm); 
                _throttles.Increment();
                mm.Dispose();
                client.Dispose();
            });
        }

#endregion
    }
}

#endif