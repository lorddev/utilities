using System;
using System.Threading;
using System.Threading.Tasks;

namespace Devlord.Utilities
{
    /// <summary>
    /// The throttles.
    /// </summary>
    public class GmailThrottles : Throttles, IEachified<MailThrottle>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GmailThrottles" /> class.
        /// </summary>
        public GmailThrottles() : base(500, 500, 500)
        {
        }
        
        #endregion
    }
}