using System;
using System.Threading.Tasks;
using Mirror;
using NaughtyAttributes;
using Stargaze.Mono.Puzzle;
using UnityEngine;
using UnityEngine.Video;

namespace Stargaze.Mono.CutSceneControllers
{
    [RequireComponent(typeof(VideoPlayer))]
    public class OutroController : MonoBehaviour
    {
        public static Action OnStartPlay;
        public static Action OnEndPlay;

        public static bool IsPlaying;
        
        private VideoPlayer _player;
        
        private void Awake()
        {
            _player = GetComponent<VideoPlayer>();
        }

        private void Start()
        {
            _player.Prepare();

            PuzzleManager.OnGameFinished += PlayVideo;

            Hide();
        }
        
        [Button("Play")]
        private async void PlayVideo()
        {
            Show();
            
            _player.Play();

            IsPlaying = true;
            OnStartPlay?.Invoke();

            await Task.Delay((int)(_player.length * 1000));
            
            _player.Stop();

            IsPlaying = false;
            OnEndPlay?.Invoke();

            Hide();
            
            DisconnectPlayer();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }
        
        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void DisconnectPlayer()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopHost();
            }
            else if (NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopClient();
            }
        }
    }
}