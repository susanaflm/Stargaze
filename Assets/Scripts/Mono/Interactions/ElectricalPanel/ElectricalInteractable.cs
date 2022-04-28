using System;
using Cinemachine;
using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.ElectricalPanel
{
    public class ElectricalInteractable : MonoBehaviour, IInteractable
    {
        public delegate void OnInteraction();
        public static OnInteraction OnInteractionEnableWire;
        
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

        public void OnInteractionStart()
        {
            puzzleCamera.gameObject.SetActive(true);
            
            wiresDoorAnimator.SetTrigger("Interacted");
            electricalDoorAnimator.SetTrigger("Interacted");

            selector.enabled = true;
            selector.SetWires(wires);

            foreach (Wire wire in wires)
            {
                wire.GetComponent<WireController>().SetBoundaries(upperRightCorner.position, lowerLeftCorner.position);
            }
        }

        public void OnInteractionEnd()
        {
            _isEveryWireConnected = true;
            puzzleCamera.gameObject.SetActive(false);
            selector.enabled = false;

            foreach (var wire in wires)
            {
                if (!wire.IsWireConnected)
                {
                    wire.GetComponent<WireController>().ResetPosition(); 
                    wire.GetComponent<WireController>().enabled = false;
                }
                
                if (!wire.IsPowerOn)
                {
                    _isEveryWireConnected = false;
                }
            }

            if (!_isEveryWireConnected)
            {
                return;
            }
            
            PuzzleManager.Instance.ActivatePower();
#if DEBUG
            Debug.Log("Power On!");
#endif
            
        }
        
        
    }
}
