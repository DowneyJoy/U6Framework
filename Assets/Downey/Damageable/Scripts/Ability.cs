using System;
using System.Collections.Generic;
using Downey.TargetingStrategy;
using UnityEngine;
using UnityUtils;
using Object = UnityEngine.Object;

namespace Downey.Damageable
{
    [Serializable]
    public class Ability
    {
        public AudioClip castSfx;
        public GameObject castVfx;
        public GameObject runningVfx;
        [Header("Effects")]
        [SerializeReference] public List<IEffectFactory<IDamageable>> effects = new();
        [Header("Targeting")]
        [SerializeReference] TargetingStrategy.TargetingStrategy targetingStrategy;

        public void Target(TargetingManager targetingManager)
        {
            if (targetingStrategy != null)
                targetingStrategy.Start(this, targetingManager);
        }
        public void Execute(IDamageable target)
        {
            Handle(target);
            foreach (var effect in effects)
            {
                var runtimeEffect = effect.Create();
                target.ApplyEffect(runtimeEffect);
            }
        }
        
        void Handle(IDamageable target)
        {
            var targetMb = target as MonoBehaviour;
            if(targetMb == null) return;
            
            if (castVfx!=null)
            {
                Object.Instantiate(castVfx, targetMb.transform.position.Add(y:2), Quaternion.identity);
            }

            if (runningVfx != null)
            {
                var runningVfxInstance = Object.Instantiate(runningVfx, targetMb.transform);
                Object.Destroy(runningVfxInstance,3f);
            }

        }
    }
}