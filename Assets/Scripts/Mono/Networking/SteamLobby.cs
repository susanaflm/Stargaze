using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mirror;
using NaughtyAttributes;
using Steamworks;
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

        private List<CSteamID> _lobbyList;

        protected Callback<LobbyCreated_t> LobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> GameLobbyJoinRequested;
        protected Callback<LobbyEnter_t> LobbyEnter;

        protected Callback<LobbyMatchList_t> GetLobbyList;

        public Action OnLobbyListUpdated;
        public ReadOnlyCollection<CSteamID> LobbyList => _lobbyList.AsReadOnly();

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);

            Instance = this;
            
            _networkManager = GetComponent<NetworkManager>();

            _lobbyList = new List<CSteamID>();
            
            LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            LobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
            
            GetLobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
        }

        [Button]
        public void HostLobby()
        {
            //SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, _networkManager.maxConnections);
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, _networkManager.maxConnections);
        }

        public void RequestLobbyList()
        {
            _lobbyList.Clear();
            
            // TODO: Maybe add filter to lobby list
            SteamMatchmaking.RequestLobbyList();
        }
        
        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            EResult result = callback.m_eResult;
            
            if (result != EResult.k_EResultOK)
            {
                Debug.LogError($"Lobby creation failed with error: {result}");
                return;
            }
            
            Debug.Log("Lobby created with success!");
            
            _networkManager.StartHost();

            CSteamID lobbyID = new CSteamID(callback.m_ulSteamIDLobby);

            SteamMatchmaking.SetLobbyData(
                lobbyID,
                LobbyDataKeys.HostAddress.ToString(),
                SteamUser.GetSteamID().ToString()
            );

            SteamMatchmaking.SetLobbyData(
                lobbyID,
                LobbyDataKeys.LobbyName.ToString(),
                $"{SteamFriends.GetPersonaName()}'s Lobby"
            );

            SteamMatchmaking.SetLobbyData(
                lobbyID,
                LobbyDataKeys.LobbyValidationCheck.ToString(),
                Random.Range(1, 100).ToString()
            );
        }

        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            Debug.Log($"Join lobby request by '{callback.m_steamIDFriend}'");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }

        private void OnLobbyEnter(LobbyEnter_t callback)
        {
            if (NetworkServer.active)
                return;

            string hostAddress = SteamMatchmaking.GetLobbyData(
                new CSteamID(callback.m_ulSteamIDLobby),
                LobbyDataKeys.HostAddress.ToString()
            );

            _networkManager.networkAddress = hostAddress;
            _networkManager.StartClient();
        }

        private void OnGetLobbyList(LobbyMatchList_t callback)
        {
            for (int i = 0; i < callback.m_nLobbiesMatching; i++)
            {
                CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
                _lobbyList.Add(lobbyID);
            }
            
            OnLobbyListUpdated?.Invoke();
        }
    }
}