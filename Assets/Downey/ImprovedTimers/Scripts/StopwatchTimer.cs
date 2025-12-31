using UnityEngine;

namespace Downey.ImprovedTimers
{
    /// <summary>
    /// Timer that counts up from zero infinity. Great from measuring duration.
    /// </summary>
    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base(0){}

        public override void Tick()
        {
            if (IsRunning)
            {
                CurrentTime += Time.deltaTime;
            }
        }

        public override bool IsFinished => false;
    }
}