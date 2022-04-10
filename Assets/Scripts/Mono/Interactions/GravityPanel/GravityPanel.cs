using System;
using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.GravityPanel
{
    public class GravityPanel : MonoBehaviour
    {
        public delegate void OnGravityStatusChange(bool gravityStatus);
        
        public static OnGravityStatusChange OnGravitySwitch;
        
        [SerializeField] private string deactivateGravityCode;
        [SerializeField] private string activateGravityCode;

        private string _currentUnlockCode;
        private string _currentCode = "";
        private bool _isGravityOn = true;

        private void Awake()
        {
            deactivateGravityCode = deactivateGravityCode.ToLower();
            activateGravityCode = activateGravityCode.ToLower();

            _currentUnlockCode = deactivateGravityCode;
        }

        private void AddCharToCode(char input)
        {
            //Add the touched button input to the code
            _currentCode += input;

            if (_currentCode.Length > _currentUnlockCode.Length)
            {
                _currentCode = $"{input}";
            }
            
            //If the player didn't put the correct button pattern, the panel will reset the code
            if (input != _currentUnlockCode[_currentCode.Length - 1])
            {
                _currentCode = "";
                //TODO:Shoot an audio warning to say to the player that it reset
            }

#if DEBUG
            Debug.Log("Code: " + _currentCode);
#endif
            if (_currentCode != _currentUnlockCode) return;

            if (_currentUnlockCode == deactivateGravityCode)
                DeactivateGravity();
            else
                ActivateGravity();

            _currentCode = "";
            
        }

        private void DeactivateGravity()
        {
            _isGravityOn = false;
            OnGravitySwitch?.Invoke(_isGravityOn);
            PuzzleManager.Instance.DeactivateGravity();
#if DEBUG
            Debug.Log("Gravity successfully deactivated");
#endif
            _currentUnlockCode = activateGravityCode;
        }

        private void ActivateGravity()
        {
            _isGravityOn = true;
            OnGravitySwitch?.Invoke(_isGravityOn);
            PuzzleManager.Instance.ActivateGravity();
#if DEBUG
            Debug.Log("Gravity successfully activated");
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
