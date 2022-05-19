using System;
using Mirror;
using Stargaze.Enums;
using Steamworks;
using UnityEngine;

namespace Stargaze.Mono.Networking
{
    public class PlayerRoleManager : NetworkBehaviour
    {
        public static Action OnRolesChanged;

        public static PlayerRoleManager Instance;
        
        [SyncVar(hook = nameof(RolesChangedCallback))] private SteamId navigatorPlayerID;
        [SyncVar(hook = nameof(RolesChangedCallback))] private SteamId engineerPlayerID;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        public override void OnStartServer()
        {
            navigatorPlayerID = new SteamId();
            engineerPlayerID = new SteamId();
            
            StargazeNetworkManager.OnServerPlayerConnected += AssignPlayerToRole;
            StargazeNetworkManager.OnServerPlayerDisconnected += ClearPlayerRole;
        }

        private void AssignPlayerToRole(NetworkConnectionToClient conn)
        {
            StargazeAuthenticationData playerAuthData = (StargazeAuthenticationData)conn.authenticationData;
            
            if (navigatorPlayerID.Value == 0)
            {
                navigatorPlayerID = playerAuthData.SteamID;
                
                Debug.Log($"{playerAuthData.SteamID} assigned as Navigator");
            }
            else if (engineerPlayerID == 0)
            {
                engineerPlayerID = playerAuthData.SteamID;
                
                Debug.Log($"{playerAuthData.SteamID} assigned as Engineer");
            }
            else
            {
                Debug.LogError("Excuse me sir, how did you got here???");
            }
        }

        private void ClearPlayerRole(NetworkConnectionToClient conn)
        {
            StargazeAuthenticationData playerAuthData = (StargazeAuthenticationData)conn.authenticationData;

            if (playerAuthData.SteamID == navigatorPlayerID)
            {
                navigatorPlayerID.Value = 0;
                
                Debug.Log("Navigator role is now empty");
            }
            else if (playerAuthData.SteamID == engineerPlayerID)
            {
                engineerPlayerID.Value = 0;
                
                Debug.Log("Engineer role is now empty");
            }
            else
            {
                Debug.LogError("Who was this guy???");
            }
        }

        public PlayerRoles GetPlayerRole(SteamId playerID)
        {
            if (playerID == navigatorPlayerID)
                return PlayerRoles.Navigator;
            else if (playerID == engineerPlayerID)
                return PlayerRoles.Engineer;
            else
                return PlayerRoles.None;
        }

        [Server]
        public void SwapRoles()
        {
            (navigatorPlayerID, engineerPlayerID) = (engineerPlayerID, navigatorPlayerID);
        }

        private void RolesChangedCallback(SteamId oldValue, SteamId newValue)
        {
            OnRolesChanged?.Invoke();
        }

        public override void OnStopClient()
        {
            Instance = null;
            Destroy(gameObject);
        }
    }
}