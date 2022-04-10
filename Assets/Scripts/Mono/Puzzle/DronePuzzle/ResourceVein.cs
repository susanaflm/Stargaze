using System;
using Stargaze.ScriptableObjects.Materials;
using UnityEngine;

namespace Stargaze.Mono.Puzzle.DronePuzzle
{
    public class ResourceVein : MonoBehaviour
    {
        [SerializeField] private ResourceMaterial resource;

        private void RemoveVein()
        {
            PuzzleManager.Instance.AddMaterial(resource);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<Drone>() != null)
            {
                RemoveVein();
            }
        }
    }
}
