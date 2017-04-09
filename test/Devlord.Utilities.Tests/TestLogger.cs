using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Xunit;

#if NETCOREAPP1_1
using LogType = Microsoft.Extensions.Logging.LogLevel;
#else
using LogType = System.Diagnostics.EventLogEntryType;
#endif

namespace Devlord.Utilities.Tests
{
    public class TestLogger : ILogger
    {
        public void Log(Exception exception)
        {
            Console.WriteLine(exception);
            Errors.Add(exception);
            throw new Exception("Logged exception found in tested code. Rethrowing...", exception); 
        }

        public void WriteEntry(string message, LogType error, LogCode errorCode = LogCode.None)
        {
            throw new NotImplementedException();
        }

        private static readonly List<Exception> Errors = new List<Exception>();

        [Fact]
        public void VerifyNoErrors()
        {
            Assert.Empty(Errors);
        }
    }
}
