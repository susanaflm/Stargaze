using UnityEngine;

namespace Stargaze.Mono.Interactions.DroneController
{
    public class DroneControllerInteractable : Interactable
    {
        [SerializeField] private GameObject droneControllerUI;
        
        public override void OnInteractionStart()
        {
            base.OnInteractionStart();
            
            droneControllerUI.SetActive(true);
        }

        public override void OnInteractionEnd()
        {
            base.OnInteractionEnd();
            
            droneControllerUI.SetActive(false);
        }
    }
}
