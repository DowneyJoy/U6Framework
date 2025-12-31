using UnityEngine;

namespace Downey.ImprovedTimers
{
    public class CountdownTimerExample : MonoBehaviour
    {
        public CountdownTimer timer;
        [SerializeField] public float timerDuration = 10;

        void Start()
        {
            timer = new CountdownTimer(timerDuration);
            timer.OnTimerStart += () => Debug.Log("Timer started");
            timer.OnTimerStop += () => Debug.Log("Timer stopped");
            timer.Start();
        }

        void OnDestroy()
        {
            timer.Dispose();
        }
    }
}