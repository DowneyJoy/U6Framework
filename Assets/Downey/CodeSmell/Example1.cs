using UnityEngine;

namespace Downey.CodeSmell.Example1
{
    public class Player : MonoBehaviour
    {
        public Item specialItem;
        public float specialCooldown;
        public float lastSpecialTime = -999f;

        public void Attack()
        {
            if (specialItem != null)
                if (CanUseSpecial()) UseSpecial(specialItem);
            else BasicAttack();
        }

        bool CanUseSpecial()
        {
            if (specialItem == null) return false;
            if (Time.time - lastSpecialTime < specialCooldown) return false;
            return true;
        }

        void UseSpecial(Item item)
        {
            lastSpecialTime = Time.time;
            /*..*/
        }

        void BasicAttack()
        {
            /* ... */
        }
    }
}