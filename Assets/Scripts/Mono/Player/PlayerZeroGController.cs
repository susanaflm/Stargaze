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

        private Vector3 _velocity;
        
        [SerializeField] private new CinemachineVirtualCamera camera;
        
        [Space]
        
        [SerializeField] private float thrustForce = 1f;
        [SerializeField] private float dragCoefficient = 1f;

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

            _velocity = _characterController.velocity;
            
            Vector3 input = _input.Strafe;
            Vector3 inputDir = transform.forward * input.z + transform.right * input.x + transform.up * input.y;
            
            _velocity += inputDir * thrustForce * Time.deltaTime;
            _velocity += -dragCoefficient * _velocity * Time.deltaTime;

            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}