using System;
using Downey.Processor;
using UnityEngine;

namespace Downey.CRTP
{
    public class Example : MonoBehaviour
    {
        private CharacterStateMachine machine;
        private ProcessorDelegate<Vector3, bool> shouldAttack;
        bool isMoving;
        private void Start()
        {
            var w = new Worker();
            w.DoWork();
            
            machine = new CharacterStateMachine();
            var idle = new IdleState();
            var move = new MoveState();
            
            idle.SetTransition(new Transition<MoveState>(move,()=>isMoving));
            
            machine.ChangeState(idle);
        }

        private void Update()
        {
            machine.Tick(Time.deltaTime);
            isMoving = Input.anyKey;
        }
    }
}