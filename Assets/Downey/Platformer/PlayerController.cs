using System;
using System.Collections.Generic;
using Cinemachine;
using Downey.Platformer.Input;
using Downey.Platformer.Utilities;
using KBCore.Refs;
using UnityEngine;

namespace Downey.Platformer
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField,Self]Rigidbody rb;
        [SerializeField,Self]Animator animator;
        [SerializeField,Self]GroundChecker groundChecker;
        [SerializeField,Anywhere]CinemachineFreeLook freeLookCam;
        [SerializeField,Anywhere]private InputReader input;
        
        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = .2f;
        [Header("Jump Settings")]
        [SerializeField]float jumpForce = 10f;
        [SerializeField]float jumpDuration = .5f;
        [SerializeField]float jumpCooldown = 0f;
        [SerializeField]float jumpMaxHeight = 2f;
        [SerializeField]float gravityMultiplier = 3f;
        
        Transform mainCam;

        float currentSpeed;
        float velocity;
        float jumpVelocity;
        const float ZeroF = 0f;

        private Vector3 movement;
        private List<Timer> timers;
        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;
        
        static readonly int Speed = Animator.StringToHash("Speed");
        private void Awake()
        {
            mainCam = Camera.main.transform;
            freeLookCam.Follow = transform;
            freeLookCam.LookAt = transform;
            
            freeLookCam.OnTargetObjectWarped(transform,transform.position - freeLookCam.transform.position - Vector3.forward);
            rb.freezeRotation = true;
            
            //Setup timers
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);
            timers = new List<Timer>(2){jumpTimer, jumpCooldownTimer};
            
            jumpTimer.OnTimerStop += ()=>jumpCooldownTimer.Start();
        }

        private void Start() => input.EnablePlayerActions();

        private void OnEnable()
        {
            input.Jump += OnJump;
        }

        void OnDisable()
        {
            input.Jump -= OnJump;
        }

        void OnJump(bool performed)
        {
            if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpTimer.Start();
            }
            else if(!performed && jumpTimer.IsRunning)
            {
                jumpTimer.Stop();
            }
        }

        private void Update()
        {
            movement = new Vector3(input.Direction.x, 0, input.Direction.y);

            HandleTimers();
            UpdateAnimator();
        }

        private void FixedUpdate()
        {
            HandleJump();
            HandleMovement();
        }

        void UpdateAnimator()
        {
            animator.SetFloat(Speed, currentSpeed);
        }

        void HandleTimers()
        {
            foreach (var timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }

        public void HandleJump()
        {
            // If not jumping and groundedd,keep jump velocity at 0
            if (!jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpVelocity = ZeroF;
                jumpTimer.Stop();
                return;
            }
            
            //If jumping or falling calculate velocity
            if (jumpTimer.IsRunning)
            {
                // Progress point for initial burst of velocity
                float launchPoint = 0.9f;
                if (jumpTimer.Progress > launchPoint)
                {
                    // Calculate the velocity required to reach the jump height using physics equations v = sqrt(2gh)
                    jumpVelocity = Mathf.Sqrt(2*jumpMaxHeight*Mathf.Abs(Physics.gravity.y));
                }
                else
                {
                    // Gravity apply less velocity as the jump progresses
                    jumpVelocity += (1-jumpTimer.Progress)*jumpForce * Time.fixedDeltaTime;
                }
            }
            else
            {
                //Gravity takes over
                jumpVelocity += Physics.gravity.y * Time.fixedDeltaTime * gravityMultiplier;
            }
            
            //Apply velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }
        public void HandleMovement()
        {
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;

            if (adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);
                HandleHorizontalController(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZeroF);
                
                //Reset horizontal velocity for snappy stop
                rb.linearVelocity = new Vector3(ZeroF,rb.linearVelocity.y,ZeroF);
            }
        }

        void HandleHorizontalController(Vector3 adjustedDirection)
        {
            // move the player
            Vector3 velocity = adjustedDirection * moveSpeed * Time.fixedDeltaTime;
            rb.linearVelocity = new Vector3(velocity.x,rb.linearVelocity.y, velocity.z);
        }
        void HandleRotation(Vector3 adjustedDirection)
        {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            //transform.LookAt(transform.position + adjustedDirection);
        }

        void SmoothSpeed(float speed)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed,speed,ref velocity,smoothTime);
        }
    }
}