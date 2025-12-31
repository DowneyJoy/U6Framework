using UnityEngine;

namespace Downey.CodeSmell
{
    public interface IEnemyBehaviour { void Tick(Enemy enemy); }
    public class Enemy : MonoBehaviour
    {
        private IEnemyBehaviour b;

        void Awake() => b = GetComponent<IEnemyBehaviour>();
        void Update() => b?.Tick(this);
    }

    [DisallowMultipleComponent]
    public class FlyingEnemy : MonoBehaviour, IEnemyBehaviour
    {
        public void Tick(Enemy enemy){}
    }
    [DisallowMultipleComponent]
    public class GroundEnemy : MonoBehaviour, IEnemyBehaviour
    {
        public void Tick(Enemy enemy){}
    }
    [DisallowMultipleComponent]
    public class BossEnemy : MonoBehaviour, IEnemyBehaviour
    {
        public void Tick(Enemy enemy){}
    }
}