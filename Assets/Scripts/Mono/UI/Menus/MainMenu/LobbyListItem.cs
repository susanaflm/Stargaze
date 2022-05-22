using Stargaze.Mono.Networking;
using Steamworks;
using TMPro;
using UnityEngine;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class LobbyListItem : MonoBehaviour
    {
        private Steamworks.Data.Lobby _lobby;

        [SerializeField] private TMP_Text lobbyNameLabel;
        
        public Steamworks.Data.Lobby Lobby
        {
            get => _lobby;
            set
            {
                _lobby = value;
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            lobbyNameLabel.text = Lobby.GetData(LobbyDataKeys.LobbyName.ToString());
        }

        public void OnPress()
        {
            SteamMatchmaking.JoinLobbyAsync(_lobby.Id);
        }
    }
}