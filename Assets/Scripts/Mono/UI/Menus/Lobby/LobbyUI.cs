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
        
        private void Start()
        {
            _networkManager = (StargazeNetworkManager)NetworkManager.singleton;
            
            swapRolesButton.gameObject.SetActive(NetworkServer.active);

            Steamworks.Data.Lobby currentLobby = new Steamworks.Data.Lobby(SteamLobby.Instance.CurrentLobbyID);
            lobbyNameLabel.text = currentLobby.GetData(LobbyDataKeys.LobbyName.ToString());
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