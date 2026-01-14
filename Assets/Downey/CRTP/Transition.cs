using System;

namespace Downey.CRTP
{
    public abstract class Transition
    {
        public abstract bool Evaluate();
        public abstract void Apply(CharacterStateMachine machine);
    }

    public class Transition<TState> : Transition where TState : CharacterState<TState>
    {
        readonly Func<bool> condition;
        private readonly TState target;

        public Transition(TState target, Func<bool> condition)
        {
            this.target = target;
            this.condition = condition;
        }
        
        public override bool Evaluate() => condition();

        public override void Apply(CharacterStateMachine machine)
        {
            machine.ChangeState(target);
        }
    }
}