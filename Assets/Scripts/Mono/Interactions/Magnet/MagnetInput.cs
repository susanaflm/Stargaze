using Stargaze.Input;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Magnet
{
    public class MagnetInput : MonoBehaviour
    {
        private InputActions _actions;

        public Vector2 Movement { get; private set; }

        private void Awake()
        {
            _actions = new InputActions();
        }

        void Update()
        {
            Movement = _actions.Magnet.Movement.ReadValue<Vector2>();
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
