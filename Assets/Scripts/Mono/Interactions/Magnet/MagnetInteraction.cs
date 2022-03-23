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
        
        [Header("Iman Panel Settings")]
        [SerializeField] private Transform lowerLeftCorner;
        [SerializeField] private Transform upperRightCorner;
        [SerializeField] private Transform cameraPos;
        [Space]
        [Header("Camera Settings")]
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        [SerializeField] private Transform defaultCameraTransform;
        [Space]
        [SerializeField] private GameObject magnetPrefab;
        

        private void Start()
        {
            //isInteractable = false;
            Restore += RestoreCamera;
            _panelPos = transform.position;
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

            playerCamera.transform.DOMove(cameraPos.position, 0.5f);
            playerCamera.transform.DORotate(cameraPos.rotation.eulerAngles, 0.5f);
            
            _magnet = Instantiate(magnetPrefab, _panelPos + new Vector3(0,0,-transform.lossyScale.z / 2), Quaternion.identity);
            _magnet.GetComponent<MagnetController>().SetBoundaries(upperRightCorner.position, lowerLeftCorner.position);
        }

        private void RestoreCamera()
        {
            playerCamera.transform.DOMove(defaultCameraTransform.position, 0.5f);
            //playerCamera.transform.DORotate(defaultCameraTransform.rotation.eulerAngles, 0.5f);
            _magnet.GetComponent<MagnetController>().DestroyMagnet();
        }
    }
}
