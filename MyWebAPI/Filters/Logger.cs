using System;

namespace MyWebAPI.Filters
{
    public class Logger
    {
        private static log4net.ILog m_InfoLogger = log4net.LogManager.GetLogger("InfoLog");
        private static log4net.ILog m_ErrorLogger = log4net.LogManager.GetLogger("ErrorLog");

        public static void Info(object message)
        {
            m_InfoLogger.Info(message);
        }


        public static void Error(Exception exception)
        {
            m_ErrorLogger.Error(exception);
        }


        public static void Error(object message, Exception exception)
        {
            m_ErrorLogger.Error(message, exception);
        }
    }
}