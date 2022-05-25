using System;
using Stargaze.Mono.Puzzle.FuelMixturePuzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.FuelMixing
{
    public class FuelMixingInteractable : MonoBehaviour, IInteractable
    {
        public delegate void OnInteraction();

        public static OnInteraction FillUI;
        
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;
        
        [SerializeField] private GameObject fuelMixingUI;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;
        
        public void OnInteractionStart()
        {
            fuelMixingUI.SetActive(true);
            FillUI?.Invoke();
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void OnInteractionEnd()
        {
            fuelMixingUI.SetActive(false);
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
