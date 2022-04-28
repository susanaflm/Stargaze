using System;
using Stargaze.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stargaze.Mono.Interactions.ElectricalPanel
{
    public class ElectricalInput : MonoBehaviour
    {
        private InputActions _actions;

        public Action Select { get; set; }

        public Action PlaceWire { get; set; }

        public Vector2 Navigate { get; private set; }

        public Vector2 WireMovement { get; private set; }

        private void Awake()
        {
            _actions = new InputActions();

            _actions.Electrical.Select.performed += _ => Select?.Invoke();
            _actions.Electrical.PlaceWire.performed += _ => PlaceWire?.Invoke();
        }

        private void Update()
        {
            Navigate = _actions.Electrical.Navigate.ReadValue<Vector2>().normalized;
            WireMovement = _actions.Electrical.WireMovement.ReadValue<Vector2>();
        }
        
        private void OnEnable()
        {
            _actions.Enable();
        }

        private void OnDisable()
        {
            _actions.Disable();
        }
    }
}
