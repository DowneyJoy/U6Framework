using System;
using System.Collections;
using Downey.TargetingStrategy;
using UnityEngine;
using UnityUtils;

namespace Downey.Damageable
{
    [RequireComponent(typeof(TargetingManager))]
    public class PlayerAbilityCaster : MonoBehaviour
    {
        public Ability[] hotbars;
        public TargetingManager targetingManager;
        private void Update()
        {
            for (int i = 0; i < hotbars.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    Cast(hotbars[i]);
                }
            }
        }

        void Cast(Ability ability)
        {
            ability.Target(targetingManager);
            if (ability.castSfx)
            {
                AudioSource.PlayClipAtPoint(ability.castSfx, transform.position);
            }
        }
    }
}