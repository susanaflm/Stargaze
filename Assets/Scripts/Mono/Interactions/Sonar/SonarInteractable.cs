using Mirror;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Sonar
{
    public class SonarInteractable : NetworkBehaviour, IInteractable
    {
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;
        
        [SerializeField] private GameObject sonarUI;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;
        
        public override void OnStartClient()
        {
            sonarUI.SetActive(false);
        }
        
        public void OnInteractionStart()
        {
            sonarUI.SetActive(true);
        }

        public void OnInteractionEnd()
        {
            sonarUI.SetActive(false);
        }
    }
}
