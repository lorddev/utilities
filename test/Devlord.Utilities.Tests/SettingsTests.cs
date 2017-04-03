using Devlord.Utilities;
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
    }
}