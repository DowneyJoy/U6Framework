using UnityEngine;

namespace Downey.AbilityData
{
    public class Health : MonoBehaviour
    {
        public int Hp = 100;

        public void ApplyDamage(int damage)
        {
            Hp -= damage;
        }
    }
}