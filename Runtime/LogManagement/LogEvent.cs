using System;

namespace GameFramework.LogManagement
{
    public enum LogLevel
    {
        Verbose,
        Debug,
        Information,
        Warning,
        Error,
        Fatal
    }

    public struct LogEvent
    {
        public string category;
        public string messageTemplate;
        public DateTimeOffset timeStamp;
        public Exception exception;
        public LogLevel level;
        public object[] properties;
        public int frame;

        public string AddParentCategory(string parentCategory)
        {
            category ??= "";
            if (String.IsNullOrEmpty(parentCategory)) return category;

            if (category.Length > 0)
            {
                category = $"{parentCategory}-> {category}";
            }
            else
            {
                category = parentCategory;
            }

            return category;
        }

        public string AddChildCategory(string childCategory)
        {
            category ??= "";
            if (String.IsNullOrEmpty(childCategory)) return category;

            if (category.Length > 0)
            {
                category = $"{category}-> {childCategory}";
            }
            else
            {
                category = childCategory;
            }

            return category;
        }
    }

    public static class LogLevelAlias
    {
        public const LogLevel Minimum = LogLevel.Verbose;
        public const LogLevel Default = LogLevel.Information;
        public const LogLevel Maximum = LogLevel.Fatal;
    }
}