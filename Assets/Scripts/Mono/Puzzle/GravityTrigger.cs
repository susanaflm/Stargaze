using Mirror;
using Stargaze.Mono.Player;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class GravityTrigger : NetworkBehaviour
    {
        [Server]
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerInput>())
            {
                PuzzleManager.Instance.CompleteGravity();
            }
        }
    }
}
