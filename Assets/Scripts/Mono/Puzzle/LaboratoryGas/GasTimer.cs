using System;
using Stargaze.Mono.Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Puzzle.LaboratoryGas
{
    public class GasTimer : MonoBehaviour
    {
        [Tooltip("Time in Minutes")]
        [SerializeField] private float timeToKill = 3;

        [Tooltip("This Door will be closed behind the player")]
        [SerializeField] private Door.Door door;

        [SerializeField] private Transform teleportPosition;

        [SerializeField] private Material poisonVignetteMat;

        private float _timer = 0.0f;
        private float _outerRingTimer;
        
        private bool _isPlayerInBounds = false;

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

                _currentInnerValue = Mathf.Clamp(_currentInnerValue, 0.0f, 1.0f);
                _currentOuterValue = Mathf.Clamp(_currentOuterValue, 0.0f, 1.0f);
                
                poisonVignetteMat.SetFloat("_InnerRing", _currentInnerValue);
                poisonVignetteMat.SetFloat("_OuterRing", _currentOuterValue);
                
                return;
            }
            
            _timer += Time.deltaTime;

            _currentInnerValue = 1.0f - (_timer / (timeToKill * 60));
            if (_currentInnerValue <= 0.5)
            {
                _outerRingTimer += Time.deltaTime;
                _currentOuterValue = 1.0f - ((_outerRingTimer / (timeToKill * 60)) * 0.5f);
            }

            poisonVignetteMat.SetFloat("_InnerRing", _currentInnerValue);
            poisonVignetteMat.SetFloat("_OuterRing", _currentOuterValue);

            if (_timer >= timeToKill * 60)
            {
                _player.GetComponent<CharacterController>().enabled = false;
                _player.transform.position = teleportPosition.position;
                _player.GetComponent<CharacterController>().enabled = true;
                
                GetComponent<AudioSource>().Play();

                _timer = 0.0f;
                _outerRingTimer = 0.0f;
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
            
            poisonVignetteMat.SetFloat("_InnerRing", 1.0f);
            poisonVignetteMat.SetFloat("_OuterRing", 1.0f);
                
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerInput>())
            {
                _timer = 0.0f;
                _isPlayerInBounds = false;
            }
        }

        private void OnDisable()
        {
            poisonVignetteMat.SetFloat("_InnerRing", 1.0f);
            poisonVignetteMat.SetFloat("_OuterRing", 1.0f);
        }
    }
}
