using System.Collections.Generic;
using UnityEngine;

namespace Downey.CodeSmell
{
    public interface ICritPolicy
    {
        float Chance(float luck,float mult);
        bool Roll(float luck,float mult);
    }

    class CritPolicy : ICritPolicy
    {
        public float baseRate;
        static readonly Dictionary<float, CritPolicy> cache = new();
        private CritPolicy(float baseRate) => this.baseRate = baseRate;
        public float Chance(float luck,float mult) => Mathf.Clamp01(baseRate * luck * mult);
        public bool Roll(float luck,float mult) => UnityEngine.Random.value < Chance(luck, mult);

        public static CritPolicy Get(float baseRate = 0.05f)
        {
            if (!cache.TryGetValue(baseRate, out var policy))
            {
                policy = new CritPolicy(baseRate);
                cache[baseRate] =  policy;
            }
            return policy;
        }
    }
}