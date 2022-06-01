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

        [SerializeField] private Material _poisonVignetteMat;

        private float _timer = 0.0f;
        private bool _isPlayerInBounds;

        private float _currentInnerValue;
        private float _currentOuterValue;

        private GameObject _player;
        
        void Update()
        {
            if (!_isPlayerInBounds)
            {
                if (_currentInnerValue <= 1)
                    _currentInnerValue += 0.01f;

                if (_currentOuterValue <= 1)
                    _currentOuterValue += 0.01f;
                
                _poisonVignetteMat.SetFloat("_InnerRing", _currentInnerValue);
                _poisonVignetteMat.SetFloat("_OuterRing", _currentOuterValue);
                
                return;
            }
            
            _timer += Time.deltaTime;

            _currentInnerValue = 1.0f - (_timer / (timeToKill * 60));
            _currentOuterValue = 1.0f - ((_timer / (timeToKill * 60)) * 0.5f);

            _poisonVignetteMat.SetFloat("_InnerRing", _currentInnerValue);
            _poisonVignetteMat.SetFloat("_OuterRing", _currentOuterValue);

            if (_timer >= timeToKill * 60)
            {
                if (_player == null)
                    return;

                _player.GetComponent<CharacterController>().enabled = false;
                _player.transform.position = teleportPosition.position;
                _player.GetComponent<CharacterController>().enabled = true;
                
                GetComponent<AudioSource>().Play();

                _timer = 0.0f;
                _isPlayerInBounds = false;
                
                _poisonVignetteMat.SetFloat("_InnerRing", 1.0f);
                _poisonVignetteMat.SetFloat("_OuterRing", 1.0f);
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
            
            _poisonVignetteMat.SetFloat("_InnerRing", 1.0f);
            _poisonVignetteMat.SetFloat("_OuterRing", 1.0f);
                
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
