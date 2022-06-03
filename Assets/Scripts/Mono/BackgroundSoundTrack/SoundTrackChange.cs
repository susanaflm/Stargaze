using System;
using UnityEngine;

namespace Stargaze.Mono.BackgroundSoundTrack
{
    public class SoundTrackChange : MonoBehaviour
    {
        [SerializeField] private AudioSource backgroundSoundTrack;
        [SerializeField] private AudioClip soundtrackToPlay;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CharacterController>())
            {
                backgroundSoundTrack.Stop();
                backgroundSoundTrack.clip = soundtrackToPlay;
                backgroundSoundTrack.Play();
                gameObject.SetActive(false);
            }
        }
    }
}
