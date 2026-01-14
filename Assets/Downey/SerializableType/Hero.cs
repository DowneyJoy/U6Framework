using System;
using UnityEngine;

namespace Downey.SerializableType
{
    public class Hero : MonoBehaviour
    {
        [Header("State Machine Type Settings")]
        [TypeFilter(typeof(IState))]
        [SerializeField]
        private SerializableType staringState;
        [Header("Event Type Settings")]
        [TypeFilter(typeof(IEvent))]
        [SerializeField]SerializableType gameOverEvent;
        
        [TypeFilter(typeof(IEvent))]
        [SerializeField]SerializableType levelCompleteEvent;

        private void Start()
        {
            Debug.Log($"Starting state: {staringState.Type}");
            Debug.Log(staringState.Type.InheritsOrImplements(typeof(IState)));
            Debug.Log(staringState.Type.InheritsOrImplements(typeof(BaseState)));
            Debug.Log($"Game over state: {gameOverEvent.Type}");
            Debug.Log(gameOverEvent.Type.InheritsOrImplements(typeof(IEvent)));
            Debug.Log($"Level complete state: {levelCompleteEvent.Type}");
            Debug.Log(levelCompleteEvent.Type.InheritsOrImplements(typeof(IEvent)));
        }
    }
    
    public interface IEvent{}
    public class EventA :  IEvent{}
    public class EventB :  IEvent{}

    public interface IState
    {
        void OnEnter();
        void OnExit();
    }

    public abstract class BaseState : IState
    {
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
    }
    
    public class WalkingState : BaseState{}
    public class RunningState :  BaseState{}
    public class JumpingState :  BaseState{}
}