using UnityEngine;

namespace Refactoring.Patterns
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        public GameObject prefab;
        public string type;
        public int health;
        public int damage;
        public float attachRange;
    }
}

