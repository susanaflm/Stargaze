using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Lever
{
    public class LeverInteractable : MonoBehaviour, IInteractable
    {
        private bool _isInteractable = true;

        private bool _isTurnedOn = false;
        private Material switchMaterial;

        [SerializeField] private bool switchable = true;
        
        [SerializeField] private List<Door.Door> attachedDoors = new();

        [SerializeField] private Material turnedOff;
        [SerializeField] private Material turnedOn;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;
        
        public void OnInteractionStart()
        {
            ToggleDoorState();
            _isTurnedOn = !_isTurnedOn;
            
            GetComponent<Renderer>().material = _isTurnedOn ? turnedOn : turnedOff;
        }

        public void OnInteractionEnd() { }

        private void ToggleDoorState()
        {
            foreach (var door in attachedDoors)
            {
                door.CmdToggleDoor();
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var door in attachedDoors)
                Debug.DrawLine(transform.position, door.transform.position, Color.yellow);
        }
    }
}
