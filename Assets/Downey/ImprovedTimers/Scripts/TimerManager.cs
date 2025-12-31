using System.Collections.Generic;
using UnityUtils;

namespace Downey.ImprovedTimers
{
    public static class TimerManager
    {
        private static readonly List<Timer> timers = new();
        static readonly List<Timer> sweep = new();
        public static void RegisteredTimer(Timer timer) => timers.Add(timer);
        public static void UnregisteredTimer(Timer timer) => timers.Remove(timer);

        public static void UpdateTimers()
        {
            foreach (Timer timer in new List<Timer>(timers))
            {
                timer.Tick();
            }
        }
        
        public static void Clear()
        { 
            sweep.RefreshWith(timers);
            foreach (Timer timer in sweep)
            {
                timer.Dispose();
            }
            
            timers.Clear();
            sweep.Clear();
        }
    }
}