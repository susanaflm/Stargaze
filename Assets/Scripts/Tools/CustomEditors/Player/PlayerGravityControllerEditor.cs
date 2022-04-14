#if UNITY_EDITOR
using Stargaze.Mono.Player;
using UnityEditor;
using UnityEngine;

namespace Stargaze.Tools.CustomEditors.Player
{
    [CustomEditor(typeof(PlayerGravityController))]
    public class PlayerGravityControllerEditor : Editor
    {
        private PlayerGravityController _controller;

        private void OnEnable()
        {
            _controller = (PlayerGravityController)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            GUILayout.Space(15f);
            
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Turn Gravity On"))
                _controller.SetGravity(true);
            
            if (GUILayout.Button("Turn Gravity Off"))
                _controller.SetGravity(false);
            
            GUILayout.EndHorizontal();
        }
    }
}
#endif