using System;
using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.GravityPanel
{
    public class GravityPanel : MonoBehaviour
    {
        [SerializeField] private string unlockCode;
        
        private string _currentCode = "";

        private void Awake()
        {
            unlockCode = unlockCode.ToLower();
        }

        private void AddCharToCode(char input)
        {
            //Add the touched button input to the code
            _currentCode += input;

            if (_currentCode.Length > unlockCode.Length)
            {
                _currentCode = $"{input}";
            }
            
            //If the player didn't put the correct button pattern, the panel will reset the code
            if (input != unlockCode[_currentCode.Length - 1])
            {
                _currentCode = "";
                //TODO:Shoot an audio warning to say to the player that it reset
            }

#if DEBUG
            Debug.Log("Code: " + _currentCode);
#endif
            if (!_currentCode.Equals(unlockCode)) return;
            
            _currentCode = "";
            DeactivateGravity();
            PuzzleManager.Instance.DeactivateGravity();
        }

        private void DeactivateGravity()
        {
            //TODO: Deactivate gravity so the other player can cross the zone
#if DEBUG
            Debug.Log("Gravity successfully deactivated");
#endif
        }

        private void OnEnable()
        {
            GravityPanelButton.SendButtonInput += AddCharToCode;
        }

        private void OnDisable()
        {
            GravityPanelButton.SendButtonInput -= AddCharToCode;
        }
    }
}
