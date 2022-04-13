using System;
using Stargaze.Mono.Puzzle.FuelMixturePuzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.FuelMixing
{
    public class FuelMixingInteractable : Interactable
    {
        public delegate void OnInteraction();

        public static OnInteraction FillUI;
        
        [SerializeField] private GameObject fuelMixingUI;
        
        public override void OnInteractionStart()
        {
            base.OnInteractionStart();
            
            fuelMixingUI.SetActive(true);
            FillUI?.Invoke();
        }

        public override void OnInteractionEnd()
        {
            base.OnInteractionEnd();
            
            fuelMixingUI.SetActive(false);
        }
    }
}
