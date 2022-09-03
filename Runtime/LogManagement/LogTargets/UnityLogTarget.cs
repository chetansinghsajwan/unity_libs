using UnityEngine;

namespace GameFramework.LogManagement
{
    public struct UnityLogTarget : ILogTarget
    {
        public void Log(LogEvent logEvent)
        {
            string message = logEvent.messageTemplate;
            message = $"<b>[{logEvent.frame} {logEvent.timeStamp.DateTime} {logEvent.level}] {logEvent.category}:</b> {message}";

            switch (logEvent.level)
            {
                case LogLevel.Verbose:
                case LogLevel.Debug:
                case LogLevel.Information:
                    Debug.Log(message);
                    break;

                case LogLevel.Warning:
                    Debug.LogWarning(message);
                    break;

                case LogLevel.Error:
                case LogLevel.Fatal:
                    Debug.LogError(message);
                    break;
            }
        }

        public void Flush()
        {
        }
    }
}