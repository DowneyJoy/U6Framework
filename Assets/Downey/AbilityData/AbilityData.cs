using System;
using System.Collections.Generic;
using UnityEngine;

namespace Downey.AbilityData
{
    [CreateAssetMenu(fileName = "AbilityData", menuName = "AbilityDatas/AbilityData")]
    class AbilityData : ScriptableObject
    {
        public string label;
        public AnimationClip AnimationClip;
        [Range(0f, 1f)] public float castTime = 2f;
        public GameObject vfxPrefab;
        [SerializeReference] public List<AbilityEffect> effects;

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(label)) label = name;
            if (effects == null) effects = new List<AbilityEffect>();
        }
    }

    [Serializable]
    abstract class AbilityEffect
    {
        public abstract void Execute(GameObject caster, GameObject target);
    }

    [Serializable]
    class DamageEffect : AbilityEffect
    {
        public int amount;

        public override void Execute(GameObject caster, GameObject target)
        {
            target.GetComponent<Health>().ApplyDamage(amount);
            Debug.Log($"{caster.name} dealt {amount} damage to {target.name}");
        }
    }

    [Serializable]
    class KnockbackEffect : AbilityEffect
    {
        public float force;

        public override void Execute(GameObject caster, GameObject target)
        {
            var dir = (target.transform.position - caster.transform.position).normalized;
            target.GetComponent<Rigidbody>().AddForce(dir * force,ForceMode.Impulse);
            Debug.Log($"{caster.name} knocked back {target.name} with force {force}");
        }
    }
}