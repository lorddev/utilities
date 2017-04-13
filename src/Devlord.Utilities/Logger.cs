// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="Lord Design">
//   © Lord Design
// </copyright>
// <license type="GPL">
//  You may use freely and commercially without modification; you can modify if result 
//   is also free.
// </license>
// <summary>
//  </summary>
// <author>Aaron Lord</author>
// --------------------------------------------------------------------------------------------------------------------


using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Devlord.Utilities
{
    public partial class Logger
    {
        protected Logger()
        {
        }
    }

    public partial interface IDevLogger
    {
        void Log(Exception exception);
    }

    public partial class ConsoleLogger : IDevLogger
    {
    }

    public enum LogCode
    {
        None,
        GenericError
    }
}