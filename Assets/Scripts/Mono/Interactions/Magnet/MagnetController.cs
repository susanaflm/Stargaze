using UnityEngine;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Interactions.Magnet
{
    public class MagnetController : MonoBehaviour
    {
        private Vector3 _upperRightCorner;
        private Vector3 _lowerLeftCorner;

        private MagnetInput _input;
        private Vector3 _lastPosition;

        [SerializeField] private float magnetSpeed;

        private void Awake()
        {
            _input = GetComponent<MagnetInput>();
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            Vector3 movementInput = _input.Movement;

            var pos  = transform.position;
            pos.x = Mathf.Clamp(pos.x + movementInput.x * magnetSpeed * Time.smoothDeltaTime, _lowerLeftCorner.x,
                _upperRightCorner.x);
            pos.y = Mathf.Clamp(pos.y + movementInput.y * magnetSpeed * Time.smoothDeltaTime, _lowerLeftCorner.y,
                _upperRightCorner.y);
            transform.position = pos;
        }

        public void SetBoundaries(Vector2 upperRightCorner, Vector2 lowerLeftCorner)
        {
            _upperRightCorner = upperRightCorner;
            _lowerLeftCorner = lowerLeftCorner;
        }

        public void DestroyMagnet()
        {
            Destroy(gameObject);
        }
    }
}
