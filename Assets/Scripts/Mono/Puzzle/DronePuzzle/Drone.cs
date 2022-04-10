using System;
using DG.Tweening;
using UnityEngine;

namespace Stargaze.Mono.Puzzle.DronePuzzle
{
    public class Drone : MonoBehaviour
    {
        [SerializeField] private float distanceToTravel;
        [SerializeField] private RectTransform parent;

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

        private void Update()
        {
            var position = _rectTransform.anchoredPosition;
            var droneSize = _rectTransform.rect.size;

            position.x = Mathf.Clamp(position.x, 0 + droneSize.x / 2, _width - droneSize.x / 2);
            position.y = Mathf.Clamp(position.y, 0 + droneSize.y / 2, _height - droneSize.y / 2);

            _rectTransform.anchoredPosition = position;
        }

        public void GoUp()
        {
            //_rectTransform.anchoredPosition += new Vector2(0, distanceToTravel);
            var anchoredPosition = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = Vector2.Lerp(anchoredPosition,
                new Vector2(anchoredPosition.x, anchoredPosition.y + distanceToTravel),
                0.5f);
        }
        
        public void GoDown()
        {
            //_rectTransform.anchoredPosition += new Vector2(0, -distanceToTravel);
            var anchoredPosition = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = Vector2.Lerp(anchoredPosition,
                new Vector2(anchoredPosition.x, anchoredPosition.y - distanceToTravel),
                0.5f);
        }
        
        public void GoLeft()
        {
            //_rectTransform.anchoredPosition += new Vector2(-distanceToTravel, 0);
            var anchoredPosition = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = Vector2.Lerp(anchoredPosition,
                new Vector2(anchoredPosition.x - distanceToTravel, anchoredPosition.y),
                0.5f);
        }
        
        public void GoRight()
        {
            //_rectTransform.anchoredPosition += new Vector2(distanceToTravel, 0);
            var anchoredPosition = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = Vector2.Lerp(anchoredPosition,
                new Vector2(anchoredPosition.x + distanceToTravel, anchoredPosition.y),
                0.5f);
        }
        
    }
}
