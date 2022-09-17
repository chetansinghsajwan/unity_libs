namespace GameFramework.Logging
{
    public struct LoggerLogTarget : ILogTarget
    {
        public LoggerLogTarget(IGameLogger logger)
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

        public IGameLogger logger;
    }
}