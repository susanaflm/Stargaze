using NaughtyAttributes;
using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Keycard
{
    public class KeycardPickup : Interactable
    {
        [Dropdown("_accessLevels")]
        [SerializeField] private int accessLevel;

        private int[] _accessLevels = new int[] {1, 2, 3};

        public override void OnInteractionStart()
        {
            base.OnInteractionStart();
            
            PuzzleManager.Instance.SetCurrentKeycardAccesslevel(accessLevel);
            Destroy(gameObject);
        }
    }
}
