using System.Collections.Generic;

namespace GameFramework
{
    public static class AsyncOperationExtensions
    {
        public static AsyncOperationAwaiter GetAwaiter(this AsyncOperation operation)
        {
            return new AsyncOperationAwaiter(operation);
        }

        public static AsyncOperationsAwaiter GetAwaiter(this IEnumerable<AsyncOperation> operations)
        {
            return new AsyncOperationsAwaiter(operations);
        }
    }
}