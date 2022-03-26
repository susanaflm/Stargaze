using Stargaze.Input;
using UnityEngine;
using Mirror;

namespace Stargaze.Mono.Interactions.Inspection
{
    public class InspectInput : MonoBehaviour
    {
        private InputActions _actions;

        public Vector2 Turn { get; private set; }

        private void Awake()
        {
            _actions = new InputActions();
        }

        void Update()
        {
            Turn = _actions.Inspect.Turn.ReadValue<Vector2>();
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
