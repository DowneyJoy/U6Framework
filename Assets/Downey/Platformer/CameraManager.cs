using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Downey.Platformer.Input;
using KBCore.Refs;
using UnityEngine;

namespace Downey.Platformer
{
    public class CameraManager : ValidatedMonoBehaviour
    {
        [Header("References")] [SerializeField, Anywhere]
        private InputReader input;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookVCam;
        [Header("Settings")] [SerializeField,Range(0.5f, 3f)] private float speedMultiplier =1f;
        
        bool isRMBPressed;
        private bool cameraMovementLock;

        private void OnEnable()
        {
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if(cameraMovementLock) return;
            if(isDeviceMouse && !isRMBPressed) return;
            
            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;
            
            freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * deviceMultiplier * speedMultiplier;
            freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * deviceMultiplier * speedMultiplier;
        }

        void OnEnableMouseControlCamera()
        {
            isRMBPressed = true;
            //Lock the cursor to the center of the screen and hide it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(DisableMouseFrame());
        }

        IEnumerator DisableMouseFrame()
        {
            cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            cameraMovementLock = false;
        }

        void OnDisableMouseControlCamera()
        {
            isRMBPressed = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            freeLookVCam.m_XAxis.m_InputAxisValue = 0;
            freeLookVCam.m_YAxis.m_InputAxisValue = 0;
        }

        private void OnDisable()
        {
            input.Look -= OnLook;
            input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }
    }
}