using System;
using UnityEngine;

namespace Stargaze.Mono.Interactions.ElectricalPanel
{
    public class WireSelector : MonoBehaviour
    {
        private Wire[] _wires;

        private int _currentWire = 0;

        private Material originalWireMaterial;

        private ElectricalInput _input;
        
        private void Awake()
        {
            _input = GetComponent<ElectricalInput>();
        }

        void Start()
        {
            _input.Select += SelectWire;
            _input.Left += GoLeft;
            _input.Right += GoRight;
        }

        // Update is called once per frame
        void Update()
        {
     
        }

        private void GoLeft()
        {
            _wires[_currentWire].SetHovered(false);

            _currentWire--;
            
            if (_currentWire < 0)
                _currentWire = _wires.Length - 1;

            _wires[_currentWire].SetHovered(true);
        }

        private void GoRight()
        {
            _wires[_currentWire].SetHovered(false);

            _currentWire++;
            
            if (_currentWire > _wires.Length - 1)
                _currentWire = 0;

            _wires[_currentWire].SetHovered(true);
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

        private void OnEnable()
        {
            _input.enabled = true;
            _wires[_currentWire].SetHovered(true);
        }

        private void OnDisable()
        {
            _wires[_currentWire].SetHovered(false);
            _input.enabled = false;
        }
    }
}
