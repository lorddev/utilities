using System;
using System.Diagnostics;
using System.Reflection;

namespace FizzWare.NBuilder.Extensions
{
    public static class MethodInfoExtensions
    {
        public static MethodInfo GetInfo(this Delegate @delegate)
        {
#if NETSTANDARD1_5
            return @delegate.GetMethodInfo();
#else
            return @delegate.Method;
#endif
        }
    }
}
