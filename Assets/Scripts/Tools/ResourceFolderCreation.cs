#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Stargaze.Tools
{
    public class CreateResourceFolderWindow : EditorWindow
    {
        private string _folderName = "";
        
        [MenuItem("Tools/Custom/Create Resource Folder")]
        [MenuItem("Assets/Create/Resource Folder", false, 21)]
        static void Init()
        {
            var window = (CreateResourceFolderWindow) GetWindow(typeof(CreateResourceFolderWindow));
            
            window.titleContent.text = "Create Resource Folder";
            
            window.Show();
        }
        
        private void OnGUI()
        {
            GUILayout.Space(10f);
            
            GUILayout.Label("Folder Name");
            GUILayout.Space(5f);
            _folderName = GUILayout.TextField(_folderName);
            
            GUILayout.Space(15f);

            if (GUILayout.Button("Create", GUILayout.Height(40)))
            {
                CreateFolder();                
            }
        }

        private void CreateFolder()
        {
            string basePath = "Assets/Resources";

            if (ProjectFolderHelper.GetOpenFolderDirectory(out string path))
            {
                basePath = path;
            }
            
            if (Directory.Exists($"{basePath}/{_folderName}"))
                return;
            
            // Create base folder
            AssetDatabase.CreateFolder(basePath, _folderName);
            
            // Create sub-folders
            AssetDatabase.CreateFolder($"{basePath}/{_folderName}", "Materials");
            AssetDatabase.CreateFolder($"{basePath}/{_folderName}", "Models");
            AssetDatabase.CreateFolder($"{basePath}/{_folderName}", "Textures");
        }
    }
}
#endif
