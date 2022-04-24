using Mirror;
using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Magnet
{
    public class MagnetPickup : NetworkBehaviour, IInteractable
    {
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;
        
        public void OnInteractionStart()
        {
            CmdPickUpMagnet();
        }

        [Command(requiresAuthority = false)]
        private void CmdPickUpMagnet()
        {
            PuzzleManager.Instance.GetMagnet();
            NetworkServer.Destroy(gameObject);
        }
        
        public void OnInteractionEnd() { }
    }
}
