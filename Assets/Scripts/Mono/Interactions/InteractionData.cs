using UnityEngine;

namespace Stargaze.Mono.Interactions
{
    [CreateAssetMenu(fileName = "InteractionData", menuName = "InteractionSystem/InteractionData")]
    public class InteractionData : ScriptableObject
    {
        private IInteractable _interactable;

        public IInteractable Interactable
        {
            get => _interactable;
            set => _interactable = value;
        }

        public void Interact()
        {
            _interactable.OnInteractionStart();
            ResetInteractable();
        }

        public bool IsSameInteractable(IInteractable otherInteractable)
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
