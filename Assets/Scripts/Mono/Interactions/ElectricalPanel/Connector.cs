using System;
using UnityEngine;

namespace Stargaze.Mono.Interactions.ElectricalPanel
{
    public class Connector : MonoBehaviour
    {
        private BoxCollider _collider;
        private bool connectedWire;
        
        public bool IsConnectorOccupied => connectedWire;

        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponent<BoxCollider>();
        }

        public void SetWireConnected(bool isWireConnected)
        {
            connectedWire = isWireConnected;
        }

    }
}
