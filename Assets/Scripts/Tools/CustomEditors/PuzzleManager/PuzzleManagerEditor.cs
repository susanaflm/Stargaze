#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Stargaze.Tools.CustomEditors.PuzzleManager
{
    [CustomEditor(typeof(Mono.Puzzle.PuzzleManager))]
    public class PuzzleManagerEditor : Editor
    {
        private Mono.Puzzle.PuzzleManager _manager;

        private void OnEnable()
        {
            _manager = (Mono.Puzzle.PuzzleManager)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            GUILayout.Space(15f);
            
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Add Materials"))
                _manager.AddMaterialsCheat();

            GUILayout.EndHorizontal();
        }
    }
    
}
#endif