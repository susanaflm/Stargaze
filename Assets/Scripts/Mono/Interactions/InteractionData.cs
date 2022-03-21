using UnityEngine;

namespace Stargaze.Mono.Interactions
{
    [CreateAssetMenu(fileName = "InteractionData", menuName = "InteractionSystem/InteractionData")]
    public class InteractionData : ScriptableObject
    {
        private Interactable _interactable;

        public Interactable Interactable
        {
            get => _interactable;
            set => _interactable = value;
        }

        public void Interact()
        {
            _interactable.OnInteraction();
            ResetInteractable();
        }

        public bool IsSameInteractable(Interactable otherInteractable)
        {
            return _interactable == otherInteractable;
        }

        public bool IsEmpty()
        {
            return _interactable == null;
        }

        public void ResetInteractable()
        {
            _interactable = null;
        }
    }
}
