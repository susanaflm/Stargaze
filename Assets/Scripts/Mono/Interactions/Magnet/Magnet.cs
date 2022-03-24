using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Magnet
{
    public class Magnet : MonoBehaviour
    {
        private List<Rigidbody> _magneticStuff = new();
        private SphereCollider _sphereCollider;
        
        [Header("Magnetic Field Settings")]
        [SerializeField] private LayerMask magneticLayer;
        [SerializeField] private float magnetStrength;
        [SerializeField] private Transform attractionPoint;

        private void Start()
        {
            _sphereCollider = GetComponent<SphereCollider>();
        }

        private void FixedUpdate()
        {
            foreach (var magneticItem in _magneticStuff)
            {
                magneticItem.AddForce((attractionPoint.position - magneticItem.position) * magnetStrength * Time.fixedDeltaTime) ;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Magnetic"))
            {
                _magneticStuff.Add(other.gameObject.GetComponent<Rigidbody>());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Magnetic"))
            {
                _magneticStuff.Remove(other.gameObject.GetComponent<Rigidbody>());
            }
        }
    }
}
