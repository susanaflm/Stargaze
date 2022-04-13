using UnityEngine;

namespace Stargaze.ScriptableObjects.Materials
{
    [CreateAssetMenu(fileName = "Material", menuName = "ResourceMat/Material")]
    public class ResourceMaterial : ScriptableObject
    {
        [Tooltip("Image to be used when crafting the fuel")]
        [SerializeField] private Sprite matImage;

        public Sprite GetSprite() => matImage;
    }
}
