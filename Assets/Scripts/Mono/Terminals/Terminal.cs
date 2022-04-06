using Stargaze.Mono.Interactions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Stargaze.Mono.Terminals
{
    public class Terminal : Interactable
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

        private void Awake()
        {
            _renderTexture = new RenderTexture(1280, 720, 16);
            terminalCamera.targetTexture = _renderTexture;
            
            display.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", _renderTexture);
            display.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", _renderTexture);
        }

        private void Start()
        {
            loginScreen.SetActive(true);
            contentScreen.SetActive(false);
        }

        public override void OnInteractionStart()
        {
            loginScreen.SetActive(false);
            contentScreen.SetActive(true);
            
            if (focusOnInteract != null)
                EventSystem.current.SetSelectedGameObject(focusOnInteract);
        }

        public override void OnInteractionEnd()
        {
            contentScreen.SetActive(false);
            loginScreen.SetActive(true);
        }
    }
}