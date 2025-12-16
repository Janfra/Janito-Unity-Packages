using UnityEngine;

namespace Janito.Timers
{
    public class CountdownTimer : BaseTimer
    {
        public override bool IsFinished => RemainingTime <= 0.0f;
        public CountdownTimer(float initialTime) : base(initialTime) { }
        public override void Tick()
        {
            if (!IsActive) return;

            if (RemainingTime > 0.0f)
            {
                RemainingTime -= Time.deltaTime;
            }

            if (IsFinished)
            {
                RemainingTime = 0.0f;
                StopTimer();
            }
        }
    }
}
