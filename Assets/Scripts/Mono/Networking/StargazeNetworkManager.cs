using System;
using Mirror;
using Stargaze.Enums;
using Steamworks;
using UnityEngine;

namespace Stargaze.Mono.Networking
{
    public class StargazeNetworkManager : NetworkRoomManager
    {
        public static Action<NetworkConnectionToClient> OnServerPlayerConnected;
        public static Action<NetworkConnectionToClient> OnServerPlayerDisconnected;

        public static Action OnPlayerEntered;
        public static Action OnPlayerExit;

        private static StargazeSpawnLocation _navigatorSpawn;
        private static StargazeSpawnLocation _engineerSpawn;

        public StargazeRoomPlayer LocalRoomPlayer;
        
        public override void OnRoomServerConnect(NetworkConnectionToClient conn)
        {
            StargazeAuthenticationData authData = (StargazeAuthenticationData)conn.authenticationData;
            
            Debug.Log($"Player with Steam ID {authData.SteamID} joined the lobby. Connection ID: {conn.connectionId}");

            OnServerPlayerConnected?.Invoke(conn);
        }

        public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
        {
            StargazeAuthenticationData authData = (StargazeAuthenticationData)conn.authenticationData;
            
            Debug.Log($"Player with Steam ID {authData.SteamID} disconnected from the lobby. Connection ID: {conn.connectionId}");

            OnServerPlayerDisconnected?.Invoke(conn);
        }

        public override GameObject OnRoomServerCreateRoomPlayer(NetworkConnectionToClient conn)
        {
            StargazeRoomPlayer roomPlayer = (StargazeRoomPlayer)Instantiate(roomPlayerPrefab, Vector3.zero, Quaternion.identity);
            roomPlayer.SetSteamID(((StargazeAuthenticationData)conn.authenticationData).SteamID);

            return roomPlayer.gameObject;
        }

        public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
        {
            SteamId playerSteamID = ((StargazeAuthenticationData)conn.authenticationData).SteamID;

            PlayerRoles role = PlayerRoleManager.Instance.GetPlayerRole(playerSteamID);

            Transform spawnTransform;
            
            switch (role)
            {
                case PlayerRoles.Navigator:
                    spawnTransform = _navigatorSpawn.transform;
                    break;
                case PlayerRoles.Engineer:
                    spawnTransform = _engineerSpawn.transform;
                    break;
                default:
                    return null;
            }

            GameObject playerObject = Instantiate(playerPrefab, spawnTransform.position, spawnTransform.rotation);
            
            return playerObject;
        }

        public override void OnRoomClientEnter()
        {
            OnPlayerEntered?.Invoke();
        }

        public override void OnRoomClientExit()
        {
            OnPlayerExit?.Invoke();
        }

        public static void RegisterSpawnLocation(StargazeSpawnLocation spawn)
        {
            switch (spawn.Role)
            {
                case PlayerRoles.Navigator:
                    _navigatorSpawn = spawn;
                    break;
                case PlayerRoles.Engineer:
                    _engineerSpawn = spawn;
                    break;
            }
        }

        public static void UnRegisterSpawnLocation(StargazeSpawnLocation spawn)
        {
            switch (spawn.Role)
            {
                case PlayerRoles.Navigator:
                    _navigatorSpawn = null;
                    break;
                case PlayerRoles.Engineer:
                    _engineerSpawn = null;
                    break;
            }
        }
    }
}