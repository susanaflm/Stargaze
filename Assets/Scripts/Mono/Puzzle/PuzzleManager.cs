using System;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    public class PuzzleManager : MonoBehaviour
    {
        public static PuzzleManager Instance;

        private bool _doesPlayerHaveMagnet = false;
        private int _keycardAccessLevel = 0;

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

        public bool DoesPlayerHaveMagnet()
        {
            return _doesPlayerHaveMagnet;
        }
    }
}
