using System;
using Stargaze.Mono.Player;
using UnityEngine;

namespace Stargaze.Mono.Puzzle.LaboratoryGas
{
    public class GasTimer : MonoBehaviour
    {
        [Tooltip("Time in Minutes")]
        [SerializeField] private float timeToKill = 3;

        [Tooltip("This Door will be closed behind the player")]
        [SerializeField] private Door.Door door;

        [SerializeField] private Transform teleportPosition;

        private float _timer = 0.0f;
        private bool _isPlayerInBounds;

        private GameObject _player;
        
        void Update()
        {
            if (!_isPlayerInBounds)
                return;

            _timer += Time.deltaTime;

            if (_timer >= timeToKill * 60)
            {
                if (_player == null)
                    return;

                _player.GetComponent<CharacterController>().enabled = false;
                _player.transform.position = teleportPosition.position;
                _player.GetComponent<CharacterController>().enabled = true;

                _timer = 0.0f;
                _isPlayerInBounds = false;
            }

#if DEBUG
            //Debug.Log($"Timer: {_timer}");
#endif
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerInput>())
            {
                _player = other.gameObject;
                _isPlayerInBounds = true;
                door.CmdToggleDoor();
            }
                
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerInput>())
            {
                _timer = 0.0f;
                _isPlayerInBounds = false;
            }
        }
    }
}
