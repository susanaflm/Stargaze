using Stargaze.Mono.Puzzle;
using Stargaze.ScriptableObjects.Materials;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Interactions.Vial
{
    public class VialInteractable : MonoBehaviour, IInteractable
    {

        [SerializeField]
        private ResourceMaterial vialMaterial;
        
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;
        
        public void OnInteractionStart()
        {
            PuzzleManager.Instance.GatheredMaterials.Add(vialMaterial);
            Destroy(gameObject);
        }

        public void OnInteractionEnd()
        {
            
        }

        public void SetResource(ResourceMaterial rm)
        {
            vialMaterial = rm;
        }
    }
}
