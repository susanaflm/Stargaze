using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Stargaze.Mono.UI.CustomElements
{
    public class Knob : MonoBehaviour, IDragHandler
    {
        public Action<float> OnValueChanged;

        float _value = 0.0f;

        [Space]
        
        [Required]
        [SerializeField] private RectTransform rectTransform;
        
        [Space]
        
        [SerializeField] private float sensitivity = 0.1f;
        
        [Space]
        
        [MinMaxSlider(-180f, 180f)]
        [SerializeField] private Vector2 range;

        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                UpdateRotation();
            }
        }

        private void Awake()
        {
            if (rectTransform == null)
                Debug.LogError($"Missing Rect Transform reference in {name}");
            
            UpdateRotation();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _value += eventData.delta.x * sensitivity;

            _value = Mathf.Clamp(_value, 0f, 1f);
            
            UpdateRotation();
            
            OnValueChanged?.Invoke(_value);
        }

        private void UpdateRotation()
        {
            rectTransform.rotation = Quaternion.Euler(
                0f,
                0f,
                range.x + (Mathf.Abs(range.x) + Mathf.Abs(range.y)) * (1f - _value)
            );
        }
    }
}