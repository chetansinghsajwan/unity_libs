using System;
using UnityEngine;

namespace GameFramework.Logging
{
    public interface ILogger
    {
        private static readonly object[] NoPropertyValues = new object[0];

        /// <summary>
        /// Write an event to the log.
        /// </summary>
        /// <param name="logEvent">The event to write.</param>
        void Write(LogEvent logEvent);

        /// <summary>
        /// Flushes the buffer.
        /// </summary>
        void Flush();

        /// <summary>
        /// Write a log event with the specified level.
        /// </summary>
        /// <param name="level">The level of the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        void Write(LogLevel level, string messageTemplate)
        {
            // Avoid the array allocation and any boxing allocations when the level isn't enabled
            if (IsEnabled(level))
            {
                Write(level, messageTemplate, NoPropertyValues);
            }
        }

        /// <summary>
        /// Write a log event with the specified level.
        /// </summary>
        /// <param name="level">The level of the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        void Write<T>(LogLevel level, string messageTemplate, T propertyValue)
        {
            // Avoid the array allocation and any boxing allocations when the level isn't enabled
            if (IsEnabled(level))
            {
                Write(level, messageTemplate, new object[] { propertyValue });
            }
        }

        /// <summary>
        /// Write a log event with the specified level.
        /// </summary>
        /// <param name="level">The level of the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        void Write<T0, T1>(LogLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            // Avoid the array allocation and any boxing allocations when the level isn't enabled
            if (IsEnabled(level))
            {
                Write(level, messageTemplate, new object[] { propertyValue0, propertyValue1 });
            }
        }

        /// <summary>
        /// Write a log event with the specified level.
        /// </summary>
        /// <param name="level">The level of the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        void Write<T0, T1, T2>(LogLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            // Avoid the array allocation and any boxing allocations when the level isn't enabled
            if (IsEnabled(level))
            {
                Write(level, messageTemplate, new object[] { propertyValue0, propertyValue1, propertyValue2 });
            }
        }

        /// <summary>
        /// Write a log event with the specified level.
        /// </summary>
        /// <param name="level">The level of the event.</param>
        /// <param name="messageTemplate"></param>
        /// <param name="propertyValues"></param>
        void Write(LogLevel level, string messageTemplate, params object[] propertyValues)
            => Write(level, (Exception)null, messageTemplate, propertyValues);

        /// <summary>
        /// Write a log event with the specified level and associated exception.
        /// </summary>
        /// <param name="level">The level of the event.</param>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        void Write(LogLevel level, Exception exception, string messageTemplate)
        {
            // Avoid the array allocation and any boxing allocations when the level isn't enabled
            if (IsEnabled(level))
            {
                Write(level, exception, messageTemplate, NoPropertyValues);
            }
        }

        /// <summary>
        /// Write a log event with the specified level and associated exception.
        /// </summary>
        /// <param name="level">The level of the event.</param>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        void Write<T>(LogLevel level, Exception exception, string messageTemplate, T propertyValue)
        {
            // Avoid the array allocation and any boxing allocations when the level isn't enabled
            if (IsEnabled(level))
            {
                Write(level, exception, messageTemplate, new object[] { propertyValue });
            }
        }

        /// <summary>
        /// Write a log event with the specified level and associated exception.
        /// </summary>
        /// <param name="level">The level of the event.</param>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        void Write<T0, T1>(LogLevel level, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            // Avoid the array allocation and any boxing allocations when the level isn't enabled
            if (IsEnabled(level))
            {
                Write(level, exception, messageTemplate, new object[] { propertyValue0, propertyValue1 });
            }
        }

        /// <summary>
        /// Write a log event with the specified level and associated exception.
        /// </summary>
        /// <param name="level">The level of the event.</param>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        void Write<T0, T1, T2>(LogLevel level, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            // Avoid the array allocation and any boxing allocations when the level isn't enabled
            if (IsEnabled(level))
            {
                Write(level, exception, messageTemplate, new object[] { propertyValue0, propertyValue1, propertyValue2 });
            }
        }

        /// <summary>
        /// Write a log event with the specified level and associated exception.
        /// </summary>
        /// <param name="level">The level of the event.</param>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        void Write(LogLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (!IsEnabled(level)) return;
            if (messageTemplate == null) return;

            // // Catch a common pitfall when a single non-object array is cast to object[]
            // if (propertyValues != null &&
            //     propertyValues.GetType() != typeof(object[]))
            //     propertyValues = new object[] { propertyValues };

            // if (BindMessageTemplate(messageTemplate, propertyValues, out var parsedTemplate, out var boundProperties))
            // {
            //     Write(new LogEvent(DateTimeOffset.Now, level, exception, parsedTemplate, boundProperties));
            // }

            LogEvent logEvent = new LogEvent();
            logEvent.messageTemplate = messageTemplate;
            logEvent.properties = propertyValues;
            logEvent.timeStamp = DateTimeOffset.Now;
            logEvent.frame = Time.frameCount;
            logEvent.exception = exception;
            logEvent.level = level;
            logEvent.category = "";

            Write(logEvent);
        }

        /// <summary>
        /// Determine if events at the specified level will be passed through
        /// to the log sinks.
        /// </summary>
        /// <param name="level">Level to check.</param>
        /// <returns>True if the level is enabled; otherwise, false.</returns>
        bool IsEnabled(LogLevel level)
            => true;

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Verbose"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example><code>
        /// Log.Verbose("Staring into space, wondering if we're alone.");
        /// </code></example>
        void Verbose(string messageTemplate)
            => Write(LogLevel.Verbose, messageTemplate, NoPropertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Verbose"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Verbose("Staring into space, wondering if we're alone.");
        /// </code></example>
        void Verbose<T>(string messageTemplate, T propertyValue)
            => Write(LogLevel.Verbose, messageTemplate, propertyValue);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Verbose"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Verbose("Staring into space, wondering if we're alone.");
        /// </code></example>
        void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => Write(LogLevel.Verbose, messageTemplate, propertyValue0, propertyValue1);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Verbose"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Verbose("Staring into space, wondering if we're alone.");
        /// </code></example>
        void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => Write(LogLevel.Verbose, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Verbose"/> level and associated exception.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Verbose("Staring into space, wondering if we're alone.");
        /// </code></example>
        void Verbose(string messageTemplate, params object[] propertyValues)
            => Verbose((Exception)null, messageTemplate, propertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Verbose"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example><code>
        /// Log.Verbose(ex, "Staring into space, wondering where this comet came from.");
        /// </code></example>
        void Verbose(Exception exception, string messageTemplate)
            => Write(LogLevel.Verbose, exception, messageTemplate, NoPropertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Verbose"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Verbose(ex, "Staring into space, wondering where this comet came from.");
        /// </code></example>
        void Verbose<T>(Exception exception, string messageTemplate, T propertyValue)
            => Write(LogLevel.Verbose, exception, messageTemplate, propertyValue);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Verbose"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Verbose(ex, "Staring into space, wondering where this comet came from.");
        /// </code></example>
        void Verbose<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => Write(LogLevel.Verbose, exception, messageTemplate, propertyValue0, propertyValue1);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Verbose"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Verbose(ex, "Staring into space, wondering where this comet came from.");
        /// </code></example>
        void Verbose<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => Write(LogLevel.Verbose, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Verbose"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Verbose(ex, "Staring into space, wondering where this comet came from.");
        /// </code></example>
        void Verbose(Exception exception, string messageTemplate, params object[] propertyValues)
            => Write(LogLevel.Verbose, exception, messageTemplate, propertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example><code>
        /// Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
        /// </code></example>
        void Debug(string messageTemplate)
            => Write(LogLevel.Debug, messageTemplate, NoPropertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
        /// </code></example>
        void Debug<T>(string messageTemplate, T propertyValue)
            => Write(LogLevel.Debug, messageTemplate, propertyValue);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
        /// </code></example>
        void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => Write(LogLevel.Debug, messageTemplate, propertyValue0, propertyValue1);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
        /// </code></example>
        void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => Write(LogLevel.Debug, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Debug"/> level and associated exception.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
        /// </code></example>
        void Debug(string messageTemplate, params object[] propertyValues)
            => Debug((Exception)null, messageTemplate, propertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Debug"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example><code>
        /// Log.Debug(ex, "Swallowing a mundane exception.");
        /// </code></example>
        void Debug(Exception exception, string messageTemplate)
            => Write(LogLevel.Debug, exception, messageTemplate, NoPropertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Debug"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Debug(ex, "Swallowing a mundane exception.");
        /// </code></example>
        void Debug<T>(Exception exception, string messageTemplate, T propertyValue)
            => Write(LogLevel.Debug, exception, messageTemplate, propertyValue);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Debug"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Debug(ex, "Swallowing a mundane exception.");
        /// </code></example>
        void Debug<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => Write(LogLevel.Debug, exception, messageTemplate, propertyValue0, propertyValue1);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Debug"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Debug(ex, "Swallowing a mundane exception.");
        /// </code></example>
        void Debug<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => Write(LogLevel.Debug, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Debug"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Debug(ex, "Swallowing a mundane exception.");
        /// </code></example>
        void Debug(Exception exception, string messageTemplate, params object[] propertyValues)
            => Write(LogLevel.Debug, exception, messageTemplate, propertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Information"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example><code>
        /// Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </code></example>
        void Information(string messageTemplate)
            => Write(LogLevel.Information, messageTemplate, NoPropertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Information"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </code></example>
        void Information<T>(string messageTemplate, T propertyValue)
            => Write(LogLevel.Information, messageTemplate, propertyValue);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Information"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </code></example>
        void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => Write(LogLevel.Information, messageTemplate, propertyValue0, propertyValue1);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Information"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </code></example>
        void Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => Write(LogLevel.Information, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Information"/> level and associated exception.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </code></example>
        void Information(string messageTemplate, params object[] propertyValues)
            => Information((Exception)null, messageTemplate, propertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Information"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example><code>
        /// Log.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </code></example>
        void Information(Exception exception, string messageTemplate)
            => Write(LogLevel.Information, exception, messageTemplate, NoPropertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Information"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </code></example>
        void Information<T>(Exception exception, string messageTemplate, T propertyValue)
            => Write(LogLevel.Information, exception, messageTemplate, propertyValue);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Information"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </code></example>
        void Information<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => Write(LogLevel.Information, exception, messageTemplate, propertyValue0, propertyValue1);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Information"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </code></example>
        void Information<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => Write(LogLevel.Information, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Information"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
        /// </code></example>
        void Information(Exception exception, string messageTemplate, params object[] propertyValues)
            => Write(LogLevel.Information, exception, messageTemplate, propertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Warning"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example><code>
        /// Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
        /// </code></example>
        void Warning(string messageTemplate)
            => Write(LogLevel.Warning, messageTemplate, NoPropertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Warning"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
        /// </code></example>
        void Warning<T>(string messageTemplate, T propertyValue)
            => Write(LogLevel.Warning, messageTemplate, propertyValue);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Warning"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
        /// </code></example>
        void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => Write(LogLevel.Warning, messageTemplate, propertyValue0, propertyValue1);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Warning"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
        /// </code></example>
        void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => Write(LogLevel.Warning, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Warning"/> level and associated exception.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
        /// </code></example>
        void Warning(string messageTemplate, params object[] propertyValues)
            => Warning((Exception)null, messageTemplate, propertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Warning"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example><code>
        /// Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
        /// </code></example>
        void Warning(Exception exception, string messageTemplate)
            => Write(LogLevel.Warning, exception, messageTemplate, NoPropertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Warning"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
        /// </code></example>
        void Warning<T>(Exception exception, string messageTemplate, T propertyValue)
            => Write(LogLevel.Warning, exception, messageTemplate, propertyValue);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Warning"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
        /// </code></example>
        void Warning<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => Write(LogLevel.Warning, exception, messageTemplate, propertyValue0, propertyValue1);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Warning"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
        /// </code></example>
        void Warning<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => Write(LogLevel.Warning, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Warning"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
        /// </code></example>
        void Warning(Exception exception, string messageTemplate, params object[] propertyValues)
            => Write(LogLevel.Warning, exception, messageTemplate, propertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example><code>
        /// Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
        /// </code></example>
        void Error(string messageTemplate)
            => Write(LogLevel.Error, messageTemplate, NoPropertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
        /// </code></example>
        void Error<T>(string messageTemplate, T propertyValue)
            => Write(LogLevel.Error, messageTemplate, propertyValue);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
        /// </code></example>
        void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => Write(LogLevel.Error, messageTemplate, propertyValue0, propertyValue1);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
        /// </code></example>
        void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => Write(LogLevel.Error, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Error"/> level and associated exception.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
        /// </code></example>
        void Error(string messageTemplate, params object[] propertyValues)
            => Error((Exception)null, messageTemplate, propertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Error"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example><code>
        /// Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
        /// </code></example>
        void Error(Exception exception, string messageTemplate)
            => Write(LogLevel.Error, exception, messageTemplate, NoPropertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Error"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
        /// </code></example>
        void Error<T>(Exception exception, string messageTemplate, T propertyValue)
            => Write(LogLevel.Error, exception, messageTemplate, propertyValue);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Error"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
        /// </code></example>
        void Error<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => Write(LogLevel.Error, exception, messageTemplate, propertyValue0, propertyValue1);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Error"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
        /// </code></example>
        void Error<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => Write(LogLevel.Error, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Error"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
        /// </code></example>
        void Error(Exception exception, string messageTemplate, params object[] propertyValues)
            => Write(LogLevel.Error, exception, messageTemplate, propertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example><code>
        /// Log.Fatal("Process terminating.");
        /// </code></example>
        void Fatal(string messageTemplate)
            => Write(LogLevel.Fatal, messageTemplate, NoPropertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Fatal("Process terminating.");
        /// </code></example>
        void Fatal<T>(string messageTemplate, T propertyValue)
            => Write(LogLevel.Fatal, messageTemplate, propertyValue);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Fatal("Process terminating.");
        /// </code></example>
        void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => Write(LogLevel.Fatal, messageTemplate, propertyValue0, propertyValue1);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Fatal("Process terminating.");
        /// </code></example>
        void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => Write(LogLevel.Fatal, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Fatal"/> level and associated exception.
        /// </summary>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Fatal("Process terminating.");
        /// </code></example>
        void Fatal(string messageTemplate, params object[] propertyValues)
            => Fatal((Exception)null, messageTemplate, propertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Fatal"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <example><code>
        /// Log.Fatal(ex, "Process terminating.");
        /// </code></example>
        void Fatal(Exception exception, string messageTemplate)
            => Write(LogLevel.Fatal, exception, messageTemplate, NoPropertyValues);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Fatal"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Fatal(ex, "Process terminating.");
        /// </code></example>
        void Fatal<T>(Exception exception, string messageTemplate, T propertyValue)
            => Write(LogLevel.Fatal, exception, messageTemplate, propertyValue);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Fatal"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Fatal(ex, "Process terminating.");
        /// </code></example>
        void Fatal<T0, T1>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
            => Write(LogLevel.Fatal, exception, messageTemplate, propertyValue0, propertyValue1);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Fatal"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template.</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Fatal(ex, "Process terminating.");
        /// </code></example>
        void Fatal<T0, T1, T2>(Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
            => Write(LogLevel.Fatal, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

        /// <summary>
        /// Write a log event with the <see cref="LogLevel.Fatal"/> level and associated exception.
        /// </summary>
        /// <param name="exception">Exception related to the event.</param>
        /// <param name="messageTemplate">Message template describing the event.</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template.</param>
        /// <example><code>
        /// Log.Fatal(ex, "Process terminating.");
        /// </code></example>
        void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
            => Write(LogLevel.Fatal, exception, messageTemplate, propertyValues);
    }
}