using System;
using System.Collections.Generic;
using System.Linq;
using Stargaze.Mono.Managers;
using Stargaze.ScriptableObjects.Settings;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class VideoOptionsMenu : MonoBehaviour
    {
        private InputAction _goBackAction;

        private (int Width, int Height)[] _resolutions;
        private int[] _refreshRates;

        [SerializeField] private VideoSettingsData settingsObject;
        
        [Space]

        [SerializeField] private TMP_Dropdown graphicsPresetDropdown;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown refreshRateDropdown;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle vsyncToggle;
        
        public Action OnMenuQuit;
        
        private void Awake()
        {
            _goBackAction = new InputAction(binding: "<Keyboard>/escape"); // TODO: How can I add a second binding for gamepad?
            
            PopulateDropdowns();
            
            _goBackAction.performed += _ =>
            {
                OnMenuQuit?.Invoke();
            };
        }

        private void PopulateDropdowns()
        {
            List<(int w, int h)> tempResolutions = new();
            List<int> tempRefreshRates = new();
            
            foreach (Resolution resolution in Screen.resolutions)
            {
                if (!tempResolutions.Any(res => res.w == resolution.width && res.h == resolution.height))
                    tempResolutions.Add((resolution.width, resolution.height));
                
                if (tempRefreshRates.All(refreshRate => refreshRate != resolution.refreshRate))
                    tempRefreshRates.Add(resolution.refreshRate);
            }

            tempResolutions.Reverse();
            tempRefreshRates.Reverse();
            
            _resolutions = tempResolutions.ToArray();
            _refreshRates = tempRefreshRates.ToArray();
            
            graphicsPresetDropdown.options.Clear();
            resolutionDropdown.options.Clear();
            refreshRateDropdown.options.Clear();

            graphicsPresetDropdown.AddOptions(QualitySettings.names.ToList());
            
            foreach ((int width, int height) in _resolutions)
                resolutionDropdown.options.Add(new TMP_Dropdown.OptionData($"{width} x {height}"));
            
            foreach (int refreshRate in _refreshRates)
                refreshRateDropdown.options.Add(new TMP_Dropdown.OptionData($"{refreshRate}Hz"));
        }

        public void OnApplyButtonPressed()
        {
            settingsObject.GraphicsPreset = (byte)graphicsPresetDropdown.value;

            (int width, int height) res = _resolutions[resolutionDropdown.value];

            settingsObject.ResolutionWidth = res.width;
            settingsObject.ResolutionHeight = res.height;

            settingsObject.RefreshRate = _refreshRates[refreshRateDropdown.value];

            settingsObject.Fullscreen = fullscreenToggle.isOn;
            settingsObject.VSync = vsyncToggle.isOn;
            
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
            graphicsPresetDropdown.value = settingsObject.GraphicsPreset;
            
            resolutionDropdown.value = Array.IndexOf(_resolutions, (settingsObject.ResolutionWidth, settingsObject.ResolutionHeight));
            refreshRateDropdown.value = Array.IndexOf(_refreshRates, settingsObject.RefreshRate);

            fullscreenToggle.isOn = settingsObject.Fullscreen;
            vsyncToggle.isOn = settingsObject.VSync;
            
            _goBackAction.Enable();
        }

        private void OnDisable()
        {
            _goBackAction.Disable();
        }
    }
}