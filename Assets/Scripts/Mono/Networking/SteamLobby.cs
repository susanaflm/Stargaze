using System.Collections.Generic;
using System.Threading.Tasks;
using Mirror;
using NaughtyAttributes;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stargaze.Mono.Networking
{
    public enum LobbyDataKeys
    {
        HostAddress,
        LobbyName,
        LobbyValidationCheck
    }
    
    [RequireComponent(typeof(NetworkManager))]
    public class SteamLobby : MonoBehaviour
    {
        public static SteamLobby Instance;
        
        private NetworkManager _networkManager;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            _networkManager = GetComponent<NetworkManager>();

            SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
            SteamFriends.OnGameLobbyJoinRequested += OnGameLobbyJoinRequested;
            SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        }

        [Button]
        public async void HostFriendOnlyLobby()
        {
            Lobby? lobbyRequest = await SteamMatchmaking.CreateLobbyAsync(_networkManager.maxConnections);

            if (lobbyRequest.HasValue)
            {
                lobbyRequest.Value.SetFriendsOnly();
                lobbyRequest.Value.SetJoinable(true);
            }
        }
        
        [Button]
        public async void HostPublicLobby()
        {
            Lobby? lobbyRequest = await SteamMatchmaking.CreateLobbyAsync(_networkManager.maxConnections);

            if (lobbyRequest.HasValue)
            {
                lobbyRequest.Value.SetPublic();
                lobbyRequest.Value.SetJoinable(true);
            }
        }

        public async Task<Lobby[]> GetLobbyList()
        {
            //SteamMatchmaking.LobbyList.WithKeyValue(LobbyDataKeys.LobbyValidationCheck.ToString(), "Stargaze");
            
            return await SteamMatchmaking.LobbyList.RequestAsync();
        }
        
        private void OnLobbyCreated(Result result, Lobby lobby)
        {
            if (result != Result.OK)
            {
                Debug.Log($"Lobby creation failed with error: {result}");
                return;
            }
            
            Debug.Log("Lobby created with success!");

            _networkManager.StartHost();

            lobby.SetData(LobbyDataKeys.HostAddress.ToString(), SteamClient.SteamId.ToString());
            lobby.SetData(LobbyDataKeys.LobbyName.ToString(), $"{SteamClient.Name}'s Lobby");
            lobby.SetData(LobbyDataKeys.LobbyValidationCheck.ToString(), "Stargaze");
        }

        private void OnGameLobbyJoinRequested(Lobby lobby, SteamId id)
        {
            SteamMatchmaking.JoinLobbyAsync(lobby.Id);
        }

        private void OnLobbyEntered(Lobby lobby)
        {
            if (NetworkServer.active)
                return;

            string hostAddress = lobby.GetData(LobbyDataKeys.HostAddress.ToString());

            _networkManager.networkAddress = hostAddress;
            _networkManager.StartClient();
        }
    }
}