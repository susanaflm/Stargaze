using Mirror;
using UnityEngine;

namespace Stargaze.ScriptableObjects.Materials
{
    [CreateAssetMenu(fileName = "Material", menuName = "Resource Material", order = -11)]
    public class ResourceMaterial : ScriptableObject
    {
        public string Name;
        public bool IsFinalProduct;

        [Tooltip("Image to be used when crafting the fuel")]
        public Sprite Sprite;
    }

    public static class ResourceMaterialSerializer
    {
        public static void WriteResourceMaterial(this NetworkWriter writer, ResourceMaterial material)
        {
            writer.WriteString(material != null ? material.name : "");
        }
        
        public static ResourceMaterial ReadResourceMaterial(this NetworkReader reader)
        {
            string materialName = reader.ReadString();

            if (string.IsNullOrEmpty(materialName))
                return null;
            
            ResourceMaterial material = Resources.Load<ResourceMaterial>($"ScriptableObjects/Materials/{materialName}");
            
            return material;
        }
    }
}
