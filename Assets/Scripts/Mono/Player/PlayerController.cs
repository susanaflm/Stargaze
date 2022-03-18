using System;
using UnityEngine;

namespace Stargaze.Mono.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public Action OnLand;
        
        private PlayerInput _input;

        private CharacterController _characterController;

        private float _verticalRotation;
        private float _horizontalRotation;

        private bool _isGrounded;
        private bool _wasGrounded;

        private Vector3 _verticalVelocity;

        [Header("Movement")]
        [SerializeField] private float movementSpeed = 1f;
        
        [Header("Looking")]
        [SerializeField] private new Transform camera;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float verticalRotationLowerBound = -90;
        [SerializeField] private float verticalRotationUpperBound = 90;

        [Header("Ground Check")]
        [SerializeField] private Vector3 groundCheckCenter;
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private LayerMask groundCheckLayer;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();

            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            OnLand += () =>
            {
                // TODO: Is it worth creating a function for this?
                _verticalVelocity = Vector3.zero;
            };
        }

        private void Update()
        {
            GroundCheck();
            
            HandleMovement();
            
            HandleRotation();
        }

        private void GroundCheck()
        {
            _wasGrounded = _isGrounded;
            
            // TODO: Is a sphere cast method better?
            _isGrounded = Physics.CheckSphere(
                transform.position + groundCheckCenter,
                groundCheckRadius,
                groundCheckLayer
            );
            
            if (!_wasGrounded && _isGrounded)
                OnLand?.Invoke();
        }

        private void HandleMovement()
        {
            Vector2 movementInput = _input.Movement;

            Vector3 dir = transform.forward * movementInput.y + transform.right * movementInput.x;
            dir.Normalize();

            _characterController.Move(dir * (movementSpeed * Time.deltaTime));

            if (!_isGrounded)
                _verticalVelocity += Physics.gravity * Time.deltaTime;

            _characterController.Move(_verticalVelocity * Time.deltaTime);
        }

        private void HandleRotation()
        {
            Vector2 lookInput = _input.Look;

            _horizontalRotation += lookInput.x * rotationSpeed * Time.deltaTime;
            _verticalRotation += -lookInput.y * rotationSpeed * Time.deltaTime;

            _verticalRotation = Mathf.Clamp(
                _verticalRotation, 
                verticalRotationLowerBound, 
                verticalRotationUpperBound
            );
            
            transform.localRotation = Quaternion.Euler(0f, _horizontalRotation, 0f);
            camera.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + groundCheckCenter, groundCheckRadius);
        }
    }
}
