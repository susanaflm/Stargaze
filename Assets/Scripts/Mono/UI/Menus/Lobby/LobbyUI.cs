using Mirror;
using Stargaze.Mono.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stargaze.Mono.UI.Menus.Lobby
{
    public class LobbyUI : MonoBehaviour
    {
        private StargazeNetworkManager _networkManager;

        [Header("UI Elements")]
        [SerializeField] private TMP_Text lobbyNameLabel;
        [SerializeField] private Transform playerList;

        [Space]
        
        [SerializeField] private LobbyPlayerListItem prefab;

        [Space]
        
        [SerializeField] private Button swapRolesButton;

        [Space]
        
        [SerializeField] private Button readyButton;
        [SerializeField] private Button unreadyButton;
        
        private void Start()
        {
            _networkManager = (StargazeNetworkManager)NetworkManager.singleton;
            
            swapRolesButton.gameObject.SetActive(NetworkServer.active);

            Steamworks.Data.Lobby currentLobby = new Steamworks.Data.Lobby(SteamLobby.Instance.CurrentLobbyID);
            lobbyNameLabel.text = currentLobby.GetData(LobbyDataKeys.LobbyName.ToString());
            
            readyButton.onClick.AddListener(() => SetPlayerReady(true));
            unreadyButton.onClick.AddListener(() => SetPlayerReady(false));
            
            readyButton.gameObject.SetActive(true);
            unreadyButton.gameObject.SetActive(false);
        }

        void UpdatePlayers()
        {
            // Clear player list            
            foreach (Transform child in playerList)
            {
                Destroy(child.gameObject);
            }
            
            // Populate player list
            foreach (StargazeRoomPlayer player in _networkManager.roomSlots)
            {
                LobbyPlayerListItem item = Instantiate(prefab, playerList);
                item.Player = player;
            }
        }
        
        public void OnSwapRolesButtonPressed()
        {
            PlayerRoleManager.Instance.SwapRoles();
        }

        public void OnLeaveLobbyButtonPressed()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                _networkManager.StopHost();
            }
            else if (NetworkClient.isConnected)
            {
                _networkManager.StopClient();
            }
        }

        private void SetPlayerReady(bool status)
        {
            _networkManager.LocalRoomPlayer.SetReady(status);

            UpdateReadyButtons();
        }

        private void UpdateReadyButtons()
        {
            readyButton.gameObject.SetActive(!_networkManager.LocalRoomPlayer.IsReady);
            unreadyButton.gameObject.SetActive(_networkManager.LocalRoomPlayer.IsReady);
        }

        private void OnEnable()
        {
            StargazeNetworkManager.OnPlayerEntered += UpdatePlayers;
            StargazeNetworkManager.OnPlayerExit += UpdatePlayers;
        }

        private void OnDisable()
        {
            StargazeNetworkManager.OnPlayerEntered -= UpdatePlayers;
            StargazeNetworkManager.OnPlayerExit -= UpdatePlayers;
        }
    }
}