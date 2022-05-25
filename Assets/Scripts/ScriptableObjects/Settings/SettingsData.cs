using System.IO;
using UnityEngine;

namespace Stargaze.ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "SettingsData", menuName = "Settings Data Object", order = -45)]
    public class SettingsData : ScriptableObject
    {
        public GameSettingsData GameSettings;
        public VideoSettingsData VideoSettings;
        public AudioSettingsData AudioSettings;

        public void Load(string filePath)
        {
            Settings settings;
            
            if (File.Exists(filePath))
            {
                settings = JsonUtility.FromJson<Settings>(File.ReadAllText(filePath));
            }
            else
            {
                settings = new Settings()
                {
                    GameSettings = new GameSettings(1f, 1f, 1f),
                    AudioSettings = new AudioSettings(1f, 1f, 1f),
                    VideoSettings = new VideoSettings(
                        Screen.currentResolution.width,
                        Screen.currentResolution.height,
                        Screen.currentResolution.refreshRate
                    )
                };
            }

            GameSettings.Apply(settings.GameSettings);
            VideoSettings.Apply(settings.VideoSettings);
            AudioSettings.Apply(settings.AudioSettings);
                
            Save(filePath);
        }

        public void Save(string filePath)
        {
            Settings settings = new Settings()
            {
                GameSettings = GameSettings.GetData(),
                VideoSettings = VideoSettings.GetData(),
                AudioSettings = AudioSettings.GetData()
            };
            
            string settingJSON = JsonUtility.ToJson(settings, true);
            
            File.WriteAllText(filePath, settingJSON);
        }
    }

    [SerializeField]
    public struct Settings
    {
        public GameSettings GameSettings;
        public VideoSettings VideoSettings;
        public AudioSettings AudioSettings;
    }
}