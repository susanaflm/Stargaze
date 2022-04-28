using UnityEngine;
using UnityEngine.UI;

namespace Stargaze.Mono.Interactions.Book
{
    public class BookInteractable : MonoBehaviour, IInteractable
    {
        private bool _isInteractable = true;
        
        [SerializeField] private bool switchable;

        [Header("Pages Images")]
        [SerializeField] private Sprite page1;
        [SerializeField] private Sprite page2;
        [Header("UI")]
        [SerializeField] private GameObject bookUI;
        [SerializeField] private Image page1UI;
        [SerializeField] private Image page2UI;

        public bool Switchable => switchable;
        
        public bool IsInteractable => _isInteractable;
        
        public void OnInteractionStart()
        {
            page1UI.sprite = page1;
            page2UI.sprite = page2;
            
            bookUI.SetActive(true);
        }

        public void OnInteractionEnd()
        {
            page1UI.sprite = null;
            page2UI.sprite = null;
            
            bookUI.SetActive(false);
        }
    }
}
