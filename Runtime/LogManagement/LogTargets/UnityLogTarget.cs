using UnityEngine;

namespace GameFramework.Logging
{
    public class UnityLogTarget : LogTarget
    {
        public override void Log(LogEvent logEvent)
        {
            _thisLogLevel = logEvent.level;
            base.Log(logEvent);
        }

        protected override void InternalWrite(string logMessage)
        {
            switch (_thisLogLevel)
            {
                case LogLevel.Verbose:
                case LogLevel.Debug:
                case LogLevel.Information:
                    Debug.Log(logMessage);
                    break;

                case LogLevel.Warning:
                    Debug.LogWarning(logMessage);
                    break;

                case LogLevel.Error:
                case LogLevel.Fatal:
                    Debug.LogError(logMessage);
                    break;
            }
        }

        public override void Flush() { }

        private LogLevel _thisLogLevel;
    }
}