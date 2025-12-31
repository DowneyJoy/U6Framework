using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Refactoring.Patterns
{
    public class EnemySpawner : MonoBehaviour
    {
        public float maxEnemies = 10;
        public List<EnemyConfig> enemyConfigs;
        [Required]
        public PlacementStrategy placementStrategy;
        
        IEnemyFactory factory = new EnemyFactory();
        
        private void Awake()
        {
            SpawnEnemies();
        }

        void SpawnEnemies()
        {
            for (int i = 0; i < maxEnemies; i++)
            {
                Enmey enmey = factory.Create(enemyConfigs[i % enemyConfigs.Count]);
                enmey.transform.position = placementStrategy.SetPosition(transform.position);
            }
        }
    }

}
