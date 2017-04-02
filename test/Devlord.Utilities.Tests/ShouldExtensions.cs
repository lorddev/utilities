using Xunit;

namespace Devlord.Utilities.Tests
{
    public static class ShouldExtensions
    {
        public static void ShouldBeInRange(this double actual, double lbound, double ubound)
        {
            Assert.InRange(actual, lbound, ubound);
        }

        public static void ShouldBeInRange(this int actual, int lbound, int ubound)
        {
            Assert.InRange(actual, lbound, ubound);
        }

        public static void ShouldEqual(this object actual, object expected)
        {
            Assert.Equal(expected, actual);
        }

        public static void ShouldBeNull<T>(this T actual) where T : class
        {
            Assert.Null(actual);
        }
    }
}