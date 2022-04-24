using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Interactions.Magnet
{
    public class MagnetController : NetworkBehaviour
    {
        private Vector3 _upperRightCorner;
        private Vector3 _lowerLeftCorner;

        private MagnetInput _input;
        private Vector3 _lastPosition;

        [SerializeField] private float magnetSpeed;
        
        private void Awake()
        {
            _input = GetComponent<MagnetInput>();
        }

        private void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            if (!hasAuthority)
                return;
            
            Vector2 input = _input.Movement;

            CmdMove(input);
        }

        [Command]
        private void CmdMove(Vector2 input)
        {
            Vector3 pos  = transform.position;
            
            pos.x = Mathf.Clamp(pos.x + input.x * magnetSpeed * Time.smoothDeltaTime, _lowerLeftCorner.x,
                _upperRightCorner.x);
            pos.y = Mathf.Clamp(pos.y + input.y * magnetSpeed * Time.smoothDeltaTime, _lowerLeftCorner.y,
                _upperRightCorner.y);
            
            transform.position = pos;
        }

        [ServerCallback]
        public void SetBoundaries(Vector2 upperRightCorner, Vector2 lowerLeftCorner)
        {
            _upperRightCorner = upperRightCorner;
            _lowerLeftCorner = lowerLeftCorner;
        }

        public void DestroyMagnet()
        {
            Destroy(gameObject);
        }
    }
}
