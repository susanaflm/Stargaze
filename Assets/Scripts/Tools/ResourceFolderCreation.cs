#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Stargaze.Tools
{
    public class CreateResourceFolderWindow : EditorWindow
    {
        private string _folderName = "";

        private bool _createModelsFolder = true;
        private bool _createTexturesFolder = true;
        private bool _createMaterialsFolder = true;
        private bool _createAnimationsFolder = false;
        private bool _createSoundsFolder = false;
        private bool _createSpritesFolder = false;
        
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
            
            GUILayout.Label("Sub Folders");
            
            GUILayout.Space(5f);
            
            GUILayout.BeginHorizontal();
            _createModelsFolder = GUILayout.Toggle(_createModelsFolder, "Models");
            if (GUILayout.Button("Add Models Folder")) AddModelsFolder();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(2f);
            
            GUILayout.BeginHorizontal();
            _createTexturesFolder = GUILayout.Toggle(_createTexturesFolder, "Textures");
            if (GUILayout.Button("Add Textures Folder")) AddTexturesFolder();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(2f);
            
            GUILayout.BeginHorizontal();
            _createMaterialsFolder = GUILayout.Toggle(_createMaterialsFolder, "Materials");
            if (GUILayout.Button("Add Materials Folder")) AddMaterialsFolder();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(2f);
            
            GUILayout.BeginHorizontal();
            _createAnimationsFolder = GUILayout.Toggle(_createAnimationsFolder, "Animations");
            if (GUILayout.Button("Add Animations Folder")) AddAnimationsFolder();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(2f);
            
            GUILayout.BeginHorizontal();
            _createSoundsFolder = GUILayout.Toggle(_createSoundsFolder, "Sounds");
            if (GUILayout.Button("Add Sounds Folder")) AddSoundsFolder();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(2f);
            
            GUILayout.BeginHorizontal();
            _createSoundsFolder = GUILayout.Toggle(_createSoundsFolder, "Sprites");
            if (GUILayout.Button("Add Sprites Folder")) AddSpritesFolder();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(15f);

            if (GUILayout.Button("Create", GUILayout.Height(25f)))
                CreateFolder();                
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
            if (_createModelsFolder)
                AssetDatabase.CreateFolder($"{basePath}/{_folderName}", "Models");
            
            if (_createTexturesFolder)
                AssetDatabase.CreateFolder($"{basePath}/{_folderName}", "Textures");
            
            if (_createMaterialsFolder)
                AssetDatabase.CreateFolder($"{basePath}/{_folderName}", "Materials");
            
            if (_createAnimationsFolder)
                AssetDatabase.CreateFolder($"{basePath}/{_folderName}", "Animations");
            
            if (_createSoundsFolder)
                AssetDatabase.CreateFolder($"{basePath}/{_folderName}", "Sounds");
            
            if (_createSpritesFolder)
                AssetDatabase.CreateFolder($"{basePath}/{_folderName}", "Sprites");
        }

        private void AddModelsFolder()
        {
            if (ProjectFolderHelper.GetOpenFolderDirectory(out string path))
            {
                AssetDatabase.CreateFolder($"{path}", "Models");
            }
        }
        
        private void AddTexturesFolder()
        {
            if (ProjectFolderHelper.GetOpenFolderDirectory(out string path))
            {
                AssetDatabase.CreateFolder($"{path}", "Textures");
            }
        }
        
        private void AddMaterialsFolder()
        {
            if (ProjectFolderHelper.GetOpenFolderDirectory(out string path))
            {
                AssetDatabase.CreateFolder($"{path}", "Materials");
            }
        }
        
        private void AddAnimationsFolder()
        {
            if (ProjectFolderHelper.GetOpenFolderDirectory(out string path))
            {
                AssetDatabase.CreateFolder($"{path}", "Animations");
            }
        }
        
        private void AddSoundsFolder()
        {
            if (ProjectFolderHelper.GetOpenFolderDirectory(out string path))
            {
                AssetDatabase.CreateFolder($"{path}", "Sounds");
            }
        }
        
        private void AddSpritesFolder()
        {
            if (ProjectFolderHelper.GetOpenFolderDirectory(out string path))
            {
                AssetDatabase.CreateFolder($"{path}", "Sprites");
            }
        }
    }
}
#endif
