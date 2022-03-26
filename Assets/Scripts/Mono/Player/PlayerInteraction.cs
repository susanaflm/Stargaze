using Stargaze.Mono.Interactions;
using Stargaze.Mono.Interactions.Inspection;
using Stargaze.Mono.Interactions.Magnet;
using UnityEngine;

namespace Stargaze.Mono.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private PlayerInput _input;
        private PlayerController _controller;
        private Camera _playerCamera;

        [Header("Data")]
        [SerializeField] private InteractionData interactionData;

        [Space] [Header("Ray Settings")]
        [SerializeField] private float rayDistance;
        [SerializeField] private float raySphereRadius;
        [SerializeField] private LayerMask interactableLayer;
        
        
        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _playerCamera = Camera.main;
            _controller = GetComponent<PlayerController>();
        }
        
        void Start()
        {
            _input.Interact += Interact;
            _input.ExitInteraction += ExitInteraction;
        }
        
        void Update()
        {
            CheckForInteractable();
        }

        private void CheckForInteractable()
        {
            RaycastHit hit;
            var camTransform = _playerCamera.transform;
            Ray ray = new Ray(camTransform.position, camTransform.forward);

            bool hitSomething = Physics.SphereCast(ray, raySphereRadius, out hit, rayDistance, interactableLayer);

            if (hitSomething)
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();

                if (interactable != null)
                {
                    if (interactionData.IsEmpty())
                        interactionData.Interactable = interactable;
                    else
                    {
                        if (!interactionData.IsSameInteractable(interactable))
                            interactionData.Interactable = interactable;
                    }
                }
            }
            else
            {
                interactionData.ResetInteractable();
            }
            
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, hitSomething ? Color.green : Color.red );
        }

        private void Interact()
        {
            if (_controller.IsPlayerInteracting)
                return;
            
            if (interactionData.IsEmpty())
                return;
            
            if (!interactionData.Interactable.IsInteractable)
                return;

            if (!interactionData.Interactable.Switchable)
                _controller.IsPlayerInteracting = true;

            interactionData.Interact();
        }

        private void ExitInteraction()
        {
            if (!_controller.IsPlayerInteracting) return;

            switch (interactionData.Interactable)
            {
                case MagnetInteraction:
                    MagnetInteraction.Restore?.Invoke();
                    break;
                case InspectInteractable:
                    InspectInteractable.Restore?.Invoke();
                    break;
            }

            _controller.IsPlayerInteracting = false;
            
        }
    }
}
