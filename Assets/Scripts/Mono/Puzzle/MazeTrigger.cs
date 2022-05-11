using System;
using Stargaze.Mono.Player;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    public class MazeTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerInput>())
            {
                PuzzleManager.Instance.CompleteMaze();
            }
        }
    }
}
