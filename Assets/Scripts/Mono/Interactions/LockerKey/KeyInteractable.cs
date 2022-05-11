using Mirror;
using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.LockerKey
{
    public class KeyInteractable : NetworkBehaviour, IInteractable
    {
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;

        public bool Switchable => switchable;
        public bool IsInteractable => _isInteractable;
        
        public void OnInteractionStart()
        {
            PuzzleManager.Instance.CmdGetLockerKey();
            Destroy(gameObject);
        }

        public void OnInteractionEnd()
        {
            
        }
    }
}
