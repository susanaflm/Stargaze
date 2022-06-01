using System;
using Mirror;
using Stargaze.Input;
using Stargaze.Mono.CutSceneControllers;
using Stargaze.Mono.UI.RadioFrequencyPanel;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stargaze.Mono.Player
{
    public class PlayerInput : NetworkBehaviour
    {
        private InputActions _actions;
        
#if DEBUG
        private InputAction _debugToggleAction = new (binding: "<Keyboard>/f11");
#endif
        
        public Action Jump { get; set; }
        
        public Action Interact { get; set; }

        public Action ExitInteraction { get; set; }

        public Vector3 Strafe { get; private set; }
        
        public Vector2 Look { get; private set; }
        
        public float Roll { get; private set; }

        private void Awake()
        {
            _actions = new InputActions();

            _actions.Player.Jump.performed += _ => Jump?.Invoke();
            _actions.Player.Interact.performed += _ => Interact?.Invoke();
            _actions.Player.ExitInteraction.performed += _ => ExitInteraction?.Invoke();
            
#if DEBUG
            _debugToggleAction.performed += _ =>
            {
                if (enabled)
                    Disable();
                else
                    Enable();
            };
#endif
            
            if (IntroController.IsPlaying)
                Disable();
            
            if (OutroController.IsPlaying)
                Disable();

            IntroController.OnStartPlay += Disable;
            IntroController.OnEndPlay += Enable;
            
            OutroController.OnStartPlay += Disable;
            OutroController.OnEndPlay += Enable;
        }

        public override void OnStartLocalPlayer()
        {
#if DEBUG
            _debugToggleAction.Enable();
#endif

            RegisterCallbacks();
            
            _actions.Enable();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (!isLocalPlayer)
                return;
            
            Strafe = _actions.Player.Strafe.ReadValue<Vector3>().normalized;
            Look = _actions.Player.Look.ReadValue<Vector2>();
            Roll = _actions.Player.Roll.ReadValue<float>();
        }

        private void RegisterCallbacks()
        {
            RadioFrequencyPanel.OnRadioFrequencyPanelShow += Disable;
            RadioFrequencyPanel.OnRadioFrequencyPanelHide += Enable;
        }
        
        private void UnregisterCallbacks()
        {
            RadioFrequencyPanel.OnRadioFrequencyPanelShow -= Disable;
            RadioFrequencyPanel.OnRadioFrequencyPanelHide -= Enable;
        }

        private void Disable()
        {
            enabled = false;
            
            ResetInputValues();
        }

        private void Enable()
        {
            enabled = true;
        }

        private void ResetInputValues()
        {
            Strafe = Vector3.zero;
            Look = Vector2.zero;
            Roll = 0f;
        }

        private void OnEnable()
        {
            if (!isLocalPlayer)
                return;
            
            RegisterCallbacks();
            
            _actions.Enable();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable()
        {
            if (!isLocalPlayer)
                return;
            
            _actions.Disable();
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnDestroy()
        {
            UnregisterCallbacks();
        }
    }
}
