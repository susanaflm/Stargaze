using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Magnet
{
    public class MagnetPickup : Interactable
    {
        public override void OnInteractionStart()
        {
            base.OnInteractionStart();
            
            PuzzleManager.Instance.GetMagnet();
            Destroy(gameObject);
        }
    }
}
