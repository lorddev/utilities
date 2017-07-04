using System;
using System.Threading;
using System.Threading.Tasks;
using Devlord.Utilities.Services;
using Xunit;
using Xunit.Abstractions;

namespace Devlord.Utilities.Tests.Services
{
    public class LoopTimerTests
    {
        public LoopTimerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private readonly ITestOutputHelper _output;

        [Fact]
        public void TestLoopTimer3X()
        {
            // This test will fail unless the assembly is marked with the
            // [assembly: CollectionBehavior(DisableTestParallelization = true)]
            // This is because when running tests in parallel, the thread prioritization
            // system behaves unexpectedly. The lesson for end-users is: If you require something
            // to happen at a certain time, make sure that owner of the Timer object is your main thread.
            var ct462 = 0;

            ServiceTimer timer = new LoopTimer(TimeSpan.FromSeconds(0.01));
            timer.AddEvent(
                (s, e) =>
                {
                    _output.WriteLine("poke: " + Interlocked.Increment(ref ct462));
                });
            timer.Run();

            var t = Task.Delay(50);
            t.Wait();

            timer.ShutDown();
            _output.WriteLine("Asserting...");
            // TODO: Running local returns 4 as expected; running on AppVeyor returns
            // 12. Presumably running in parallel on steroids?

            ct462.ShouldBeInRange(1, 6);
        }
    }
}
