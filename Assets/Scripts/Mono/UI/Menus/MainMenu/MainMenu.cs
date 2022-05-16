using Mirror;
using Stargaze.Mono.Networking;
using UnityEngine;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public void OnJoinGameButtonPressed()
        {
            // TODO: Enable lobby list section
        }
        
        public void OnHostGameButtonPressed()
        {
            // TODO: Show lobby host options
            SteamLobby.Instance.HostFriendOnlyLobby();
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