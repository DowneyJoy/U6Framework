using System;
using Downey.InputManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlatformerInput;

namespace Downey.Platformer.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Platformer/InputReader", order = 0)]
    public class InputReader : ScriptableObject,IPlayerActions
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2,bool> Look = delegate { };
        public event UnityAction EnableMouseControlCamera = delegate { };
        public event UnityAction DisableMouseControlCamera = delegate { };
        public event UnityAction<bool> Jump = delegate { };
        public event UnityAction<bool> Dash = delegate { };
        public event UnityAction Attack = delegate { };
        private PlatformerInput inputActions;
        
        public Vector3 Direction => (Vector3)inputActions.Player.Move.ReadValue<Vector2>();

        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlatformerInput();
                inputActions.Player.SetCallbacks(this);
            }
            //inputActions.Enable();
        }

        public void EnablePlayerActions()
        {
            inputActions.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look.Invoke(context.ReadValue<Vector2>(),isDeviceMouse(context));
        }
        bool isDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started) {
                Attack.Invoke();
            }
        }

        public void OnMouseControlCamera(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    EnableMouseControlCamera.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    DisableMouseControlCamera.Invoke();
                    break;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase) {
                case InputActionPhase.Started:
                    Jump.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump.Invoke(false);
                    break;
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            switch (context.phase) {
                case InputActionPhase.Started:
                    Dash.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Dash.Invoke(false);
                    break;
            }
        }

        #region UI

        //     public void OnNavigate(InputAction.CallbackContext context) { }
        //
        //     public void OnSubmit(InputAction.CallbackContext context) { }
        //
        //     public void OnCancel(InputAction.CallbackContext context) { }
        //
        //     public void OnPoint(InputAction.CallbackContext context) { }
        //
        //     public void OnClick(InputAction.CallbackContext context) { }
        //
        //     public void OnRightClick(InputAction.CallbackContext context) { }
        //
        //     public void OnMiddleClick(InputAction.CallbackContext context) { }
        //
        //     public void OnScrollWheel(InputAction.CallbackContext context) { }
        //
        //     public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }
        //
        //     public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }
        #endregion
    }
}