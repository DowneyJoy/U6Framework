using Downey.Damageable;

namespace Downey.TargetingStrategy
{
    public abstract class TargetingStrategy
    {
        protected Ability ability;
        protected TargetingManager targetingManager;
        protected bool isTargeting = false;
    
        public bool IsTargeting => isTargeting;
    
        public abstract void Start(Ability ability,TargetingManager targetingManager);
        public virtual void Update(){}
        public virtual void Cancel(){}
    }

    public class SelfTargeting : TargetingStrategy
    {
        public override void Start(Ability ability, TargetingManager targetingManager)
        {
            this.ability = ability;
            this.targetingManager = targetingManager;

            if (targetingManager.transform.TryGetComponent<IDamageable>(out var target))
            {
                ability.Execute(target);
            }
        }
    }
}
