using System;
using System.Collections.Generic;
using Mirror;
using Stargaze.ScriptableObjects.Materials;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    public class PuzzleManager : NetworkBehaviour
    {
        public static PuzzleManager Instance;
        public Action<ResourceMaterial> OnCollectMaterial;
        public Action<List<Door.Door>> OnElectricalPuzzleComplete;
        public Action<List<Door.Door>> OnElectricalPuzzleUndo;
        public Action<List<Door.Door>> OnMazePuzzleComplete;
        public Action<List<Door.Door>> OnGravityPuzzleComplete;

        [SyncVar]
        private bool _doesPlayerHaveMagnet = false;

        [SyncVar]
        private bool _doesPlayerHaveLockerKey = false;

        [SyncVar]
        private bool _isPowerOn = false;
        
        [SyncVar]
        private bool _gravityStatus = true;

        private bool _gravityPuzzleComplete = false;
        private bool _mazePuzzleComplete = false;

        private SyncList<ResourceMaterial> _gatheredMaterials = new();
        
        [Tooltip("Electrical Puzzle Doors")] [SerializeField]
        private List<Door.Door> electricalDoors = new();

        [Tooltip("Player Maze Puzzle Doors")] [SerializeField]
        private List<Door.Door> mazeDoors = new();

        [Tooltip("Gravity Puzzle Doors")] [SerializeField]
        private List<Door.Door> gravityDoors = new();
        
        [SerializeField]
        private List<ResourceMaterial> cheatMaterials = new();
        

        /// <summary>
        /// Variable so the player can't interact with doors and progress the game without gravity
        /// </summary>
        public bool GravityStatus => _gravityStatus;

        public SyncList<ResourceMaterial> GatheredMaterials => _gatheredMaterials;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        [Server]
        public void GetMagnet()
        {
            _doesPlayerHaveMagnet = true;
        }

        [Command(requiresAuthority = false)]
        public void CmdGetLockerKey()
        {
            _doesPlayerHaveLockerKey = true;
        }

        [Command(requiresAuthority = false)]        
        public void CmdSetPowerStatus(bool status)
        {
            _isPowerOn = status;

            if (_isPowerOn)
            {
                OnElectricalPuzzleComplete?.Invoke(electricalDoors);
            }
            else
            {
                OnElectricalPuzzleUndo?.Invoke(electricalDoors);
            }
            
#if DEBUG
            Debug.Log("Power On!");
#endif
        }

        [Server]
        public void DeactivateGravity()
        {
            _gravityStatus = false;
        }
        
        [Server]
        public void ActivateGravity()
        {
            _gravityStatus = true;
        }

        [Server]
        public void AddMaterial(ResourceMaterial mat)
        {
            if (mat == null)
                return;
            
            foreach (var material in _gatheredMaterials)
                if (material == mat)
                    return;

            _gatheredMaterials.Add(mat);

#if DEBUG
            Debug.Log($"Successfully added {mat.name}");   
#endif
        }

        public bool DoesPlayerHaveMagnet() => _doesPlayerHaveMagnet;

        public bool DoesPlayerHaveLockerKey() => _doesPlayerHaveLockerKey;

        public bool IsPowerOn() => _isPowerOn;

        public void AddMaterialsCheat()
        {
            _gatheredMaterials.AddRange(cheatMaterials);
        }

        [Server]
        public void CollectMaterial(ResourceMaterial resourceMaterial)
        {
            OnCollectMaterial?.Invoke(resourceMaterial);
        }

        [Server]
        public void CompleteGravity()
        {
            if (_gravityPuzzleComplete)
                return;

            OnGravityPuzzleComplete?.Invoke(gravityDoors);
            _gravityPuzzleComplete = true;
        }

        [Server]
        public void CompleteMaze()
        {
            if (_mazePuzzleComplete)
                return;

            OnMazePuzzleComplete?.Invoke(mazeDoors);
            _mazePuzzleComplete = true;
        }
    }
}
