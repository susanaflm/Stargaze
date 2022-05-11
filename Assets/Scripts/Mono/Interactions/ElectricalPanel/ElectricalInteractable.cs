using System;
using Cinemachine;
using Stargaze.Mono.Puzzle;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stargaze.Mono.Interactions.ElectricalPanel
{
    public class ElectricalInteractable : MonoBehaviour, IInteractable
    {
        public delegate void OnConnection();
        public static OnConnection OnWireConnected;
        
        private bool _isInteractable = true;

        private bool _isEveryWireConnected = true;

        [SerializeField] private bool switchable;
        
        [SerializeField] private CinemachineVirtualCamera puzzleCamera;

        [Header("Animation")]
        [SerializeField] private Animator wiresDoorAnimator;
        [SerializeField] private Animator electricalDoorAnimator;
        [Space]
        [Header("Wire")]
        [SerializeField] private Wire[] wires;
        [SerializeField] private Connector[] connectors;

        [SerializeField] private WireSelector selector;
        [Space]
        [Header("Bounds")]
        [SerializeField] private Transform lowerLeftCorner;
        [SerializeField] private Transform upperRightCorner;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;
        
        private void Start()
        {
            puzzleCamera.gameObject.SetActive(false);
        }

        private void CheckWires()
        {
            _isEveryWireConnected = true;
            
            foreach (var wire in wires)
            {
                if (!wire.IsPowerOn)
                {
                    _isEveryWireConnected = false;
                }
            }

            if (!_isEveryWireConnected)
            {
                if (PuzzleManager.Instance.IsPowerOn())
                {
                    PuzzleManager.Instance.CmdSetPowerStatus(false);
                }
                
                return;
            }
            
            PuzzleManager.Instance.CmdSetPowerStatus(true);
        }

        public void OnInteractionStart()
        {
            OnWireConnected += CheckWires;
            
            puzzleCamera.gameObject.SetActive(true);
            
            wiresDoorAnimator.SetTrigger("Interacted");
            electricalDoorAnimator.SetTrigger("Interacted");

            selector.SetWires(wires);
            selector.enabled = true;

            foreach (Wire wire in wires)
            {
                wire.GetComponent<WireController>().SetBoundaries(upperRightCorner.localPosition, lowerLeftCorner.localPosition);
            }
        }

        public void OnInteractionEnd()
        {
            OnWireConnected -= CheckWires;
            
            puzzleCamera.gameObject.SetActive(false);
            selector.enabled = false;

            foreach (var wire in wires)
            {
                if (!wire.IsWireConnected)
                {
                    wire.GetComponent<WireController>().ResetPosition();
                    wire.GetComponent<WireController>().enabled = false;
                }
            }
        }
    }
}
