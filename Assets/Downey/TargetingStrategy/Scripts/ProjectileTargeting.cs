using Downey.Damageable;
using UnityEngine;
using UnityUtils;

namespace Downey.TargetingStrategy
{
    public class ProjectileTargeting : TargetingStrategy
    {
        public GameObject projectilePrefab;
        public float projectileSpeed = 10f;

        public override void Start(Ability ability, TargetingManager targetingManager)
        {
            this.ability = ability;
            this.targetingManager = targetingManager;

            if (projectilePrefab != null)
            {
                var flatForward = targetingManager.cam.transform.forward.With(y:0).normalized;
                var forwardRotation = Quaternion.LookRotation(flatForward);
                var projectile = Object.Instantiate(projectilePrefab,targetingManager.transform.position.Add(y:1),forwardRotation);
                projectile.GetComponent<ProjectileController>().Initialize(ability, projectileSpeed);
            }
        }
    }
}