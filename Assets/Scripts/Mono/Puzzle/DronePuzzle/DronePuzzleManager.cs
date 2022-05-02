using System;
using Mirror;
using Stargaze.Enums;
using UnityEngine;

namespace Stargaze.Mono.Puzzle.DronePuzzle
{
    public class DronePuzzleManager : NetworkBehaviour
    {
        public static DronePuzzleManager Instance;
        
        [SyncVar(hook = nameof(DronePositionChangedCallback))]
        private Vector2 _dronePosition;

        public Action<Vector2> OnDronePositionChanged;

        public Vector2 DronePosition => _dronePosition;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public override void OnStartServer()
        {
            _dronePosition = Vector2.zero;
        }

        [Server]
        public void MoveDrone(Direction2D dir)
        {
            _dronePosition += dir switch
            {
                Direction2D.Up => Vector2.up,
                Direction2D.Down => Vector2.down,
                Direction2D.Left => Vector2.left,
                Direction2D.Right => Vector2.right,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }

        [Server]
        public void CollectResource()
        {
            Debug.Log("Collecting resource");
        }

        private void DronePositionChangedCallback(Vector2 oldPosition, Vector2 newPosition)
        {
            OnDronePositionChanged?.Invoke(newPosition);
        }
    }
}