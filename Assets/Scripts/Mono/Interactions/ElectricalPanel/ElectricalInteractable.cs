using System;
using Cinemachine;
using UnityEngine;

namespace Stargaze.Mono.Interactions.ElectricalPanel
{
    public class ElectricalInteractable : MonoBehaviour, IInteractable
    {
        public delegate void OnInteraction();

        public static OnInteraction OnInteractionEnableWire;
        
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;
        
        [SerializeField] private CinemachineVirtualCamera puzzleCamera;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;
        
        private void Start()
        {
            puzzleCamera.gameObject.SetActive(false);
        }

        public void OnInteractionStart()
        {
            puzzleCamera.gameObject.SetActive(true);
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            //TODO: Enable Wire Grabbing
            OnInteractionEnableWire?.Invoke();
        }

        public void OnInteractionEnd()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            puzzleCamera.gameObject.SetActive(false);

            //TODO: Disable Wire Grabbing
        }
    }
}
