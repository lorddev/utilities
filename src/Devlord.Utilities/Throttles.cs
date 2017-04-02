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
        /// Initializes a new instance of the <see cref="Throttles" /> class.
        /// </summary>
        public Throttles()
        {
            MinuteThrottle = new MailThrottle { Interval = ThrottleInterval.Minute, Limit = 180 };
            HourlyThrottle = new MailThrottle { Interval = ThrottleInterval.Hour, Limit = 3600 };
            DailyThrottle = new MailThrottle { Interval = ThrottleInterval.Day, Limit = 10000 };
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