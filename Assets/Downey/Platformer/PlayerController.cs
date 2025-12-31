using System;
using Cinemachine;
using Downey.Platformer.Input;
using KBCore.Refs;
using UnityEngine;

namespace Downey.Platformer
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField,Self]CharacterController controller;
        [SerializeField,Self]Animator animator;
        [SerializeField,Anywhere]CinemachineFreeLook freeLookCam;
        [SerializeField, Anywhere] private InputReader input;
        
        [Header("Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = .2f;
        
        Transform mainCam;

        float currentSpeed;
        float velocity;
        const float ZeroF = 0f;
        
        static readonly int Speed = Animator.StringToHash("Speed");
        private void Awake()
        {
            mainCam = Camera.main.transform;
            freeLookCam.Follow = transform;
            freeLookCam.LookAt = transform;
            
            freeLookCam.OnTargetObjectWarped(transform,transform.position - freeLookCam.transform.position - Vector3.forward);
        }

        private void Start() => input.EnablePlayerActions();

        private void Update()
        {
            HandleMovement();
            UpdateAnimator();
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Speed", currentSpeed);
        }
        void HandleMovement()
        {
            var movementDirection = new Vector3(input.Direction.x, 0, input.Direction.y).normalized;
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movementDirection;

            if (adjustedDirection.magnitude > ZeroF)
            {
                HandleRotation(adjustedDirection);
                HandleCharacterController(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(ZeroF);
            }
        }

        void HandleCharacterController(Vector3 adjustedDirection)
        {
            var adjustedSpeed = adjustedDirection*(moveSpeed * Time.deltaTime);
            controller.Move(adjustedSpeed);
        }
        void HandleRotation(Vector3 adjustedDirection)
        {
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);
        }

        void SmoothSpeed(float speed)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed,speed,ref velocity,smoothTime);
        }
    }
}