using System;
using Mirror;
using Stargaze.Mono.Interactions.GravityPanel;
using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Player
{
    [RequireComponent(typeof(PlayerGroundController), typeof(PlayerZeroGController))]
    public class PlayerGravityController : NetworkBehaviour
    {
        private PlayerGroundController _groundController;
        private PlayerZeroGController _zeroGController;

        public Action OnGravityOn;
        public Action OnGravityOff;

        private void Awake()
        {
            _groundController = GetComponent<PlayerGroundController>();
            _zeroGController = GetComponent<PlayerZeroGController>();
        }

        public override void OnStartLocalPlayer()
        {
            bool isGravityOn;
            
            if (PuzzleManager.Instance != null)
                isGravityOn = PuzzleManager.Instance.GravityStatus;
            else
                isGravityOn = true;
            
            _groundController.enabled = isGravityOn;
            _zeroGController.enabled = !isGravityOn;
            
            GravityPanel.OnGravitySwitch += UpdateGravityStatus;
        }

        private void UpdateGravityStatus(bool isGravityOn)
        {
            if (isGravityOn)
            {
                _zeroGController.ResetRotation();
                OnGravityOn?.Invoke();
            }
            else
            {
                _groundController.RecenterCamera();
                OnGravityOff?.Invoke();
            }
            
            _groundController.enabled = isGravityOn;
            _zeroGController.enabled = !isGravityOn;
        }

#if UNITY_EDITOR
        public void SetGravity(bool isGravityOn) => UpdateGravityStatus(isGravityOn);
#endif
    }
}