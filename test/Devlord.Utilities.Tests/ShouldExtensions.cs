using NUnit.Framework;

namespace Devlord.Utilities.Tests
{
    public static class ShouldExtensions
    {
        public static void ShouldBeInRange(this double actual, double lbound, double ubound)
        {
            Assert.GreaterOrEqual(actual, lbound);
            Assert.LessOrEqual(actual, ubound);
        }
    }
}