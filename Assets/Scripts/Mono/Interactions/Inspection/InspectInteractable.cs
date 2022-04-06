using System;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Inspection
{
    public class InspectInteractable : Interactable
    {

        [SerializeField] private GameObject interactionUI;

        private GameObject _inspectObject;

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

        public override void OnInteractionEnd()
        {
            base.OnInteractionEnd();
            
            RestoreUI();
        }

        private void RestoreUI()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            interactionUI.SetActive(false);
            Destroy(_inspectObject);
        }
    }
}
