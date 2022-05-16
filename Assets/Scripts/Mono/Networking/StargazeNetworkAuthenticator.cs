using System.Collections.Generic;
using Mirror;
using Stargaze.Enums;
using Steamworks;
using Telepathy;
using UnityEngine;

namespace Stargaze.Mono.Networking
{
    public class StargazeNetworkAuthenticator : NetworkAuthenticator
    {
        private readonly HashSet<NetworkConnection> connectionsPendingDisconnect = new();
        
#region Server

        public override void OnStartServer()
        {
            NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
        }

        public override void OnStopServer()
        {
            NetworkServer.UnregisterHandler<AuthRequestMessage>();
        }

        public override void OnServerAuthenticate(NetworkConnectionToClient conn)
        {
            // No need to do anything
        }

        private void OnAuthRequestMessage(NetworkConnectionToClient conn, AuthRequestMessage message)
        {
            Debug.Log($"Received authentication request from Steam user with ID {message.SteamID}");
            
            if (connectionsPendingDisconnect.Contains(conn)) return;

            conn.authenticationData = new StargazeAuthenticationData()
            {
                SteamID = message.SteamID
            };

            AuthResponseMessage response = new AuthResponseMessage()
            {
                code = (int)NetworkResponseCodes.Success
            };
            
            conn.Send(response);
            
            ServerAccept(conn);
        }

#endregion

#region Client

        public override void OnStartClient()
        {
            NetworkClient.RegisterHandler<AuthResponseMessage>(OnAuthResponseMessage, false);
        }

        public override void OnStopClient()
        {
            NetworkClient.UnregisterHandler<AuthResponseMessage>();
        }

        public override void OnClientAuthenticate()
        {
            AuthRequestMessage message = new AuthRequestMessage()
            {
                SteamID = SteamClient.SteamId
            };
            
            NetworkClient.connection.Send(message);
        }

        private void OnAuthResponseMessage(AuthResponseMessage message)
        {
            if (message.code == (int)NetworkResponseCodes.Success)
            {
                Debug.Log("Connection accepted");
                
                ClientAccept();
            }
            else
            {
                Debug.Log($"Connection rejected with code {message.code}");
                
                NetworkManager.singleton.StopHost();
            }
        }

#endregion
        
    }
    
    public struct AuthRequestMessage : NetworkMessage
    {
        public SteamId SteamID;
    }

    public struct AuthResponseMessage : NetworkMessage
    {
        public int code;
    }
    
    public struct StargazeAuthenticationData
    {
        public SteamId SteamID;
    }
}