using System;
using System.Threading;
using System.Collections.Concurrent;

namespace GameFramework.LogManagement
{
    public class AsyncLogger : ILogger
    {
        ~AsyncLogger()
        {
            _dispose = true;
            _worker.Join();

            _mSyncEvent.Close();
            _aSyncEvent.Close();
        }

        public AsyncLogger(ILogger logger, bool completeTasksBeforeDisposing = true)
        {
            if (logger is null)
            {
                throw new NullReferenceException("AsyncLogger: cannot log to NULL logger");
            }

            _completeTasksBeforeDisposing = completeTasksBeforeDisposing;

            _logger = logger;
            _dispose = false;
            _logEvents = new ConcurrentQueue<LogEvent>();
            _aSyncEvent = new AutoResetEvent(false);
            _mSyncEvent = new ManualResetEvent(false);
            _worker = new Thread(WorkerProc);

            _worker.Start();
        }

        protected void WorkerProc()
        {
            while (true)
            {
                if (_dispose)
                {
                    if (_logEvents.Count == 0 && _completeTasksBeforeDisposing is false)
                    {
                        _mSyncEvent.Set();
                        break;
                    }
                }

                if (_logEvents.TryDequeue(out LogEvent logEvent))
                {
                    try
                    {
                        _logger.Write(logEvent);
                    }
                    catch (Exception) { }
                }
                else
                {
                    _mSyncEvent.Set();
                    _aSyncEvent.WaitOne();
                    _mSyncEvent.Reset();
                }
            }
        }

        public void Wait()
        {
            _mSyncEvent.WaitOne();
        }

        public void Write(LogEvent logEvent)
        {
            _logEvents.Enqueue(logEvent);
            _aSyncEvent.Set();
        }

        public void Flush()
        {
            Wait();
        }

        protected readonly ConcurrentQueue<LogEvent> _logEvents;
        protected readonly AutoResetEvent _aSyncEvent;
        protected readonly ManualResetEvent _mSyncEvent;
        protected readonly Thread _worker;
        protected readonly ILogger _logger;
        protected readonly bool _completeTasksBeforeDisposing;
        protected bool _dispose;
    }
}