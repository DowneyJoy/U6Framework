using UnityEngine;
using UnityEngine.UI;

namespace Downey.CodeSmell.Example3
{
    public class Player : MonoBehaviour
    {
        public float luck;

        public bool RollCrit(float mult)
        {
            //return UnityEngine.Random.value < (luck * mult * 0.05f);
            return CritPolicy.Get().Roll(luck, mult);
        }
    }

    public class Enemy : MonoBehaviour
    {
        public float threat;

        public bool TakeHit(float mult)
        {
            //var isCrit = UnityEngine.Random.value < (0.05f * mult * Mathf.Clamp01(1f - threatAdjust()));
            // 
            //return isCrit;
            float luck = Mathf.Clamp01(1f - threat * 0.1f);
            return CritPolicy.Get().Roll(luck, mult);
        }
    }

    public class Tooltip : MonoBehaviour
    {
        public Text text;

        public void ShowCrit(float luck, float mult)
        {
            //var p = 0.05f * luck * mult;
            var p = CritPolicy.Get().Chance(luck, mult);
            text.text = $"Crit chance: {(int)(p * 100)}%";
        }
    }
}