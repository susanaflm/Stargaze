using System;
using Stargaze.Enums;
using Stargaze.Mono.Networking;
using TMPro;
using UnityEngine;

namespace Stargaze.Mono.UI.Menus.Lobby
{
    public class LobbyPlayerListItem : MonoBehaviour
    {
        private StargazeRoomPlayer _player;
        
        [Header("UI Elements")]
        [SerializeField] private TMP_Text usernameLabel;
        [SerializeField] private GameObject navigatorIndicator;
        [SerializeField] private GameObject engineerIndicator;
        [SerializeField] private GameObject readyIndicator;

        public StargazeRoomPlayer Player
        {
            get => _player;
            set
            {
                if (_player != null)
                {
                    // TODO: Remove callbacks
                }
                
                _player = value;

                if (_player != null)
                {
                    UpdateUI();
                    
                    // TODO: Add callbacks
                }
            }
        }

        private void UpdateUI()
        {
            usernameLabel.text = _player.Username;
            
            UpdateRoleIndicator();

            UpdateReadyStatus();
        }

        private void UpdateRoleIndicator()
        {
            navigatorIndicator.SetActive(_player.Role == PlayerRoles.Navigator);
            engineerIndicator.SetActive(_player.Role == PlayerRoles.Engineer);
        }

        private void UpdateReadyStatus()
        {
            readyIndicator.SetActive(_player.IsReady);
        }
    }
}