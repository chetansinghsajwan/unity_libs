namespace GameFramework.LogManagement
{
    public interface ILogFormatter
    {
        string Format(LogEvent logEvent);
    }
}