namespace GameFramework.LogManagement
{
    public struct LoggerLogTarget : ILogTarget
    {
        public LoggerLogTarget(ILogger logger)
        {
            this.logger = logger;
        }

        public void Log(LogEvent logEvent)
        {
            logger.Write(logEvent);
        }

        public void Flush()
        {
            logger.Flush();
        }

        public ILogger logger;
    }
}