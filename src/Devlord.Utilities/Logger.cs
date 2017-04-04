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

namespace Devlord.Utilities
{
    public partial class Logger
    {
        protected Logger()
        {
        }
    }

#if !NETSTANDARD1_3

    public interface ILogger
    {
        void Log(Exception exception);

        void WriteEntry(string message, EventLogEntryType error, LogCode code = LogCode.None);
    }

    public partial class ConsoleLogger : ILogger
    {
        public void Log(Exception exception)
        {
            WriteEntry(exception.ToString(), EventLogEntryType.Error);
        }

        public void WriteEntry(string message, EventLogEntryType error, LogCode code = LogCode.None)
        {
            Console.WriteLine($"message: {message}, error: {error}, code: {code}");
        }
    }

#endif

    public enum LogCode
    {
        None,
        GenericError
    }
}