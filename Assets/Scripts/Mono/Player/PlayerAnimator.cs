using System;
using Stargaze.Mono.UI.RadioFrequencyPanel;
using UnityEngine;

namespace Stargaze.Mono.Player
{
    [RequireComponent(typeof(PlayerGroundController))]
    public class PlayerAnimator : MonoBehaviour
    {
        private PlayerGroundController _playerGroundController;
        
        private Animator _animator;

        private void Awake()
        {
            _playerGroundController = GetComponent<PlayerGroundController>();
            
            _animator = GetComponent<Animator>();

            if (_animator == null)
                _animator = GetComponentInChildren<Animator>();

            if (_animator == null)
                _animator = GetComponentInParent<Animator>();
            
            if (_animator == null)
                Debug.LogError($"No animator component found on {name} neither on his children nor parent.");
        }

        private void Start()
        {
            _playerGroundController.OnJump += () =>
            {
                _animator.SetTrigger("Jump");
            };
            
            _playerGroundController.OnLand += () =>
            {
                _animator.SetTrigger("Land");
            };

            RadioFrequencyPanel.OnRadioFrequencyPanelShow += () =>
            {
                _animator.SetTrigger("WristUp");
            };
            
            RadioFrequencyPanel.OnRadioFrequencyPanelHide += () =>
            {
                _animator.SetTrigger("WristDown");
            };
        }

        private void Update()
        {
            Vector2 dir = _playerGroundController.AnimationDir;
            
            _animator.SetFloat("DirX", dir.x);
            _animator.SetFloat("DirY", dir.y);
            
            _animator.SetBool("IsGrounded", _playerGroundController.IsGrounded);
        }
    }
}