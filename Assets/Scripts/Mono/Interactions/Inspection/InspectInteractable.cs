using System;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Inspection
{
    public class InspectInteractable : MonoBehaviour, IInteractable
    {
        private bool _isInteractable = true;
        
        private GameObject _inspectObject;

        private AudioSource _source;

        [SerializeField] private bool switchable;

        [SerializeField] private GameObject interactionUI;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void OnInteractionStart()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            interactionUI.SetActive(true);

            if (_source != null)
                _source.Play();

            _inspectObject = Instantiate(gameObject, Vector3.zero, Quaternion.identity);
            
            foreach (var comp in _inspectObject.GetComponents<Component>())
            {
                if (comp is Transform or MeshFilter or MeshRenderer)
                {
                    continue;
                }
                
                Destroy(comp);
            }
            
            _inspectObject.AddComponent<InspectionTurn>();
            _inspectObject.layer = LayerMask.NameToLayer("UI");
        }

        public void OnInteractionEnd()
        {
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
