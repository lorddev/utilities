using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Devlord.Utilities.Cryptography;
using Devlord.Utilities.Exceptions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Devlord.Utilities.Tests
{
    public class MailTests : IClassFixture<DevlordTestConfiguration>
    {
        private readonly DevlordTestConfiguration _fixture;

        public MailTests(DevlordTestConfiguration fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public void EncryptPassword()
        {
            Debug.WriteLine("asdf");

            var crypt = new Crypt
            {
                Key =
                    new byte[]
                    {
                        80, 20, 245, 0, 124, 61, 192, 137, 232, 79, 249, 228, 1, 246, 187, 3, 228,
                        215, 250, 11, 131, 33, 180, 143, 41, 217, 4, 16, 219, 34, 50, 115, 96, 140,
                        146, 24, 5, 69, 58, 183, 66, 88, 58, 44, 213, 81, 26, 187, 247, 101, 163,
                        248, 103, 3, 179, 60, 14, 152, 90, 230, 92, 69, 100, 246, 32, 27, 201, 123,
                        99, 229, 66, 118, 185, 241, 136, 38, 174, 104, 203, 207, 4, 175, 223, 104,
                        140, 234, 20, 228, 209, 175, 94, 212, 105, 165, 47, 61, 100, 219, 18, 224
                    }
            };
            Debug.WriteLine(crypt.HideSecretPassword("AuOsVkTXzzrcq58RZ+AsOBzL5KwkztAyXB5ZQOIfOY7E"));
        }

        [Fact(Skip = "Dude, don't be sending emails from unit tests!")]
        public async Task TestMailbot()
        {
            var botMail = new Botmail
            {
                Addressees =
                    //new[] { "support@lorddesign.net", "success@simulator.amazonses.com" },
                    new[] { "success@simulator.amazonses.com" },
                Addresser = "support@lorddesign.net",
                Subject = "Test from lorddev admin",
                Body = "Test from lorddev admin",
                Ssl = true
            };
            //93alEy5RzF6W45ahn1XYZKnSwx/O48oVNychnDsV0k/jz36mfTipI2eRYdunpg5h

            try
            {
                var thisOptions = _fixture.Options.MailSettings.FirstOrDefault();

                if (thisOptions == null)
                {
                    throw new DevlordConfigurationException("Missing MailSettings for test project.");
                }
                
                await new Mailbot
                {
                    SmtpPort = thisOptions.SmtpPort,
                    SmtpLogin = thisOptions.SmtpLogin,
                    SmtpServer = thisOptions.SmtpServer,
                    EncryptedPassword = thisOptions.SmtpPassword
                }.QueueMail(botMail);
            }
            catch (Exception e)
            {
                if (!e.Message.Contains("message quota exceeded"))
                {
                    throw new MailException(e);
                }
            }
        }
    }

    public class MailException : Exception
    {
        internal MailException(Exception innerException) : base(innerException.Message, innerException)
        {
        }
    }
}