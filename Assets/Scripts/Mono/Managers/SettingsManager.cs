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

            audioMixer.SetFloat("MasterVolume", -20 + (20 * audioSettingsData.MasterVolume));
            audioMixer.SetFloat("UIVolume", -20 + (20 * audioSettingsData.UIVolume));
            audioMixer.SetFloat("EffectsVolume", -20 + (20 * audioSettingsData.SFXVolume));
            audioMixer.SetFloat("ComsVolume", -20 + (20 * audioSettingsData.ComsVolume));
            
            if (save)
                settingsData.Save($"{Application.persistentDataPath}/{SettingsFileName}");
        }
    }
}