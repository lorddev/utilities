// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailThrottle.cs" company="Lord Design">
//   (c) Lord Design. Modified GPL: You may use freely and commercially without modification; you can modify if result 
//   is also free.
// </copyright>
// <author>aaron@lorddesign.net</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Devlord.Utilities.Resources;

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
        public static List<T> ParseList<T>(IDataReader dr)
        {
            var list = new List<T>();

            var properties = typeof(T).GetProperties();

            VerifyTypeMatch<T>(dr, properties);

            while (dr.Read())
            {
                var instance = Activator.CreateInstance<T>();
                foreach (var pi in properties)
                {
                    pi.SetValue(instance, dr[pi.Name], null);
                }

                list.Add(instance);
            }

            return list;
        }

        private static void VerifyTypeMatch<T>(IDataRecord dr, IEnumerable<PropertyInfo> properties)
        {
            // Counter for when each field is found.
            var dictionary = new HashSet<string>(StringComparer.CurrentCultureIgnoreCase);
            
            // Throw an error if the class expects columns that aren't being returned.
            foreach (var pi in properties)
            { 
                // Increment
                dictionary.Add(pi.Name);
            }
            
            // Don't throw an error if the data set more verbose than the class we're filling
            for (int i = 0; i < dr.FieldCount; i++)
            { 
                // Decrement.
                string column = dr.GetName(i);
                if (dictionary.Contains(column))
                {
                    dictionary.Remove(column);
                }
            }

            if (dictionary.Count > 0)
            {
                var ex = new Exception(ExceptionText.DRMapperTypeTooComplex);
                ex.Data.Add("Type", typeof(T));
                ex.Data.Add("Missing fields", string.Join(", ", dictionary));
                throw ex;
            }
        }

        public static T ParseRecord<T>(IDataReader dr, int rowIndex = 0)
        {
            var properties = typeof(T).GetProperties();

            var currentRow = 0;
            while (dr.Read())
            {
                if (currentRow < rowIndex)
                {
                    currentRow++;
                    continue;
                }

                var instance = Activator.CreateInstance<T>();
                foreach (var pi in properties)
                {
                    pi.SetValue(instance, dr[pi.Name], null);
                }

                return instance;
            }

            if (rowIndex > currentRow)
            {
                throw new IndexOutOfRangeException(ExceptionText.DRMapperIndexOutOfRange);
            }

            return default(T);
        }
    }
}