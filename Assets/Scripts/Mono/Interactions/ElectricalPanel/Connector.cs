using UnityEngine;

namespace Stargaze.Mono.Interactions.ElectricalPanel
{
    public class Connector : MonoBehaviour
    {
        private BoxCollider _collider;
        
        // Start is called before the first frame update
        void Start()
        {
            _collider = GetComponent<BoxCollider>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
