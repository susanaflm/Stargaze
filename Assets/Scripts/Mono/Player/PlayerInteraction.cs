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

            bool hitSomething = Physics.SphereCast(ray, raySphereRadius, out hit, rayDistance);

            if (hitSomething)
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();

                if (interactable == null)
                    interactable = hit.transform.GetComponentInParent<Interactable>();

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
            
#if DEBUG
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, hitSomething ? Color.green : Color.red );
#endif
        }

        private void Interact()
        {
            if (_controller.IsPlayerInteracting)
                return;
            
            if (interactionData.IsEmpty())
                return;

            if (!interactionData.Interactable.IsInteractable)
            {
                if (interactionData.Interactable is MagnetInteraction)
                {
                    ShowUI();
                }
                
                return;
            }

            if (!interactionData.Interactable.Switchable)
                _controller.IsPlayerInteracting = true;

            interactionData.Interact();
        }

        private void ExitInteraction()
        {
            if (!_controller.IsPlayerInteracting) return;
            
            interactionData.Interactable.OnInteractionEnd();

            _controller.IsPlayerInteracting = false;
        }
        
        //TODO: Until the player gets the magnet, The Magnet Interaction will display a message that the user cannot interact with it
        private void ShowUI()
        {
#if DEBUG
            Debug.Log("Alô! Não tens o íman!");
#endif
        }
    }
}
