using System;
using System.Linq;
using System.Threading.Tasks;
using Devlord.Utilities.Exceptions;
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
                    SmtpPassword = thisOptions.SmtpPassword
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