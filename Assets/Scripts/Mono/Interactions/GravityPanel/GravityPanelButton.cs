using Stargaze.Mono.Puzzle;
using UnityEngine;
using UnityEngine.UI;

namespace Stargaze.Mono.Interactions.GravityPanel
{
    public class GravityPanelButton : Interactable
    {
        public delegate void ButtonPressed(char buttonInput);

        public static ButtonPressed SendButtonInput;
        
        [SerializeField] private char buttonInput;

        public override void OnInteractionStart()
        {
            base.OnInteractionStart();
            
            //TODO: Animate Button
            
            //If the gravity puzzle is completed the player will no longer be able to interact with the gravity
            if (PuzzleManager.Instance.GravityPuzzleStatus)
            {
                return;
            }
            
            SendButtonInput?.Invoke(buttonInput);
        }
    }
}
