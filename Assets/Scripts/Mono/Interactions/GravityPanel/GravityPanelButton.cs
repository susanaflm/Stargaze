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
            
            //TODO:Animate Button
            
            //If the gravity is already deactivated the player can still click on the button but they don't work
            if (PuzzleManager.Instance.WasGravityDeactivated)
            {
                return;
            }
            
            SendButtonInput?.Invoke(buttonInput);
        }
    }
}
