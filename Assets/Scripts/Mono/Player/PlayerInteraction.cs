using Stargaze.Mono.Interactions;
using UnityEngine;

namespace Stargaze.Mono.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        private PlayerInput _input;
        private PlayerController _controller;

        [Header("Data")]
        [SerializeField] private InteractionData interactionData;

        [Space] [Header("Ray Settings")]
        [SerializeField] private float rayDistance;
        [SerializeField] private float raySphereRadius;
        [SerializeField] private LayerMask interactableLayer;
        
        [SerializeField] private Camera _playerCamera;
        
        private void Awake()
        {
            _input = GetComponent<PlayerInput>();

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
            if (interactionData.IsEmpty())
                return;
            
            _controller.IsPlayerInteracting = true;

            if (!interactionData.Interactable.IsInteractable)
                return;

            interactionData.Interact();
        }

        private void ExitInteraction()
        {
            _controller.IsPlayerInteracting = false;
        }
    }
}
