using Mirror;
using Stargaze.Enums;
using Stargaze.Mono.Puzzle.DronePuzzle;
using UnityEngine;
using UnityEngine.UI;

namespace Stargaze.Mono.Terminals
{
    public class DroneHubTerminal : Terminal
    {
        private DronePuzzleManager _droneManager;
        
        [Header("Inputs")]
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private Button upButton;
        [SerializeField] private Button downButton;

        [Space]
        
        [SerializeField] private Button collectButton;

        protected override void TerminalAwake()
        {
            leftButton.onClick.AddListener(() => CmdMoveDrone(Direction2D.Left));
            rightButton.onClick.AddListener(() => CmdMoveDrone(Direction2D.Right));
            upButton.onClick.AddListener(() => CmdMoveDrone(Direction2D.Up));
            downButton.onClick.AddListener(() => CmdMoveDrone(Direction2D.Down));
            
            collectButton.onClick.AddListener(CmdCollect);
        }

        public override void OnStartClient()
        {
            _droneManager = DronePuzzleManager.Instance;
            
            if (_droneManager == null)
                Debug.LogError($"Can find {nameof(DronePuzzleManager)} instance. Did you instantiate it?");
        }

        [Command(requiresAuthority = false)]
        private void CmdMoveDrone(Direction2D dir)
        {
            _droneManager.MoveDrone(dir);
        }

        [Command(requiresAuthority = false)]
        private void CmdCollect()
        {
            _droneManager.CollectResource();
        }
    }
}