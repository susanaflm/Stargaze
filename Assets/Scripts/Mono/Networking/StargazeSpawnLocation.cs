using Stargaze.Enums;
using UnityEngine;

namespace Stargaze.Mono.Networking
{
    public class StargazeSpawnLocation : MonoBehaviour
    {
        [SerializeField] private PlayerRoles role;

        public PlayerRoles Role => role;

        private void Awake()
        {
            StargazeNetworkManager.RegisterSpawnLocation(this);
        }

        private void OnDestroy()
        {
            StargazeNetworkManager.UnRegisterSpawnLocation(this);
        }
    }
}