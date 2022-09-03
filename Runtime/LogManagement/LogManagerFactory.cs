using System;

namespace GameFramework.LogManagement
{
    public static class LogManagerFactory
    {
        public static Func<LogManager> Create = DefaultFactory;

        public static LogManager DefaultFactory()
        {
            return new LogManager();
        }
    }
}