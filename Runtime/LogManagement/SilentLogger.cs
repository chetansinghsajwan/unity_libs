namespace GameFramework.LogManagement
{
    public struct SilentLogger : ILogger
    {
        public bool IsEnabled(LogLevel level) => false;

        public void Write(LogEvent logEvent) { }

        public void Flush() { }
    }
}