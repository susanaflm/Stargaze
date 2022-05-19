using System;
using Mirror;
using Stargaze.Enums;
using Steamworks;

namespace Stargaze.Mono.Networking
{
    public class StargazeRoomPlayer : NetworkRoomPlayer
    {
        [SyncVar(hook = nameof(SteamIDChangedCallback))]
        private SteamId _steamId;

        [SyncVar(hook = nameof(RoleChangedCallback))]
        private PlayerRoles _role;

        private Friend _friend;

        public Action OnRoleChanged;
        
        public string Username => _friend.Name;

        public PlayerRoles Role => _role;

        public bool IsReady => readyToBegin;
        
        public override void OnStartServer()
        {
            _role = PlayerRoleManager.Instance.GetPlayerRole(_steamId);

            PlayerRoleManager.OnRolesChanged += () =>
            {
                _role = PlayerRoleManager.Instance.GetPlayerRole(_steamId);
            };
        }

        [Server]
        public void SetSteamID(SteamId id)
        {
            _steamId = id;
        }

        private void SteamIDChangedCallback(SteamId oldValue, SteamId newValue)
        {
            _friend = new Friend(newValue);
        }

        private void RoleChangedCallback(PlayerRoles oldValue, PlayerRoles newValue)
        {
            OnRoleChanged?.Invoke();
        }
    }
}