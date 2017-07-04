// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailThrottle.cs" company="Lord Design">
//   © Lord Design. Modified GPL: You may use freely and commercially without modification; you can modify if result 
//   is also free.
// </copyright>
// <author>aaron@lorddesign.net</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Devlord.Utilities
{
    /// <summary>
    /// Provides strong-typed results for data returned from ADO data readers
    /// </summary>
    /// <remarks>
    /// Similar to what EntityFramework and Automapper  do, but with less "startup" overhead. It's useful when working on
    /// small projects that don't have the EF scaffolding in place.
    /// See http://improve.dk/performance-comparison-reading-data-from-the-database-strongly-typed/
    /// </remarks>
    public class DRMapper
    {
        public static List<T> ParseList<T>(IDataReader dr)
        {
            var list = new List<T>();

            var properties = typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public |
                                                     BindingFlags.Instance);

            var fieldTranslator = new TypeMatcher<T>(properties).VerifyTypeMatch(dr).GetFieldTranslator();

            while (dr.Read())
            {
                var instance = Activator.CreateInstance<T>();
                foreach (var pi in properties)
                {
                    var fieldName = fieldTranslator[pi.Name.ToLowerInvariant()];
                    if (dr[fieldName] != DBNull.Value)
                        pi.SetValue(instance, dr[fieldTranslator[pi.Name.ToLowerInvariant()]], null);
                }

                list.Add(instance);
            }

            return list;
        }


        [Obsolete("This feature has been deprecated. The workaround is to sort in your query.", true)]
        public static T ParseRecord<T>(IDataReader dr, int rowIndex)
        {
            throw new NotImplementedException(
                "This feature has been deprecated. The workaround is to sort in your query.");
        }

        public static T ParseRecord<T>(IDataReader dr)
        {
            var properties = typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public |
                                                     BindingFlags.Instance);

            var fieldTranslator = new TypeMatcher<T>(properties).VerifyTypeMatch(dr).GetFieldTranslator();

            while (dr.Read())
            {
                var instance = Activator.CreateInstance<T>();
                foreach (var pi in properties)
                {
                    var fieldName = fieldTranslator[pi.Name.ToLowerInvariant()];
                    if (dr[fieldName] != DBNull.Value)
                        pi.SetValue(instance, dr[fieldName], null);
                }

                return instance;
            }

            return default(T);
        }
    }
}