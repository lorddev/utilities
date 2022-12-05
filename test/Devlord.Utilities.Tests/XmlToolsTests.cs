using System.Linq;
using Xunit;

namespace Devlord.Utilities.Tests
{
    public class XmlToolsTests
    {
        [Fact]
        public void ShouldSerializeObject()
        {
            var testObj = new TestData { Id = 45 };
            var stringResults = testObj.ToXmlString();

            // Different versions of the framework serialize attributes in a different order.
            var expected = new[]
            {
                @"<TestData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                + @"xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><Id>45</Id>"
                + "<LastSeen>0001-01-01T00:00:00</LastSeen></TestData>",
                @"<TestData xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" "
                + @"xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><Id>45</Id>"
                + "<LastSeen>0001-01-01T00:00:00</LastSeen></TestData>"
            };
            Assert.True(expected.Contains(stringResults), "Format of serialized string is not expected. "
                   + $"Expected \r\n{expected[0]}\r\nOr\r\n{expected[1]}\r\nBut got:\r\n{stringResults}");
        }
    }
}