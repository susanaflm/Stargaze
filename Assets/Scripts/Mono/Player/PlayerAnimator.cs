using System;
using UnityEngine;

namespace Stargaze.Mono.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAnimator : MonoBehaviour
    {
        private PlayerController _playerController;
        
        private Animator _animator;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            
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
            Vector2 dir = _playerController.AnimationDir;
            
            _animator.SetFloat("X_Dir", dir.x);
            _animator.SetFloat("Y_Dir", dir.y);
        }
    }
}