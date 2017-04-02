using Elmah;
using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
#if NET451
using System.Web;
#endif
#if NETSTANDARD1_5
using Microsoft.AspNetCore.Http;
using Elmah.Io;
#endif

namespace Devlord.Utilities
{
    public class Logger
    {
        protected Logger()
        {
        }

#if !NETSTANDARD1_5
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
#endif
#if NETSTANDARD1_5

        public static void Log(Exception e)
        {
            Log(new ConsoleLogger(), e);
        }


#endif

        public static void Log(ILogger logger, Exception e)
        {
            logger.WriteEntry(e.ToString(), EventLogEntryType.Error);
        }

       

      
    }
    public class ConsoleLogger : ILogger
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

    public interface ILogger
    {
        void Log(Exception exception);

        void WriteEntry(string message, EventLogEntryType error, LogCode code = LogCode.None);
    }

    public enum LogCode
    {
        None,
        GenericError
    }
}
