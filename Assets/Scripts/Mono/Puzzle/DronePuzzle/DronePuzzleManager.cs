using System;
using System.Collections.Generic;
using Mirror;
using Stargaze.Enums;
using Stargaze.ScriptableObjects.Materials;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stargaze.Mono.Puzzle.DronePuzzle
{
    [Serializable]
    public struct ResourceMaterialEntry
    {
        public ResourceMaterial Material;

        [HideInInspector]
        public Vector2 Position;
    }
    
    public class DronePuzzleManager : NetworkBehaviour
    {
        public static DronePuzzleManager Instance;
        
        [SyncVar(hook = nameof(DronePositionChangedCallback))]
        private Vector2 _dronePosition;

        private SyncList<ResourceMaterialEntry> _materials = new();

        [SerializeField] private ResourceMaterialEntry[] materialsToSpawn;

        [Space]
        
        [SerializeField] private float moveDistance = 1f;
        [SerializeField] private float collectionDistance = 1f;

        [Space]
        
        [SerializeField] private float minSpawnDistance = 2f;

        public Action<Vector2> OnDronePositionChanged;
        public Action OnMaterialListChanged;

        public Vector2 DronePosition => _dronePosition;

        public SyncList<ResourceMaterialEntry> Materials => _materials;

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

            for (int i = 0; i < materialsToSpawn.Length; i++)
            {
                materialsToSpawn[i].Position = GenerateMaterialSpawnPosition(i);
            }
            
            _materials.AddRange(materialsToSpawn);
        }
        
        public override void OnStartClient()
        {
            _materials.Callback += MaterialListChangedCallback;
        }

        [Server]
        public void MoveDrone(Direction2D dir)
        {
            _dronePosition += dir switch
            {
                Direction2D.Up => Vector2.up * moveDistance,
                Direction2D.Down => Vector2.down * moveDistance,
                Direction2D.Left => Vector2.left * moveDistance,
                Direction2D.Right => Vector2.right * moveDistance,
                _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
            };
        }

        [Server]
        public void CollectResource()
        {
            List<int> indicesToCollect = new();

            for (int i = 0; i < _materials.Count; i++)
            {
                Vector2 dirToMat = _dronePosition - _materials[i].Position;
                float distanceToMat = dirToMat.magnitude;

                if (distanceToMat <= collectionDistance)
                {
                    indicesToCollect.Add(i);
                }
            }

            foreach (int i in indicesToCollect)
            {
                PuzzleManager.Instance.AddMaterial(_materials[i].Material);
                _materials.RemoveAt(i);
            }
            
            Debug.Log($"{_materials.Count}");
        }

        private Vector2 GenerateMaterialSpawnPosition(int index)
        {
            Vector2 position;
            bool awayFromDrone, awayFromMaterials = true;
            
            do
            {
                position = new Vector2(
                    Random.Range(-10, 11),
                    Random.Range(-10, 11)
                );

                awayFromDrone = (position - _dronePosition).magnitude > minSpawnDistance;

                for (int i = 0; i < index && awayFromMaterials; i++)
                {
                    //awayFromMaterials &= (materialsToSpawn[i].Position - position).magnitude > minSpawnDistance;
                    awayFromMaterials &= position != materialsToSpawn[i].Position;
                }
                    
            } while (!awayFromDrone || !awayFromMaterials);
            
            return position;
        }

        private void DronePositionChangedCallback(Vector2 oldPosition, Vector2 newPosition)
        {
            OnDronePositionChanged?.Invoke(newPosition);
        }

        private void MaterialListChangedCallback(SyncList<ResourceMaterialEntry>.Operation op, int index, ResourceMaterialEntry oldMaterial, ResourceMaterialEntry newMaterial)
        {
            OnMaterialListChanged?.Invoke();
        }
    }
}