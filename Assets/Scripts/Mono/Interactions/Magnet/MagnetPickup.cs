using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Magnet
{
    public class MagnetPickup : MonoBehaviour, IInteractable
    {
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;
        
        public void OnInteractionStart()
        {
            PuzzleManager.Instance.GetMagnet();
            Destroy(gameObject);
        }
        
        public void OnInteractionEnd() { }
    }
}
