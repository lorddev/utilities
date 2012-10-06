// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Lord Design">
//   Modified GPL: You can alter it if the result is free. You have permission
//   to incorporate it as-is in a project that is not free.
// </copyright>
// <created>09/12/2012 3:47 PM</created>
// <author>aaron</author>
// -----------------------------------------------------------------------

namespace LordDesign.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Extensions
    {
        #region Public Methods and Operators

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        public static IEnumerable<T> GetPage<T>(this IEnumerable<T> collection, int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            return collection.Skip(skip).Take(pageSize);
        }

        public static bool HasMorePages(this IPaginable collection)
        {
            return collection.TotalResults > collection.PageNumber * collection.PageSize;
        }

        public static int GetPageCount(this IPaginable collection)
        {
            decimal percent = collection.TotalResults / (decimal)collection.PageSize;
            return (int)Math.Ceiling(percent);
        }

        #endregion


        public static XElement RemoveNamespaces(this XElement e)
        {
            // Courtesy http://stackoverflow.com/a/7238007/16454 (Dexter Legaspi)
            return new XElement(e.Name.LocalName,
                e.Nodes().Select(n => n is XElement ? RemoveNamespaces(n as XElement) : n),
                       e.HasAttributes ? e.Attributes().Select(a => a) : null);
        }
    }
}