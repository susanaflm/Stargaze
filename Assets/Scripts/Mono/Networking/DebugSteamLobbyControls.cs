using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace Stargaze.Mono.Networking
{
    public class DebugSteamLobbyControls : MonoBehaviour
    {
        private SteamLobby _steamLobby;
        
        private bool _showLobbyListWindow = false;

        private Rect _lobbyListWindowRect = new Rect(20, 20, 500, 400);

        private List<CSteamID> _lobbyList;

        private void Awake()
        {
            _lobbyList = new List<CSteamID>();
        }

        private void Start()
        {
            _steamLobby = SteamLobby.Instance;
            _steamLobby.OnLobbyListUpdated += UpdateLobbyList;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 200, 200));
            
            if (GUILayout.Button("Host Lobby"))
                SteamLobby.Instance.HostLobby();

            if (GUILayout.Button("Lobby List"))
                _showLobbyListWindow = !_showLobbyListWindow;

            GUILayout.EndArea();

            if (_showLobbyListWindow)
                _lobbyListWindowRect = GUI.Window(0, _lobbyListWindowRect, DrawLobbyListWindow, "Lobby List");
        }

        private void DrawLobbyListWindow(int id)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            if (GUILayout.Button("Refresh"))
                _steamLobby.RequestLobbyList();
            
            foreach (CSteamID steamID in _lobbyList)
            {
                string name = SteamMatchmaking.GetLobbyData(steamID, LobbyDataKeys.LobbyName.ToString());
                
                GUILayout.BeginHorizontal();
                
                GUILayout.Label($"{name}");

                if (GUILayout.Button("Join"))
                {
                    SteamMatchmaking.JoinLobby(steamID);
                }
                
                GUILayout.EndHorizontal();
            }
        }

        private void UpdateLobbyList()
        {
            _lobbyList.Clear();

            foreach (CSteamID id in _steamLobby.LobbyList)
            {
                if (string.IsNullOrEmpty(SteamMatchmaking.GetLobbyData(id, LobbyDataKeys.LobbyValidationCheck.ToString())))
                    continue;
                    
                _lobbyList.Add(id);
            }
        }

        private void OnDestroy()
        {
            _steamLobby.OnLobbyListUpdated -= UpdateLobbyList;
        }
    }
}