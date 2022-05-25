using System;
using UnityEngine;

namespace Stargaze.ScriptableObjects.Settings
{
    [CreateAssetMenu(fileName = "GameSettingsData", menuName = "Game Settings Data Object", order = -44)]
    public class GameSettingsData : ScriptableObject
    {
        public float CameraSensitivity;
        public float MagnetSensitivity;
        public float EletricalSensitivity;

        public void Apply(GameSettings settings)
        {
            CameraSensitivity = settings.CameraSensitivity;
            MagnetSensitivity = settings.MagnetSensitivity;
            EletricalSensitivity = settings.EletricalSensitivity;
        }

        public GameSettings GetData()
        {
            return new GameSettings(CameraSensitivity, MagnetSensitivity, EletricalSensitivity);
        }
    }

    [Serializable]
    public struct GameSettings
    {
        public float CameraSensitivity;
        public float MagnetSensitivity;
        public float EletricalSensitivity;

        public GameSettings(float cameraSensitivity = 1f, float magnetSensitivity = 1f, float eletricalSensitivity = 1f)
        {
            CameraSensitivity = cameraSensitivity;
            MagnetSensitivity = magnetSensitivity;
            EletricalSensitivity = eletricalSensitivity;
        }
    }
}