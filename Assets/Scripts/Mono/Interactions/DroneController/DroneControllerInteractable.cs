using UnityEngine;

namespace Stargaze.Mono.Interactions.DroneController
{
    public class DroneControllerInteractable : MonoBehaviour, IInteractable
    {
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;
        
        [SerializeField] private GameObject droneControllerUI;

        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;

        public void OnInteractionStart()
        {
            droneControllerUI.SetActive(true);
        }

        public void OnInteractionEnd()
        {
            droneControllerUI.SetActive(false);
        }
    }
}
