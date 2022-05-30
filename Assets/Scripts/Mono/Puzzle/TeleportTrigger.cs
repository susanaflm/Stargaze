using System;
using UnityEngine;

namespace Stargaze.Mono.Puzzle
{
    public class TeleportTrigger : MonoBehaviour
    {
        [SerializeField] private Transform positionToTeleport;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CharacterController>())
            {
                other.GetComponent<CharacterController>().enabled = false;
                other.transform.position = positionToTeleport.position;
                other.GetComponent<CharacterController>().enabled = true;
                
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
