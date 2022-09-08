using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityAsyncOperation = UnityEngine.AsyncOperation;

namespace GameFramework.Extensions
{
    public class UnityAsyncOperationsAwaiter : INotifyCompletion
    {
        public UnityAsyncOperationsAwaiter(IEnumerable<UnityAsyncOperation> asyncOps)
        {
            _length = 0;
            _completed = 0;
            _continuation = null;
            _operations = asyncOps;

            _operations ??= new UnityAsyncOperation[0];
            foreach (var operation in _operations)
            {
                operation.completed += OnRequestCompleted;
                _length++;
            }
        }

        public bool IsCompleted
        {
            get
            {
                foreach (var operation in _operations)
                {
                    if (operation.isDone is false)
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

        private void OnRequestCompleted(UnityAsyncOperation asyncOperation)
        {
            _completed++;

            if (_completed == _length)
            {
                _continuation?.Invoke();
            }
        }

        private IEnumerable<UnityAsyncOperation> _operations;
        private Action _continuation;
        private int _completed;
        private int _length;
    }
}