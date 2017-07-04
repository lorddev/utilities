// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEachified.cs" company="Lord Design">
//   © Lord Design. Modified GPL: You may use freely and commercially without modification; you can modify if result 
//   is also free.
// </copyright>
// <author>aaron@lorddesign.net</author>
// --------------------------------------------------------------------------------------------------------------------

namespace Devlord.Utilities
{
    using System;

    /// <summary>
    /// Enforces a rule that a collection can perform a certain action on each of its items.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IEachified<T>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The for each.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        void ForEach(Action<T> action);

        #endregion
    }
}