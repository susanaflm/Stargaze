using System;
using Mirror;
using Stargaze.Mono.Puzzle;
using Stargaze.ScriptableObjects.Materials;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Interactions.Vial
{
    public class VialInteractable : NetworkBehaviour, IInteractable
    {
        [SerializeField]
        private ResourceMaterial vialMaterial;
        
        private bool _isInteractable = true;

        [SerializeField] private bool switchable;
        
        public bool Switchable => switchable;

        public bool IsInteractable => _isInteractable;

        public void OnInteractionStart()
        {
            CmdCollectVial();
        }

        [Command(requiresAuthority = false)]
        private void CmdCollectVial()
        {
            PuzzleManager.Instance.AddMaterial(vialMaterial);
            Destroy(gameObject);
            NetworkServer.UnSpawn(gameObject);
        }

        public void OnInteractionEnd()
        {
            
        }

        public void SetResource(ResourceMaterial rm)
        {
            vialMaterial = rm;
            
            GetComponent<Renderer>().material.SetColor("_BaseColor", rm.vialColor);
        }
    }
}
