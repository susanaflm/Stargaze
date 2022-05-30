using System;
using Stargaze.Mono.Puzzle.DronePuzzle;
using UnityEngine;

namespace Stargaze.Mono.Terminals
{
    public class LidarHubTerminal : Terminal
    {
        private DronePuzzleManager _droneManager;

        [Space]
        [SerializeField] private RectTransform droneIndicator;
        [SerializeField] private GameObject materialIndicatorPrefab;
        [SerializeField] private RectTransform materialsParent;
        [SerializeField] private Vector2 radarSize;

        public override void OnStartClient()
        {
            _droneManager = DronePuzzleManager.Instance;
            
            if (_droneManager == null)
                Debug.LogError($"Can find {nameof(DronePuzzleManager)} instance. Did you instantiate it?");
            
            _droneManager.OnDronePositionChanged += UpdateDronePosition;
            _droneManager.OnMaterialListChanged += UpdateMaterials;
            
            UpdateDronePosition(_droneManager.DronePosition);
            UpdateMaterials();
        }

        private void UpdateDronePosition(Vector2 newPosition)
        {
            float sX = materialsParent.rect.width / radarSize.x;
            float sY = materialsParent.rect.height / radarSize.y;

            droneIndicator.anchoredPosition = new Vector2(newPosition.x * sX, newPosition.y * sY);
        }

        private void UpdateMaterials()
        {
            foreach (RectTransform child in materialsParent)
            {
                Destroy(child.gameObject);
            }
            
            float sX = materialsParent.rect.width / radarSize.x;
            float sY = materialsParent.rect.height / radarSize.y;
            
            foreach (var material in _droneManager.Materials)
            {
                Vector2 pos = new Vector2(material.Position.x * sX, material.Position.y * sY);
                
                // TODO: Optimize this?
                GameObject obj = Instantiate(materialIndicatorPrefab, materialsParent);
                obj.GetComponent<RectTransform>().anchoredPosition = pos;
            }
        }

        public override void OnInteractionStart()
        {
            base.OnInteractionStart();
            
            GetComponent<AudioSource>().Play();
        }
    }
}