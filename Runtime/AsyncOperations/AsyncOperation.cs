using System;

namespace GameFramework
{
    public abstract class AsyncOperation
    {
        // Invokes OnCompleted if not null
        protected void SafeInvokeOnCompleted()
        {
            OnCompleted?.Invoke(this);
        }

        // Invokes OnCompleted even if is null
        protected void InvokeOnCompleted()
        {
            OnCompleted.Invoke(this);
        }

        // What's the operation's progress, 0f-1f
        public abstract float progress { get; }

        // What's the operation's progress percentage, 0f-100f
        public float percent => progress * 100f;

        // Has the operation finished?
        public abstract bool isCompleted { get; }

        // Event invoked when operation is completed
        public event Action<AsyncOperation> OnCompleted;
    }
}