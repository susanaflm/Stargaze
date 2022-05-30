using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Stargaze.Mono.Terminals
{
    public class DoorKeycodeTerminal : Terminal
    {
        [SyncVar(hook = nameof(OnInputCodeChanged))]
        private string _currentInput;

        private AudioSource _audioSource;
        
        [Header("Audio")]
        [SerializeField] private AudioClip buttonClick; 
        [SerializeField] private AudioClip buttonClearClick;
        [SerializeField] private AudioClip correctCode;
        
        [Header("Security")]
        [SerializeField] private string code;
        [SerializeField] private TMP_Text codeDisplay;

        [Header("Events")]
        [SerializeField] private UnityEvent onCodeValidated;

        protected override void TerminalAwake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public override void OnStartServer()
        {
            _currentInput = "";
        }

        public void OnNumberInput(int value)
        {
            _audioSource.PlayOneShot(buttonClick);
            CmdNumberInput(value);
        }

        [Command(requiresAuthority = false)]
        private void CmdNumberInput(int number)
        {
            _currentInput += number.ToString();
        }

        public void OnClearButtonPressed()
        {
            _audioSource.PlayOneShot(buttonClearClick);
            CmdClearInput();
        }

        [Command(requiresAuthority = false)]
        private void CmdClearInput()
        {
            _currentInput = "";
        }

        public void OnOkButtonPressed()
        {
            CmdValidateInput();
        }

        [Command(requiresAuthority = false)]
        private void CmdValidateInput()
        {
            if (_currentInput != code)
                return;
            
            _audioSource.PlayOneShot(correctCode);
            onCodeValidated.Invoke();
        }

        private void OnInputCodeChanged(string oldValue, string newValue)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            codeDisplay.text = _currentInput;
        }
    }
}