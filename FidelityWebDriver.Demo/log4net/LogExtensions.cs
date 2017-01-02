using System;
using log4net;
using log4net.Core;

namespace Sonneville.FidelityWebDriver.Demo.log4net
{
    public static class LogExtentions
    {
        public static void Trace(this ILog log, string message, Type declaringType, Exception exception = null)
        {
            log.Logger.Log(declaringType, Level.Trace, message, exception);
        }

        public static void Verbose(this ILog log, string message, Type declaringType, Exception exception = null)
        {
            log.Logger.Log(declaringType, Level.Verbose, message, exception);
        }
    }
}
