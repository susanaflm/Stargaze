using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class OptionsMenu : MonoBehaviour
    {
        private InputAction _goBackAction;

        [SerializeField] private GameOptionsMenu gameOptionsMenu;
        [SerializeField] private VideoOptionsMenu videoOptionsMenu;
        [SerializeField] private AudioOptionsMenu audioOptionsMenu;
        
        public Action OnMenuQuit;
        
        private void Awake()
        {
            _goBackAction = new InputAction(binding: "<Keyboard>/escape"); // TODO: How can I add a second binding for gamepad?

            _goBackAction.performed += _ =>
            {
                OnMenuQuit?.Invoke();
            };

            gameOptionsMenu.OnMenuQuit += () =>
            {
                gameOptionsMenu.Hide();
                Show();
            };
            
            videoOptionsMenu.OnMenuQuit += () =>
            {
                videoOptionsMenu.Hide();
                Show();
            };
            
            audioOptionsMenu.OnMenuQuit += () =>
            {
                audioOptionsMenu.Hide();
                Show();
            };
        }

        public void OnGameOptionsButtonPressed()
        {
            gameOptionsMenu.Show();
            Hide();
        }
        
        public void OnVideoOptionsButtonPressed()
        {
            videoOptionsMenu.Show();
            Hide();
        }
        
        public void OnAudioOptionsButtonPressed()
        {
            audioOptionsMenu.Show();
            Hide();
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
        
        private void OnEnable()
        {
            _goBackAction.Enable();
        }

        private void OnDisable()
        {
            _goBackAction.Disable();
        }
    }
}