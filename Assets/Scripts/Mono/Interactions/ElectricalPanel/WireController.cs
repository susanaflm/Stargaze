using System;
using Stargaze.ScriptableObjects.Settings;
using UnityEngine;

namespace Stargaze.Mono.Interactions.ElectricalPanel
{
    public class WireController : MonoBehaviour
    {
        private Vector3 _lowerLeftCorner;
        private Vector3 _upperRightCorner;

        private ElectricalInput _input;

        private Wire _wire;
        
        private Connector _hoveringConnection;
        private Connector _connection;
        
        [SerializeField] private WireSelector selector;
        [SerializeField] private Transform parentTransform;
        [SerializeField] private GameSettingsData gameSettings;
        
        private void Awake()
        {
            _input = GetComponent<ElectricalInput>();
            _wire = GetComponent<Wire>();
        }

        private void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            Vector2 input = _input.WireMovement;

            var wireTransform = transform;
            Vector3 pos  = wireTransform.localPosition;

            pos += -wireTransform.right * (input.x * gameSettings.EletricalSensitivity * Time.deltaTime);
            pos += wireTransform.up * (input.y * gameSettings.EletricalSensitivity * Time.deltaTime);
            
            //TODO: Rework Clamp Function
            pos.x = Mathf.Clamp(pos.x, _upperRightCorner.x, _lowerLeftCorner.x);
            pos.y = Mathf.Clamp(pos.y, _lowerLeftCorner.y, _upperRightCorner.y);
            pos.z = Mathf.Clamp(pos.z, _lowerLeftCorner.z, _upperRightCorner.z);
            
            transform.localPosition = pos;
        }
        
        private void ConnectCable()
        {
            if (_hoveringConnection == null)
                return;

            _connection = _hoveringConnection;

            if (_connection.IsConnectorOccupied)
            {
                return;
            }
            
            _wire.ConnectCable(_connection);
            
            transform.position = _connection.transform.position;
            
            selector.SetActive();
            GetComponent<WireController>().enabled = false;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Connector>())
            {
                _hoveringConnection = other.GetComponent<Connector>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Connector>())
            {
                _hoveringConnection = null;
            }
        }
        
        public void SetBoundaries(Vector3 upperRightCorner, Vector3 lowerLeftCorner)
        {
            _upperRightCorner = upperRightCorner;
            _lowerLeftCorner = lowerLeftCorner;
        }

        public void ResetPosition()
        {
            transform.position = _wire.GetOriginalPosition();
        }

        private void OnEnable()
        {
            //transform.localRotation = parentTransform.rotation;

            _input.enabled = true;
            _input.PlaceWire += ConnectCable;
        }

        private void OnDisable()
        {
            _input.PlaceWire -= ConnectCable;
            _input.enabled = false;
        }
    }
}
