using System;
using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.LockerDoor
{
    public class LockerDoorInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool switchable;

        private Animator doorAnimator;

        private bool isInteractable = true;

        public bool Switchable => switchable;
        public bool IsInteractable => isInteractable;


        private void Awake()
        {
            doorAnimator = GetComponent<Animator>();
        }

        public void OnInteractionStart()
        {
            if (PuzzleManager.Instance.DoesPlayerHaveLockerKey())
            {
                doorAnimator.SetTrigger("Open");
            }
        }

        public void OnInteractionEnd()
        {
            
        }
    }
}
