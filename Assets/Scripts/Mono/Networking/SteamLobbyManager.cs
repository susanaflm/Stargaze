using Mirror;
using NaughtyAttributes;
using Steamworks;
using UnityEngine;

namespace Stargaze.Mono.Networking
{
    [RequireComponent(typeof(NetworkManager))]
    public class SteamLobbyManager : MonoBehaviour
    {
        private const string HostAddressKey = "HostAddress";
        
        public static SteamLobbyManager Instance;
        
        private NetworkManager _networkManager;

        protected Callback<LobbyCreated_t> LobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> GameLobbyJoinRequested;
        protected Callback<LobbyEnter_t> LobbyEnter;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);

            Instance = this;
            
            _networkManager = GetComponent<NetworkManager>();
            
            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            LobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
        }

        [Button]
        public void HostLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _networkManager.maxConnections);
        }
        
        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            EResult result = callback.m_eResult;
            
            if (result != EResult.k_EResultOK)
            {
                Debug.LogError($"Lobby creation failed with error: {result}");
                return;
            }
            
            _networkManager.StartHost();

            SteamMatchmaking.SetLobbyData(
                new CSteamID(callback.m_ulSteamIDLobby),
                HostAddressKey,
                SteamUser.GetSteamID().ToString()
            );
        }

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEnter(LobbyEnter_t callback)
        {
            if (NetworkServer.active)
                return;

            string hostAddress = SteamMatchmaking.GetLobbyData(
                new CSteamID(callback.m_ulSteamIDLobby),
                HostAddressKey
            );

            _networkManager.networkAddress = hostAddress;
            _networkManager.StartClient();
        }
    }
}