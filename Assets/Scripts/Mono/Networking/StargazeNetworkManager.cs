using Mirror;
using UnityEngine;

namespace Stargaze.Mono.Networking
{
    public class StargazeNetworkManager : NetworkRoomManager
    {
        public override void OnRoomServerConnect(NetworkConnectionToClient conn)
        {
            StargazeAuthenticationData authData = (StargazeAuthenticationData)conn.authenticationData;
            
            Debug.Log($"Player with Steam ID {authData.SteamID} joined the lobby");
        }
    }
}