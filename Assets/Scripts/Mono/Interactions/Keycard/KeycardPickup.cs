using NaughtyAttributes;
using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Keycard
{
    public class KeycardPickup : Interactable
    {
        [Dropdown("accessLevels")]
        [SerializeField] private int accessLevel;

        private int[] accessLevels = new int[] {1, 2};

        public override void OnInteractionStart()
        {
            base.OnInteractionStart();
            
            PuzzleManager.Instance.SetCurrentKeycardAccesslevel(accessLevel);
            Destroy(gameObject);
        }
    }
}
