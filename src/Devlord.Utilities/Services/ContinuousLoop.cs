using System;
using System.Threading;

namespace Devlord.Utilities.Services
{
    /// <summary>
    /// This is a loop that always runs. If you use it for long-running operations, it basically makes sure that your
    /// task starts again as soon as it's finished.
    /// </summary>
    /// <remarks>Using fire-and-forget code inside your event handler might cause an overflow, since a new invocation will occur as soon as control
    /// returns from the event.</remarks>
    public class ContinuousLoop : ServiceTimer
    {
        #region Fields

        private int _queuedTimers;

        private int _workerCount;

        #endregion

        #region Constructors and Destructors

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adding multiple events to the timer will allow the events to run consecutively, though in no particular order.
        /// </summary>
        /// <param name="elapsedHandler"></param>
        /// <returns></returns>
        
        public override ServiceTimer AddEvent(ServiceTimerEventHandler elapsedHandler)
        {
            _queuedTimers++;
            Events += (s, e) =>
            {
                try
                {
                    elapsedHandler.Invoke(this, e);
                }
                catch (Exception error)
                {
                    Logger.Log(error);
                }

                if (Interlocked.Increment(ref _workerCount) == _queuedTimers)
                {
                    // Restart the timer immediately.
                    Interlocked.Exchange(ref _workerCount, 0);
                    LocalTimer.Change(0, Timeout.Infinite);
                }
            };

            return this;
        }

        public override void Run()
        {
            // New timer with immediate start, manual repeat.
            LocalTimer = new Timer(AllCallbacks, new ServiceTimerState(), 0, Timeout.Infinite);
        }

        #endregion
    }
}