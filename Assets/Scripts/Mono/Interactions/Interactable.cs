using System.Collections;
using System.Collections.Generic;
using Stargaze.Mono.Interactions;
using UnityEngine;

namespace Stargaze
{
    public class Interactable : MonoBehaviour , IInteractable
    {
        #region Variables

        [Header("Interactable Settings")]
        private bool _isInteractable = true;
        
        [SerializeField] private bool isSwitchable;

        #endregion

        #region Properties
        
        public bool Switchable => isSwitchable;

        public bool IsInteractable => _isInteractable;

        #endregion


        #region Methods
        
        public void OnInteraction()
        {
           Debug.Log($"Interacted with: {gameObject.name}");
        }

        #endregion
        
      
    }
}
