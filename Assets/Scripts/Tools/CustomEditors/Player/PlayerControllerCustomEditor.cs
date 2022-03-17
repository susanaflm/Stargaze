#if UNITY_EDITOR
using Stargaze.Mono.Player;
using UnityEditor;

namespace Stargaze.Tools.CustomEditors.Player
{
    [CustomEditor(typeof(PlayerController))]
    public class PlayerControllerCustomEditor : Editor
    {
        private SerializedProperty _movementSpeed;
        
        private SerializedProperty _camera;
        private SerializedProperty _rotationSpeed;
        private SerializedProperty _verticalRotationLowerBound;
        private SerializedProperty _verticalRotationUpperBound;

        private float _verticalRotationLowerBoundValue;
        private float _verticalRotationUpperBoundValue;
        
        private void OnEnable()
        {
            _movementSpeed = serializedObject.FindProperty("movementSpeed");

            _camera = serializedObject.FindProperty("camera");
            _rotationSpeed = serializedObject.FindProperty("rotationSpeed");
            _verticalRotationLowerBound = serializedObject.FindProperty("verticalRotationLowerBound");
            _verticalRotationUpperBound = serializedObject.FindProperty("verticalRotationUpperBound");
        }

        public override void OnInspectorGUI()
        {
            _verticalRotationLowerBoundValue = _verticalRotationLowerBound.floatValue;
            _verticalRotationUpperBoundValue = _verticalRotationUpperBound.floatValue;
            
            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true);
            
            EditorGUILayout.PropertyField(_movementSpeed);
            
            EditorGUILayout.PropertyField(_camera);
            EditorGUILayout.PropertyField(_rotationSpeed);
            
            EditorGUILayout.LabelField($"Vertical Rotation Bounds: [{_verticalRotationLowerBoundValue:F2}, {_verticalRotationUpperBoundValue:F2}]");
            EditorGUILayout.MinMaxSlider(
                ref _verticalRotationLowerBoundValue, 
                ref _verticalRotationUpperBoundValue, 
                -90, 
                90
            );

            _verticalRotationLowerBound.floatValue = _verticalRotationLowerBoundValue;
            _verticalRotationUpperBound.floatValue = _verticalRotationUpperBoundValue;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
