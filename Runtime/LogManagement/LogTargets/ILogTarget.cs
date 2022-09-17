namespace GameFramework.Logging
{
    public interface ILogTarget
    {
        void Log(LogEvent logEvent);
        void Flush();
    }

    public abstract class LogTarget : ILogTarget
    {
        // we set it to minimum to disable filtering at target level
        public const LogLevel DEFAULT_LOG_LEVEL = LogLevelAlias.Minimum;
        public static readonly ILogFormatter DEFAULT_FORMATTER = new NullFormatter();

        public LogTarget() : this(DEFAULT_LOG_LEVEL, DEFAULT_FORMATTER) { }

        public LogTarget(LogLevel level, ILogFormatter formatter)
        {
            this.logLevel = level;
            this.formatter = formatter;
        }

        public bool IsEnabled(LogLevel level)
        {
            return level >= logLevel;
        }

        public virtual void Log(LogEvent logEvent)
        {
            if (IsEnabled(logEvent.level))
            {
                string logMessage = null;
                if (formatter is not null)
                {
                    logMessage = formatter.Format(logEvent);
                }

                logMessage ??= logEvent.messageTemplate;

                InternalWrite(logMessage);
            }
        }

        protected abstract void InternalWrite(string log);
        public abstract void Flush();

        public ILogFormatter formatter;
        public LogLevel logLevel;
    }
}