using System;
using Mirror;
using Stargaze.Mono.UI.RadioFrequencyPanel;
using UnityEngine;

namespace Stargaze.Mono.Player
{
    [RequireComponent(typeof(PlayerGroundController))]
    [RequireComponent(typeof(PlayerGravityController))]
    public class PlayerAnimator : NetworkBehaviour
    {
        private PlayerGroundController _playerGroundController;
        private PlayerGravityController _playerGravityController;
        
        private Animator _animator;
        private NetworkAnimator _networkAnimator;

        private void Awake()
        {
            _playerGroundController = GetComponent<PlayerGroundController>();
            _playerGravityController = GetComponent<PlayerGravityController>();
            
            _animator = GetComponent<Animator>();
            _networkAnimator = GetComponent<NetworkAnimator>();

            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();

            if (_animator == null)
                _animator = GetComponentInParent<Animator>();
            
            if (_animator == null)
                Debug.LogError($"No animator component found on {name} neither on his children nor parent.");
        }

        public override void OnStartLocalPlayer()
        {
            _playerGroundController.OnJump += () =>
            {
                _networkAnimator.SetTrigger("Jump");
            };
            
            _playerGroundController.OnLand += () =>
            {
                _networkAnimator.SetTrigger("Land");
            };

            _playerGravityController.OnGravityOff += () =>
            {
                _networkAnimator.SetTrigger("GravityOff");
            };
            
            _playerGravityController.OnGravityOn += () =>
            {
                _networkAnimator.SetTrigger("GravityOn");
            };

            RadioFrequencyPanel.OnRadioFrequencyPanelShow += () =>
            {
                _networkAnimator.SetTrigger("WristUp");
            };
            
            RadioFrequencyPanel.OnRadioFrequencyPanelHide += () =>
            {
                _networkAnimator.SetTrigger("WristDown");
            };
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;
            
            Vector2 dir = _playerGroundController.AnimationDir;
            
            _animator.SetFloat("DirX", dir.x);
            _animator.SetFloat("DirY", dir.y);
            
            _animator.SetBool("IsGrounded", _playerGroundController.IsGrounded);
        }
    }
}