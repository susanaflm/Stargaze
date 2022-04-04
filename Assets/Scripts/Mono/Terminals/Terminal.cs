using System;
using Stargaze.Mono.Interactions;
using UnityEngine;

namespace Stargaze.Mono.Terminals
{
    public class Terminal : Interactable
    {
        private RenderTexture _renderTexture;
        
        [Header("Terminal Settings")]
        [SerializeField] private Camera terminalCamera;
        [SerializeField] private GameObject display;
        [SerializeField] private GameObject cursor;

        private void Awake()
        {
            _renderTexture = new RenderTexture(1280, 720, 16);
            terminalCamera.targetTexture = _renderTexture;
            
            display.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", _renderTexture);
            display.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", _renderTexture);
        }

        private void Start()
        {
            cursor.SetActive(false);
        }

        public override void OnInteractionStart()
        {
            cursor.SetActive(true);
        }

        public override void OnInteractionEnd()
        {
            cursor.SetActive(false);
        }
    }
}