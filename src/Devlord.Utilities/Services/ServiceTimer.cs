using System;
using System.Threading;

namespace Devlord.Utilities.Services
{
    public delegate void ServiceTimerEventHandler(object sender, ServiceTimerState e);

    public class ServiceTimerState : EventArgs
    {
        public object State { get; set; }
    }

    public abstract class ServiceTimer : IDisposable
    {
        #region Fields

        protected internal Timer LocalTimer;

        #endregion

        #region Public Methods and Operators

        public virtual ServiceTimer AddEvent(ServiceTimerEventHandler elapsedHandler)
        {
            Events += elapsedHandler;
            return this;
        }

        protected ServiceTimerEventHandler Events = (s, e) => { };

        public abstract void Run();

        protected virtual void AllCallbacks(object state)
        {
            Exception innerException = null;
            foreach (ServiceTimerEventHandler @event in Events.GetInvocationList())
            {
                try
                {
                    @event.Invoke(this, state as ServiceTimerState ?? new ServiceTimerState { State = state });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    innerException = e;
                }
            }

            // Todo: Where should we stick these exceptions?
        }

        #endregion

        public void ShutDown()
        {
            LocalTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <remarks>You might not want to dispose of this yourself unless you have a way of making sure all of your threads
        /// are complete. You might end up with a "cannot access disposed object" error.</remarks>
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) LocalTimer?.Dispose();
        }
    }
}