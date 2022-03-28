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
        private InputActions _actions;

        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            _actions = new InputActions();

            _actions.Coms.PushToTalk.performed += _ =>
            {
                SteamUser.VoiceRecord = true;
            };
            
            _actions.Coms.PushToTalk.canceled += _ =>
            {
                SteamUser.VoiceRecord = false;
            };
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;
            
            if (SteamUser.HasVoiceData)
            {
                byte[] buffer = new byte[20480];
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
        private void RpcReceiveVoiceData(byte[] voiceBuffer, int size)// TODO: Pass radio channel?
        {
            Stream compressedStream = new MemoryStream(voiceBuffer);
            
            byte[] buffer = new byte[20480];
            Stream stream = new MemoryStream(buffer);
            
            int bytesRead = SteamUser.DecompressVoice(compressedStream, size, stream);

            Debug.Log(bytesRead);
        }
        
        private void OnEnable()
        {
            _actions.Enable();
        }

        private void OnDisable()
        {
            _actions.Disable();
        }
    }
}