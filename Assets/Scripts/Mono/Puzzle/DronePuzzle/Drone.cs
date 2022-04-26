using System;
using DG.Tweening;
using Mirror;
using UnityEngine;

namespace Stargaze.Mono.Puzzle.DronePuzzle
{
    public class Drone : NetworkBehaviour
    {
        [SerializeField] private float distanceToTravel;
        [SerializeField] private RectTransform parent;

        [SyncVar(hook = nameof(OnPositionChanged))]
        private Vector2 _position;
        
        private float _width;
        private float _height;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            var size = parent.rect.size;
            
            _width = size.x;
            _height = size.y;
        }

        public override void OnStartServer()
        {
            _position = _rectTransform.anchoredPosition;
        }

        [Command(requiresAuthority = false)]
        public void CmdGoUp()
        {
            _position = Vector2.Lerp(
                _position,
                new Vector2(_position.x, _position.y + distanceToTravel),
                0.5f
            );
            
            ClampPosition();
        }
        
        [Command(requiresAuthority = false)]
        public void CmdGoDown()
        {
            _position = Vector2.Lerp(
                _position,
                new Vector2(_position.x, _position.y - distanceToTravel),
                0.5f
            );
            
            ClampPosition();
        }
        
        [Command(requiresAuthority = false)]
        public void CmdGoLeft()
        {
            _position = Vector2.Lerp(
                _position,
                new Vector2(_position.x - distanceToTravel, _position.y),
                0.5f
            );
            
            ClampPosition();
        }
        
        [Command(requiresAuthority = false)]
        public void CmdGoRight()
        {
            _position = Vector2.Lerp(
                _position,
                new Vector2(_position.x + distanceToTravel, _position.y),
                0.5f
            );
            
            ClampPosition();
        }
        
        private void ClampPosition()
        {
            Vector2 droneSize = _rectTransform.rect.size;
            
            _position.x = Mathf.Clamp(_position.x, 0 + droneSize.x / 2, _width - droneSize.x / 2);
            _position.y = Mathf.Clamp(_position.y, 0 + droneSize.y / 2, _height - droneSize.y / 2);
        }

        private void OnPositionChanged(Vector2 oldPosition, Vector2 newPosition)
        {
            _rectTransform.anchoredPosition = _position;
        }
    }
}
