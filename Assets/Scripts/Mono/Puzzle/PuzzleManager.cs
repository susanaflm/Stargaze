using System;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    public class PuzzleManager : MonoBehaviour
    {
        public static PuzzleManager Instance;

        private bool doesPlayerHaveMagnet = false;
        private int keycardAccessLevel = 0;

        private void Awake()
        {
            Instance = this;
        }
        
        public int GetKeycardAccessLevel()
        {
            return keycardAccessLevel;
        }

        public void SetCurrentKeycardAccesslevel(int kcAccess)
        {
            keycardAccessLevel = kcAccess;
        }

        public void GetMagnet()
        {
            doesPlayerHaveMagnet = true;
        }

        public bool DoesPlayerHaveMagnet()
        {
            return doesPlayerHaveMagnet;
        }
    }
}
