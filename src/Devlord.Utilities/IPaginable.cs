// -----------------------------------------------------------------------
// <copyright file="IPaginable.cs" company="Lord Design">
//   Modified GPL: You can alter it if the result is free. You have permission
//   to incorporate it as-is in a project that is not free.
// </copyright>
// <created>09/12/2012 10:36 PM</created>
// <author>aaron</author>
// -----------------------------------------------------------------------

namespace Devlord.Utilities
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides an interface to access paginable data. Use in conjunction with the GetPage() extension method.
    /// </summary>
    /// <remarks>
    /// The following methods are also implemented automatically via extension methods: int GetPageCount() and 
    /// bool HasMorePages().
    /// </remarks>
    public interface IPaginable
    {
        #region Public Properties

        int PageNumber { get; set; }

        int PageSize { get; set; }

        int TotalResults { get; set; }

        #endregion
    }

    public interface IPaginable<T> : IPaginable
    {
        IEnumerable<T> Items { get; set; }
    }
}
