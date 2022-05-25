using Mirror;
using Stargaze.Mono.Player;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    public class GravityTrigger : NetworkBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerInput>())
            {
                CmdCompleteGravity();
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdCompleteGravity()
        {
            PuzzleManager.Instance.CompleteGravity();
        }
    }
}
