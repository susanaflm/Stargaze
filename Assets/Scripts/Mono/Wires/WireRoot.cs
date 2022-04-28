using System.Collections.Generic;
using UnityEngine;

namespace Stargaze.Mono.Wires
{
    public class WireRoot : MonoBehaviour
    {
        public float RigidbodyMass = 1f;
        public float ColliderRadius = 0.1f;
        public float JointSpring = 0.1f;
        public float JointDamper = 5f;
        public Vector3 RotationOffset;
        public Vector3 PositionOffset;

        private List<Transform> _copySource;
        private List<Transform> _copyDestination;
        private static GameObject _rigidBodyContainer;

        void Awake()
        {
            if (_rigidBodyContainer == null) _rigidBodyContainer = new GameObject("WireRigidbodyContainer");

            _copySource = new List<Transform>();
            _copyDestination = new List<Transform>();

            //add children
            AddChildren(transform);
        }

        private void AddChildren(Transform parent)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                var child = parent.GetChild(i);
                var representative = new GameObject(child.gameObject.name);
                representative.transform.parent = _rigidBodyContainer.transform;
                //rigidbody
                var childRigidbody = representative.gameObject.AddComponent<Rigidbody>();
                childRigidbody.useGravity = true;
                childRigidbody.isKinematic = false;
                childRigidbody.freezeRotation = true;
                childRigidbody.mass = RigidbodyMass;

                //collider
                var collider = representative.gameObject.AddComponent<SphereCollider>();
                collider.center = Vector3.zero;
                collider.radius = ColliderRadius;

                //DistanceJoint
                var joint = representative.gameObject.AddComponent<DistanceJoint3D>();
                joint.ConnectedRigidbody = parent;
                joint.DetermineDistanceOnStart = true;
                joint.Spring = JointSpring;
                joint.Damper = JointDamper;
                joint.DetermineDistanceOnStart = false;
                joint.Distance = Vector3.Distance(parent.position, child.position);

                //add copy source
                _copySource.Add(representative.transform);
                _copyDestination.Add(child);

                AddChildren(child);
            }
        }

        public void Update()
        {
            for (int i = 0; i < _copySource.Count; i++)
            {
                _copyDestination[i].position = _copySource[i].position + PositionOffset;
                _copyDestination[i].rotation = _copySource[i].rotation * Quaternion.Euler(RotationOffset);
            }
        }
    }
}