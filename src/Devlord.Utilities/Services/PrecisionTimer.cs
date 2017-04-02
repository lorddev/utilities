using System;
using System.Threading;
using Timer = System.Threading.Timer;

namespace Devlord.Utilities.Services
{
    public class PrecisionTimer : ServiceTimer
    {
        private TimeSpan _countDown;
        #region Constructors and Destructors

        public PrecisionTimer(DateTime eventTimeUtc)
        {
            _countDown = eventTimeUtc.Subtract(DateTime.UtcNow);
        }

        protected PrecisionTimer()
        {
        }

        private ServiceTimerState _state = new ServiceTimerState { State = 0 };

        protected override void AllCallbacks(object state)
        {
            _state = state as ServiceTimerState ?? new ServiceTimerState { State = state };

            base.AllCallbacks(_state);
        }

        public override void Run()
        {
            LocalTimer = new Timer(AllCallbacks, _state, (int)_countDown.TotalMilliseconds, Timeout.Infinite);
        }

        #endregion
        
    }
}