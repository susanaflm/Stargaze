using System;
using Mirror;
using NaughtyAttributes;
using UnityEngine;

namespace Stargaze.Mono.Networking
{
    public class StargazeNetworkManager : NetworkRoomManager
    {
        public static Action<NetworkConnectionToClient> OnServerPlayerConnected;
        public static Action<NetworkConnectionToClient> OnServerPlayerDisconnected;

        public static Action OnPlayerEntered;
        public static Action OnPlayerExit;
        
        private string _lobbyName;
        
        public string LobbyName { get; set; }
        
        public override void OnRoomServerConnect(NetworkConnectionToClient conn)
        {
            StargazeAuthenticationData authData = (StargazeAuthenticationData)conn.authenticationData;
            
            Debug.Log($"Player with Steam ID {authData.SteamID} joined the lobby");

            OnServerPlayerConnected?.Invoke(conn);
        }

        public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
        {
            StargazeAuthenticationData authData = (StargazeAuthenticationData)conn.authenticationData;
            
            Debug.Log($"Player with Steam ID {authData.SteamID} disconnected from the lobby");

            OnServerPlayerDisconnected?.Invoke(conn);
        }

        public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
        {
            StargazeRoomPlayer roomPlayer = (StargazeRoomPlayer)Instantiate(roomPlayerPrefab, Vector3.zero, Quaternion.identity);
            roomPlayer.SetSteamID(((StargazeAuthenticationData)conn.authenticationData).SteamID);

            return roomPlayer.gameObject;
        }

        public override void OnRoomClientEnter()
        {
            OnPlayerEntered?.Invoke();
        }

        public override void OnRoomClientExit()
        {
            OnPlayerExit?.Invoke();
        }
    }
}