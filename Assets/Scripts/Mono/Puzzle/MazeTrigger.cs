using System;
using Mirror;
using Stargaze.Mono.Player;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    public class MazeTrigger : NetworkBehaviour
    {
        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerInput>())
            {
                PuzzleManager.Instance.CompleteMaze();
            }
        }
    }
}
