using System.Collections.Generic;

namespace GameFramework.Logging
{
    public static class GameLog
    {
        public static GameLogSystem System;
        public static ILogger Logger = new SilentLogger();

        public static LogLevel DefaultLogLevel = LogLevel.Information;

        public static ILogger CreateLogger(string category, params ILogTarget[] logTargets)
        {
            return CreateLogger(category, logTargets as IEnumerable<ILogTarget>);
        }

        public static ILogger CreateLogger(string category, IEnumerable<ILogTarget> logTargets)
        {
            return System.CreateLogger(category, logTargets);
        }

        public static ILogger CreateSubLogger(ILogger logger, string category, params ILogTarget[] logTargets)
        {
            return CreateSubLogger(logger, category, logTargets as IEnumerable<ILogTarget>);
        }

        public static ILogger CreateSubLogger(ILogger logger, string category, IEnumerable<ILogTarget> logTargets)
        {
            return System.CreateSubLogger(logger, category, logTargets);
        }

        public static string GetAddressFor(string logFile)
        {
            return System.GetAddressFor(logFile);
        }
    }
}