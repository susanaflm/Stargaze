using System;
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

        [SerializeField] private float wireSpeed = 2.0f;
        [SerializeField] private WireSelector selector;
        
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
            Vector3 pos  = wireTransform.position;

            pos -= wireTransform.right * (input.x * wireSpeed * Time.deltaTime);
            pos += wireTransform.up * (input.y * wireSpeed * Time.deltaTime);

            pos.x = Mathf.Clamp(pos.x, _lowerLeftCorner.x, _upperRightCorner.x);
            pos.y = Mathf.Clamp(pos.y, _lowerLeftCorner.y, _upperRightCorner.y);
            pos.z = Mathf.Clamp(pos.z, _lowerLeftCorner.z, _upperRightCorner.z);
            
            transform.position = pos;
        }
        
        private void ConnectCable()
        {
            if (_hoveringConnection == null)
                return;

            transform.position = _hoveringConnection.transform.position;

            _connection = _hoveringConnection;
            _wire.ConnectCable(_connection);
            
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
            _input.PlaceWire += ConnectCable;
        }

        private void OnDisable()
        {
            _input.PlaceWire -= ConnectCable;
        }
    }
}
