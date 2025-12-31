using Downey.Damageable;
using UnityEngine;

namespace Downey.TargetingStrategy
{
    public class ProjectileController : MonoBehaviour
    {
        Ability ability;
        float speed;

        public void Initialize(Ability ability, float speed)
        {
            this.ability = ability;
            this.speed = speed;
            Destroy(gameObject, 5f);
        }
        
        void Update() => transform.Translate(Vector3.forward * speed * Time.deltaTime);

        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player")) return;

            if (other.gameObject.TryGetComponent<IDamageable>(out var target))
            {
                ability.Execute(target);
                Destroy(gameObject);
            }
        }
    }
}