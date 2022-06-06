using System;
using Stargaze.Mono.Managers;
using Stargaze.ScriptableObjects.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class GameOptionsMenu : MonoBehaviour
    {
        private InputAction _goBackAction;

        [SerializeField] private GameSettingsData settingsObject;
        
        [Space]
        
        [SerializeField] private Slider cameraSensitivitySlider;
        [SerializeField] private Slider magnetSensitivitySlider;
        [SerializeField] private Slider electricalSensitivitySlider;

        [Space]
        
        [SerializeField] private TMP_Text cameraSensitivityValue;
        [SerializeField] private TMP_Text magnetSensitivityValue;
        [SerializeField] private TMP_Text electricalSensitivityValue;
        
        public Action OnMenuQuit;
        
        private void Awake()
        {
            _goBackAction = new InputAction(binding: "<Keyboard>/escape"); // TODO: How can I add a second binding for gamepad?

            cameraSensitivitySlider.onValueChanged.AddListener(OnCameraSensitivityValueChanged);
            magnetSensitivitySlider.onValueChanged.AddListener(OnMagnetSensitivityValueChanged);
            electricalSensitivitySlider.onValueChanged.AddListener(OnElectricalSensitivityValueChanged);
            
            _goBackAction.performed += _ =>
            {
                OnMenuQuit?.Invoke();
            };
        }
        
        private void OnCameraSensitivityValueChanged(float value)
        {
            cameraSensitivityValue.text = $"{value:F1}";
        }
        
        private void OnMagnetSensitivityValueChanged(float value)
        {
            magnetSensitivityValue.text = $"{value:F1}";
        }

        private void OnElectricalSensitivityValueChanged(float value)
        {
            electricalSensitivityValue.text = $"{value:F1}";
        }

        public void OnApplyButtonPressed()
        {
            settingsObject.CameraSensitivity = cameraSensitivitySlider.value;
            settingsObject.MagnetSensitivity = magnetSensitivitySlider.value;
            settingsObject.EletricalSensitivity = electricalSensitivitySlider.value;
            
            SettingsManager.Instance.ApplySettings(true);
        }
        
        public void OnGoBackButtonPressed()
        {
            OnMenuQuit?.Invoke();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        private void OnEnable()
        {
            cameraSensitivitySlider.value = settingsObject.CameraSensitivity;
            magnetSensitivitySlider.value = settingsObject.MagnetSensitivity;
            electricalSensitivitySlider.value = settingsObject.EletricalSensitivity;
            
            cameraSensitivityValue.text = $"{cameraSensitivitySlider.value:F1}";
            magnetSensitivityValue.text = $"{magnetSensitivitySlider.value:F1}";
            electricalSensitivityValue.text = $"{electricalSensitivitySlider.value:F1}";
            
            _goBackAction.Enable();
        }

        private void OnDisable()
        {
            _goBackAction.Disable();
        }
    }
}