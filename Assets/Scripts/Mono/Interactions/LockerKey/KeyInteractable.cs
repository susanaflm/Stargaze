using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.LockerKey
{
    public class KeyInteractable : MonoBehaviour, IInteractable
    {
        
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;

        public bool Switchable => switchable;
        public bool IsInteractable => _isInteractable;
        
        public void OnInteractionStart()
        {
            PuzzleManager.Instance.GetLockerKey();
            Destroy(gameObject);
        }

        public void OnInteractionEnd()
        {
            
        }
    }
}
