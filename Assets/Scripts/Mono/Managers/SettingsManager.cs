using System;
using Stargaze.ScriptableObjects.Settings;
using UnityEngine;
using UnityEngine.Audio;

namespace Stargaze.Mono.Managers
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance;

        private const string SettingsFileName = "settings.json";

        [SerializeField] private SettingsData settingsData;

        [Space]
        
        [SerializeField] private AudioMixer audioMixer;

        public SettingsData SettingsData => settingsData;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            settingsData.Load($"{Application.persistentDataPath}/{SettingsFileName}");
            
            ApplySettings();
        }

        public void ApplySettings(bool save = false)
        {
            VideoSettingsData videoSettingsData = settingsData.VideoSettings;
            
            Screen.SetResolution(videoSettingsData.ResolutionWidth, videoSettingsData.ResolutionHeight, videoSettingsData.Fullscreen, videoSettingsData.RefreshRate);
            QualitySettings.vSyncCount = videoSettingsData.VSync ? 1 : 0;
            QualitySettings.SetQualityLevel(videoSettingsData.GraphicsPreset);

            AudioSettingsData audioSettingsData = settingsData.AudioSettings;

            audioMixer.SetFloat("MasterVolume", audioSettingsData.MasterVolume > 0 ? -20 + (20 * audioSettingsData.MasterVolume) : -80);
            audioMixer.SetFloat("UIVolume", audioSettingsData.UIVolume > 0 ? -20 + (20 * audioSettingsData.UIVolume) : -80);
            audioMixer.SetFloat("EffectsVolume", audioSettingsData.SFXVolume > 0 ? -20 + (20 * audioSettingsData.SFXVolume) : -80);
            audioMixer.SetFloat("ComsVolume", audioSettingsData.ComsVolume > 0 ? -20 + (20 * audioSettingsData.ComsVolume) : -80);
            
            if (save)
                settingsData.Save($"{Application.persistentDataPath}/{SettingsFileName}");
        }
    }
}