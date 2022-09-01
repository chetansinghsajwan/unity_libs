using System;
using System.Runtime.CompilerServices;
using UnityAsyncOperation = UnityEngine.AsyncOperation;

namespace GameFramework.Extensions
{
    public class UnityAsyncOperationAwaiter : INotifyCompletion
    {
        public UnityAsyncOperationAwaiter(UnityAsyncOperation asyncOp)
        {
            _continuation = null;
            _operation = asyncOp;

            if (_operation is not null)
            {
                _operation.completed += OnRequestCompleted;
            }
        }

        public bool IsCompleted => _operation.isDone;

        public void GetResult()
        {
        }

        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }

        private void OnRequestCompleted(UnityAsyncOperation asyncOperation)
        {
            _continuation?.Invoke();
        }

        private UnityAsyncOperation _operation;
        private Action _continuation;
    }
}