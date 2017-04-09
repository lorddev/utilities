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


#if NET451
using System.Web;
using Elmah;
using System;
using System.Diagnostics;
using LogType = System.Diagnostics.EventLogEntryType;


namespace Devlord.Utilities
{
    public partial class Logger
    {
        public static void Log(Exception e)
        {
            if (HttpContext.Current != null)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
            }
            else
            {
                ErrorLog.Default.Log(new Error(e));
            }

            Debugger.Log(0, "DEBUG", "Exception: " + e);
        }

        public static void Log(ILogger logger, Exception e)
        {
            logger.WriteEntry(e.ToString(), EventLogEntryType.Error);
        }
    }

    public partial interface ILogger
    {
        void WriteEntry(string message, LogType level, LogCode code = LogCode.None);
    }

    public partial class ConsoleLogger : ILogger
    {
        public void Log(Exception exception)
        {
            WriteEntry(exception.ToString(), LogType.Error);
        }

        public void WriteEntry(string message, LogType error, LogCode code = LogCode.None)
        {
            Console.WriteLine($"message: {message}, error: {error}, code: {code}");
        }
    }
}

#endif