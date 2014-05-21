using System;
using System.Timers;

namespace LordDesign.Utilities.Services
{
    public class PrecisionTimer : ServiceTimer
    {
        #region Constructors and Destructors

        public PrecisionTimer(DateTime eventTimeUtc)
        {
            TimeSpan countDown = eventTimeUtc.Subtract(DateTime.UtcNow);
            LocalTimer = new Timer { AutoReset = false, Interval = countDown.TotalMilliseconds };
        }

        protected PrecisionTimer()
        {
        }

        #endregion

        #region Public Methods and Operators

        public override ServiceTimer AddEvent(ElapsedEventHandler elapsedHandler)
        {
            LocalTimer.Elapsed += elapsedHandler;
            return this;
        }

        #endregion
    }
}