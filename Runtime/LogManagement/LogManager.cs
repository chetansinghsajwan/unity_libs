using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.LogManagement
{
    public class LogManager
    {
        public static LogManager Instance;
        public static ILogger Logger = new SilentLogger();

        public static LogLevel DefaultLogLevel = LogLevel.Information;

        public virtual void Init()
        {
#if UNITY_EDITOR
            _logPath = Application.dataPath.Replace("Assets", "Logs/");
#else
            _logPath = Application.temporaryCachePath;
#endif

            Debug.Log($"<b>GameLog LogPath:</b> {_logPath}");

            Logger = CreateGlobalLogger();
            if (Logger is null)
            {
                Logger = new SilentLogger();
            }
        }

        public virtual void Shutdown()
        {
        }

        protected virtual ILogger CreateGlobalLogger()
        {
            Logger logger = new Logger("", LogLevel.Information,
                new FileLogTarget(GetAddressFor("GameLog"), FileMode.Create),
                new UnityLogTarget());

            return new AsyncLogger(logger);
        }

        public virtual ILogger CreateLogger(string category, params ILogTarget[] logTargets)
        {
            return CreateLogger(category, logTargets as IEnumerable<ILogTarget>);
        }

        public virtual ILogger CreateLogger(string category, IEnumerable<ILogTarget> logTargets)
        {
            return CreateSubLogger(Logger, category, logTargets);
        }

        public virtual ILogger CreateSubLogger(ILogger logger, string category, params ILogTarget[] logTargets)
        {
            return CreateSubLogger(logger, category, logTargets as IEnumerable<ILogTarget>);
        }

        public virtual ILogger CreateSubLogger(ILogger logger, string category, IEnumerable<ILogTarget> logTargets)
        {
            List<ILogTarget> logTargetList = new List<ILogTarget>();
            logTargetList.Add(new LoggerLogTarget(logger));
            logTargetList.AddRange(logTargets);

            return new Logger(category, DefaultLogLevel, logTargetList.ToArray());
        }

        public virtual string GetAddressFor(string logFile)
        {
            string name = Path.ChangeExtension(logFile, ".log");
            return Path.Combine(_logPath, name);
        }

        protected string _logPath;
        public string LogPath => _logPath;
    }
}