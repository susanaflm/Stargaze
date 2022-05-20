using System;
using Stargaze.Mono.Interactions.Inspection;
using Stargaze.ScriptableObjects.Materials;
using UnityEngine;
using UnityEngine.UI;

namespace Stargaze.Mono.Interactions.Book
{
    public class BookInteractable : MonoBehaviour, IInteractable
    {
        private bool _isInteractable = true;

        private GameObject _inspectObject;
        
        [SerializeField] private bool switchable;
        [Tooltip("Does this book have an animation to turn pages?")]
        [SerializeField] private bool _doesItHaveAnimator = false;

        [Header("UI")]
        [SerializeField] private GameObject pageTurnButtons;
        [SerializeField] private Button buttonBack;
        [SerializeField] private Button buttonFront;
        [SerializeField] private GameObject inspectionUI;

        [Header("Opened Book Prefab")]
        [SerializeField] private GameObject bookPrefab;
        
        public bool Switchable => switchable;
        
        public bool IsInteractable => _isInteractable;

        public void OnInteractionStart()
        {
            inspectionUI.SetActive(true);
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            _inspectObject = Instantiate(bookPrefab, Vector3.zero, Quaternion.identity);

            _inspectObject.AddComponent<InspectionTurn>();
            _inspectObject.layer = LayerMask.NameToLayer("UI");

            if (_doesItHaveAnimator)
            {
                _inspectObject.AddComponent<BookPageTurn>();
                _inspectObject.GetComponent<BookPageTurn>().AssignButtons(buttonBack, buttonFront);
                pageTurnButtons.SetActive(true);
            }
        }

        public void OnInteractionEnd()
        {
            RestoreUI();
        }
        
        
        private void RestoreUI()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            inspectionUI.SetActive(false);
            pageTurnButtons.SetActive(false);
            Destroy(_inspectObject);
        }
    }
}
