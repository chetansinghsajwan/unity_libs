using System;
using System.IO;

namespace GameFramework.LogManagement
{
    public class StreamLogTarget : ILogTarget
    {
        ~StreamLogTarget()
        {
            _stream?.Close();
        }

        public StreamLogTarget(Stream stream)
        {
            if (stream is null) throw new NullReferenceException("Stream is NULL");

            _stream = stream;
            _writer = new StreamWriter(stream);
        }

        public void Log(LogEvent logEvent)
        {
            _writer.Write(logEvent.messageTemplate);
        }

        public void Flush()
        {
            _writer.Flush();
        }

        protected Stream stream
        {
            get => _stream;
            set
            {
                if (value is null) throw new NullReferenceException("Stream is null");

                _stream?.Close();

                _stream = value;
                _writer = new StreamWriter(value);
            }
        }

        public StreamWriter writer
        {
            get => _writer;
        }

        protected Stream _stream;
        protected StreamWriter _writer;
    }
}