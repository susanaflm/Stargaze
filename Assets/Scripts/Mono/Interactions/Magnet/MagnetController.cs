using System;
using System.Collections.Generic;
using Mirror;
using Stargaze.ScriptableObjects.Settings;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Interactions.Magnet
{
    public class MagnetController : NetworkBehaviour
    {
        private Vector3 _upperRightCorner;
        private Vector3 _lowerLeftCorner;

        private MagnetInput _input;
        
        [SerializeField] private GameSettingsData gameSettings;
        
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
            var magnetTransform = transform;
            Vector3 pos  = magnetTransform.position;

            pos += magnetTransform.right * (input.x * gameSettings.MagnetSensitivity * Time.deltaTime);
            pos += magnetTransform.up * (input.y * gameSettings.MagnetSensitivity * Time.deltaTime);

            /*
            pos.x = Mathf.Clamp(pos.x, _lowerLeftCorner.x, _upperRightCorner.x);
            pos.y = Mathf.Clamp(pos.y, _lowerLeftCorner.y, _upperRightCorner.y);
            pos.z = Mathf.Clamp(pos.z, _lowerLeftCorner.z, _upperRightCorner.z);*/
            
            transform.position = pos;
        }

        [ServerCallback]
        public void SetBoundaries(Vector3 upperRightCorner, Vector3 lowerLeftCorner)
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
