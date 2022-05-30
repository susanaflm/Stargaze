using Mirror;
using Stargaze.Mono.Interactions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Stargaze.Mono.Terminals
{
    public class Terminal : NetworkBehaviour, IInteractable
    {
        private RenderTexture _renderTexture;
        
        [Header("Terminal Settings")]
        [SerializeField] private Camera terminalCamera;
        [SerializeField] private GameObject display;

        [Header("Screens")]
        [SerializeField] private GameObject loginScreen;
        [SerializeField] private GameObject contentScreen;
        
        [Space]
        
        [SerializeField] private GameObject focusOnInteract;

        public bool Switchable => false;
        public bool IsInteractable => true;
        
        private void Awake()
        {
            _renderTexture = new RenderTexture(1280, 720, 16);
            terminalCamera.targetTexture = _renderTexture;
            
            display.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", _renderTexture);
            display.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", _renderTexture);
            
            TerminalAwake();
        }

        protected virtual void TerminalAwake() { }

        private void Start()
        {
            loginScreen.SetActive(true);
            contentScreen.SetActive(false);
            
            TerminalStart();
        }
        
        protected virtual void TerminalStart() { }

        public virtual void OnInteractionStart()
        {
            loginScreen.SetActive(false);
            contentScreen.SetActive(true);
            
            if (focusOnInteract != null)
                EventSystem.current.SetSelectedGameObject(focusOnInteract);
        }

        public void OnInteractionEnd()
        {
            contentScreen.SetActive(false);
            loginScreen.SetActive(true);
        }
    }
}