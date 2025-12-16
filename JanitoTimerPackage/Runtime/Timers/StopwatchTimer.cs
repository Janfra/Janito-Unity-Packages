using UnityEngine;

namespace Janito.Timers
{
    /// <summary>
    /// Counts time since the stopwatch has been started. Not meant to expect it to stop automatically, it should be manually stopped.
    /// </summary>
    /// </remarks>Counts time elapsed instead of remaining time. As such, it is recommended to use elapsed time and not remaining time.<remarks>
    public class StopwatchTimer : BaseTimer
    {
        public override bool IsFinished => false;

        public StopwatchTimer(float initialTime) : base(initialTime) 
        {
            _InitialTime = 0.0f;
        }

        public override void Reset()
        {
            RemainingTime = 0.0f;
        }

        public override void Reset(float newTime)
        {
            RemainingTime = 0.0f;
        }

        public override void Tick()
        {
            if (!IsActive) return;

            RemainingTime += Time.deltaTime;
        }
    }
}
