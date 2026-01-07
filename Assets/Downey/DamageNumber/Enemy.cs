using Downey.ImprovedTimers;
using UnityEngine;

namespace Downey.DamageNumber
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private DamageNumberSpawner damageNumberSpawner;
        [SerializeField] private int damageAmount = 10;
        [SerializeField] private float spwanInterval = 0.2f;

        private CountdownTimer timer;

        void Start()
        {
            timer = new CountdownTimer(spwanInterval);
            timer.OnTimerStop += () =>
            {
                damageNumberSpawner.SpawnDamageNumber(damageAmount, transform.position);
                timer.Start();
            };
            timer.Start();
        }
        
        public void TakeDamage(int damage) => damageNumberSpawner.SpawnDamageNumber(damage, transform.position);
    }
}