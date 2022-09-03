namespace GameFramework.LogManagement
{
    public interface ILogTarget
    {
        void Log(LogEvent logEvent);
        void Flush();
    }
}