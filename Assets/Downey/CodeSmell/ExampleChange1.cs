using UnityEngine;

namespace Downey.CodeSmell
{
    public interface ISpecialAttack
    {
        bool CanUse();
        void Use();
    }

    public class NoopAttack : ISpecialAttack
    {
        public bool CanUse(){return false;}
        public void Use(){}
    }
    public class SpecialAttack : ISpecialAttack
    {
        public Item specialItem;
        public float specialCooldown;
        public float lastSpecialTime = -999f;

        public bool CanUse()
        {
            if (specialItem == null) return false;
            if (Time.time - lastSpecialTime < specialCooldown) return false;
            return true;
        }

        public void Use()
        {
            lastSpecialTime = Time.time;
            // Use the special Item
            Debug.Log("Using special item: " + specialItem.name);
        }
    }
    public class Player : MonoBehaviour
    {
        public ISpecialAttack specialAttack = new NoopAttack();
        public void Attack()
        {
            if (specialAttack.CanUse())
                specialAttack.Use();
            else BasicAttack();
        }
        void BasicAttack()
        {
            /* ... */
        }
    }
}