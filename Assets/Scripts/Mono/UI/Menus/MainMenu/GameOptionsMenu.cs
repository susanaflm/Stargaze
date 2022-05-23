using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class GameOptionsMenu : MonoBehaviour
    {
        private InputAction _goBackAction;
        
        public Action OnMenuQuit;
        
        private void Awake()
        {
            _goBackAction = new InputAction(binding: "<Keyboard>/escape"); // TODO: How can I add a second binding for gamepad?

            _goBackAction.performed += _ =>
            {
                OnMenuQuit?.Invoke();
            };
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