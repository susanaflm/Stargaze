using UnityEngine;

namespace Stargaze.Mono.Player
{
    public class PlayerFootsteps : MonoBehaviour
    {
        [SerializeField] private AudioClip step;
        
        private void Step()
        {
            GetComponent<AudioSource>().PlayOneShot(step);
        }
        
        private void Land()
        {
            // TODO: Play land sound
        }
    }
}