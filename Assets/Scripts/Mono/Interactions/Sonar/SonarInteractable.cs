using UnityEngine;

namespace Stargaze.Mono.Interactions.Sonar
{
    public class SonarInteractable : MonoBehaviour, IInteractable
    {
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;
        
        [SerializeField] private GameObject sonarUI;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;
        
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
