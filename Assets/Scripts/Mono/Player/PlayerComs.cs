using System;
using System.IO;
using Mirror;
using Stargaze.Input;
using Steamworks;
using UnityEngine;

namespace Stargaze.Mono.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerComs : NetworkBehaviour
    {
        private const ushort VoiceDataBufferSize = 20480;
        
        private InputActions _actions;

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
                
                CmdSendVoiceData(buffer, bytesWritten);
            }
        }

        [Command]
        private void CmdSendVoiceData(byte[] voiceBuffer, int size) // TODO: Pass radio channel?
        {
            RpcReceiveVoiceData(voiceBuffer, size);
        }

        [ClientRpc(includeOwner = false)]
        //[ClientRpc]
        private void RpcReceiveVoiceData(byte[] voiceBuffer, int size)// TODO: Pass radio channel?
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