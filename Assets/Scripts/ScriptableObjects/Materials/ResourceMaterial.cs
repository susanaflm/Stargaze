using UnityEngine;

namespace Stargaze.ScriptableObjects.Materials
{
    [CreateAssetMenu(fileName = "VeinMaterial", menuName = "ResourceVeinMat/VeinMaterial")]
    public class ResourceMaterial : ScriptableObject
    {
        [Tooltip("Image to be used when crafting the fuel")]
        [SerializeField] private Sprite matImage;
    }
}
