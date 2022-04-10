using UnityEngine;

namespace Stargaze.Mono.Interactions.Sonar
{
    public class SonarInteractable : Interactable
    {
        [SerializeField] private GameObject sonarUI;
        
        public override void OnInteractionStart()
        {
            base.OnInteractionStart();
            
            sonarUI.SetActive(true);
        }

        public override void OnInteractionEnd()
        {
            base.OnInteractionEnd();
            
            sonarUI.SetActive(false);
        }
    }
}
