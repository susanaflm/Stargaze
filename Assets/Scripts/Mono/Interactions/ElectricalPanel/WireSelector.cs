using System;
using UnityEngine;

namespace Stargaze.Mono.Interactions.ElectricalPanel
{
    public class WireSelector : MonoBehaviour
    {
        private Wire[] _wires;

        private int _currentWire;

        private Material originalWireMaterial;

        private ElectricalInput _input;
        
        private void Awake()
        {
            _input = GetComponent<ElectricalInput>();
        }

        void Start()
        {
            _currentWire = 0;

            _input.Select += SelectWire;
        }

        // Update is called once per frame
        void Update()
        {
            Vector2 navigationInput = _input.Navigate;

            _wires[_currentWire].SetHovered(true);
            
            if (navigationInput.x > 0)
            {
                _wires[_currentWire].SetHovered(false);

                _currentWire++;
            }

            if (navigationInput.x < 0)
            {
                _wires[_currentWire].SetHovered(false);

                _currentWire--;
            }

            if (_currentWire < 0)
            {
                _currentWire = _wires.Length - 1;
            }

            if (_currentWire > _wires.Length - 1)
            {
                _currentWire = 0;
            }
        }

        private void SelectWire()
        {
            _wires[_currentWire].GetComponent<WireController>().enabled = true;
            _wires[_currentWire].DisconnectCable();

            enabled = false;
        }

        public void SetActive()
        {
            enabled = true;
        }

        public void  SetWires(Wire[] wires)
        {
            _wires = wires;
        }

        private void OnDisable()
        {
            _wires[_currentWire].SetHovered(false);
        }
    }
}
