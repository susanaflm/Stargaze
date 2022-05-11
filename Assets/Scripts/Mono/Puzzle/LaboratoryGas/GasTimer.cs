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
        [SerializeField] private Door.Door _door;

        private float _timer = 0.0f;
        private bool _isPlayerInBounds;
        
        
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= timeToKill * 60)
            {
                //TODO: Kill Player
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerInput>())
                _isPlayerInBounds = true;
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
