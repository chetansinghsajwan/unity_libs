using System;
using System.Collections.Generic;

namespace GameFramework.LogManagement
{
    public class Logger : ILogger
    {
        public Logger(string category, LogLevel level, params ILogTarget[] logTargets)
            : this(category, level, logTargets as IEnumerable<ILogTarget>) { }

        public Logger(string category, LogLevel level, IEnumerable<ILogTarget> logTargets)
        {
            category ??= string.Empty;

            this.category = category;
            this.level = level;

            this.logTarget = new MultiLogTarget(logTargets);
        }

        public bool IsEnabled(LogLevel level)
        {
            return level >= this.level;
        }

        public void Write(LogEvent logEvent)
        {
            if (IsEnabled(logEvent.level) is false) return;

            logEvent.AddParentCategory(category);

            logEvent.messageTemplate = String.Format(
                logEvent.messageTemplate, logEvent.properties);

            logEvent.properties = new object[0];

            logTarget.Log(logEvent);
        }

        public void Flush()
        {
            logTarget.Flush();
        }

        protected readonly string category;
        public LogLevel level;
        public ILogTarget logTarget;
    }
}