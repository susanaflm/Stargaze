using System;
using Cinemachine;
using DG.Tweening;
using Mirror;
using Stargaze.Mono.Puzzle;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Magnet
{
    public class MagnetInteraction : NetworkBehaviour, IInteractable
    {
        private bool _isInteractable = false;
        
        private Vector3 _panelPos;

        private GameObject _magnet;

        [SerializeField] private bool switchable;
        
        [Header("Magnet Panel Settings"),Tooltip("Defines the area that the magnet can slide on")]
        [SerializeField] private Transform lowerLeftCorner;
        [SerializeField] private Transform upperRightCorner;
        [Space]
        [SerializeField] private CinemachineVirtualCamera puzzleCamera;
        [Space]
        [SerializeField] private GameObject magnetPrefab;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;
        

        private void Start()
        {
            _panelPos = transform.position;
            puzzleCamera.gameObject.SetActive(false);
        }

        private void Update()
        {
            //TODO: Don't like this
            _isInteractable = PuzzleManager.Instance.DoesPlayerHaveMagnet();
        }

        /*
        private void SetInteractableOn()
        {
            isInteractable = true;
        }
        */
        
        public void OnInteractionStart()
        {
            puzzleCamera.gameObject.SetActive(true);
            
            CmdCreateMagnet();
        }

        [Command(requiresAuthority = false)]
        private void CmdCreateMagnet(NetworkConnectionToClient sender = null)
        {
            var planeTransform = transform;
            _magnet = Instantiate(magnetPrefab, _panelPos, planeTransform.rotation);
            _magnet.GetComponent<MagnetController>().SetBoundaries(upperRightCorner.position, lowerLeftCorner.position);
            
            NetworkServer.Spawn(_magnet, sender);
        }

        public void OnInteractionEnd()
        {
            RestoreCamera();

            CmdDestroyMagnet();
        }

        private void RestoreCamera()
        {
            puzzleCamera.gameObject.SetActive(false);
        }

        [Command(requiresAuthority = false)]
        private void CmdDestroyMagnet()
        {
            _magnet.GetComponent<MagnetController>().DestroyMagnet();
        }
    }
}
