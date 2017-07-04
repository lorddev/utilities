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

using Elmah.Io;

#if !NET451

using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System;

namespace Devlord.Utilities
{
    public partial interface IDevLogger
    {
        void WriteEntry(string message, LogLevel logLevel, LogCode code = LogCode.None);
    }

    public partial class Logger
    {
        public static void Log(Exception e)
        {
            Log(new ConsoleLogger(), e);
        }
        
        public static void Log(IDevLogger logger, Exception e)
        {
            logger.WriteEntry(e.ToString(), LogLevel.Error);
        }
    }

    public partial class ConsoleLogger : IDevLogger
    {
        public void Log(Exception exception)
        {
            WriteEntry(exception.ToString(), LogLevel.Error);
        }

        public void WriteEntry(string message, LogLevel error, LogCode code = LogCode.None)
        {
            Console.WriteLine($"message: {message}, error: {error}, code: {code}");
        }
    }
    
}
#endif