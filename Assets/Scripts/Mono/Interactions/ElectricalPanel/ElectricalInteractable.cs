using System;
using Cinemachine;
using UnityEngine;

namespace Stargaze.Mono.Interactions.ElectricalPanel
{
    public class ElectricalInteractable : Interactable
    {
        public delegate void OnInteraction();

        public static OnInteraction OnInteractionEnableWire;
        
        [SerializeField] private CinemachineVirtualCamera puzzleCamera;
        
        private void Start()
        {
            puzzleCamera.gameObject.SetActive(false);
        }

        public override void OnInteractionStart()
        {
            base.OnInteractionStart();
            
            puzzleCamera.gameObject.SetActive(true);
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            //TODO: Enable Wire Grabbing
            OnInteractionEnableWire?.Invoke();
        }

        public override void OnInteractionEnd()
        {
            base.OnInteractionEnd();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            puzzleCamera.gameObject.SetActive(false);

            //TODO: Disable Wire Grabbing
        }
    }
}
