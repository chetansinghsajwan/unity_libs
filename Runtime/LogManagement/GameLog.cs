namespace GameFramework.Logging
{
    public static class GameLog
    {
        public static GameLogSystem System;
        public static IGameLogger Logger = new SilentLogger();
        public static LogLevel DefaultLogLevel = LogLevel.Information;
    }
}