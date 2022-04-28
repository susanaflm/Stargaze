using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.LockerDoor
{
    public class LockerDoorInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool switchable;
        
        public bool Switchable { get; }
        public bool IsInteractable { get; }
        public void OnInteractionStart()
        {
            if (PuzzleManager.Instance.DoesPlayerHaveLockerKey())
            {
                //TODO: Animate Door opening
            }
        }

        public void OnInteractionEnd()
        {
            throw new System.NotImplementedException();
        }
    }
}
