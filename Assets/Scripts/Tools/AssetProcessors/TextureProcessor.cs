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
                if (importer.assetPath.ToUpper().Contains("NORMAL"))
                {
                    importer.textureType = TextureImporterType.NormalMap;
                }
            }
        }
    }
}
#endif
