using Mirror;
using Stargaze.Mono.Networking;
using UnityEngine;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private JoinGameMenu joinGameMenu;
        [SerializeField] private HostGameMenu hostGameMenu;
        
        public void OnJoinGameButtonPressed()
        {
            joinGameMenu.Show();
            Hide();
        }
        
        public void OnHostGameButtonPressed()
        {
            hostGameMenu.Show();
            Hide();
        }
        
        public void OnOptionsButtonPressed()
        {
            // TODO: Enable options section
        }
        
        public void OnCreditsButtonPressed()
        {
            // TODO: Enable credits section
        }
        
        public void OnQuitGameButtonPressed()
        {
            Application.Quit();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}