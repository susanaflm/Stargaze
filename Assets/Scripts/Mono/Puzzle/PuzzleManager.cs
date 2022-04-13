using System;
using System.Collections.Generic;
using Stargaze.ScriptableObjects.Materials;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    public class PuzzleManager : MonoBehaviour
    {
        public static PuzzleManager Instance;

        private int _keycardAccessLevel = 0;
        
        private bool _doesPlayerHaveMagnet = false;
        
        private bool _gravityStatus = true;
        private bool _isGravityPuzzleComplete = false;

        private List<ResourceMaterial> _gatheredMaterials = new();

        /// <summary>
        /// If true the puzzle was completed
        /// </summary>
        public bool GravityPuzzleStatus => _isGravityPuzzleComplete; 
        
        /// <summary>
        /// Varaible so the player can't interact with doors and progress the game without gravity
        /// </summary>
        public bool GravityStatus => _gravityStatus;

        public List<ResourceMaterial> GatheredMaterials => _gatheredMaterials;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }
        
        public int GetKeycardAccessLevel()
        {
            return _keycardAccessLevel;
        }

        public void SetCurrentKeycardAccesslevel(int kcAccess)
        {
            _keycardAccessLevel = kcAccess;
        }

        public void GetMagnet()
        {
            _doesPlayerHaveMagnet = true;
        }

        public void DeactivateGravity()
        {
            _gravityStatus = false;
        }
        
        public void ActivateGravity()
        {
            _gravityStatus = true;
            _isGravityPuzzleComplete = true;
        }

        public void AddMaterial(ResourceMaterial mat)
        {
            if (mat == null)
                return;

            if (_gatheredMaterials.Exists(r => r.Equals(mat)))
                return;

            _gatheredMaterials.Add(mat);

#if DEBUG
            Debug.Log($"Successfully added {mat.name}");   
#endif
        }

        public bool DoesPlayerHaveMagnet() => _doesPlayerHaveMagnet;
    }
}
