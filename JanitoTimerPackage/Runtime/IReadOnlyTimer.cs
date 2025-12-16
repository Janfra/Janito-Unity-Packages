namespace Janito.Timers
{
    public interface IReadOnlyTimer 
    {
        public float RemainingTime { get; }
        public float ElapsedTime { get; }
        public bool IsActive { get; }
        public bool IsFinished { get; }
        public bool IsPaused { get; }  
        public float Progress { get; }
        public float InverseProgress { get; }   
    }
}
