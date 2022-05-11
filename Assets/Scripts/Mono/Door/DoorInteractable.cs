using System;
using Stargaze.Mono.Interactions;
using UnityEngine;

namespace Stargaze.Mono.Door
{
    public class DoorInteractable : MonoBehaviour, IInteractable
    {
        public bool Switchable => true;
        public bool IsInteractable => true;

        private bool isOpened = false;
        private Animator _doorAnimator;

        private void Start()
        {
            _doorAnimator = GetComponent<Animator>();
        }

        public void OnInteractionStart()
        {
            ToggleDoor();
        }

        public void OnInteractionEnd()
        {
            
        }
        
        private void ToggleDoor()
        {
            if (isOpened)
                CloseDoor();
            else
                OpenDoor();
        }

        private void OpenDoor()
        {
            isOpened = true;
            _doorAnimator.SetBool("Opened", true);
        }

        private void CloseDoor()
        {
            isOpened = false;
            _doorAnimator.SetBool("Opened", false);
        }
    }
}
