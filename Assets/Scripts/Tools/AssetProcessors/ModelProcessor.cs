#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Stargaze.Tools.AssetProcessors
{
    public class ModelProcessor : AssetPostprocessor
    {
        private void OnPreprocessModel()
        {
            ModelImporter importer = assetImporter as ModelImporter;
            
            if (importer == null) return;

            if (importer.importSettingsMissing)
            {
                importer.materialImportMode = ModelImporterMaterialImportMode.None;
            }
        }

        private void OnPostprocessModel(GameObject g)
        {
            g.transform.rotation = Quaternion.identity;
        }
    }
}
#endif
