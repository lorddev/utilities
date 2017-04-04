using System;
using Xunit;

namespace Devlord.Utilities.Tests
{
    public class SettingsTests
    {
        [Fact]
        public void SettingsTest()
        {
            Settings.Default.GetValue<int>("Devlord.Utilities:SmtpPort").ShouldEqual(587);
        }

        [Fact]
        public void SettingsUriExceptionTest()
        {
            Exception thrown = null;
            try
            {
                Settings.Default.GetValue<int>("Devlord.Utilities:SmtpPort").ShouldEqual(587);
            }
            catch (Exception e)
            {
                thrown = e;
            }

            Assert.False(thrown is ArgumentException, "ArgumentException was thrown");
            Assert.Null(thrown);
        }
    }
}