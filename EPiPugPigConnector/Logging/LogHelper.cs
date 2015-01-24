using System;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.Text;
using EPiPugPigConnector.Properties;
using Microsoft.SqlServer.Server;

namespace EPiPugPigConnector.Logging
{
    //Log levels in order:
    //ALL
    //DEBUG
    //INFO
    //WARN
    //ERROR
    //FATAL
    //OFF
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
        private const string PUGPIG_CONNECTOR_LOGGER_NAME = "PugPigConnectorLogger";
        private const string PUGPIG_CONNECTOR_FILE_APPENDER_NAME = "PugPigConnectorFileAppender";

        public static ILog PugPigLogger
        {
            get
            {
                return log4net.LogManager.GetLogger(PUGPIG_CONNECTOR_LOGGER_NAME);
            }
        }

        /// <summary>
        /// Adds a message to the pugpig logger. Default logging level is Error, if not given (optional).
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel">Default value is Error</param>
        public static void Log(string message, LogLevel logLevel = LogLevel.Error)
        {
            if (Properties.Settings.Default.pugpig_log_enabled)
            {
                switch (logLevel)
                {
                    case LogLevel.Debug:
                        PugPigLogger.Debug(message);
                        break;
                    case LogLevel.Info:
                        PugPigLogger.Info(message);
                        break;
                    case LogLevel.Warn:
                        PugPigLogger.Warn(message);
                        break;
                    case LogLevel.Error:
                        PugPigLogger.Error(message);
                        break;
                    case LogLevel.Fatal:
                        PugPigLogger.Fatal(message);
                        break;
                    default:
                        PugPigLogger.Debug(message);
                        break;
                }
            }
        }

        public static void SetupPugpigConnectorLogging()
        {
            //http://stackoverflow.com/questions/308436/log4net-programmatically-specify-multiple-loggers-with-multiple-file-appenders

            //Adds a log4net logger programmatically.
            string loggerName = PUGPIG_CONNECTOR_LOGGER_NAME;
            string fileAppenderName = PUGPIG_CONNECTOR_FILE_APPENDER_NAME;
            string logFile = GetFileName();
            string logLevel = GetLogLevel();

            SetLevel(loggerName, logLevel);
            AddAppender(loggerName, CreateFileAppender(fileAppenderName, logFile));

            Log("Pugpig connector logger started", LogLevel.Debug);        
        }

        private static string GetFileName()
        {
            if (!string.IsNullOrEmpty(Settings.Default.pugpig_log_filename))
            {
                return Settings.Default.pugpig_log_filename;
            }
            return @"App_Data\pugpig_connector.log"; //fallback
        }

        private static string GetLogLevel()
        {
            if (!string.IsNullOrEmpty(Settings.Default.pugpig_log_loglevel))
            {
                return Settings.Default.pugpig_log_loglevel.ToUpper();
            }
            return "ALL"; //fallback
        }

        // Set the level for a named logger
        private static void SetLevel(string loggerName, string levelName)
        {
            ILog log = LogManager.GetLogger(loggerName);
            Logger l = (Logger)log.Logger;

            l.Level = l.Hierarchy.LevelMap[levelName];
        }

        // Add an appender to a logger
        private static void AddAppender(string loggerName, IAppender appender)
        {
            ILog log = LogManager.GetLogger(loggerName);
            Logger l = (Logger)log.Logger;

            l.AddAppender(appender);
        }

        // Create a new file appender
        private static IAppender CreateFileAppender(string name, string fileName)
        {
            RollingFileAppender appender = new RollingFileAppender();
            appender.Name = name; 
            appender.File = fileName;
            appender.StaticLogFileName = true;
            appender.DatePattern = ".yyyyMMdd";
            appender.RollingStyle = RollingFileAppender.RollingMode.Date; //days
            appender.MaxSizeRollBackups = 30; //days
            appender.MaxFileSize = (2*1024*1024); // The default maximum file size is 10MB (10*1024*1024)
            appender.AppendToFile = true;

            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = "%date %level %type.%method - %message%n"; //"%d [%t] %-5p %c [%x] - %m%n"
            layout.ActivateOptions();

            appender.Layout = layout;
            appender.ActivateOptions();

            return appender;
        }
    }
}
