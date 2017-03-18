// -----------------------------------------------------------------------
// <copyright file="DataManager.cs" company="Lord Design">
//   Copyright (c) 2012 Lord Design, Paradise California.
//   Modified GPL: You can alter it if the result is free. You have permission
//   to incorporate it as-is in a project that is not free.
// </copyright>
// <created>09/29/2012 12:24 PM</created>
// <author>aaron</author>
// -----------------------------------------------------------------------

namespace Devlord.Utilities
{
    using System.Collections.Generic;

    /// <summary>
    /// Abstract business class to enforce an interface for CRUD mapping. T Can be used as a datacontract from a JSON
    /// service that interfaces with your UI, or it can be the actual entity sent to the database, or it could be another
    /// intermediate type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DataManager<T>
    {
        #region Public Properties

        public int LoggedOnUserId { get; set; }

        #endregion

        #region Public Methods and Operators

        public abstract void Create(T instance);

        public abstract IPaginable<T> LoadActive();

        public abstract IPaginable<T> LoadPage(int pageNumber, int pageSize);

        public abstract T LoadSingleItem(int id);

        public abstract void Update(T instance);

        public abstract void Delete(T instance);

        #endregion
    }
}