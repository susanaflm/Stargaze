using Cinemachine;
using Mirror;
using UnityEngine;

namespace Stargaze.Mono.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerZeroGController : NetworkBehaviour
    {
        private PlayerInput _input;
        
        private CharacterController _characterController;

        private Vector3 _strafeVelocity;
        private float _rollVelocity;
        
        [SerializeField] private new CinemachineVirtualCamera camera;

        [Space]
        
        [SerializeField] private Transform centerOfMass;
        
        [Space]
        
        [SerializeField] private float thrustForce = 1f;
        [SerializeField] private float dragCoefficient = 1f;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float rollSpeed = 1f;

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

        private void Update()
        {
            if (!isLocalPlayer)
                return;

            Strafe();
            
            Rotation();
        }

        private void Strafe()
        {
            _strafeVelocity = _characterController.velocity;
            
            Vector3 input = _input.Strafe;
            Vector3 inputDir = transform.forward * input.z + transform.right * input.x + transform.up * input.y;
            
            _strafeVelocity += inputDir * thrustForce * Time.deltaTime;
            _strafeVelocity += -dragCoefficient * _strafeVelocity * Time.deltaTime;

            _characterController.Move(_strafeVelocity * Time.deltaTime);
        }

        private void Rotation()
        {
            Vector2 lookInput = _input.Look;
            float rollInput = _input.Roll;

            Vector3 comPos = centerOfMass.position;

            _rollVelocity += rollInput * rollSpeed * Time.deltaTime;
            _rollVelocity += -dragCoefficient * _rollVelocity * Time.deltaTime;
            
            transform.RotateAround(comPos, transform.up, lookInput.x * rotationSpeed * Time.deltaTime);
            transform.RotateAround(comPos, transform.right, -lookInput.y * rotationSpeed * Time.deltaTime);
            transform.RotateAround(comPos, transform.forward, _rollVelocity * Time.deltaTime);
        }
    }
}