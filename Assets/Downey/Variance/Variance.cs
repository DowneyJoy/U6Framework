using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downey.Variance
{
    delegate void TargetHandler<in T>(T target);
    delegate T SelectTarget<out T>();
    //delegate T BadSelector<T>(T candidate);

    class TargetingSystem : MonoBehaviour
    {
        private void Start()
        {
            TargetHandler<Player> playerHandler = LogTarget;
            TargetHandler<Target> targetHandler2 = new TargetHandler<Target>(LogTarget);

            SelectTarget<Boss> selectBoss = () => new GameObject("Boss").AddComponent<Boss>();
            SelectTarget<Enemy> sslectEnemy = selectBoss;
            Enemy e = sslectEnemy();
            
            
            List<Boss> bosses = new List<Boss>();
            DamageAll(bosses);
            IEnumerator<Boss> das;
        }

        void DamageAll(IEnumerable<Enemy> enemies)
        {
            foreach (Boss e in enemies)
            {
                e.health -= 10;
            }
            IComparer<string> comparer = Comparer<string>.Create((a, b) => a.CompareTo(b));
        }

        void LogTarget(Target t)
        {
            Debug.Log($"Targeting {t.name}");
        }
    }
    class Target : MonoBehaviour { }
    class Player : Target { }

    class Enemy : MonoBehaviour
    {
        public int health;
    }
    class Boss : Enemy { }
    
}