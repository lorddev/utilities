// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContinuousLoopTests.cs" company="Lord Design">
//   © 2017 Lord Design
// </copyright>
// <license type="GPL-3.0">
//   You may use freely and commercially without modification; if you make changes, please share back to the
//   community.
// </license>
// <author>Aaron Lord</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Devlord.Utilities.Tests;
using Xunit;
using Xunit.Abstractions;

// ReSharper disable CheckNamespace

namespace Devlord.Utilities.Services.Tests
{
    public class ContinuousLoopTests
    {
        public ContinuousLoopTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private readonly ITestOutputHelper _output;

        [Fact]
        public void TestContinuousLoop()
        {
            _output.WriteLine("Test app start logging.");
            var success = false;
            ServiceTimer timedMultiple = new ContinuousLoop();
            timedMultiple.AddEvent(LoopedElapsed)
                .AddEvent(LoopedElapsedTwo)
                .AddEvent(
                    (s, e) =>
                    {
                        success = true;
                    });
            timedMultiple.Run();
            
            Task.Delay(5).Wait();
            timedMultiple.ShutDown();

            // Wait for threads to settle down 
            Task.Delay(50).Wait();

            Assert.True(success);

            // Wait for threads to settle down 
            Task.Delay(100).Wait();
        }

        private void LoopedElapsed(object sender, ServiceTimerState e)
        {
            _output.WriteLine("Test message ONE");
        }

        private void LoopedElapsedTwo(object sender, ServiceTimerState e)
        {
            _output.WriteLine("Test message TWO");
        }
    }
}
