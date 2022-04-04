#if UNITY_EDITOR
using System;
using UnityEditor;

namespace Stargaze.Tools.AssetProcessors
{
    public class TextureProcessor : AssetPostprocessor
    {
        private void OnPreprocessTexture()
        {
            TextureImporter importer = assetImporter as TextureImporter;
            
            if (importer == null) return;

            if (importer.importSettingsMissing)
            {
                string pathToUpper = importer.assetPath.ToUpper();
                
                if (pathToUpper.Contains("NORMAL"))
                {
                    importer.textureType = TextureImporterType.NormalMap;
                }
                else if (pathToUpper.Contains("SPRITE"))
                {
                    importer.textureType = TextureImporterType.Sprite;
                }
            }
        }
    }
}
#endif
