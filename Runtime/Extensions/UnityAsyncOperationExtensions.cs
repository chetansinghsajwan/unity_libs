using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityAsyncOperation = UnityEngine.AsyncOperation;

namespace GameFramework.Extensions
{
    public static class UnityAsyncOperationExtensions
    {
        public static UnityAsyncOperationAwaiter GetAwaiter(this UnityAsyncOperation operation)
        {
            return new UnityAsyncOperationAwaiter(operation);
        }

        public static UnityAsyncOperationsAwaiter GetAwaiter(this IEnumerable<UnityAsyncOperation> operations)
        {
            return new UnityAsyncOperationsAwaiter(operations);
        }
    }
}