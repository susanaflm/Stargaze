using Stargaze.ScriptableObjects.Materials;
using TMPro;
using UnityEngine;

namespace Stargaze.Mono.UI.FuelMixingPuzzle
{
    public class CraftingResource : MonoBehaviour
    {
        public delegate void OnButtonClick(ResourceMaterial resourceMaterial);

        public static OnButtonClick OnAddToRecipe;

        [SerializeField] private TMP_Text textBox;
        
        private ResourceMaterial _resource;


        public void AddToRecipe()
        {
            OnAddToRecipe?.Invoke(_resource);
        }
        
        public void SetResource(ResourceMaterial resourceMaterial)
        {
            _resource = resourceMaterial;
            textBox.text = resourceMaterial.name;
        }

        private void OnDisable()
        {
            Destroy(gameObject);
        }
    }
}
