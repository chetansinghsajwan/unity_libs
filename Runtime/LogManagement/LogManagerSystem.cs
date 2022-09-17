using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Logging
{
    [GameSystemRegistration(typeof(GameLogSystem))]
    public class GameLogSystem : GameSystem
    {
        public struct DefaultLogFormatter : ILogFormatter
        {
            public string Format(LogEvent logEvent)
            {
                string message = $"[{logEvent.frame} {logEvent.timeStamp.DateTime} {logEvent.level}] {logEvent.category}: {logEvent.messageTemplate}";

                if (logEvent.exception is not null)
                {
                    message = $"{message}{Environment.NewLine}{logEvent.exception}";
                }

                return message;
            }
        }

        protected override void OnRegistered(GameSystem system)
        {
            base.OnRegistered(system);

            GameLog.System = this;

#if UNITY_EDITOR
            _logPath = Application.dataPath.Replace("Assets", "Logs/");
#else
            _logPath = Application.temporaryCachePath;
#endif

            Debug.Log($"<b>GameLog LogPath:</b> {_logPath}");

            GameLog.Logger = CreateGlobalLogger();
            if (GameLog.Logger is null)
            {
                GameLog.Logger = new SilentLogger();
            }
        }

        protected virtual ILogger CreateGlobalLogger()
        {
            var file = new FileLogTarget(GetAddressFor("GameLog"), FileMode.Create);
            file.formatter = new DefaultLogFormatter();

            var unity = new UnityLogTarget();
            unity.formatter = new DefaultLogFormatter();

            Logger logger = new Logger("", GameLog.DefaultLogLevel, file, unity);
            return new AsyncLogger(logger);
        }

        public virtual ILogger CreateLogger(string category, params ILogTarget[] logTargets)
        {
            return CreateLogger(category, logTargets as IEnumerable<ILogTarget>);
        }

        public virtual ILogger CreateLogger(string category, IEnumerable<ILogTarget> logTargets)
        {
            return CreateSubLogger(GameLog.Logger, category, logTargets);
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

            return new Logger(category, GameLog.DefaultLogLevel, logTargetList.ToArray());
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