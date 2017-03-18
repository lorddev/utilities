namespace Devlord.Utilities
{
    using System;
    using System.Diagnostics;
    using System.Web;

    using Elmah;

    public class Logger
    {
        public static void Log(Exception e)
        {
            if (HttpContext.Current != null)
            {
                ErrorSignal.FromCurrentContext().Raise(e);
            }
            else
            {
                ErrorLog.Default.Log(new Error(e));
            }

            Debugger.Log(0, "DEBUG", "Exception: " + e);
        }
    }
}
