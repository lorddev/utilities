using System;
using System.Threading;
using Devlord.Utilities.Services;

namespace Devlord.Utilities.Services
{
    public class LoopTimer : PrecisionTimer
    {
        #region Constructors and Destructors

        private double _interval = 0;

        public LoopTimer(TimeSpan interval)
        {
            _interval = interval.TotalMilliseconds;
        }

        public override void Run()
        {
            LocalTimer = new Timer(AllCallbacks, new ServiceTimerState(), 0, (int) _interval);
        }

        #endregion
    }
}