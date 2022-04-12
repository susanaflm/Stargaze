using System;
using Stargaze.ScriptableObjects.Materials;
using UnityEngine;

namespace Stargaze.ScriptableObjects.Crafting
{
    [CreateAssetMenu(fileName = "CraftRecipes", menuName = "FuelComponentsRecipes")]
    public class CraftRecipes : ScriptableObject
    {
        public Recipe[] recipes;
    }

    [Serializable]
    public struct Recipe
    {
        public ResourceMaterial matA;
        public ResourceMaterial matB;
        
        [Space]
        public ResourceMaterial result;
    }

}
