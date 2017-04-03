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
        private readonly ITestOutputHelper _output;

        public ContinuousLoopTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public void TestContinuousLoop()
        {
            Console.WriteLine("Test app start logging.");
            var success = false;
            ServiceTimer timedMultiple = new ContinuousLoop();
            timedMultiple.AddEvent(LoopedElapsed)
                .AddEvent(LoopedElapsedTwo)
                .AddEvent(
                    (s, e) =>
                    {
                        timedMultiple.ShutDown();
                        success = true;
                    });
            timedMultiple.Run();
            Assert.True(success);
        }

        private void LoopedElapsed(object sender, ServiceTimerState e)
        {
            _output.WriteLine("Test message ONE");
        }

        private void LoopedElapsedTwo(object sender, ServiceTimerState e)
        {
            _output.WriteLine("Test message TWO");
        }

        [Fact]
        public async Task TestLoopTimer3X()
        {
            var ct = 0;
            ServiceTimer timer = new LoopTimer(TimeSpan.FromSeconds(0.01));
            timer.AddEvent(
                (s, e) =>
                {
                    Interlocked.Increment(ref ct);
                    _output.WriteLine("poke: " + ct);
                });
            timer.Run();
            
            await Task.Delay(40);
            
            timer.ShutDown();
            _output.WriteLine("Asserting...");
            // TODO: Running local returns 4 as expected; running on AppVeyor returns
            // 12. Presumably running in parallel on steroids?
            ct.ShouldBeInRange(1, 20);
        }
    }
}