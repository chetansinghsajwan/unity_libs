using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.LogManagement
{
    public static class LogManager
    {
        public static LogManagerSystem System;
        public static ILogger Logger = new SilentLogger();

        public static LogLevel DefaultLogLevel = LogLevel.Information;
    }
}