using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stargaze.Mono.Interactions.Inspection
{
    public class InspectionTurn : MonoBehaviour
    {
        private Vector3 _lastPos;

        private Vector3 _scale;

        private void Start()
        {
            _scale = transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                _lastPos = Mouse.current.position.ReadValue();
            }

            if (Mouse.current.leftButton.isPressed)
            {
                var delta = (Vector3)Mouse.current.position.ReadValue() - _lastPos;
                _lastPos = Mouse.current.position.ReadValue();

                var axis = Quaternion.AngleAxis(-90f, Vector3.forward) * delta;
                transform.rotation = Quaternion.AngleAxis(delta.magnitude * 0.5f, axis) * transform.rotation;
            }

            Vector2 scroll = Mouse.current.scroll.ReadValue();

            switch (scroll.y)
            {
                case > 0:
                    _scale *= 1.1f;
                    transform.localScale = _scale;
                    break;
                case < 0:
                    _scale /= 1.1f;
                    transform.localScale = _scale;
                    break;
            }

            //TODO: Proper implementation of InputActions

        }
    }
}
