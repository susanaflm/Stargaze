using System;
using Stargaze.Input;
using UnityEngine;

namespace Stargaze.Mono.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private InputActions _actions;
        
        public Action Jump { get; set; }
        
        public Vector2 Movement { get; private set; }
        
        public Vector2 Look { get; private set; }

        private void Awake()
        {
            _actions = new InputActions();

            _actions.Player.Jump.performed += _ => Jump?.Invoke();
        }

        private void Update()
        {
            Movement = _actions.Player.Movement.ReadValue<Vector2>().normalized;
            Look = _actions.Player.Look.ReadValue<Vector2>();
        }

        private void OnEnable()
        {
            _actions.Enable();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable()
        {
            _actions.Disable();
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
