using DG.Tweening;
using Mirror;
using NaughtyAttributes;
using Stargaze.Mono.Puzzle;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Door
{
    public class Door : NetworkBehaviour
    {
        [SyncVar]
        [SerializeField] private bool isOpened;
        [SyncVar]
        [SerializeField] private bool requirePower = true;

        private Animator _doorAnimator;

        private void Awake()
        {
            _doorAnimator = GetComponent<Animator>();
            _doorAnimator.SetBool("Opened" , isOpened);
        }
        
        [Server]
        public void OpenDoor()
        {
            isOpened = true;
            
            RpcOpenDoor();
        }

        [ClientRpc]
        private void RpcOpenDoor()
        {
            Debug.Log($"Door: {gameObject.name} has been opened");

            _doorAnimator.SetBool("Opened", true);
        }

        [Server]
        public void CloseDoor()
        {
            isOpened = false;

            RpcCloseDoor();
        }

        [ClientRpc]
        private void RpcCloseDoor()
        {
            Debug.Log($"Door: {gameObject.name} has been closed");
            
            _doorAnimator.SetBool("Opened", false);
        }

        public void ToggleDoor()
        {
            if (requirePower)
            {
                if (!PuzzleManager.Instance.IsPowerOn() || !PuzzleManager.Instance.GravityStatus)
                {
                    Debug.Log("Can't open door! Turn Power On or activate gravity");
                    return;
                }
            }
            
            if (isOpened)
                CloseDoor();
            else
                OpenDoor();
        }
    }
}
