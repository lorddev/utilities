using System;
using System.Timers;

namespace LordDesign.Utilities.Services
{
    public class ContinuousLoop : ServiceTimer
    {
        #region Fields

        private int _runningTimers;

        #endregion

        #region Constructors and Destructors

        public ContinuousLoop()
        {
            var looped = new Timer { AutoReset = false, Interval = TimeSpan.FromSeconds(10D).TotalMilliseconds };
            LocalTimer = looped;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adding multiple events to the timer will allow the events to run consecutively. If they are synchronous 
        /// events, then they will run one at a time (in no particular order).
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public override ServiceTimer AddEvent(ElapsedEventHandler @event)
        {
            LocalTimer.Elapsed += (s, e) =>
            {
                ++_runningTimers;
                try
                {
                    @event.Invoke(s, e);
                }
                catch (Exception)
                {
                    Console.WriteLine(@"Error in MarketService.");
                }

                if (--_runningTimers == 0)
                {
                    LocalTimer.Start();
                }
            };

            return this;
        }

        #endregion
    }
}