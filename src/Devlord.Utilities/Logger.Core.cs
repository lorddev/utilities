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


#if NETSTANDARD1_5 || NETSTANDARD1_3

using System;

#if NETSTANDARD1_5
using System.Diagnostics;
#endif

//using Elmah.Io;


namespace Devlord.Utilities
{
    public partial class Logger
    {
        public static void Log(Exception e)
        {
            Log(new ConsoleLogger(), e);
        }

#if NETSTANDARD1_3
        public static void Log(ILogger logger, Exception e)
        {
            logger.WriteEntry(e.ToString(), "Error");
        }
#else

        public static void Log(ILogger logger, Exception e)
        {
            logger.WriteEntry(e.ToString(), EventLogEntryType.Error);
        }

#endif

    }

#if NETSTANDARD1_3

    public interface ILogger
    {
        void Log(Exception exception);

        void WriteEntry(string message, object error, LogCode code = LogCode.None);
    }

    public partial class ConsoleLogger : ILogger
    {
        public void Log(Exception exception)
        {
            WriteEntry(exception.ToString(), "Error");
        }

        public void WriteEntry(string message, object error, LogCode code = LogCode.None)
        {
            Console.WriteLine($"message: {message}, error: {error}, code: {code}");
        }
    }

#endif
}
#endif