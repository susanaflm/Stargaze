using Mirror;
using Stargaze.Mono.Networking;
using UnityEngine;

namespace Stargaze.Mono.UI.Menus.Lobby
{
    public class LobbyUI : MonoBehaviour
    {
        private StargazeNetworkManager _networkManager;

        [Header("UI Elements")]
        [SerializeField] private Transform playerList;

        [Space]
        
        [SerializeField] private LobbyPlayerListItem prefab;
        
        private void Start()
        {
            _networkManager = (StargazeNetworkManager)NetworkManager.singleton;
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