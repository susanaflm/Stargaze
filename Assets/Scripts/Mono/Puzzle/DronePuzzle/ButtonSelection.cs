using UnityEngine;
using UnityEngine.UI;

namespace Stargaze.Mono.Puzzle.DronePuzzle
{
    public class ButtonSelection : MonoBehaviour
    {
        [SerializeField] private Button firstButton;

        private void OnEnable()
        {
            firstButton.Select();
        }
    }
}
