using Steamworks;
using UnityEngine;

namespace Stargaze.Mono.Networking
{
    public class StandaloneSteamInitializer : MonoBehaviour
    {
        [SerializeField] private uint appID;

        private void Awake()
        {
            SteamClient.Init(appID);
        }

        private void OnDestroy()
        {
            SteamClient.Shutdown();
        }
    }
}