using System;

namespace GameFramework
{
    public abstract class AsyncOperation
    {
        /// <summary>
        /// Invokes OnCompleted if not null
        /// </summary>
        protected void SafeInvokeOnCompleted()
        {
            OnCompleted?.Invoke(this);
        }

        /// <summary>
        /// Invokes OnCompleted even if is null
        /// </summary>
        protected void InvokeOnCompleted()
        {
            OnCompleted.Invoke(this);
        }

        /// <summary>
        /// What's the operation's progress, 0f-1f
        /// </summary>
        public abstract float progress { get; }

        /// <summary>
        /// What's the operation's progress percentage, 0f-100f
        /// </summary>
        public float percent => progress * 100f;

        /// <summary>
        /// Has the operation finished?
        /// </summary>
        public abstract bool isCompleted { get; }

        /// <summary>
        /// Event invoked when operation is completed
        /// </summary>
        public event Action<AsyncOperation> OnCompleted;
    }
}