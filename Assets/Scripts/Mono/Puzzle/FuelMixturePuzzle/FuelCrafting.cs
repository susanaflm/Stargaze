using Mirror;
using Stargaze.Mono.Interactions.Vial;
using Stargaze.Mono.UI.FuelMixingPuzzle;
using Stargaze.ScriptableObjects.Crafting;
using Stargaze.ScriptableObjects.Materials;
using UnityEngine;
using UnityEngine.UI;

namespace Stargaze.Mono.Puzzle.FuelMixturePuzzle
{
    public class FuelCrafting : NetworkBehaviour
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
        [Space]
        [SerializeField] private Button fabricateButton;
        [Space]
        [Header("Output")]
        [SerializeField] private Transform outputPos;
        [SerializeField] private GameObject vialPrefab;

        [SyncVar(hook = nameof(MaterialAChangedCallback))]
        private ResourceMaterial _matA;
        [SyncVar(hook = nameof(MaterialBChangedCallback))]
        private ResourceMaterial _matB;

        [SyncVar]
        private float _craftTimer = 0.0f;

        [SyncVar(hook = nameof(RpcResultMaterialChangedCallback))]
        private ResourceMaterial _resultMaterial;
        
        private void Update()
        {
            HandleCraftTime();
        }
        
        [ServerCallback]
        private void HandleCraftTime()
        {
            if (_craftTimer > 0)
            {
                _craftTimer -= Time.deltaTime;
                
                RpcSetFabricateButtonInteractable(false);
            }
            else
            {
                RpcSetFabricateButtonInteractable(true);

                if (_resultMaterial == null)
                    return;
                
                GameObject vialObject = Instantiate(vialPrefab, outputPos.position, Quaternion.identity);

                foreach (Component comp in vialObject.GetComponents<Component>())
                {
                    if (comp is Rigidbody)
                    {
                        Destroy(comp);
                    }
                }
                
                vialObject.GetComponent<VialInteractable>().SetResource(_resultMaterial);
                NetworkServer.Spawn(vialObject);

                RpcSelectFabricateButton();
                GetComponent<AudioSource>().Play();
                
                _resultMaterial = null;
            }
        }

        [ClientRpc]
        private void RpcSetFabricateButtonInteractable(bool interactable)
        {
            fabricateButton.interactable = interactable;
        }

        [ClientRpc]
        private void RpcSelectFabricateButton()
        {
            fabricateButton.Select();
        }

        private void AssignCraftingComponent(ResourceMaterial material)
        {
            CmdAssignCraftingComponent(material);
        }

        [Command(requiresAuthority = false)]
        private void CmdAssignCraftingComponent(ResourceMaterial material)
        {
            if (_matA == null)
            {
                _matA = material;
            }
            else if (_matA != null && _matB == null)
            {
                _matB = material;
            }
            else
            {
                //TODO: Throw a sound and a warning that the player can't add more crafting components
#if DEBUG
                Debug.Log("Maximum number of components achieved");       
#endif
            }
        }

        private void MaterialAChangedCallback(ResourceMaterial oldMaterial, ResourceMaterial newMaterial)
        {
            if (newMaterial != null)
            {
                GameObject obj = Instantiate(matPrefab, slot1.transform);
                obj.GetComponent<Image>().sprite = newMaterial.Sprite;
            }
            else
            {
                foreach (Transform child in slot1.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        
        private void MaterialBChangedCallback(ResourceMaterial oldMaterial, ResourceMaterial newMaterial)
        {
            if (newMaterial != null)
            {
                GameObject obj = Instantiate(matPrefab, slot2.transform);
                obj.GetComponent<Image>().sprite = newMaterial.Sprite;
            }
            else
            {
                foreach (Transform child in slot2.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        private void RpcResultMaterialChangedCallback(ResourceMaterial oldMaterial, ResourceMaterial newMaterial)
        {
            if (_resultMaterial != null)
            {
                GameObject obj = Instantiate(matPrefab, resultSlot.transform);
                obj.GetComponent<Image>().sprite = newMaterial.Sprite;
            }
            else
            {
                foreach (Transform child in resultSlot.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdTryCraft()
        {
            if (_matA == null || _matB == null)
            {
                //TODO: Trigger a Sound and a show a warning that crafting components are missing
#if DEBUG
                Debug.Log("Crafting Failed! Missing components");       
#endif
            }
            
            foreach (var recipe in recipes.recipes)
            {
                if ((recipe.matA == _matA && recipe.matB == _matB) || (recipe.matA == _matB && recipe.matB == _matA))
                {
                    ResourceMaterial result = recipe.result;
                    
                    //TODO: Trigger a little Animation and a sound

#if DEBUG
                    Debug.Log($"Crating Success! Crafted: {result.name}");
#endif
                    
                    _matA = null;
                    _matB = null;
                    
                    _resultMaterial = result;
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
            CmdTryCraft();
        }
    }
}
