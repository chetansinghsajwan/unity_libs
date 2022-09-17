namespace GameFramework.Logging
{
    public interface ILogFormatter
    {
        string Format(LogEvent logEvent);
    }
}