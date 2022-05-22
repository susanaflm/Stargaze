using System;
using System.Collections.Generic;
using Stargaze.Mono.Interactions.FuelMixing;
using Stargaze.Mono.Puzzle;
using Stargaze.ScriptableObjects.Materials;
using UnityEngine;
using UnityEngine.UI;

namespace Stargaze.Mono.UI.FuelMixingPuzzle
{
    public class GatheredMaterialUI : MonoBehaviour
    {
        private int _current = 0;
        private int _currentBaseChild;
        private int _currentCraftedChild;
        
        [SerializeField] private GameObject materialPrefab;

        [SerializeField] private GameObject baseMaterials;
        [SerializeField] private GameObject craftedMaterials;
        

        private void FillUiWithGatheredMaterials()
        {
            if (PuzzleManager.Instance.GatheredMaterials.Count == 0)
                return;

            _current = 0;
            _currentBaseChild = 0;
            _currentCraftedChild = 0;

            foreach (var resourceMaterial in PuzzleManager.Instance.GatheredMaterials)
            {
                if (!resourceMaterial.CraftedComponent)
                {
                    var r = Instantiate(materialPrefab, baseMaterials.transform.GetChild(_currentBaseChild).transform);
                    r.GetComponent<Image>().sprite = resourceMaterial.Sprite;
                    r.GetComponent<CraftingResource>().SetResource(resourceMaterial);

                    //Selects one of the components
                    if (_current == 0)
                        r.GetComponent<Button>().Select();

                    _currentBaseChild++;
                }
                else
                {
                    var r = Instantiate(materialPrefab, craftedMaterials.transform.GetChild(_currentCraftedChild).transform);
                    r.GetComponent<Image>().sprite = resourceMaterial.Sprite;
                    r.GetComponent<CraftingResource>().SetResource(resourceMaterial);
                    
                    _currentCraftedChild++;
                }
                
                _current++;
 
            }
        }

        private void OnEnable()
        {
            FuelMixingInteractable.FillUI += FillUiWithGatheredMaterials;
        }

        private void OnDisable()
        {
            FuelMixingInteractable.FillUI -= FillUiWithGatheredMaterials;
        }
    }
}
