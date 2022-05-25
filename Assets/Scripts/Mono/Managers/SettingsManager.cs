using System;
using Stargaze.ScriptableObjects.Settings;
using UnityEngine;

namespace Stargaze.Mono.Managers
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance;

        private const string SettingsFileName = "settings.json";

        [SerializeField] private SettingsData settingsData;

        public Action OnSettingsChanged;

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
            
            // TODO: Apply audio
            
            if (save)
                settingsData.Save($"{Application.persistentDataPath}/{SettingsFileName}");
        }
    }
}