using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class DoorController : NetworkBehaviour
    {
        [Server]
        private void ToggleDoors(List<Door.Door> doors)
        {
            foreach (Door.Door door in doors)
            {
                door.ToggleDoor();
                
                Debug.Log($"Opening door {door}");
            }
        }
        
        private void OnEnable()
        {
            PuzzleManager.Instance.OnGravityPuzzleComplete += ToggleDoors;
            PuzzleManager.Instance.OnMazePuzzleComplete += ToggleDoors;
            PuzzleManager.Instance.OnElectricalPuzzleComplete += ToggleDoors;
            PuzzleManager.Instance.OnElectricalPuzzleUndo += ToggleDoors;
        }

        private void OnDisable()
        {
            PuzzleManager.Instance.OnGravityPuzzleComplete -= ToggleDoors;
            PuzzleManager.Instance.OnMazePuzzleComplete -= ToggleDoors;
            PuzzleManager.Instance.OnElectricalPuzzleComplete -= ToggleDoors;
            PuzzleManager.Instance.OnElectricalPuzzleUndo -= ToggleDoors;
        }
    }
}
