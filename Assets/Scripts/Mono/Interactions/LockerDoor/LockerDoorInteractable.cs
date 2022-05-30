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

        [SerializeField] private AudioClip openLocker;
        [SerializeField] private AudioClip tryOpenLocker;

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
                GetComponent<AudioSource>().PlayOneShot(openLocker);
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(tryOpenLocker);
            }
        }

        public void OnInteractionEnd()
        {
            
        }
    }
}
