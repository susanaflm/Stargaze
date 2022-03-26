using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stargaze.Mono.Interactions.Inspection
{
    public class InspectionTurn : MonoBehaviour
    {
        private Vector3 _lastPos;

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
            
            //TODO: Proper implementation of InputActions
            
        }
    }
}
