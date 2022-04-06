using System;
using Cinemachine;
using DG.Tweening;
using Stargaze.Mono.Puzzle;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Interactions.Magnet
{
    public class MagnetInteraction : Interactable
    {
        private Vector3 _panelPos;

        private GameObject _magnet;
        
        [Header("Magnet Panel Settings"),Tooltip("Defines the area that the magnet can slide on")]
        [SerializeField] private Transform lowerLeftCorner;
        [SerializeField] private Transform upperRightCorner;
        [Space]
        [SerializeField] private CinemachineVirtualCamera puzzleCamera;
        [Space]
        [SerializeField] private GameObject magnetPrefab;
        

        private void Start()
        {
            isInteractable = false;
            _panelPos = transform.position;
            puzzleCamera.gameObject.SetActive(false);
        }

        private void Update()
        {
            //TODO: Don't like this
            isInteractable = PuzzleManager.Instance.DoesPlayerHaveMagnet();
        }

        /*
        private void SetInteractableOn()
        {
            isInteractable = true;
        }
        */
        
        public override void OnInteractionStart()
        {
            base.OnInteractionStart();

            puzzleCamera.gameObject.SetActive(true);
            _magnet = Instantiate(magnetPrefab, _panelPos + new Vector3(0,0,-transform.lossyScale.z / 2), Quaternion.identity);
            _magnet.GetComponent<MagnetController>().SetBoundaries(upperRightCorner.position, lowerLeftCorner.position);
        }

        public override void OnInteractionEnd()
        {
            base.OnInteractionEnd();
            
            RestoreCamera();
        }

        private void RestoreCamera()
        {
            puzzleCamera.gameObject.SetActive(false);
            _magnet.GetComponent<MagnetController>().DestroyMagnet();
        }
    }
}
