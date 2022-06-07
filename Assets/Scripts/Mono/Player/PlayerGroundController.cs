using System;
using Cinemachine;
using DG.Tweening;
using Mirror;
using Stargaze.ScriptableObjects.Settings;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerGroundController : NetworkBehaviour
    {
        private PlayerInput _input;

        private CharacterController _characterController;

        private float _verticalRotation;

        private bool _isGrounded;
        private bool _wasGrounded;
        private bool _isSliding;

        private Vector3 _strafeVelocity;
        private Vector3 _verticalVelocity;
        private Vector3 _slidingVelocity;

        private Vector3 _desiredVelocity;
        private Vector3 _velocity;
        
        private RaycastHit _groundContactPointHit;

        private bool _isPlayerInteracting = false;

        [Header("Movement")]
        [SerializeField] private float strafeSpeed = 1f;
        [SerializeField] private float strafeDampingTime = 0.25f;
        [SerializeField] private float slideSpeed;
        [SerializeField] private float slideDampingTime = 0.5f;

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

        [Header("Settings")] [SerializeField] private GameSettingsData gameSettings;

        public Action OnJump;
        public Action OnLand;
        
        public Vector2 AnimationDir { get; private set; }

        public bool IsGrounded => _isGrounded;
        
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
            if (!isLocalPlayer || _isPlayerInteracting)
                return;
            
            GroundCheck();

            CalculateSlidingVelocity();

            CalculateStrafeVelocity();
            
            CalculateVerticalVelocity();
            
            HandleRotation();

            _characterController.Move((_strafeVelocity + _slidingVelocity + _verticalVelocity) * Time.deltaTime);
        }
        
        private void GroundCheck()
        {
            _wasGrounded = _isGrounded;
            
            _isGrounded = Physics.CheckSphere(
                transform.position + groundCheckCenter,
                groundCheckRadius,
                groundCheckLayer
            );

            if (!_wasGrounded && _isGrounded)
                OnLand?.Invoke();
        }

        private void CalculateSlidingVelocity()
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
                _slidingVelocity = Vector3.zero;
                return;
            }

            _isSliding = true;

            Vector3 tangent = Vector3.Cross(normal, transform.up);
            Vector3 binormal = Vector3.Cross(normal, tangent);
            
            Vector3 targetVelocity = binormal * slideSpeed;
            Vector3.SmoothDamp(
                transform.position,
                transform.position + targetVelocity,
                ref _slidingVelocity,
                slideDampingTime,
                slideSpeed
            );
        }

        private void CalculateStrafeVelocity()
        {
            if (_isSliding)
            {
                _strafeVelocity = Vector3.zero;
                return;
            }
            
            Vector3 movementInput = _input.Strafe;

            Vector3 dir = transform.forward * movementInput.z + transform.right * movementInput.x;
            dir.Normalize();

            Vector3 targetVelocity = dir * strafeSpeed;
            Vector3.SmoothDamp(
                transform.position,
                transform.position + targetVelocity,
                ref _strafeVelocity,
                strafeDampingTime,
                strafeSpeed
            );

            Vector3 localVelocity = transform.InverseTransformVector(_strafeVelocity);
            AnimationDir = new Vector2(localVelocity.x / strafeSpeed, localVelocity.z / strafeSpeed);
        }

        private void CalculateVerticalVelocity()
        {
            if (_isGrounded)
                return;
                
            _verticalVelocity += Physics.gravity * Time.deltaTime;
        }
        
        private void Jump()
        {
            if (!isLocalPlayer || !enabled)
                return;
            
            if (_isGrounded && !_isSliding)
            {
                _verticalVelocity += transform.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
                OnJump?.Invoke();
            }
        }

        private void HandleRotation()
        {
            Vector2 lookInput = _input.Look;

            _verticalRotation += -lookInput.y * rotationSpeed * gameSettings.CameraSensitivity * Time.deltaTime;

            _verticalRotation = Mathf.Clamp(
                _verticalRotation, 
                -lookUpLimit,
                lookDownLimit
            );
            
            transform.Rotate(transform.up, lookInput.x * rotationSpeed * gameSettings.CameraSensitivity * Time.deltaTime);
            camera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);
        }

        public void RecenterCamera()
        {
            camera.transform.DOLocalRotate(Vector3.zero, 0.5f);
            _verticalRotation = 0f;
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
