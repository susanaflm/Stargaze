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

        private SerializedProperty _groundCheckCenter;
        private SerializedProperty _groundCheckRadius;
        private SerializedProperty _groundCheckLayer;

        private void OnEnable()
        {
            _movementSpeed = serializedObject.FindProperty("movementSpeed");

            _camera = serializedObject.FindProperty("camera");
            _rotationSpeed = serializedObject.FindProperty("rotationSpeed");
            _verticalRotationLowerBound = serializedObject.FindProperty("verticalRotationLowerBound");
            _verticalRotationUpperBound = serializedObject.FindProperty("verticalRotationUpperBound");

            _groundCheckCenter = serializedObject.FindProperty("groundCheckCenter");
            _groundCheckRadius = serializedObject.FindProperty("groundCheckRadius");
            _groundCheckLayer = serializedObject.FindProperty("groundCheckLayer");
        }

        public override void OnInspectorGUI()
        {
            float verticalRotationLowerBoundValue = _verticalRotationLowerBound.floatValue;
            float verticalRotationUpperBoundValue = _verticalRotationUpperBound.floatValue;
            
            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true);
            
            EditorGUILayout.PropertyField(_movementSpeed);
            
            EditorGUILayout.PropertyField(_camera);
            EditorGUILayout.PropertyField(_rotationSpeed);
            
            EditorGUILayout.LabelField($"Vertical Rotation Bounds: [{verticalRotationLowerBoundValue:F2}, {verticalRotationUpperBoundValue:F2}]");
            EditorGUILayout.MinMaxSlider(
                ref verticalRotationLowerBoundValue, 
                ref verticalRotationUpperBoundValue, 
                -90, 
                90
            );

            _verticalRotationLowerBound.floatValue = verticalRotationLowerBoundValue;
            _verticalRotationUpperBound.floatValue = verticalRotationUpperBoundValue;

            EditorGUILayout.PropertyField(_groundCheckCenter);
            EditorGUILayout.PropertyField(_groundCheckRadius);
            EditorGUILayout.PropertyField(_groundCheckLayer);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
