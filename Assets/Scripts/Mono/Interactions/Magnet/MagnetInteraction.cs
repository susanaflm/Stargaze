using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Interactions.Magnet
{
    public class MagnetInteraction : Interactable
    {
        public static Action Restore;
        
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
            //isInteractable = false;
            Restore += RestoreCamera;
            _panelPos = transform.position;
            puzzleCamera.gameObject.SetActive(false);
        }

        private void Update()
        {
            //TODO: Check Player Inventory for Iman object, prob not on update
            /*
                if (//Check for iman)
                {
                    _isInteractable = true
                }
             */
        }

        public override void OnInteraction()
        {
            base.OnInteraction();

            puzzleCamera.gameObject.SetActive(true);
            _magnet = Instantiate(magnetPrefab, _panelPos + new Vector3(0,0,-transform.lossyScale.z / 2), Quaternion.identity);
            _magnet.GetComponent<MagnetController>().SetBoundaries(upperRightCorner.position, lowerLeftCorner.position);
        }

        private void RestoreCamera()
        {
            puzzleCamera.gameObject.SetActive(false);
            _magnet.GetComponent<MagnetController>().DestroyMagnet();
        }
    }
}
