using System;
using System.IO;
using Mirror;
using Stargaze.Input;
using Stargaze.ScriptableObjects.Coms;
using Steamworks;
using UnityEngine;

namespace Stargaze.Mono.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerComs : NetworkBehaviour
    {
        public static ushort Frequency; 
        
        private const ushort VoiceDataBufferSize = 20480;
        
        private InputActions _actions;

        [SerializeReference] private ComsSettings comsSettings;
        
        [Space]
        
        [SerializeField] private AudioSource audioSource;

        public override void OnStartLocalPlayer()
        {
            _actions = new InputActions();
            _actions.Enable();

            _actions.Coms.PushToTalk.performed += _ =>
            {
                SteamUser.VoiceRecord = true;
            };
            
            _actions.Coms.PushToTalk.canceled += _ =>
            {
                SteamUser.VoiceRecord = false;
            };

            if (audioSource == null)
            {
                Debug.LogError("Missing reference to 'Audio Source' in Player Coms");
            }

            Frequency = comsSettings.StartingFrequency;
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;
            
            if (SteamUser.HasVoiceData)
            {
                byte[] buffer = new byte[VoiceDataBufferSize];
                Stream stream = new MemoryStream(buffer);
                int bytesWritten = SteamUser.ReadVoiceData(stream);
                
                CmdSendVoiceData(buffer, bytesWritten, Frequency);
            }
        }

        [Command]
        private void CmdSendVoiceData(byte[] voiceBuffer, int size, ushort frequency)
        {
            if (frequency < comsSettings.EmergencyFrequency - comsSettings.ErrorMargin &&
                frequency > comsSettings.EmergencyFrequency + comsSettings.ErrorMargin)
                return;
            
            RpcReceiveVoiceData(voiceBuffer, size);
        }

        [ClientRpc(includeOwner = false)]
        private void RpcReceiveVoiceData(byte[] voiceBuffer, int size)
        {
            Stream compressedStream = new MemoryStream(voiceBuffer);
            
            byte[] buffer = new byte[VoiceDataBufferSize];
            Stream stream = new MemoryStream(buffer);
            
            int bytesRead = SteamUser.DecompressVoice(compressedStream, size, stream);
            
            float[] voiceClipData = new float[bytesRead / 2];

            for (int i = 0; i < voiceClipData.Length; i++)
            {
                voiceClipData[i] = (short)(buffer[i * 2] | buffer[i * 2 + 1] << 8) / 32768.0f;
            }

            if (voiceClipData.Length == 0)
                return;
            
            audioSource.clip = AudioClip.Create(
                $"{name} coms clip",
                voiceClipData.Length,
                1,
                VoiceDataBufferSize,
                false
            );
            
            audioSource.clip.SetData(voiceClipData, 0);
            audioSource.Play();
        }
        
        private void OnEnable()
        {
            if (!isLocalPlayer)
                return;
            
            _actions.Enable();
        }

        private void OnDisable()
        {
            if (!isLocalPlayer)
                return;
            
            _actions.Disable();
        }
    }
}