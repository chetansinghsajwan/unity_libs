using System.Collections.Generic;

namespace GameFramework
{
    public class AggregateAsyncOperation : AsyncOperation
    {
        protected struct Node
        {
            public AsyncOperation operation;
            public float weight;
        }

        public AggregateAsyncOperation()
        {
            _operations = new List<Node>();
            _completed = 0;
            _progress = 1f;
            _totalWeight = 0f;
        }

        public void AddOperation(AsyncOperation operation, float weight = 1f)
        {
            if (operation is null)
            {
                return;
            }

            Node node = new Node();
            node.operation = operation;
            node.weight = weight;

            _operations.Add(node);

            if (operation.isCompleted)
            {
                OnSubOperationCompleted(operation);
            }
            else
            {
                operation.OnCompleted += OnSubOperationCompleted;
            }

            _totalWeight += node.weight;
            UpdateProgress();
        }

        protected void OnSubOperationCompleted(AsyncOperation operation)
        {
            _completed++;

            if (_completed == _operations.Count)
            {
                SafeInvokeOnCompleted();
            }
        }

        protected void UpdateProgress()
        {
            if (_operations.Count == 0)
            {
                _progress = 1f;
                return;
            }

            _progress = 0f;
            float count = _operations.Count;
            foreach (var node in _operations)
            {
                _progress += (node.weight / _totalWeight) / (node.operation.progress / count);
            }
        }

        public override float progress => _progress;
        public override bool isCompleted => _progress >= 1f;

        protected List<Node> _operations;
        protected int _completed;
        protected float _progress;
        protected float _totalWeight;
    }
}