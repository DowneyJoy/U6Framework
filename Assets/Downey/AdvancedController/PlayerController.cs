using System;
using Downey.ImprovedTimers;
using Downey.InputManager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityUtils;

namespace Downey.AdvancedController
{
    public class PlayerController : MonoBehaviour
    {
        #region Fields
        [SerializeField, Required] private InputReader input;
        
        Transform tr;
        PlayerMover mover;
        CeilingDetector ceilingDetector;

        private bool jumpInputIsLocked, jumpKeyWasPressed, jumpKeyWasLetGo, jumpKeyIsPressed;

        public float movementSpeed = 7f;
        public float airControlRate = 2f;
        public float jumpSpeed = 10f;
        public float jumpDuration = .2f;
        public float airFriction = .5f;
        public float groundFriction = 100f;
        public float gravity = 30f;
        public float slideGravity = 5f;
        public float slopeLimit = 30f;
        public bool useLocalMomentum;
        
        Platformer.StateMachine stateMachine;
        CountdownTimer jumpTimer;
        
        [SerializeField]Transform cameraTransform;
        
        Vector3 momentum,savedVelocity,savedMovementVelocity;
        
        public event Action<Vector3> OnJump = delegate { };
        public event Action<Vector3> OnLand = delegate { };
        #endregion

        private void Awake()
        {
            tr = transform;
            mover = GetComponent<PlayerMover>();
            ceilingDetector = GetComponent<CeilingDetector>();
            jumpTimer = new CountdownTimer(jumpDuration);
            // SetupStateMachine();
        }
        
        public Vector3 GetMomentum() => useLocalMomentum ? tr.localToWorldMatrix* momentum : momentum;

        void FixedUpdate()
        {
            mover.CheckForGround();
            HandleMomentum();
            // TODO calculate movement velocity
        }
        Vector3 CalculateMovementVelocity() => CalculateMovementDirection() * movementSpeed;
        
        Vector3 CalculateMovementDirection()
        {
            Vector3 direction = cameraTransform == null
                ? tr.right * input.Direction.x + tr.forward * input.Direction.y
                : Vector3.ProjectOnPlane(cameraTransform.right,tr.up).normalized * input.Direction.x +
                  Vector3.ProjectOnPlane(cameraTransform.forward,tr.up).normalized * input.Direction.y;
            
            return direction.magnitude > 1f ? direction.normalized : direction;
        }

        void HandleMomentum()
        {
            if(useLocalMomentum) momentum = tr.localToWorldMatrix*momentum;
            
            Vector3 verticalMomentum = VectorMath.ExtractDotVector(momentum, tr.up);
            Vector3 horizontalMomentum = momentum - verticalMomentum;

            verticalMomentum -= tr.up * (gravity * Time.deltaTime);
            //if(stateMachine.CurrentState is GroundedState)
        }
        
        //bool IsGrounded()=> stateMachine.Cu
    }
}