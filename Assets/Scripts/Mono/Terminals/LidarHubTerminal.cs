using Stargaze.Mono.Puzzle.DronePuzzle;
using UnityEngine;

namespace Stargaze.Mono.Terminals
{
    public class LidarHubTerminal : Terminal
    {
        private DronePuzzleManager _droneManager;

        public override void OnStartClient()
        {
            _droneManager = DronePuzzleManager.Instance;
            
            if (_droneManager == null)
                Debug.LogError($"Can find {nameof(DronePuzzleManager)} instance. Did you instantiate it?");
            
            _droneManager.OnDronePositionChanged += UpdateDronePosition;
        }

        private void UpdateDronePosition(Vector2 newPosition)
        {
            Debug.Log($"New drone position: {newPosition}");
        }
    }
}