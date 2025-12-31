using System;
using Downey.InputManager;
using UnityEngine;

namespace Downey.TargetingStrategy
{
    public class TargetingManager : MonoBehaviour
    {
        public InputReader input;
        public Camera cam;
        
        TargetingStrategy currentStrategy;

        private void Update()
        {
            if (currentStrategy != null && currentStrategy.IsTargeting)
            {
                currentStrategy.Update();
            }
        }
        
        public void SetTargetingStrategy(TargetingStrategy strategy) => currentStrategy = strategy;
        public void ClearTargetingStrategy() => currentStrategy = null;
    }
}
