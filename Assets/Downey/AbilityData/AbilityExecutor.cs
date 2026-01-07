using System;
using Downey.AdvancedController;
using Downey.ImprovedTimers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtils;

namespace Downey.AbilityData
{
    public class AbilityExecutor : MonoBehaviour
    {
        [SerializeField] AbilityData ability;
        [SerializeField]GameObject target;
        
        AnimationController animationController;
        CountdownTimer castTimer;

        private void Awake()
        {
            animationController = GetComponent<AnimationController>();
            castTimer = new CountdownTimer(ability.castTime);
            //castTimer.OnTimerStart = () => animationController.OrNull()?.Pla
        }

        void SpawnVFX()
        {
            if(ability.vfxPrefab == null) return;
            var vfx = Instantiate(ability.vfxPrefab,transform.position,transform.rotation);
        }

        public void Execute(GameObject target)
        {
            foreach (var effect in ability.effects)
            {
                effect.Execute(gameObject,target);
            }
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Execute(target);
            }
        }
    }
}