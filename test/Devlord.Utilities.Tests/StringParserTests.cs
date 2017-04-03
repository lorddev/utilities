using System.Linq;
using Devlord.Utilities.Text;
using Xunit;

namespace Devlord.Utilities.Tests
{
    public class StringParserTests
    {
        [Fact]
        public void TestDecimalParse()
        {
            var expected = new[] { 24.1M }.AsEnumerable();
            var target = "24.1 miles";

            var format = "{0} miles";

            var actual = StringFormat.Parse<decimal>(format, target);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestDoubleParse()
        {
            var expected = new[] { 24.1D }.AsEnumerable();
            var target = "24.1 miles";

            var format = "{0} miles";

            var actual = StringFormat.Parse<double>(format, target);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestIntParse()
        {
            var expected = new[] { 24 }.AsEnumerable();
            const string target = "24 miles";

            const string format = "{0} miles";

            var actual = StringFormat.Parse<int>(format, target);

            Assert.Equal(expected, actual);
        }
    }
}