using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FizzWare.NBuilder.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetTypeWithoutNullability(this Type t)
        {
            return t.GetTypeInfo().IsGenericType &&
                   t.GetGenericTypeDefinition() == typeof(Nullable<>)
                       ? t.GetTypeInfo().GetGenericArguments().Single()
                       : t;
        }

        public static IList<MemberInfo> GetPublicInstancePropertiesAndFields(this Type t)
        {
            var memberInfos = new List<MemberInfo>();
            memberInfos.AddRange(t.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance));
            memberInfos.AddRange(t.GetTypeInfo().GetFields());
            return memberInfos;
        }

#if NETSTANDARD1_5

        public static PropertyInfo[] GetProperties(this Type type, BindingFlags flags)
        {
            return type.GetTypeInfo().GetProperties(flags);
        }

        public static FieldInfo[] GetFields(this Type type)
        {
            return type.GetTypeInfo().GetFields();
        }
#endif
#if NET35
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }
#endif

    }
}
