using UnityEngine;

namespace Stargaze.Mono.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput _input;

        private CharacterController _characterController;

        private float _verticalRotation;
        private float _horizontalRotation;

        [Header("Movement")]
        [SerializeField] private float movementSpeed = 1f;
        
        [Header("Looking")]
        [SerializeField] private new Transform camera;
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private float verticalRotationLowerBound = -90;
        [SerializeField] private float verticalRotationUpperBound = 90;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();

            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleMovement();
            
            HandleRotation();
        }

        private void HandleMovement()
        {
            Vector2 movementInput = _input.Movement;

            Vector3 dir = transform.forward * movementInput.y + transform.right * movementInput.x;
            dir.Normalize();

            _characterController.Move(dir * (movementSpeed * Time.deltaTime));
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
    }
}
