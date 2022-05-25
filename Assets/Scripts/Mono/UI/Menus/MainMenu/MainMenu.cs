using UnityEngine;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Menus")]
        [SerializeField] private JoinGameMenu joinGameMenu;
        [SerializeField] private HostGameMenu hostGameMenu;
        [SerializeField] private OptionsMenu optionsMenu;
        [SerializeField] private CreditsMenu creditsMenu;

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

            creditsMenu.OnMenuQuit += () =>
            {
                creditsMenu.Hide();
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
            creditsMenu.Show();
            Hide();
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