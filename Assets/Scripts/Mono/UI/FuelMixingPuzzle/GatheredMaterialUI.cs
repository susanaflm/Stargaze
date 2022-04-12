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
        [SerializeField] private GameObject materialPrefab;
        private int _current = 0;

        private void FillUiWithGatheredMaterials()
        {
            if (PuzzleManager.Instance.GatheredMaterials.Count == 0)
                return;

            foreach (var resourceMaterial in PuzzleManager.Instance.GatheredMaterials)
            {
                var r = Instantiate(materialPrefab, transform);
                r.GetComponent<Image>().sprite = resourceMaterial.GetSprite();
                r.GetComponent<CraftingResource>().SetResource(resourceMaterial);

                //Selects one of the components
                if (_current == 0)
                    r.GetComponent<Button>().Select();
                
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
