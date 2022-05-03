using Mirror;
using UnityEngine;

namespace Stargaze.ScriptableObjects.Materials
{
    [CreateAssetMenu(fileName = "Material", menuName = "Resource Material", order = -11)]
    public class ResourceMaterial : ScriptableObject
    {
        public string Name;

        [Tooltip("Image to be used when crafting the fuel")]
        public Sprite Sprite;
    }

    public static class ResourceMaterialSerializer
    {
        public static void WriteResourceMaterial(this NetworkWriter writer, ResourceMaterial material)
        {
            writer.WriteString(material.name);
        }
        
        public static ResourceMaterial ReadResourceMaterial(this NetworkReader reader)
        {
            string materialName = reader.ReadString();
            
            ResourceMaterial material = Resources.Load<ResourceMaterial>($"ScriptableObjects/Materials/{materialName}");
            
            return material;
        }
    }
}
