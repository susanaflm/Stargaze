using Stargaze.Input;
using UnityEngine;

namespace Stargaze.Mono.Terminals
{
    [RequireComponent(typeof(RectTransform))]
    public class VirtualCursor : MonoBehaviour
    {
        private InputActions _actions;

        private Rect _canvasRect;
        private RectTransform _rectTransform;

        private Vector2 _cursorPos;

        [SerializeField] private RectTransform Canvas;
        
        private void Awake()
        {
            _actions = new InputActions();
            
            if (Canvas == null)
                Debug.LogError("Missing reference to Canvas Rect");
            
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _canvasRect = Canvas.rect;
            
            _cursorPos = new Vector2(_canvasRect.width / 2f, -_canvasRect.height / 2f);
        }

        private void Update()
        {
            Vector2 input = _actions.UI.VirtualCursorDelta.ReadValue<Vector2>();

            // TODO: Sensitivity variable
            _cursorPos += input * 15f * Time.deltaTime;

            _cursorPos.x = Mathf.Clamp(_cursorPos.x, 0f, _canvasRect.width);
            _cursorPos.y = Mathf.Clamp(_cursorPos.y, -_canvasRect.height, 0f);
            
            _rectTransform.anchoredPosition = _cursorPos;
        }

        private void OnEnable()
        {
            _actions.Enable();
        }

        private void OnDisable()
        {
            _actions.Disable();
        }
    }
}