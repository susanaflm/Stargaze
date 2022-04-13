using System;
using Stargaze.Mono.UI.FuelMixingPuzzle;
using Stargaze.ScriptableObjects.Crafting;
using Stargaze.ScriptableObjects.Materials;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Stargaze.Mono.Puzzle.FuelMixturePuzzle
{
    public class FuelCrafting : MonoBehaviour
    {
        [Header("Ui Positions")]
        [SerializeField] private GameObject slot1;
        [SerializeField] private GameObject slot2;
        [SerializeField] private GameObject resultSlot;
        [Space]
        [Header("Crafting Recipes")]
        [SerializeField] private CraftRecipes recipes;

        [Header("Resource Material Prefab")]
        [SerializeField] private GameObject matPrefab;

        [SerializeField] private Button fabricateButton;

        private ResourceMaterial _matA;
        private ResourceMaterial _matB;

        private float _craftTimer = 0.0f;
        
        private void Update()
        {
            if (_craftTimer > 0)
            {
                _craftTimer -= Time.deltaTime;
                fabricateButton.interactable = false;
            }
            else
            {
                fabricateButton.interactable = true;

                if (resultSlot.transform.childCount != 0)
                {
                    var child = resultSlot.transform.GetChild(0);
                    Destroy(child.gameObject);
                    
                    fabricateButton.Select();
                }
            }
        }

        private void AssignCraftingComponent(ResourceMaterial r)
        {
            if (_matA == null)
            {
                _matA = r;
                var m = Instantiate(matPrefab, slot1.transform);
                m.GetComponent<Image>().sprite = r.GetSprite();
            }
            else if (_matA != null && _matB == null)
            {
                _matB = r;
                var m = Instantiate(matPrefab, slot2.transform);
                m.GetComponent<Image>().sprite = r.GetSprite();
            }
            else
            {
                //TODO: Throw a sound and a warning that the player can't add more crafting components
#if DEBUG
                Debug.Log("Maximum number of components achieved");       
#endif
            }
        }

        private void TryCraft(ResourceMaterial matA, ResourceMaterial matB)
        {
            if (matA == null || matB == null)
            {
                //TODO: Trigger a Sound and a show a warning that crafting components are missing
#if DEBUG
                Debug.Log("Crafting Failed! Missing components");       
#endif
            }
            
            foreach (var recipe in recipes.recipes)
            {
                if ((recipe.matA == matA && recipe.matB == matB) || (recipe.matA == matB && recipe.matB == matA))
                {
                    ResourceMaterial result = recipe.result;

                    _matA = null;
                    _matB = null;
                    //TODO: Trigger a little Animation and a sound
                    //TODO: Spawn a component at the output of the machine so the player can collect it and use it
                    
                    var m = Instantiate(matPrefab, resultSlot.transform);
                    m.GetComponent<Image>().sprite = result.GetSprite();

#if DEBUG
                    Debug.Log($"Crating Success! Crafted: {result.name}");
#endif
                    var child = slot1.transform.GetChild(0);
                    Destroy(child.gameObject);
                    child = slot2.transform.GetChild(0);
                    Destroy(child.gameObject);

                    _craftTimer = 5.0f;
                    return;
                }
            }
            
            //The Crafting Recipe doesn't exist
            //TODO: Warn The player with a sound 
#if DEBUG
            Debug.Log("Crafting Failed! Not Possible to combine these");
#endif
            _matA = null;
            _matB = null;
            
            var child1 = slot1.transform.GetChild(0);
            Destroy(child1.gameObject);
            child1 = slot2.transform.GetChild(0);
            Destroy(child1.gameObject);
        }

        private void OnEnable()
        {
            CraftingResource.OnAddToRecipe += AssignCraftingComponent;
        }

        private void OnDisable()
        {
            CraftingResource.OnAddToRecipe -= AssignCraftingComponent;
        }

        public void OnButtonClick()
        {
            TryCraft(_matA,_matB);
        }
    }
}
