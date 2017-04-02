// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MailThrottle.cs" company="Lord Design">
//   © Lord Design. Modified GPL: You may use freely and commercially without modification; you can modify if result 
//   is also free.
// </copyright>
// <summary>
//   Multi-threaded mailer to keep your systems running by sending messages asynchronously. If you're using it in a
//   service that sends a lot of messages, be aware that you'll be limited by your .NET working threads.
// </summary>
// <author>aaron@lorddesign.net</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Devlord.Utilities
{
    /// <summary>
    /// The mail throttle.
    /// </summary>
    public class MailThrottle
    {
        #region Fields
        /// <summary>
        /// The counter.
        /// </summary>
        private readonly List<DateTime> _counter = new List<DateTime>();

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        public ThrottleInterval Interval { get; set; }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        public int Limit { get; set; }

        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// The count.
        /// </summary>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        public int Count()
        {
            var startTime = GetStartTime();
            var count = _counter.Count(x => x > startTime);
            Compact(startTime);
            return count;
        }

        /// <summary>
        /// The increment.
        /// </summary>
        public void Increment()
        {
            _counter.Add(DateTime.Now);
            Compact();
        }

        #endregion

        #region Methods
        /// <summary>
        /// The compact.
        /// </summary>
        private void Compact()
        {
            Compact(GetStartTime());
        }

        /// <summary>
        /// The compact.
        /// </summary>
        /// <param name="startTime">
        /// The start time.
        /// </param>
        private void Compact(DateTime startTime)
        {
            _counter.RemoveAll(x => x < startTime);
        }

        /// <summary>
        /// The get start time.
        /// </summary>
        /// <returns>
        /// The <see cref="DateTime" />.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private DateTime GetStartTime()
        {
            // Per minute 180, Per hour 3600, Per day 10,000
            // http://support.google.com/a/bin/answer.py?hl=en&answer=1366776
            switch (Interval)
            {
                case ThrottleInterval.Minute:
                    return DateTime.Now.AddMinutes(-1);
                case ThrottleInterval.Hour:
                    return DateTime.Now.AddHours(-1);
                case ThrottleInterval.Day:
                    return DateTime.Now.AddDays(-1);
            }

            throw new ArgumentOutOfRangeException();
        }

        #endregion
    }
}