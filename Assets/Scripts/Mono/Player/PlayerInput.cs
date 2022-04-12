using System;
using Mirror;
using Stargaze.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stargaze.Mono.Player
{
    public class PlayerInput : NetworkBehaviour
    {
        private InputActions _actions;
        
#if DEBUG
        private InputAction _debugToggleAction = new (binding: "<Keyboard>/f12");
#endif
        
        public Action Jump { get; set; }
        
        public Action Interact { get; set; }

        public Action ExitInteraction { get; set; }

        public Vector3 Strafe { get; private set; }
        
        public Vector2 Look { get; private set; }

        private void Awake()
        {
            _actions = new InputActions();

            _actions.Player.Jump.performed += _ => Jump?.Invoke();
            _actions.Player.Interact.performed += _ => Interact?.Invoke();
            _actions.Player.ExitInteraction.performed += _ => ExitInteraction?.Invoke();
            
#if DEBUG
            _debugToggleAction.performed += _ => enabled = !enabled;
#endif
        }

        public override void OnStartLocalPlayer()
        {
#if DEBUG
            _debugToggleAction.Enable();
#endif
            
            _actions.Enable();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;
            
            Strafe = _actions.Player.Strafe.ReadValue<Vector3>().normalized;
            Look = _actions.Player.Look.ReadValue<Vector2>();
        }

        private void OnEnable()
        {
            if (!isLocalPlayer)
                return;
            
            _actions.Enable();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable()
        {
            if (!isLocalPlayer)
                return;
            
            _actions.Disable();
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
