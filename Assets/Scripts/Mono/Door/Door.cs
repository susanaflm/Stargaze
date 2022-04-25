using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Door
{
    public class Door : MonoBehaviour
    {
        private bool powerState = true;
        
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
