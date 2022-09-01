using System;
using System.Runtime.CompilerServices;

namespace GameFramework
{
    public class AsyncOperationAwaiter : INotifyCompletion
    {
        public AsyncOperationAwaiter(AsyncOperation asyncOp)
        {
            _continuation = null;
            _operation = asyncOp;

            if (_operation is not null)
            {
                _operation.OnCompleted += OnOperationCompleted;
            }
        }

        public bool IsCompleted => _operation.isCompleted;

        public virtual void GetResult() { }

        public virtual void OnCompleted(Action continuation)
        {
            _continuation = continuation;
        }

        protected virtual void OnOperationCompleted(AsyncOperation asyncOp)
        {
            _continuation?.Invoke();
        }

        protected AsyncOperation _operation;
        protected Action _continuation;
    }
}