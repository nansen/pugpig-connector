using System;

namespace EPiPugPigConnector.Logging
{
    public enum LogLevel
    {
        All,
        Debug,
        Info,
        Warn,
        Error,
        Fatal,
        Off
    }

    public static class LogHelper
    {
        public static void Log(string message, LogLevel logLevel)
        {
            //Log levels in order:
            //ALL
            //DEBUG
            //INFO
            //WARN
            //ERROR
            //FATAL
            //OFF

            log4net.ILog log = log4net.LogManager.GetLogger("DefaultLogger");

            switch (logLevel)
            {
                case LogLevel.Debug:
                    log.Debug(message);
                    break;
                case LogLevel.Info:
                    log.Info(message);
                    break;
                case LogLevel.Warn:
                    log.Warn(message);
                    break;
                case LogLevel.Error:
                    log.Error(message);
                    break;
                case LogLevel.Fatal:
                    log.Fatal(message);
                    break;
                default:
                    log.Debug(message);
                    break;
            }
        }
    }
}
