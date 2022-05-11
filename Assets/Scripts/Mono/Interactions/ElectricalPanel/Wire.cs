using System;
using UnityEngine;

namespace Stargaze.Mono.Interactions.ElectricalPanel
{
    public class Wire : MonoBehaviour
    {
        private ElectricalInput _input;

        private Material _material;
        private Vector3 _originalPos;
        
        //Connection Variables
        private bool _isWireConnectedCorrectly = false;
        private bool _isWireConnected;
        
        private bool _isPlayerInteracting;
        private Connector _connector;
        
        [Tooltip("The Spot in which the wire will work to give power to the electrical box")]
        [SerializeField] private Connector desiredConnector;

        public bool IsPowerOn => _isWireConnectedCorrectly;

        public bool IsWireConnected => _isWireConnected;
        
        
        private void Start()
        {
            _material = GetComponent<Renderer>().material;
            _originalPos = transform.position;
        }

        public void SetHovered(bool isHovered)
        {
            _material.color = isHovered ? Color.blue : Color.white;
        }
        
        public void ConnectCable(Connector connection)
        {
            _isWireConnected = true;
            _connector = connection;
            _connector.SetWireConnected(true);

            _isWireConnectedCorrectly = _connector == desiredConnector;
            ElectricalInteractable.OnWireConnected?.Invoke();
        }

        public void DisconnectCable()
        {
            _isWireConnected = false;

            if (_connector == null)
                return;
            
            _connector.SetWireConnected(false);
        }

        public Vector3 GetOriginalPosition()
        {
            return _originalPos;
        }
    }
}
