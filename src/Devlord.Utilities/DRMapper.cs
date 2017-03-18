// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailThrottle.cs" company="Lord Design">
//   (c) Lord Design. Modified GPL: You may use freely and commercially without modification; you can modify if result 
//   is also free.
// </copyright>
// <author>aaron@lorddesign.net</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Devlord.Utilities
{
    /// <summary>
    /// Provides strong-typed results for data returned from ADO data readers
    /// </summary>
    /// <remarks>Similar to what EntityFramework and Automapper  do, but with less "startup" overhead. It's useful when working on 
    /// small projects that don't have the EF scaffolding in place.
    /// See http://improve.dk/performance-comparison-reading-data-from-the-database-strongly-typed/
    /// </remarks>
    public class DRMapper
    {
        public static List<T> ParseList<T>(SqlDataReader dr)
        {
            var list = new List<T>();

            var properties = typeof(T).GetProperties();
            var instance = Activator.CreateInstance<T>();

            while (dr.Read())
            {
                foreach (var pi in properties)
                {
                    pi.SetValue(instance, dr[pi.Name], null);
                }

                list.Add(instance);
            }

            return list;
        }

        public static T ParseRecord<T>(SqlDataReader dr, int rowIndex = 0)
        {
            var properties = typeof(T).GetProperties();
            var instance = Activator.CreateInstance<T>();

            var currentRow = 0;
            while (dr.Read())
            {
                if (currentRow < rowIndex)
                {
                    currentRow++;
                    continue;
                }

                foreach (var pi in properties)
                {
                    pi.SetValue(instance, dr[pi.Name], null);
                }

                return instance;
            }

            return default(T);
        }
    }
}