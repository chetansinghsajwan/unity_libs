namespace GameFramework.Logging
{
    public struct NullFormatter : ILogFormatter
    {
        public string Format(LogEvent logEvent)
        {
            if (logEvent.messageTemplate is null)
                return "";

            return logEvent.messageTemplate;
        }
    }
}