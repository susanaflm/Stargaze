using System;
using Stargaze.ScriptableObjects.Materials;
using TMPro;
using UnityEngine;

namespace Stargaze.Mono.UI.FuelMixingPuzzle
{
    public class CraftingResource : MonoBehaviour
    {
        public delegate void OnButtonClick(ResourceMaterial resourceMaterial);

        public static OnButtonClick OnAddToRecipe;

        private ResourceMaterial _resource;
        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void AddToRecipe()
        {
            _source.Play();
            OnAddToRecipe?.Invoke(_resource);
        }
        
        public void SetResource(ResourceMaterial resourceMaterial)
        {
            _resource = resourceMaterial;
        }

        private void OnDisable()
        {
            Destroy(gameObject);
        }
    }
}
