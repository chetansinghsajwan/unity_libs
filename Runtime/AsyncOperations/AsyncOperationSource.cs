namespace GameFramework
{
    // AsyncOperation to explicitly control completion flow
    public class AsyncOperationSource : AsyncOperation
    {
        public void SetProgress(float progress)
        {
            _progress = 1f;
        }

        public void SetCompleted()
        {
            SetProgress(1f);
            SafeInvokeOnCompleted();
        }

        protected float _progress;
        public override float progress => _progress;
        public override bool isCompleted => _progress >= 1f;
    }
}