using System;
using System.Timers;

namespace Devlord.Utilities.Services
{
    public class LoopTimer : PrecisionTimer
    {
        #region Constructors and Destructors

        public LoopTimer(TimeSpan interval)
        {
            var regularLoop = new Timer { AutoReset = true, Interval = interval.TotalMilliseconds };
            LocalTimer = regularLoop;
        }

        #endregion
    }
}