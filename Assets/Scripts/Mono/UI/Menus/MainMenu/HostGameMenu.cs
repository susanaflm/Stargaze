using Stargaze.Mono.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class HostGameMenu : MonoBehaviour
    {
        private InputAction _goBackAction;
        
        [Header("Menus")]
        [SerializeField] private MainMenu mainMenu;

        [Header("Fields")]
        [SerializeField] private TMP_InputField lobbyNameInput;
        [SerializeField] private TMP_Dropdown lobbyPrivacyDropdown;
        
        private void Awake()
        {
            _goBackAction = new InputAction(binding: "<Keyboard>/escape"); // TODO: How can I add a second binding for gamepad?

            _goBackAction.performed += _ =>
            {
                mainMenu.Show();
                Hide();
            };
        }

        public void OnHostGameButtonPressed()
        {
            string lobbyName = lobbyNameInput.text;
            bool isPublic = lobbyPrivacyDropdown.value == 0;
            
            SteamLobby.Instance.HostLobby(lobbyName, isPublic);
        }
        
        public void OnGoBackButtonPressed()
        {
            mainMenu.Show();
            Hide();
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