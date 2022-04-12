using System;
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

        private void Update()
        {
            Vector2 dir = _playerGroundController.AnimationDir;
            
            _animator.SetFloat("X_Dir", dir.x);
            _animator.SetFloat("Y_Dir", dir.y);
        }
    }
}