using System;
using Stargaze.Mono.Puzzle.DronePuzzle;
using UnityEngine;

namespace Stargaze.Mono.Terminals
{
    public class LidarHubTerminal : Terminal
    {
        private DronePuzzleManager _droneManager;

        [Space]
        [SerializeField] private GameObject materialIndicator;
        [SerializeField] private RectTransform materialsParent;
        [SerializeField] private Vector2 radarSize;

        public override void OnStartClient()
        {
            _droneManager = DronePuzzleManager.Instance;
            
            if (_droneManager == null)
                Debug.LogError($"Can find {nameof(DronePuzzleManager)} instance. Did you instantiate it?");
            
            _droneManager.OnDronePositionChanged += UpdateDronePosition;
            _droneManager.OnMaterialListChanged += UpdateMaterials;
            
            UpdateMaterials();
        }

        private void UpdateDronePosition(Vector2 newPosition)
        {
            Debug.Log($"New drone position: {newPosition}");
        }

        private void UpdateMaterials()
        {
            float sX = materialsParent.rect.width / radarSize.x;
            float sY = materialsParent.rect.height / radarSize.y;
            
            foreach (var material in _droneManager.Materials)
            {
                Vector2 pos = new Vector2(material.Position.x * sX, material.Position.y * sY);
                
                // TODO: Optimize this
                GameObject obj = Instantiate(materialIndicator, materialsParent);
                obj.GetComponent<RectTransform>().anchoredPosition = pos;
            }
        }
    }
}