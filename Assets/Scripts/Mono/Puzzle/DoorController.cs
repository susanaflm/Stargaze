using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    public class DoorController : MonoBehaviour
    {
        private void OpenDoors(List<Door.Door> doors)
        {
            foreach (Door.Door door in doors)
            {
                door.ToggleDoor();
            }
        }
        
        private void OnEnable()
        {
            PuzzleManager.Instance.OnGravityPuzzleComplete += OpenDoors;
            PuzzleManager.Instance.OnMazePuzzleComplete += OpenDoors;
            PuzzleManager.Instance.OnElectricalPuzzleComplete += OpenDoors;
            PuzzleManager.Instance.OnElectricalPuzzleUndo += OpenDoors;
        }

        private void OnDisable()
        {
            PuzzleManager.Instance.OnGravityPuzzleComplete -= OpenDoors;
            PuzzleManager.Instance.OnMazePuzzleComplete -= OpenDoors;
            PuzzleManager.Instance.OnElectricalPuzzleComplete -= OpenDoors;
            PuzzleManager.Instance.OnElectricalPuzzleUndo -= OpenDoors;
        }
    }
}
