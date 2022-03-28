using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Door
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private bool isOpened;

        private void OpenDoor()
        {
            Debug.Log($"Door: {gameObject.name} has been opened");
            isOpened = true;

            //TODO: The Actual door might need to have an animator trigger
            
            var position = transform.position;
            transform.DOMove(new Vector3(position.x,position.y + 3,position.z), 2f);
        }

        private void CloseDoor()
        {
            Debug.Log($"Door: {gameObject.name} has been closed");
            isOpened = false;

            
            //TODO: The Actual door might need to have an animator trigger
            
            var position = transform.position;
            transform.DOMove(new Vector3(position.x,position.y - 3,position.z), 2f);
        }

        public void ToggleDoor()
        {
            if (isOpened)
                CloseDoor();
            else
                OpenDoor();
        }
    }
}
