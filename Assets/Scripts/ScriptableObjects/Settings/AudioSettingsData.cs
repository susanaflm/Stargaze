using System;
using UnityEngine;

namespace Stargaze.ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "AudioSettingsData", menuName = "Audio Settings Data Object", order = -44)]
    public class AudioSettingsData : ScriptableObject
    {
        public float MasterVolume;
        public float UIVolume;
        public float SFXVolume;

        public void Apply(AudioSettings settings)
        {
            MasterVolume = settings.MasterVolume;
            UIVolume = settings.UIVolume;
            SFXVolume = settings.SFXVolume;
        }

        public AudioSettings GetData()
        {
            return new AudioSettings(MasterVolume, UIVolume, SFXVolume);
        }
    }
    
    [Serializable]
    public struct AudioSettings
    {
        public float MasterVolume;
        public float UIVolume;
        public float SFXVolume;

        public AudioSettings(float masterVolume = 1f, float uiVolume = 1f, float sfxVolume = 1f)
        {
            MasterVolume = masterVolume;
            UIVolume = uiVolume;
            SFXVolume = sfxVolume;
        }
    }
}