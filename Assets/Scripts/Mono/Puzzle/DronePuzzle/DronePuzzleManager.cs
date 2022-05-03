using System;
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

            _materials.Callback += MaterialListChangedCallback;
        }

        public override void OnStartServer()
        {
            _dronePosition = Vector2.zero;

            for (int i = 0; i < materialsToSpawn.Length; i++)
            {
                // TODO: Evaluate if this spawn position is good or not
                materialsToSpawn[i].Position = new Vector2(
                    Random.Range(-10, 11),
                    Random.Range(-10, 11)
                );
            }
            
            _materials.AddRange(materialsToSpawn);
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

        private void MaterialListChangedCallback(SyncList<ResourceMaterialEntry>.Operation op, int index, ResourceMaterialEntry oldMaterial, ResourceMaterialEntry newMaterial)
        {
            OnMaterialListChanged?.Invoke();
        }
    }
}