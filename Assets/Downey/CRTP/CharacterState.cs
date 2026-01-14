using System.Collections.Generic;
using UnityEngine;

namespace Downey.CRTP
{
    class MoveState : CharacterState<MoveState>
    {
        protected override void OnEnter()
        {
            Debug.Log("#State# Entered Move");
        }

        protected override void OnExit()
        {
            Debug.Log("#State# Exited Move");
        }

        protected override void OnTick(float dt)
        {
            //
        }
    }

    class IdleState : CharacterState<IdleState>
    {
        protected override void OnEnter()
        {
            Debug.Log("#State# Entered Idle");
        }

        protected override void OnExit()
        {
            Debug.Log("#State# Exited Idle");
        }

        protected override void OnTick(float dt)
        {
            //
        }
    }

    public class CharacterStateMachine
    {
        CharacterState current;

        public void ChangeState<TState>(TState newState) where TState : CharacterState<TState>
        {
            current?.Exit();
            current = newState;
            current?.Enter();
        }

        public void Tick(float dt)
        {
            //current?.Tick(dt);
            
            if(current == null)return;

            var t = current.GetTransition();
            if (t != null)
            {
                t.Apply(this);
                return;
            }
            current.Tick(dt);
        }
    }
    public abstract class CharacterState<TState> : CharacterState where TState : CharacterState<TState>
    {
        public override void Enter()
        {
            ((TState)this).OnEnter();
        }

        public override void Exit()
        {
            ((TState)this).OnExit();
        }

        public override void Tick(float dt)
        {
            ((TState)this).OnTick(dt);
        }
        
        protected abstract void OnEnter();
        protected abstract void OnExit();
        protected abstract void OnTick(float dt);
    }
    public abstract class CharacterState
    {
        readonly List<Transition> transitions = new List<Transition>();

        public Transition GetTransition()
        {
            for (int i = 0; i < transitions.Count; i++)
            {
                if(transitions[i].Evaluate()) return transitions[i];
            }
            return null;
        }

        public void SetTransition<TState>(Transition<TState> transition) where TState : CharacterState<TState>
        {
            transitions.Add(transition);
        }
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Tick(float dt);
    }
}