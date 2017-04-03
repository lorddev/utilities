using System;
using System.Threading;
using System.Threading.Tasks;

namespace Devlord.Utilities
{
    /// <summary>
    /// The throttles.
    /// </summary>
    public class Throttles : IEachified<MailThrottle>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Throttles" /> class with the default Google Apps values from
        /// a few years ago.
        /// </summary>
        public Throttles() : this(180, 3600, 1000)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Throttles" /> class.
        /// </summary>
        protected Throttles(int min, int hour, int day)
        {
            MinuteThrottle = new MailThrottle { Interval = ThrottleInterval.Minute, Limit = min };
            HourlyThrottle = new MailThrottle { Interval = ThrottleInterval.Hour, Limit = hour };
            DailyThrottle = new MailThrottle { Interval = ThrottleInterval.Day, Limit = day };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the daily throttle.
        /// </summary>
        public MailThrottle DailyThrottle { get; }

        /// <summary>
        /// Gets the hourly throttle.
        /// </summary>
        public MailThrottle HourlyThrottle { get; }

        /// <summary>
        /// Gets the minute throttle.
        /// </summary>
        public MailThrottle MinuteThrottle { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The for each.
        /// </summary>
        /// <param name="func">
        /// The func.
        /// </param>
        public void ForEach(Action<MailThrottle> func)
        {
            func(MinuteThrottle);
            func(HourlyThrottle);
            func(DailyThrottle);
        }

        /// <summary>
        /// The increment.
        /// </summary>
        public void Increment()
        {
            ForEach(throttle => throttle.Increment());
        }

        /// <summary>
        /// The wait.
        /// </summary>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public void Wait()
        {
            ForEach(
                x =>
                {
                    while (x.Count() >= x.Limit)
                    {
                        Console.Write("Waiting...");
                        Task.Delay(100);
                    }
                });
        }

        #endregion
    }
}