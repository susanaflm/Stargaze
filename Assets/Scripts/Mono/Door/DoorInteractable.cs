using System;
using Mirror;
using Stargaze.Mono.Interactions;
using UnityEngine;

namespace Stargaze.Mono.Door
{
    public class DoorInteractable : NetworkBehaviour, IInteractable
    {
        public bool Switchable => true;
        public bool IsInteractable => true;
        
        private Animator _doorAnimator;

        [SyncVar(hook = nameof(DoorOpenStateChangedCallback))]
        private bool isOpened = false;

        private void Awake()
        {
            _doorAnimator = GetComponent<Animator>();
        }

        public void OnInteractionStart()
        {
            CmdToggleDoor();
        }

        [Command(requiresAuthority = false)]
        private void CmdToggleDoor()
        {
            isOpened = !isOpened;
        }

        public void OnInteractionEnd()
        {
            
        }
        
        private void DoorOpenStateChangedCallback(bool oldState, bool newState)
        {
            if (oldState == newState)
                return;
            
            if (newState)
                OpenDoor();
            else
                CloseDoor();
        }

        private void OpenDoor()
        {
            _doorAnimator.SetBool("Opened", true);
        }

        private void CloseDoor()
        {
            _doorAnimator.SetBool("Opened", false);
        }
    }
}
