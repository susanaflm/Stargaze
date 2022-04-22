using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Lever
{
    public class LeverInteractable : MonoBehaviour, IInteractable
    {
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;
        
        [SerializeField] private List<Door.Door> attachedDoors = new();
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;
        
        public void OnInteractionStart()
        {
            ToggleDoorState();
        }

        public void OnInteractionEnd() { }

        private void ToggleDoorState()
        {
            foreach (var door in attachedDoors)
            {
                door.ToggleDoor();
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var door in attachedDoors)
                Debug.DrawLine(transform.position, door.transform.position, Color.yellow);
        }
    }
}
