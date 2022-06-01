using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace Stargaze.Mono.CutSceneControllers
{
    [RequireComponent(typeof(VideoPlayer))]
    public class IntroController : MonoBehaviour
    {
        public static Action OnStartPlay;
        public static Action OnEndPlay;

        public static bool IsPlaying;
        
        private VideoPlayer _player;
        
        private void Awake()
        {
            _player = GetComponent<VideoPlayer>();

            _player.prepareCompleted += source => PlayVideo();
        }

        private void Start()
        {
            _player.Prepare();
        }

        private async void PlayVideo()
        {
            _player.Play();

            IsPlaying = true;
            OnStartPlay?.Invoke();

            await Task.Delay((int)(_player.length * 1000));
            
            _player.Stop();

            IsPlaying = false;
            OnEndPlay?.Invoke();

            Hide();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}