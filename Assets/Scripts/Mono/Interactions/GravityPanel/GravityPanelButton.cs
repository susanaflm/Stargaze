using Stargaze.Mono.Puzzle;
using UnityEngine;
using UnityEngine.UI;

namespace Stargaze.Mono.Interactions.GravityPanel
{
    public class GravityPanelButton : MonoBehaviour, IInteractable
    {
        public delegate void ButtonPressed(char buttonInput);

        public static ButtonPressed SendButtonInput;
        
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;
        
        [SerializeField] private char buttonInput;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;

        public void OnInteractionStart()
        {
            //TODO: Animate Button
            
            //If the gravity puzzle is completed the player will no longer be able to interact with the gravity
            if (PuzzleManager.Instance.GravityPuzzleStatus)
            {
                return;
            }
            
            SendButtonInput?.Invoke(buttonInput);
        }

        public void OnInteractionEnd() { }
    }
}
