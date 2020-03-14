using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;
namespace DataFusion.Interfaces
{
    public class LogDogCollar : ILogDogCollar
    {
        private string _logName;
        private string _dir;
        public LogDogCollar()
        {
            BasicConfigurator.Configure();
        }

        public void Setup(string dir, string logName, string level = "ALL")
        {
            _dir = dir;
            _logName = logName;

            var repositoryName = _logName + "repository";
            var basePath = Path.GetDirectoryName(GetType().Assembly.Location);

            ILoggerRepository repository = null;
            var repositories = LogManager.GetAllRepositories();
            foreach (var r in repositories)
            {
                if (r.Name.Equals(repositoryName))
                {
                    repository = r;
                    break;
                }
            }
            Hierarchy hierarchy = null;
            if (repository == null)
            {
                //Create a new repository
                repository = LogManager.CreateRepository(repositoryName);

                hierarchy = (Hierarchy)repository;
                hierarchy.Root.Additivity = false;

                //Add appenders you need: here I need a rolling file and a memoryappender
                var rollingAppender = CreateFileAppender(_logName + "Appender", Path.Combine(basePath, "Logs", _dir, _logName));
                hierarchy.Root.AddAppender(rollingAppender);

                //#if DEBUG
                var consoleAppender = CreateConsoleAppender();
                hierarchy.Root.AddAppender(consoleAppender);
                BasicConfigurator.Configure(repository, rollingAppender, consoleAppender);
                //#else
                //                BasicConfigurator.Configure(repository, rollingAppender);
                //#endif

                var appenders = repository.GetAppenders();
            }

            SetLevel(_logName + "logger", level);
        }

        public ILogDog GetLogger()
        {
            ILog log = LogManager.GetLogger(_logName + "repository", _logName + "logger");

            var appenders = log.Logger.Repository.GetAppenders();
            return new LogDog(log);
        }

        public static void SetLevel(string loggerName, string levelName)
        {
            ILog log = LogManager.GetLogger(loggerName);
            Logger l = (Logger)log.Logger;

            l.Level = l.Hierarchy.LevelMap[levelName];
        }

        // Add an appender to a logger
        public static void AddAppender(string loggerName, IAppender appender)
        {
            ILog log = LogManager.GetLogger(loggerName);
            AddAppender(log, appender);
        }

        // Add an appender to a logger
        public static void AddAppender(ILog log, IAppender appender)
        {
            Logger l = (Logger)log.Logger;
            l.AddAppender(appender);
        }

        private const string Log4netPattern = "[%level][%d]:%m%n";
        public static IAppender CreateConsoleAppender()
        {
            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = Log4netPattern;
            layout.ActivateOptions();
            var consoleAppender = new ConsoleAppender
            {
                Name = "ConsoleAppender",
                Layout = layout
            };

            return consoleAppender;
        }

        // Create a new file appender
        public static IAppender CreateFileAppender(string name, string fileName)
        {
            RollingFileAppender appender = new RollingFileAppender();
            appender.Name = name;
            appender.File = fileName;
            appender.AppendToFile = true;

            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = Log4netPattern;
            layout.ActivateOptions();

            appender.Layout = layout;
            appender.MaxSizeRollBackups = 7;
            appender.MaximumFileSize = "2GB";
            appender.RollingStyle = RollingFileAppender.RollingMode.Date;
            appender.StaticLogFileName = false;
            appender.DatePattern = "yyyyMMdd'.txt'";
            appender.ActivateOptions();

            return appender;
        }

        public void Dispose()
        {
            var log = GetLogger();
            log.Dispose();
            _logName = null;
        }
    }
}
