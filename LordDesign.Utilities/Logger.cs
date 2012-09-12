using System;
using System.Web;
using Elmah;

namespace LordDesign.Utilities
{
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
        }
    }
}
