using System;
using UnityEngine;

namespace Downey.CodeSmell.Example4
{
    public interface IMovement
    {
        void Move();
    }
    public interface IAttack
    {
        void Execute();
    }
    
    class GroundMover : MonoBehaviour, IMovement
    {
        public float speed = 3f;
        public void Move(){}
    }

    class FlyMover : MonoBehaviour, IMovement
    {
        public float flapForce = 5f;
        public void Move(){}
    }

    public class Enemy : MonoBehaviour
    {
        IMovement movement;
        private IAttack attack;

        private void Awake()
        {
            movement = GetComponent<IMovement>();
            attack = GetComponent<IAttack>();
        }

        void OnValidate()
        {
            if(GetComponent<IMovement>() == null) Debug.LogError($"{gameObject.name} has no {nameof(IMovement)} component.");
            if(GetComponent<IAttack>() == null) Debug.LogError($"{gameObject.name} has no {nameof(IAttack)} component.");
        }
        
        void Update() => movement.Move();
        public void Attack() => attack.Execute();
    }
}