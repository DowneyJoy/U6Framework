using System.Collections.Generic;
using UnityEngine;

namespace Downey.Damageable
{
    public class Enemy : MonoBehaviour , IDamageable
    {
        public int health = 50;
        private readonly List<IEffect<IDamageable>> activeEffects = new();

        public void TakeDamage(int amount)
        {
            health -= amount;
            Debug.Log($"Enemy took {amount} damage. Health now {health}");

            if (health <= 0)
                Die();
        }

        public void ApplyEffect(IEffect<IDamageable> effect)
        {
            effect.OnCompleted += RemoveEffect;
            activeEffects.Add(effect);
            effect.Apply(this);
        }

        void RemoveEffect(IEffect<IDamageable> effect)
        {
            effect.OnCompleted -= RemoveEffect;
            activeEffects.Remove(effect);
        }

        void Die()
        {
            Debug.Log("Enemy died.");

            foreach (var effect in activeEffects)
            {
                effect.OnCompleted -= RemoveEffect;
                effect.Cancel();
            }
            activeEffects.Clear();
            Destroy(gameObject);
        }
    }
}