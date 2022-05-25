using System;
using UnityEngine;

namespace Stargaze.ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "VideoSettingsData", menuName = "Video Settings Data Object", order = -44)]
    public class VideoSettingsData : ScriptableObject
    {
        public byte GraphicsPreset;
        public int ResolutionWidth;
        public int ResolutionHeight;
        public int RefreshRate;
        public bool Fullscreen;
        public bool VSync;

        public void Apply(VideoSettings settings)
        {
            GraphicsPreset = settings.GraphicsPreset;
            ResolutionWidth = settings.ResolutionWidth;
            ResolutionHeight = settings.ResolutionHeight;
            RefreshRate = settings.RefreshRate;
            Fullscreen = settings.Fullscreen;
            VSync = settings.VSync;
        }

        public VideoSettings GetData()
        {
            return new VideoSettings(ResolutionWidth, ResolutionHeight, RefreshRate, GraphicsPreset, Fullscreen, VSync);
        }
    }

    [Serializable]
    public struct VideoSettings
    {
        public byte GraphicsPreset;
        public int ResolutionWidth;
        public int ResolutionHeight;
        public int RefreshRate;
        public bool Fullscreen;
        public bool VSync;

        public VideoSettings(int resolutionWidth, int resolutionHeight, int refreshRate, byte graphicsPreset = 1, bool fullscreen = true, bool vSync = false)
        {
            GraphicsPreset = graphicsPreset;
            ResolutionWidth = resolutionWidth;
            ResolutionHeight = resolutionHeight;
            RefreshRate = refreshRate;
            Fullscreen = fullscreen;
            VSync = vSync;
        }
    }
}