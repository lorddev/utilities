using System.Timers;

namespace LordDesign.Utilities.Services
{
    public abstract class ServiceTimer
    {
        #region Fields

        protected internal Timer LocalTimer;

        #endregion

        #region Public Methods and Operators

        public abstract ServiceTimer AddEvent(ElapsedEventHandler elapsedHandler);

        public void Run()
        {
            LocalTimer.Start();
        }

        #endregion

        public void ShutDown()
        {
           LocalTimer.Stop();
        }
    }
}