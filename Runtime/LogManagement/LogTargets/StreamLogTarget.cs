using System;
using System.IO;

namespace GameFramework.LogManagement
{
    public class StreamLogTarget : LogTarget
    {
        ~StreamLogTarget()
        {
            _writer?.Flush();
            _stream?.Flush();
            _stream?.Close();
        }

        public StreamLogTarget(Stream stream)
        {
            if (stream is null) throw new NullReferenceException("Stream is NULL");

            _stream = stream;
            _writer = new StreamWriter(stream);
        }

        protected override void InternalWrite(string logMessage)
        {
            _writer.Write(logMessage);
            _writer.Flush();
        }

        public override void Flush()
        {
            _writer.Flush();
        }

        public Stream stream
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