using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GameFramework
{
    public class AsyncOperationsAwaiter : INotifyCompletion
    {
        public AsyncOperationsAwaiter(IEnumerable<AsyncOperation> asyncOps)
        {
            _length = 0;
            _completed = 0;
            _continuation = null;
            _operations = asyncOps;

            _operations ??= new AsyncOperation[0];
            foreach (var operation in _operations)
            {
                operation.OnCompleted += OnOperationCompleted;
                _length++;
            }
        }

        public bool IsCompleted
        {
            get
            {
                foreach (var operation in _operations)
                {
                    if (operation.isCompleted == false)
                        return false;
                }

                return true;
            }
        }

        public void GetResult() { }

        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }

        private void OnOperationCompleted(AsyncOperation asyncOperation)
        {
            _completed++;

            if (_completed == _length)
            {
                _continuation();
            }
        }

        private IEnumerable<AsyncOperation> _operations;
        private Action _continuation;
        private int _completed;
        private int _length;
    }
}