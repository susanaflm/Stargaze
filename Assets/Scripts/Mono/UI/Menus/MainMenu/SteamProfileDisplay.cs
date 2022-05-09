using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stargaze.Mono.UI.Menus.MainMenu
{
    public class SteamProfileDisplay : MonoBehaviour
    {
        [SerializeField] private RawImage profilePic;
        [SerializeField] private TMP_Text username;

        private void Start()
        {
            SetProfileData();
        }

        private async void SetProfileData()
        {
            if (!SteamClient.IsValid)
                return;
            
            username.text = SteamClient.Name;

            Steamworks.Data.Image? profileImage = await SteamFriends.GetLargeAvatarAsync(SteamClient.SteamId);
            
            if (!profileImage.HasValue)
            {
                Debug.LogError($"Failed to get profile picture from Steam user with ID {SteamClient.SteamId}");
                return;
            }
            
            Debug.Log("Profile picture fetched with success!");

            int width = (int)profileImage.Value.Width;
            int height = (int)profileImage.Value.Height;

            Texture2D profileImageTexture = new Texture2D(width, height);

            for (int i = 0; i < width * height; i++)
            {
                Steamworks.Data.Color pixel = profileImage.Value.GetPixel(i % width, i / width);
                
                profileImageTexture.SetPixel(
                    i % width,
                    height - i / width,
                    new Color(pixel.r / 255f, pixel.g / 255f, pixel.b / 255f, pixel.a / 255f)
                );
            }

            profileImageTexture.wrapMode = TextureWrapMode.Clamp;
            profileImageTexture.Apply();

            profilePic.texture = profileImageTexture;
        }
    }
}