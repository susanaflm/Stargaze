using System;
using Stargaze.Mono.Interactions;
using Stargaze.Mono.Interactions.Inspection;
using Stargaze.Mono.Interactions.Magnet;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Stargaze.Mono.Player
{
    public class PlayerInteraction : MonoBehaviour
    {

        public static Action OnHoverInteractable;
        public static Action OnHoverInteractableExit;
        public static Action OnInteractEnter;
        public static Action OnInteractExit;
        
        private PlayerInput _input;
        private PlayerGroundController _controller;
        private Camera _playerCamera;

        private IInteractable _currentInteractable;

        [Header("Data")]
        [SerializeField] private InteractionData interactionData;

        [Space] [Header("Ray Settings")]
        [SerializeField] private float rayDistance;
        [SerializeField] private float raySphereRadius;
        [SerializeField] private LayerMask detectableLayers;


        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _playerCamera = Camera.main;
            _controller = GetComponent<PlayerGroundController>();
        }
        
        void Start()
        {
            _input.Interact += Interact;
            _input.ExitInteraction += ExitInteraction;
        }
        
        void Update()
        {
            if (_controller.IsPlayerInteracting)
                return;
   
            CheckForInteractable();
        }

        private void CheckForInteractable()
        {
            RaycastHit hit;
            var camTransform = _playerCamera.transform;
            Ray ray = new Ray(camTransform.position, camTransform.forward);

            bool hitSomething = Physics.Raycast(ray, out hit, rayDistance, detectableLayers);

            if (hitSomething)
            {
                IInteractable interactable = hit.transform.GetComponent<IInteractable>();

                if (interactable == null)
                    interactable = hit.transform.GetComponentInParent<IInteractable>();

                if (interactable == null)
                {
                    interactionData.ResetInteractable();
                }

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

            if (interactionData.Interactable != null)
                OnHoverInteractable?.Invoke();
            else
                OnHoverInteractableExit?.Invoke();

#if DEBUG
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, hitSomething && interactionData.Interactable != null ? Color.green : Color.red );
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
            {
                _controller.IsPlayerInteracting = true;
                OnInteractEnter?.Invoke();
            }
            
            _currentInteractable = interactionData.Interactable;
            interactionData.Interact();
            
        }

        private void ExitInteraction()
        {
            if (!_controller.IsPlayerInteracting) return;
            
            _currentInteractable.OnInteractionEnd();

            _currentInteractable = null;
            _controller.IsPlayerInteracting = false;
            
            OnInteractExit?.Invoke();
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
