// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="Lord Design">
//   © Lord Design
// </copyright>
// <license type="GPL">
//  You may use freely and commercially without modification; you can modify if result 
//   is also free.
// </license>
// <summary>
//  </summary>
// <author>Aaron Lord</author>
// --------------------------------------------------------------------------------------------------------------------


#if NET451 || NET462
using System.Web;
using Elmah;
using System;
using System.Diagnostics;

namespace Devlord.Utilities
{
    public partial class Logger
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
        

        public static void Log(ILogger logger, Exception e)
        {
            logger.WriteEntry(e.ToString(), EventLogEntryType.Error);
        }
    }
}

#endif