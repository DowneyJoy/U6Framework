using UnityEngine;

namespace Downey.CodeSmell.Example4
{
    abstract class EnemyBase : MonoBehaviour
    {
        public float groundSpeed;

        public virtual void Walk()
        {
            
        }

        public abstract void Attack();
    }

    class FlyingEnemy : EnemyBase
    {
        public override void Walk()
        {
            throw new System.NotImplementedException();
        }

        public override void Attack()
        {
            
        }
    }
}