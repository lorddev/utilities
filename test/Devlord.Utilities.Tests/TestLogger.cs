using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Xunit;
#if NETCOREAPP1_1
using LogType = Microsoft.Extensions.Logging.LogLevel;

#elif NET462
using LogType = System.Diagnostics.EventLogEntryType;
#endif

namespace Devlord.Utilities.Tests
{
    public class TestLogger : IDevLogger
    {
        public void Log(Exception exception)
        {
            Console.WriteLine(exception);
            Errors.Add(exception);
            throw new Exception("Logged exception found in tested code. Rethrowing...", exception);
        }

        public void WriteEntry(string message, LogLevel error, LogCode errorCode = LogCode.None)
        {
            throw new NotImplementedException();
        }

        private static readonly List<Exception> Errors = new List<Exception>();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void VerifyNoErrors()
        {
            Assert.Empty(Errors);
        }
    }
}