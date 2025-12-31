using UnityEngine;

namespace Refactoring.Patterns
{
    public interface IEnemyFactory
    {
        Enmey Create(EnemyConfig  config);
    }

    public class EnemyFactory : IEnemyFactory
    {
        public Enmey Create(EnemyConfig config)
        {
            GameObject enemy = Object.Instantiate(config.prefab);
            Enmey enemyComponent = enemy.GetComponent<Enmey>();
            enemyComponent.Initialize(config);
            return enemyComponent;
        }
    }
}
