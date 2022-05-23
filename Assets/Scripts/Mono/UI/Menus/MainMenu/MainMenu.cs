using UnityEngine;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private JoinGameMenu joinGameMenu;
        [SerializeField] private HostGameMenu hostGameMenu;
        [SerializeField] private OptionsMenu optionsMenu;

        private void Awake()
        {
            joinGameMenu.OnMenuQuit += () =>
            {
                joinGameMenu.Hide();
                Show();
            };

            hostGameMenu.OnMenuQuit += () =>
            {
                hostGameMenu.Hide();
                Show();
            };

            optionsMenu.OnMenuQuit += () =>
            {
                optionsMenu.Hide();
                Show();
            };
        }

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
            optionsMenu.Show();
            Hide();
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