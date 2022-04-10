using System;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    public class PuzzleManager : MonoBehaviour
    {
        public static PuzzleManager Instance;

        private int _keycardAccessLevel = 0;
        
        private bool _doesPlayerHaveMagnet = false;
        private bool _wasGravityDeactivated = false;

        public bool WasGravityDeactivated => _wasGravityDeactivated;

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
            _wasGravityDeactivated = true;
        }

        public bool DoesPlayerHaveMagnet() => _doesPlayerHaveMagnet;
    }
}
