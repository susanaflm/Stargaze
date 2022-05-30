using System;
using Stargaze.Mono.Networking;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class JoinGameMenu : MonoBehaviour
    {
        private InputAction _goBackAction;

        [Header("Lobby List")]
        [SerializeField] private Transform lobbyListParent;
        [SerializeField] private LobbyListItem lobbyListItemPrefab;

        public Action OnMenuQuit;

        private void Awake()
        {
            _goBackAction = new InputAction(binding: "<Keyboard>/escape"); // TODO: How can I add a second binding for gamepad?

            _goBackAction.performed += _ =>
            {
                OnMenuQuit?.Invoke();
            };
        }

        private void OnEnable()
        {
            FillLobbyList();
            
            _goBackAction.Enable();
        }

        private async void FillLobbyList()
        {
            // Clear list
            foreach (Transform child in lobbyListParent)
            {
                Destroy(child.gameObject);
            }
            
            Steamworks.Data.Lobby[] lobbies = await SteamLobby.Instance.GetLobbyList();

            // Fill list
            foreach (Steamworks.Data.Lobby lobby in lobbies)
            {
                if (string.IsNullOrEmpty(lobby.GetData(LobbyDataKeys.LobbyValidationCheck.ToString())))
                    continue;
                
                LobbyListItem item = Instantiate(lobbyListItemPrefab, lobbyListParent);
                item.Lobby = lobby;
            }
        }

        public void OnGoBackButtonPressed()
        {
            OnMenuQuit?.Invoke();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _goBackAction.Disable();
        }
    }
}