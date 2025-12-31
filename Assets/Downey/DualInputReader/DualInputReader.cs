using System;
using System.Collections.Generic;
using Downey.HSM;
using Downey.InputManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using static InputSystem_Actions;

namespace Downey.DualInputReader
{
    public interface IIputReader
    {
        Vector2 Direction { get; }
        event UnityAction Attack;
        void EnablePlayerInput();
    }
    public class DualInputReader : MonoBehaviour, IInputReader, IPlayerActions, IUIActions
    {
        public event UnityAction Attack = delegate { };
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2,bool> Look = delegate { };
        
        readonly PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        readonly List<RaycastResult> raycastResults = new List<RaycastResult>();

        bool IsPointerOverUI(Vector2 screenPosition)
        {
            pointerEventData.position = screenPosition;
            raycastResults.Clear();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            return raycastResults.Count > 0;
        }
        static bool IsMouse(InputAction.CallbackContext context) => context.control.device is Mouse;
        
        InputSystem_Actions inputActions;
        
        public Vector2 Direction => inputActions.Player.Move.ReadValue<Vector2>();

        public void EnablePlayerActions()
        {
            if (inputActions == null)
            {
                inputActions = new InputSystem_Actions();
                
                inputActions.Player.SetCallbacks(this);
                inputActions.UI.SetCallbacks(this);
            }
            
            inputActions.Enable();
            inputActions.UI.Disable();

            var eventSystem = EventSystem.current;
            if (eventSystem == null)
            {
                Debug.LogWarning("No EventSystem found in scene");
                return;
            }

            var uiModule = eventSystem.GetComponent<InputSystemUIInputModule>();
            if (uiModule == null)
            {
                Debug.LogWarning("No UI Module found on EventSystem.");
                return;
            }

            if (uiModule.actionsAsset != inputActions.asset)
            {
                uiModule.actionsAsset = inputActions.asset;
            }
        }

        private void OnEnable() => EnablePlayerActions();
        // --- IPlayerActions ---
        public bool IsUIEngaged { get;private set; }
        public event UnityAction UIEngaged =  delegate { };
        public event UnityAction UIDisengaged =  delegate { };

        private GameObject lastUISelection;
        public GameObject firstUISelection;
        public void OnUIEngage(InputAction.CallbackContext context)
        {
            if(!context.performed && !context.canceled) return;
            Debug.Log($"UIEngage: {context.control.device.name} - {context.control.name}");
            IsUIEngaged = !IsUIEngaged;

            if (IsUIEngaged)
            {
                SetPlayerActionsEnabled(false);
                inputActions.UI.Enable();
                if(lastUISelection == null) lastUISelection = firstUISelection;
                EventSystem.current.SetSelectedGameObject(lastUISelection);
                UIEngaged.Invoke();
            }
            else
            {
                inputActions.UI.Disable();
                SetPlayerActionsEnabled(true);
                lastUISelection = EventSystem.current.currentSelectedGameObject;
                EventSystem.current.SetSelectedGameObject(null);
                UIDisengaged.Invoke();
            }
        }

        void SetPlayerActionsEnabled(bool value)
        {
            var map = inputActions.asset.FindActionMap("Player");

            foreach (var action in map.actions)
            {
                if(action == inputActions.Player.UIEngage) continue;
                
                if(value)
                    action.Enable();
                else
                    action.Disable();
            }
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed || context.canceled)
            {
                Move.Invoke(context.ReadValue<Vector2>());
            }
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.performed || context.canceled)
            {
                Look.Invoke(context.ReadValue<Vector2>(),IsMouse(context));
            }
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
            var pointer = Pointer.current;
            if(pointer != null && IsPointerOverUI(pointer.position.ReadValue())) return;
            Debug.Log($"Attack: {context.control.device.name} - {context.control.name}");
            Attack.Invoke();
        }
        public void OnInteract(InputAction.CallbackContext context){}
        public void OnCrouch(InputAction.CallbackContext context){}
        public void OnJump(InputAction.CallbackContext context){}
        public void OnPrevious(InputAction.CallbackContext context){}
        public void OnNext(InputAction.CallbackContext context){}
        public void OnSprint(InputAction.CallbackContext context){}
        
        // --- IUIActions ---
        public void OnNavigate(InputAction.CallbackContext context){}
        public void OnSubmit(InputAction.CallbackContext context){}
        public void OnClick(InputAction.CallbackContext context){}
        public void OnCancel(InputAction.CallbackContext context){}
        public void OnPoint(InputAction.CallbackContext context){}
        public void OnRightClick(InputAction.CallbackContext context){}
        public void OnMiddleClick(InputAction.CallbackContext context){}
        public void OnScrollWheel(InputAction.CallbackContext context){}
        public void OnTrackedDevicePosition(InputAction.CallbackContext context){}
        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context){}
        
    }
}