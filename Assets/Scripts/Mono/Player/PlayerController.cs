using System;
using Cinemachine;
using Mirror;
using UnityEngine;

namespace Stargaze.Mono.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : NetworkBehaviour
    {
        public Action OnLand;
        
        private PlayerInput _input;

        private CharacterController _characterController;

        private float _verticalRotation;
        private float _horizontalRotation;

        private bool _isGrounded;
        private bool _wasGrounded;
        private bool _isSliding;

        private Vector3 _verticalVelocity;

        private bool _isPlayerInteracting = false;

        private RaycastHit _groundContactPointHit;

        [Header("Movement")]
        [SerializeField] private float movementSpeed = 1f;

        [Header("Jumping")]
        [SerializeField] private float jumpHeight = 1f;
        
        [Header("Looking")]
        [SerializeField] private new CinemachineVirtualCamera camera;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] [Range(0f, 90f)] private float lookDownLimit = 90;
        [SerializeField] [Range(0f, 90f)] private float lookUpLimit = 90;

        [Header("Ground Check")]
        [SerializeField] private Vector3 groundCheckCenter;
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private LayerMask groundCheckLayer;

        [Header("Slope Sliding")]
        [SerializeField] private float slidingSpeed;

        public Vector2 AnimationDir { get; private set; }
        
        public bool IsPlayerInteracting
        {
            get => _isPlayerInteracting;
            set => _isPlayerInteracting = value;
        }

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();

            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            if (!isLocalPlayer)
            {
                camera.enabled = false;
                _characterController.enabled = false;
            }
        }

        public override void OnStartLocalPlayer()
        {
            OnLand += () =>
            {
                // TODO: Is it worth creating a function for this?
                _verticalVelocity = Vector3.zero;
            };

            _input.Jump += Jump;
        }

        private void Update()
        {
            if (_isPlayerInteracting)
                return;

            GroundCheck();

            HandleSliding();

            HandleMovement();
            
            HandleRotation();
        }

        private void GroundCheck()
        {
            if (!isLocalPlayer)
                return;
            
            _wasGrounded = _isGrounded;
            
            _isGrounded = Physics.CheckSphere(
                transform.position + groundCheckCenter,
                groundCheckRadius,
                groundCheckLayer
            );

            if (!_wasGrounded && _isGrounded)
                OnLand?.Invoke();
        }

        private void HandleSliding()
        {
            if (!_isGrounded)
                return;
            
            bool hit = Physics.Raycast(
                transform.position + groundCheckCenter, 
                -transform.up, 
                out _groundContactPointHit
            );
            
            if (!hit)
                return;

            Vector3 normal = _groundContactPointHit.normal;

            float slopeAngle = Vector3.Angle(normal, transform.up);
            
            if (slopeAngle <= _characterController.slopeLimit)
            {
                _isSliding = false;
                return;
            }

            _isSliding = true;

            Vector3 tangent = Vector3.Cross(normal, transform.up);
            Vector3 binormal = Vector3.Cross(normal, tangent);

            _characterController.Move(binormal * slidingSpeed * Time.deltaTime);
        }

        private void HandleMovement()
        {
            if (!isLocalPlayer)
                return;
            
            Vector2 movementInput = _input.Movement;

            Vector3 dir = transform.forward * movementInput.y + transform.right * movementInput.x;
            dir.Normalize();

            _characterController.Move(dir * (movementSpeed * Time.deltaTime));

            if (!_isGrounded)
                _verticalVelocity += Physics.gravity * Time.deltaTime;

            _characterController.Move(_verticalVelocity * Time.deltaTime);

            // TODO: This will make the character animate even when we is walking against a wall. Do we want to fix this?
            AnimationDir = movementInput;
        }

        private void Jump()
        {
            if (!isLocalPlayer)
                return;
            
            if (_isGrounded && !_isSliding)
                _verticalVelocity += transform.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }

        private void HandleRotation()
        {
            if (!isLocalPlayer)
                return;
            
            Vector2 lookInput = _input.Look;

            _horizontalRotation += lookInput.x * rotationSpeed * Time.deltaTime;
            _verticalRotation += -lookInput.y * rotationSpeed * Time.deltaTime;

            _verticalRotation = Mathf.Clamp(
                _verticalRotation, 
                -lookUpLimit,
                lookDownLimit
            );
            
            transform.localRotation = Quaternion.Euler(0f, _horizontalRotation, 0f);
            camera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + groundCheckCenter, groundCheckRadius);

            if (_isGrounded)
            {
                // Contact point
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_groundContactPointHit.point, 0.05f);
                
                // Contact point normal
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(
                    _groundContactPointHit.point,
                    _groundContactPointHit.point + _groundContactPointHit.normal * 0.25f 
                );
            }
        }
    }
}
