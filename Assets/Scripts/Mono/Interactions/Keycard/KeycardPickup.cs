using NaughtyAttributes;
using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Keycard
{
    public class KeycardPickup : MonoBehaviour, IInteractable
    {
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;
        
        [Dropdown("_accessLevels")]
        [SerializeField] private int accessLevel;

        private int[] _accessLevels = new int[] {1, 2, 3};
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;

        public void OnInteractionStart()
        {
            PuzzleManager.Instance.SetCurrentKeycardAccesslevel(accessLevel);
            Destroy(gameObject);
        }

        public void OnInteractionEnd() { }
    }
}
