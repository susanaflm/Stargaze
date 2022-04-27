using DG.Tweening;
using Mirror;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Door
{
    public class Door : NetworkBehaviour
    {
        [SyncVar]
        private bool powerState = true;
        
        [SyncVar]
        [SerializeField] private bool isOpened;

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

            //TODO: The Actual door might need to have an animator trigger
            
            var position = transform.position;
            transform.DOMove(new Vector3(position.x,position.y + 3,position.z), 2f);
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
            
            //TODO: The Actual door might need to have an animator trigger
            
            var position = transform.position;
            transform.DOMove(new Vector3(position.x,position.y - 3,position.z), 2f);
        }

        public void ToggleDoor()
        {
            if (!powerState)
            {
                Debug.Log("Can't open door! Turn Power On or activate gravity");
                return;
            }

            if (isOpened)
                CloseDoor();
            else
                OpenDoor();
        }
    }
}
