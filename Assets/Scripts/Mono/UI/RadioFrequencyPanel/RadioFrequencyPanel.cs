using System;
using Stargaze.Input;
using Stargaze.Mono.Player;
using Stargaze.Mono.UI.CustomElements;
using TMPro;
using UnityEngine;

namespace Stargaze.Mono.UI.RadioFrequencyPanel
{
    public class RadioFrequencyPanel : MonoBehaviour
    {
        public static Action OnRadioFrequencyPanelShow;
        public static Action OnRadioFrequencyPanelHide;
        
        private InputActions _actions;

        [SerializeField] private GameObject content;

        [Header("Fields")]
        [SerializeField] private TMP_Text currentFrequency;
        [SerializeField] private Knob knob;

        [Header("Settings")]
        [SerializeField] private ushort minFrequency = 1000;
        [SerializeField] private ushort maxFrequency = 5000;

        private void Awake()
        {
            _actions = new InputActions();

            _actions.UI.ShowRadioFrequencyPanel.performed += _ =>
            {
                if (content.activeSelf)
                    Hide();
                else
                    Show();
            };

            knob.OnValueChanged += ChangeFrequency;
            
            Hide();
        }

        private void UpdateFrequencyDisplay()
        {
            ushort frequency = PlayerComs.Frequency;

            currentFrequency.text = $"{frequency * 0.1f:F1} Hz";
            
            ushort range = (ushort)(maxFrequency - minFrequency);
            float value = (frequency - minFrequency) / (float)range;

            knob.Value = value;
        }

        private void ChangeFrequency(float value)
        {
            ushort range = (ushort)(maxFrequency - minFrequency);
            PlayerComs.Frequency = (ushort)(minFrequency + (range * value));
            
            UpdateFrequencyDisplay();
        }
        
        private void OnEnable()
        {
            _actions.Enable();
        }

        private void Show()
        {
            content.SetActive(true);
            
            UpdateFrequencyDisplay();
            
            OnRadioFrequencyPanelShow?.Invoke();
        }
        
        private void Hide()
        {
            content.SetActive(false);
            
            OnRadioFrequencyPanelHide?.Invoke();
        }

        private void OnDisable()
        {
            _actions.Disable();
        }
    }
}