using System;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Inspection
{
    public class InspectInteractable : Interactable
    {

        public static Action Restore;

        [SerializeField] private GameObject interactionUI;

        private GameObject _inspectObject;
        
        private void Start()
        {
            Restore += RestoreUI;
        }

        public override void OnInteractionStart()
        {
            base.OnInteractionStart();
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            interactionUI.SetActive(true);
            
            _inspectObject = Instantiate(gameObject, Vector3.zero, Quaternion.identity);
            _inspectObject.AddComponent<InspectionTurn>();
            _inspectObject.layer = LayerMask.NameToLayer("UI");
        }

        private void RestoreUI()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            interactionUI.SetActive(false);
            Destroy(_inspectObject);
        }

        private void OnDestroy()
        {
            Restore -= RestoreUI;
        }
    }
}
