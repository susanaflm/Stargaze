using System;
using Mirror;
using Stargaze.Input;
using Stargaze.Mono.UI.Menus.MainMenu;
using UnityEngine;

namespace Stargaze.Mono.UI.Menus.PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        public static Action OnPause;
        public static Action OnResume;
        
        private InputActions _actions;

        private bool _isPaused = false;
        private bool _inInSubMenu = false;

        [SerializeField] private GameObject content;
        [SerializeField] private GameObject mainMenuContent;

        [Space]
        
        [SerializeField] private OptionsMenu optionsMenu;

        private void Awake()
        {
            _actions = new InputActions();

            Hide();
            
            _actions.UI.Pause.performed += _ => TogglePause();

            optionsMenu.OnMenuQuit += () =>
            {
                _inInSubMenu = false;
                
                optionsMenu.Hide();
                ShowMainMenu();
            };
        }

        public void OnResumeButtonPressed()
        {
            TogglePause();
        }

        public void OnOptionsButtonPressed()
        {
            _inInSubMenu = true;
            
            optionsMenu.Show();
            HideMainMenu();
        }

        public void OnQuitToMenuButtonPressed()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopHost();
            }
            else if (NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopClient();
            }
        }

        public void OnQuitToDesktopButtonPressed()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopHost();
            }
            else if (NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopClient();
            }
            
            Application.Quit();
        }

        private void TogglePause()
        {
            if (_inInSubMenu)
                return;
            
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                Show();
                OnPause?.Invoke();
            }
            else
            {
                Hide();
                OnResume?.Invoke();
            }
        }
        
        private void Show()
        {
            content.SetActive(true);
        }
        
        private void Hide()
        {
            content.SetActive(false);
        }

        private void ShowMainMenu()
        {
            mainMenuContent.SetActive(true);
        }

        private void HideMainMenu()
        {
            mainMenuContent.SetActive(false);
        }

        private void OnEnable()
        {
            _actions.Enable();
        }

        private void OnDisable()
        {
            _actions.Disable();
        }
    }
}