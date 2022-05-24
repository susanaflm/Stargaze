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
        
        [Space]
        
        [SerializeField] private GameObject navigatorNotReadyIndicator;
        [SerializeField] private GameObject navigatorReadyIndicator;
        
        [Space]
        
        [SerializeField] private GameObject engineerNotReadyIndicator;
        [SerializeField] private GameObject engineerReadyIndicator;

        public StargazeRoomPlayer Player
        {
            get => _player;
            set
            {
                if (_player != null)
                {
                    _player.OnRoleChanged -= UpdateRoleIndicator;
                    _player.OnReadyStatusChanged -= UpdateRoleIndicator;
                }
                
                _player = value;

                if (_player != null)
                {
                    UpdateUI();

                    _player.OnRoleChanged += UpdateRoleIndicator;
                    _player.OnReadyStatusChanged += UpdateRoleIndicator;
                }
            }
        }

        private void UpdateUI()
        {
            usernameLabel.text = _player.Username;
            
            UpdateRoleIndicator();
        }

        private void UpdateRoleIndicator()
        {
            if (_player.Role == PlayerRoles.Engineer)
            {
                engineerReadyIndicator.SetActive(_player.IsReady);
                engineerNotReadyIndicator.SetActive(!_player.IsReady);
                
                navigatorReadyIndicator.SetActive(false);
                navigatorNotReadyIndicator.SetActive(false);
            }
            else if (_player.Role == PlayerRoles.Navigator)
            {
                navigatorReadyIndicator.SetActive(_player.IsReady);
                navigatorNotReadyIndicator.SetActive(!_player.IsReady);
                
                engineerReadyIndicator.SetActive(false);
                engineerNotReadyIndicator.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _player.OnRoleChanged -= UpdateRoleIndicator;
            _player.OnReadyStatusChanged -= UpdateRoleIndicator;
        }
    }
}