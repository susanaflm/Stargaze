using Steamworks;
using Steamworks.Data;
using UnityEngine;

namespace Stargaze.Mono.Networking
{
    public class DebugSteamLobbyControls : MonoBehaviour
    {
        private SteamLobby _steamLobby;
        
        private bool _showLobbyListWindow = false;

        private Rect _lobbyListWindowRect = new Rect(10, 90, 500, 400);

        private Lobby[] _lobbyList;

        private void Start()
        {
            _steamLobby = SteamLobby.Instance;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 200, 200));
            
            if (GUILayout.Button("Host Friends Only Lobby"))
                SteamLobby.Instance.HostFriendOnlyLobby();
            
            if (GUILayout.Button("Host Public Lobby"))
                SteamLobby.Instance.HostPublicLobby();

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
                RefreshLobbyList();

            if (_lobbyList == null)
                return;
            
            foreach (Lobby lobby in _lobbyList)
            {
                if (string.IsNullOrEmpty(lobby.GetData(LobbyDataKeys.LobbyValidationCheck.ToString())))
                    continue;
                
                string name = lobby.GetData(LobbyDataKeys.LobbyName.ToString());
                
                GUILayout.BeginHorizontal();
                
                GUILayout.Label($"{name}");

                if (GUILayout.Button("Join"))
                {
                    SteamMatchmaking.JoinLobbyAsync(lobby.Id);
                }
                
                GUILayout.EndHorizontal();
            }
        }
        
        private async void RefreshLobbyList()
        {
            _lobbyList = await _steamLobby.GetLobbyList();
        }
    }
}