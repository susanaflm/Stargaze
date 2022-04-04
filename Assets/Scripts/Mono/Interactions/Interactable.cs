using UnityEngine;

namespace Stargaze.Mono.Interactions
{
    public class Interactable : MonoBehaviour , IInteractable
    {
        #region Variables

        [Header("Interactable Settings")]
        [SerializeField,Tooltip("Set if the Interactable will be something like a button or a lever")]
        private bool isSwitchable;
        
        protected bool isInteractable = true;

        #endregion

        #region Properties
        
        public bool Switchable => isSwitchable;

        public bool IsInteractable => isInteractable;

        #endregion


        #region Methods
        
        public virtual void OnInteractionStart()
        {
           Debug.Log($"Started interaction with: {name}");
        }

        public virtual void OnInteractionEnd()
        {
            Debug.Log($"Ended interaction with: {name}");
        }

        #endregion
        
      
    }
}
