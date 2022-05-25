using System;
using Mirror;
using Stargaze.Mono.Player;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    public class MazeTrigger : NetworkBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerInput>())
            {
                CmdCompleteMaze();
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdCompleteMaze()
        {
            PuzzleManager.Instance.CompleteMaze();
        }
    }
}
