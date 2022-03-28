using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Lever
{
    public class LeverInteractable : Interactable
    {
        [SerializeField] private List<Door.Door> attachedDoors = new();
        
        public override void OnInteraction()
        {
            base.OnInteraction();
            
            ToggleDoorState();
        }

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
