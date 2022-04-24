using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Stargaze.Mono.Interactions.Magnet
{
    public class Magnet : NetworkBehaviour
    {
        private List<Rigidbody> _magneticStuff = new();
        private SphereCollider _sphereCollider;
        
        [Header("Magnetic Field Settings")]
        [SerializeField] private LayerMask magneticLayer;
        [SerializeField] private float magnetStrength;
        [SerializeField] private Transform attractionPoint;

        public override void OnStartServer()
        {
            _sphereCollider = GetComponent<SphereCollider>();
        }

        private void FixedUpdate()
        {
            AttractItems();
        }

        [ServerCallback]
        private void AttractItems()
        {
            foreach (var magneticItem in _magneticStuff)
            {
                magneticItem.AddForce((attractionPoint.position - magneticItem.position) * (magnetStrength * Time.fixedDeltaTime));
            }
        }

        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Magnetic"))
            {
                _magneticStuff.Add(other.gameObject.GetComponent<Rigidbody>());
            }
        }

        [ServerCallback]
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Magnetic"))
            {
                _magneticStuff.Remove(other.gameObject.GetComponent<Rigidbody>());
            }
        }
    }
}
