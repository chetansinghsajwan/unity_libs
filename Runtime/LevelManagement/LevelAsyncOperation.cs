namespace GameFramework.LevelManagement
{
    public class LevelAsyncOperation : AggregateAsyncOperation
    {
        public static readonly LevelAsyncOperation Completed
            = new LevelAsyncOperation();

        public LevelAsyncOperation() { }
    }
}