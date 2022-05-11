using System;
using Mirror;
using Stargaze.Mono.Interactions.Vial;
using Stargaze.ScriptableObjects.Materials;
using UnityEngine;

namespace Stargaze.Mono.Puzzle.DronePuzzle
{
    public class VialSpawner : NetworkBehaviour
    {

        [SerializeField] private GameObject _vialPrefab;
        [SerializeField] private Transform spawnLocation;
        
        public override void OnStartServer()
        {
            PuzzleManager.Instance.OnCollectMaterial += InstantiateVial;
        }
        
        [Server]
        private void InstantiateVial(ResourceMaterial resourceMaterial)
        {
            GameObject go = Instantiate(_vialPrefab, spawnLocation.position, Quaternion.identity);
            go.GetComponent<VialInteractable>().SetResource(resourceMaterial);
            
            NetworkServer.Spawn(go);
        }

        private void OnEnable()
        {
            if (isServer)
            {
                PuzzleManager.Instance.OnCollectMaterial += InstantiateVial;
            }
        }

        private void OnDisable()
        {
            if (isServer)
            {
                PuzzleManager.Instance.OnCollectMaterial -= InstantiateVial;
            }
        }
    }
}
