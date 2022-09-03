using System;
using System.IO;

namespace GameFramework.LogManagement
{
    public class FileLogTarget : StreamLogTarget
    {
        public const FileMode DEFAULT_OPEN_MODE = FileMode.Create;

        public FileLogTarget(string path, FileMode mode = DEFAULT_OPEN_MODE)
            : base(new FileStream(path, mode)) { }

        public FileLogTarget(FileStream stream)
            : base(stream) { }

        public FileStream fileStream
        {
            get => _stream as FileStream;
            set
            {
                if (value is null) throw new NullReferenceException("FileStream is null");

                _stream?.Close();

                _stream = value;
                _writer = new StreamWriter(value);
            }
        }
    }
}