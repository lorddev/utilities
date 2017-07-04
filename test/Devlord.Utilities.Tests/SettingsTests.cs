using System;
using Xunit;

namespace Devlord.Utilities.Tests
{
    public class SettingsTests
    {
        [Fact]
        public void SettingsTest()
        {
            Exception thrown = null;
            try
            {
                Settings.Default.SmtpPort.ShouldEqual(587);
            }
            catch (Exception e)
            {
                thrown = e;
            }

            Assert.False(thrown is ArgumentException, "ArgumentException was thrown");
            Assert.Null(thrown);
        }
        
        [Fact(Skip="blech")]
        public void TestDonaldsHair()
        {
            Devlord.Utilities.Settings.Default.SmtpPassword.ShouldEqual("Donald's Hair");
        }
    }
}