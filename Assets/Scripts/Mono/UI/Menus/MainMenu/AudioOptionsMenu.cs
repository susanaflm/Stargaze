using System;
using Stargaze.Mono.Managers;
using Stargaze.ScriptableObjects.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class AudioOptionsMenu : MonoBehaviour
    {
        private InputAction _goBackAction;
        
        public Action OnMenuQuit;

        [SerializeField] private AudioSettingsData settingsObject;

        [Space]
        
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider uiVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider comsVolumeSlider;
        
        [Space]
        
        [SerializeField] private TMP_Text masterVolumeValue;
        [SerializeField] private TMP_Text uiVolumeValue;
        [SerializeField] private TMP_Text sfxVolumeValue;
        [SerializeField] private TMP_Text comsVolumeValue;
        
        private void Awake()
        {
            _goBackAction = new InputAction(binding: "<Keyboard>/escape"); // TODO: How can I add a second binding for gamepad?

            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeValueChanged);
            uiVolumeSlider.onValueChanged.AddListener(OnUIVolumeValueChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeValueChanged);
            comsVolumeSlider.onValueChanged.AddListener(OnComsVolumeValueChanged);
            
            _goBackAction.performed += _ =>
            {
                OnMenuQuit?.Invoke();
            };
        }

        private void OnMasterVolumeValueChanged(float value)
        {
            masterVolumeValue.text = $"{value * 100:F1}";
        }
        
        private void OnUIVolumeValueChanged(float value)
        {
            uiVolumeValue.text = $"{value * 100:F1}";
        }
        
        private void OnSFXVolumeValueChanged(float value)
        {
            sfxVolumeValue.text = $"{value * 100:F1}";
        }
        
        private void OnComsVolumeValueChanged(float value)
        {
            comsVolumeValue.text = $"{value * 100:F1}";
        }

        public void OnApplyButtonPressed()
        {
            settingsObject.MasterVolume = masterVolumeSlider.value;
            settingsObject.UIVolume = uiVolumeSlider.value;
            settingsObject.SFXVolume = sfxVolumeSlider.value;
            settingsObject.ComsVolume = comsVolumeSlider.value;
            
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
            masterVolumeSlider.value = settingsObject.MasterVolume;
            uiVolumeSlider.value = settingsObject.UIVolume;
            sfxVolumeSlider.value = settingsObject.SFXVolume;
            comsVolumeSlider.value = settingsObject.ComsVolume;
            
            masterVolumeValue.text = $"{masterVolumeSlider.value * 100:F1}";
            uiVolumeValue.text = $"{uiVolumeSlider.value * 100:F1}";
            sfxVolumeValue.text = $"{sfxVolumeSlider.value * 100:F1}";
            comsVolumeValue.text = $"{comsVolumeSlider.value * 100:F1}";
            
            _goBackAction.Enable();
        }

        private void OnDisable()
        {
            _goBackAction.Disable();
        }
    }
}