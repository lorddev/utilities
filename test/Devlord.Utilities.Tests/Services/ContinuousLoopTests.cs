using System;
using System.Threading.Tasks;
using Xunit;

// ReSharper disable CheckNamespace

namespace Devlord.Utilities.Services.Tests
{
    public class ServiceTimerTests
    {
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

        private static void LoopedElapsed(object sender, ServiceTimerState e)
        {
            Console.WriteLine("Test message ONE");
        }

        private static void LoopedElapsedTwo(object sender, ServiceTimerState e)
        {
            Console.WriteLine("Test message TWO");
        }

        [Fact]
        public void TestLoopTimerConstructor()
        {
            var success = false;
            ServiceTimer timer = new LoopTimer(TimeSpan.FromSeconds(1));
            timer.AddEvent(
                (s, e) =>
                {
                    timer.ShutDown();
                    success = true;
                });
            timer.Run();

            Task.Delay(1000);

            Assert.True(success);
        }
    }
}