using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Devlord.Utilities.Text;
using NUnit.Framework;

namespace Devlord.Utilities.Tests
{
    [TestFixture]
    public class StringParserTests
    {
        #region Public Methods and Operators

        [Test]
        public void TestIntParse()
        {
            IEnumerable<int> expected = new[] { 24 }.AsEnumerable();
            string target = "24 miles";

            string format = "{0} miles";

            IEnumerable<int> actual = StringFormat.Parse<int>(format, target);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDecimalParse()
        {
            var expected = new[] { 24.1M }.AsEnumerable();
            string target = "24.1 miles";

            string format = "{0} miles";
            
            var actual = StringFormat.Parse<decimal>(format, target);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestDoubleParse()
        {
            var expected = new[] { 24.1D }.AsEnumerable();
            string target = "24.1 miles";

            string format = "{0} miles";

            var actual = StringFormat.Parse<double>(format, target);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}