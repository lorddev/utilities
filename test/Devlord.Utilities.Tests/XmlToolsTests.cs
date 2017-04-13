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
            stringResults.ShouldEqual(@"<TestData xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                                      + @"xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><Id>45</Id>"
                                      + "<LastSeen>0001-01-01T00:00:00</LastSeen></TestData>");
        }
    }
}